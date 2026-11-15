package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.dto.booking.BookingRequest;
import com.coachbooking.dto.trip.*;
import com.coachbooking.repo.*;
import java.time.*;
import java.util.*;
import java.util.concurrent.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.EnabledIfEnvironmentVariable;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.TestConfiguration;
import org.springframework.context.annotation.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.test.context.DynamicPropertyRegistry;
import org.springframework.test.context.DynamicPropertySource;
import org.springframework.test.context.bean.override.mockito.MockitoSpyBean;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

/**
 * Requires a disposable, empty PostgreSQL database named coachbooking_feature_test. Flyway
 * exercises the actual migrations; never point this at an application database.
 */
@EnabledIfEnvironmentVariable(named = "TRIP_TEST_DB_URL", matches = ".*/coachbooking_feature_test")
@DataJpaTest(
    showSql = false,
    properties = {
      "spring.flyway.enabled=true", "spring.jpa.hibernate.ddl-auto=none",
      "logging.level.org.hibernate.SQL=OFF", "spring.jpa.show-sql=false"
    })
@AutoConfigureTestDatabase(replace = AutoConfigureTestDatabase.Replace.NONE)
@Import({
  TripService.class,
  ScheduleOccurrenceService.class,
  TripScheduleService.class,
  BookingService.class,
  InvoiceService.class,
  TripPlanningPostgresTest.Mocks.class
})
@Transactional(propagation = Propagation.NOT_SUPPORTED)
class TripPlanningPostgresTest {
  @DynamicPropertySource
  static void database(DynamicPropertyRegistry r) {
    r.add("spring.datasource.url", () -> System.getenv("TRIP_TEST_DB_URL"));
    r.add("spring.datasource.username", () -> "feature_test");
    r.add("spring.datasource.password", () -> "feature_test_local_only");
  }

  @TestConfiguration
  static class Mocks {
    @Bean
    com.fasterxml.jackson.databind.ObjectMapper objectMapper() {
      return new com.fasterxml.jackson.databind.ObjectMapper().findAndRegisterModules();
    }

    @Bean
    CurrentUserService currentUserService() {
      return mock(CurrentUserService.class);
    }

    @Bean
    SeatStatusService seatStatusService() {
      return mock(SeatStatusService.class);
    }
  }

  @Autowired JdbcTemplate jdbc;
  @Autowired TripService trips;
  @Autowired TripScheduleService schedules;
  @Autowired BookingService bookings;
  @Autowired InvoiceService invoices;
  @Autowired TripRepository tripRepository;
  @Autowired TicketRepository ticketRepository;
  @Autowired InvoiceRepository invoiceRepository;
  @Autowired TripScheduleRepository scheduleRepository;
  @Autowired ScheduleOccurrenceRepository occurrenceRepository;
  @MockitoSpyBean PaymentRepository payments;
  int vehicleId, driverId, routeId;
  LocalDate date;
  Account actor;
  static int serial;

  @BeforeEach
  void fixture() {
    reset(payments);
    // Each test gets its own vehicle, driver and route; no seed data is modified.
    int n =
        jdbc.queryForObject(
                "select count(*) from vehicles where license_plate like 'TEST-%'", Integer.class)
            + 1;
    serial = n;
    vehicleId =
        jdbc.queryForObject(
            "insert into vehicles(company_id,type_id,license_plate) values(1,1,?) returning vehicle_id",
            Integer.class,
            "TEST-" + n);
    driverId =
        jdbc.queryForObject(
            """
        insert into drivers(full_name,date_of_birth,citizen_id,gender,phone,address,license_number,issue_date)
        values('Test driver','1990-01-01',?,true,?,'Test','D','2010-01-01') returning driver_id
        """,
            Integer.class,
            String.format("99999999%04d", n),
            String.format("099999%04d", n));
    routeId =
        jdbc.queryForObject(
            "insert into routes(departure,destination,distance,estimated_duration) values(?,?,100,'02:00:00') returning route_id",
            Integer.class,
            "Test origin " + n,
            "Test destination " + n);
    jdbc.update(
        "insert into ticket_prices(company_id,type_id,route_id,price) values(1,1,?,150000)",
        routeId);
    date = LocalDate.now(TripPlanningTime.ZONE).plusDays(100);
    actor = new Account();
    actor.setAccountId(999);
    actor.setRoleId(2);
  }

  TripRequest request(LocalDateTime time) {
    return new TripRequest(vehicleId, routeId, driverId, time);
  }

  int schedule(LocalDate day) {
    return schedules
        .save(
            null,
            new ScheduleRequest(
                "Test recurring",
                vehicleId,
                routeId,
                driverId,
                LocalTime.of(8, 0),
                Set.of(1, 2, 3, 4, 5, 6, 7),
                day,
                day.plusDays(10),
                true,
                Set.of()))
        .getScheduleId();
  }

  BookingRequest booking(Trip trip, int seat) {
    return new BookingRequest(
        trip.getTripId(),
        List.of(seat),
        "Test customer",
        "0901234567",
        LocalDate.of(1990, 1, 1),
        true,
        "Cash",
        null);
  }

  @Test
  void concurrentManualRequestsCannotOverlapButTouchingIntervalsAreAllowed() throws Exception {
    LocalDateTime time = date.atTime(8, 0);
    var results =
        concurrently(
            () -> trips.create(request(time)), () -> trips.create(request(time.plusMinutes(30))));
    assertEquals(1, results.stream().filter(r -> r instanceof Trip).count());
    assertEquals(1, results.stream().filter(r -> r instanceof ApiException).count());
    Trip winner = (Trip) results.stream().filter(r -> r instanceof Trip).findFirst().orElseThrow();
    assertNotNull(trips.create(request(winner.getEstimatedArrivalTime())));
    int anotherVehicle =
        jdbc.queryForObject(
            "insert into vehicles(company_id,type_id,license_plate) values(1,1,?) returning vehicle_id",
            Integer.class,
            "OTHER-" + serial);
    assertThrows(
        ApiException.class,
        () ->
            trips.create(new TripRequest(anotherVehicle, routeId, driverId, time.plusMinutes(45))));
  }

  @Test
  void concurrentGenerationIsIdempotentAndDeletedOccurrencesStayDeleted() throws Exception {
    int id = schedule(date);
    concurrently(
        () -> schedules.run(id, date, date, false), () -> schedules.run(id, date, date, false));
    var occurrence = occurrenceRepository.findByScheduleIdAndServiceDate(id, date).orElseThrow();
    assertEquals("CREATED", occurrence.getStatus());
    assertEquals(
        1L,
        jdbc.queryForObject(
            "select count(*) from trips where vehicle_id=?", Long.class, vehicleId));
    trips.delete(occurrence.getTripId());
    assertEquals("DELETED", schedules.run(id, date, date, false).get(0).status());
    assertEquals(
        0L,
        jdbc.queryForObject(
            "select count(*) from trips where vehicle_id=?", Long.class, vehicleId));
  }

  @Test
  void concurrentBookingsCannotSellOneSeatTwice() throws Exception {
    Trip trip = trips.create(request(date.atTime(8, 0)));
    BookingRequest request = booking(trip, 1);
    var results =
        concurrently(
            () -> bookings.bookTickets(actor, request), () -> bookings.bookTickets(actor, request));
    assertEquals(
        1,
        results.stream()
            .filter(r -> r instanceof com.coachbooking.dto.booking.BookingResponse)
            .count());
    assertEquals(1, results.stream().filter(r -> r instanceof ApiException).count());
    assertEquals(List.of(1), ticketRepository.findSeatNumbersByTripId(trip.getTripId()));
  }

  @Test
  void soldTripKeepsItsFareWhenPriceTableChanges() {
    Trip old = trips.create(request(date.atTime(8, 0)));
    bookings.bookTickets(actor, booking(old, 1));
    jdbc.update(
        "update ticket_prices set price=200000 where company_id=1 and type_id=1 and route_id=?",
        routeId);
    assertEquals(
        0,
        old.getActualPrice()
            .compareTo(
                trips
                    .update(
                        old.getTripId(),
                        new TripRequest(vehicleId, routeId, driverId, old.getDepartureTime()))
                    .getActualPrice()));
  }

  @Test
  void bookingNeedsNoStopsAndRollsBackPaymentFailure() {
    Trip trip = trips.create(request(date.atTime(8, 0)));
    BookingRequest request = booking(trip, 1);
    var response = bookings.bookTickets(actor, request);
    assertEquals(1, invoices.detail(response.invoiceId(), actor).tickets().size());
    assertThrows(ApiException.class, () -> bookings.bookTickets(actor, request));
    long before = invoiceRepository.count();
    doThrow(new IllegalStateException("Test payment failure")).when(payments).save(any());
    assertThrows(IllegalStateException.class, () -> bookings.bookTickets(actor, booking(trip, 2)));
    assertEquals(before, invoiceRepository.count());
    assertEquals(List.of(1), ticketRepository.findSeatNumbersByTripId(trip.getTripId()));
  }

  @Test
  void invalidVehicleRollsBackTripAndPartialGenerationCanBeRetried() {
    int id = schedule(date);
    trips.create(request(date.atTime(8, 30)));
    var results = schedules.run(id, date, date.plusDays(1), false);
    assertEquals("ERROR", results.get(0).status());
    assertEquals("CREATED", results.get(1).status());
    assertTrue(scheduleRepository.findById(id).orElseThrow().getLastResult().contains("ERROR"));
    assertEquals(
        "EXISTS", schedules.run(id, date.plusDays(1), date.plusDays(1), false).get(0).status());
    long before = tripRepository.count();
    assertThrows(
        ApiException.class,
        () -> trips.create(new TripRequest(-1, routeId, driverId, date.plusDays(3).atTime(8, 0))));
    assertEquals(before, tripRepository.count());
  }

  private List<Object> concurrently(Callable<?> first, Callable<?> second) throws Exception {
    CountDownLatch start = new CountDownLatch(1);
    try (var pool = Executors.newFixedThreadPool(2)) {
      List<Future<Object>> futures = new ArrayList<>();
      for (var task : List.of(first, second))
        futures.add(
            pool.submit(
                () -> {
                  start.await();
                  try {
                    return task.call();
                  } catch (Exception ex) {
                    return ex;
                  }
                }));
      start.countDown();
      return List.of(
          futures.get(0).get(20, TimeUnit.SECONDS), futures.get(1).get(20, TimeUnit.SECONDS));
    }
  }
}
