package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.config.SepayProperties;
import com.coachbooking.domain.*;
import com.coachbooking.dto.booking.*;
import com.coachbooking.dto.payment.SepayIpn;
import com.coachbooking.dto.payment.SepayLookupResult;
import com.coachbooking.dto.payment.SepayOrderResponse;
import com.coachbooking.repo.*;
import java.math.BigDecimal;
import java.time.*;
import java.util.*;
import java.util.concurrent.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.TestConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Import;
import org.springframework.test.context.bean.override.mockito.MockitoSpyBean;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

/**
 * Isolated H2 only. Each service call commits its own real transaction, including worker threads.
 */
@DataJpaTest(
    properties = {
      "spring.flyway.enabled=false",
      "spring.jpa.hibernate.ddl-auto=create-drop",
      "spring.jpa.show-sql=false",
      "logging.level.org.hibernate.SQL=OFF",
      "app.sepay.enabled=true",
      "app.sepay.environment=sandbox",
      "app.sepay.merchant-id=test-merchant",
      "app.sepay.secret-key=test-merchant-key",
      "app.sepay.ipn-secret=test-ipn-key",
      "app.sepay.frontend-url=https://tickets.example.com"
    })
@Import({BookingService.class, SepayService.class, SepayTransactionTest.Config.class})
@Transactional(propagation = Propagation.NOT_SUPPORTED)
class SepayTransactionTest {
  private static final Instant NOW = Instant.parse("2030-01-01T00:00:00Z");

  @TestConfiguration
  static class Config {
    @Bean
    Clock clock() {
      return mock(Clock.class);
    }

    @Bean
    SeatStatusService seatStatus() {
      return mock(SeatStatusService.class);
    }

    @Bean
    CurrentUserService currentUser() {
      return mock(CurrentUserService.class);
    }

    @Bean
    SepayProperties sepayProperties() {
      SepayProperties props = new SepayProperties();
      props.setEnabled(true);
      props.setMerchantId("test-merchant");
      props.setSecretKey("test-merchant-key");
      props.setIpnSecret("test-ipn-key");
      props.setFrontendUrl("https://tickets.example.com");
      return props;
    }
  }

  @Autowired BookingService booking;
  @Autowired SepayService sepay;
  @Autowired Clock clock;
  @Autowired SepayProperties config;
  @Autowired CurrentUserService currentUser;
  @Autowired SeatStatusService seatStatus;
  @Autowired AccountRepository accounts;
  @Autowired CustomerRepository customers;
  @Autowired VehicleTypeRepository vehicleTypes;
  @Autowired VehicleRepository vehicles;
  @Autowired TripRepository trips;
  @Autowired SeatHoldRepository holds;
  @Autowired InvoiceRepository invoices;
  @Autowired TicketRepository tickets;
  @Autowired SepayNotificationRepository notifications;
  @MockitoSpyBean PaymentRepository payments;
  @MockitoSpyBean SepaySessionRepository sessions;
  private Account actor;
  private Integer tripId;
  private UUID token;

  @BeforeEach
  void setup() {
    config.setConfirmationMode("IPN");
    reset(payments, sessions, clock, currentUser, seatStatus);
    when(clock.instant()).thenReturn(NOW);
    notifications.deleteAll();
    sessions.deleteAll();
    payments.deleteAll();
    tickets.deleteAll();
    holds.deleteAll();
    invoices.deleteAll();
    customers.deleteAll();
    accounts.deleteAll();
    trips.deleteAll();
    vehicles.deleteAll();
    vehicleTypes.deleteAll();
    actor = new Account();
    actor.setUsername("sepay-customer");
    actor.setEmail("sepay@example.com");
    actor.setRoleId(3);
    actor = accounts.save(actor);
    when(currentUser.isCustomer(any())).thenReturn(true);
    Customer customer = new Customer();
    customer.setAccountId(actor.getAccountId());
    customer.setFullName("Test Customer");
    customer.setPhone("0901234567");
    customer.setDateOfBirth(LocalDate.of(1990, 1, 1));
    customer.setGender(false);
    customers.save(customer);
    VehicleType type = new VehicleType();
    type.setTypeName("Test 40 seats");
    type.setSeatCapacity(40);
    type.setIsActive(true);
    type = vehicleTypes.save(type);
    Vehicle vehicle = new Vehicle();
    vehicle.setCompanyId(1);
    vehicle.setTypeId(type.getTypeId());
    vehicle.setLicensePlate("51B-12345");
    vehicle.setIsActive(true);
    vehicle = vehicles.save(vehicle);
    Trip trip = new Trip();
    trip.setVehicleId(vehicle.getVehicleId());
    trip.setRouteId(1);
    trip.setDriverId(1);
    trip.setDepartureTime(LocalDateTime.of(2030, 1, 1, 8, 0));
    trip.setActualPrice(new BigDecimal("150000"));
    tripId = trips.save(trip).getTripId();
    token = UUID.randomUUID();
    for (int seat : List.of(1, 2)) {
      SeatHold hold = new SeatHold();
      hold.setHoldToken(token);
      hold.setTripId(tripId);
      hold.setAccountId(actor.getAccountId());
      hold.setSeatNumber(seat);
      hold.setCreatedAt(LocalDateTime.now());
      hold.setExpiresAt(LocalDateTime.now().plusMinutes(10));
      holds.save(hold);
    }
  }

  private BookingRequest request() {
    return new BookingRequest(tripId, List.of(1, 2), null, null, null, null, "Chuyển khoản", token);
  }

  private SepayIpn ipn(UUID sessionId) {
    SepaySession session = sessions.findById(sessionId).orElseThrow();
    return new SepayIpn(
        "ORDER_PAID",
        new SepayIpn.Order(session.getReference(), "CAPTURED", session.getAmount(), "VND"),
        new SepayIpn.Transaction(
            "txn-1", "APPROVED", "PAYMENT", session.getAmount(), "VND", "BANK_TRANSFER"));
  }

  @Test
  void bookingReplayAndRepeatedIpnAreIdempotent() {
    BookingResponse first = booking.bookTickets(actor, request());
    assertFalse(first.isPaid());
    assertEquals(0, payments.count());
    assertEquals(2, tickets.count());
    assertEquals(0, holds.count());
    BookingResponse replay = booking.bookTickets(actor, request());
    assertEquals(first.paymentSessionId(), replay.paymentSessionId());
    assertEquals(1, invoices.count());
    assertEquals(
        sepay.checkout(first.paymentSessionId(), actor),
        sepay.checkout(first.paymentSessionId(), actor));
    sepay.receive(ipn(first.paymentSessionId()));
    sepay.receive(ipn(first.paymentSessionId()));
    assertEquals(1, payments.count());
    assertEquals(1, notifications.count());
    assertTrue(invoices.findById(first.invoiceId()).orElseThrow().getIsPaid());
    assertEquals("PAID", sepay.status(first.paymentSessionId(), actor).paymentStatus());
  }

  @Test
  void concurrentBookingUsingSameHoldCreatesOnlyOneInvoice() throws Exception {
    var results =
        race(
            () -> booking.bookTickets(actor, request()),
            () -> booking.bookTickets(actor, request()));
    assertEquals(
        ((BookingResponse) results.get(0)).paymentSessionId(),
        ((BookingResponse) results.get(1)).paymentSessionId());
    assertEquals(1, invoices.count());
    assertEquals(1, sessions.count());
    assertEquals(2, tickets.count());
    assertEquals(0, payments.count());
  }

  @Test
  void concurrentDuplicateIpnCreatesOnlyOnePayment() throws Exception {
    var response = booking.bookTickets(actor, request());
    var payload = ipn(response.paymentSessionId());
    race(
        () -> {
          sepay.receive(payload);
          return true;
        },
        () -> {
          sepay.receive(payload);
          return true;
        });
    assertEquals(1, payments.count());
    assertEquals(1, notifications.count());
    assertEquals("PAID", sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
  }

  @Test
  void concurrentCancelAndIpnNeverLeavesPaidInvoiceWithoutSeats() throws Exception {
    var response = booking.bookTickets(actor, request());
    var payload = ipn(response.paymentSessionId());
    race(
        () -> {
          sepay.receive(payload);
          return true;
        },
        () -> {
          try {
            return sepay.cancelInvoice(response.invoiceId());
          } catch (ApiException ex) {
            assertEquals(org.springframework.http.HttpStatus.CONFLICT, ex.getStatus());
            return false;
          }
        });
    boolean paid = invoices.findById(response.invoiceId()).orElseThrow().getIsPaid();
    assertEquals(paid ? 1 : 0, payments.count());
    assertEquals(paid ? 2 : 0, tickets.count());
    assertEquals(
        paid ? "PAID" : "REVIEW_REQUIRED",
        sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertEquals(1, notifications.count());
  }

  @Test
  void expiryAndIpnAtDeadlineRequireReviewAndNeverReissueSeats() throws Exception {
    var response = booking.bookTickets(actor, request());
    var payload = ipn(response.paymentSessionId());
    when(clock.instant()).thenReturn(NOW.plusSeconds(600));
    race(
        () -> {
          sepay.expireSession(response.paymentSessionId());
          return true;
        },
        () -> {
          sepay.receive(payload);
          return true;
        });
    assertEquals(0, payments.count());
    assertEquals(0, tickets.count());
    assertEquals(
        "REVIEW_REQUIRED",
        sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertFalse(invoices.findById(response.invoiceId()).orElseThrow().getIsPaid());
  }

  @Test
  void checkoutExpiryIsCommittedEvenThoughResponseIsConflict() {
    var response = booking.bookTickets(actor, request());
    when(clock.instant()).thenReturn(NOW.plusSeconds(600));
    assertThrows(ApiException.class, () -> sepay.checkout(response.paymentSessionId(), actor));
    assertEquals(
        "EXPIRED", sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertEquals(0, tickets.count());
    assertEquals(1, invoices.count());
  }

  @Test
  void failedPaymentInsertRollsBackInvoiceSessionAndNotification() {
    var response = booking.bookTickets(actor, request());
    doThrow(new IllegalStateException("payment storage failed")).when(payments).save(any());
    var payload = ipn(response.paymentSessionId());
    assertThrows(IllegalStateException.class, () -> sepay.receive(payload));
    assertFalse(invoices.findById(response.invoiceId()).orElseThrow().getIsPaid());
    assertEquals(
        "PENDING", sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertEquals(0, notifications.count());
    assertEquals(0, payments.count());
    assertEquals(2, tickets.count());
  }

  @Test
  void sessionFailureRollsBackEntireBookingAndPreservesHold() {
    doThrow(new IllegalStateException("session storage failed")).when(sessions).save(any());
    assertThrows(IllegalStateException.class, () -> booking.bookTickets(actor, request()));
    assertEquals(0, invoices.count());
    assertEquals(0, tickets.count());
    assertEquals(0, payments.count());
    assertEquals(2, holds.count());
    verify(seatStatus, never()).publishAfterCommit(any());
  }

  @Test
  void latePaymentDoesNotAffectSeatResoldAfterExpiry() {
    var response = booking.bookTickets(actor, request());
    var payload = ipn(response.paymentSessionId());
    when(clock.instant()).thenReturn(NOW.plusSeconds(601));
    sepay.expireSession(response.paymentSessionId());
    Invoice another = new Invoice();
    another.setCreatedAt(LocalDateTime.now());
    another.setTotalAmount(new BigDecimal("150000"));
    another.setIsPaid(true);
    another = invoices.save(another);
    Ticket resold = new Ticket();
    resold.setInvoiceId(another.getInvoiceId());
    resold.setTripId(tripId);
    resold.setSeatNumber(1);
    resold.setPrice(new BigDecimal("150000"));
    tickets.save(resold);
    sepay.receive(payload);
    assertEquals(1, tickets.count());
    assertEquals(another.getInvoiceId(), tickets.findAll().get(0).getInvoiceId());
    assertEquals(
        "REVIEW_REQUIRED",
        sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertEquals(0, payments.count());
  }

  private void pollingMode() {
    var environment = new org.springframework.mock.env.MockEnvironment();
    environment.setActiveProfiles("local");
    config.setRuntimeEnvironment(environment);
    config.setConfirmationMode("POLLING");
  }

  private SepayLookupResult result(SepayIpn payload) {
    var order = payload.order();
    return new SepayLookupResult(
        new SepayOrderResponse.Order(
            order.orderInvoiceNumber(),
            order.orderStatus(),
            order.orderAmount(),
            order.orderCurrency(),
            List.of(payload.transaction())),
        null,
        null);
  }

  @Test
  void pollingAndIpnRaceCommitOnlyOnePayment() throws Exception {
    pollingMode();
    var response = booking.bookTickets(actor, request());
    var payload = ipn(response.paymentSessionId());
    var claim = sepay.claimSync(response.paymentSessionId(), actor).orElseThrow();
    race(
        () -> {
          sepay.completeSync(claim, result(payload));
          return true;
        },
        () -> {
          sepay.receive(payload);
          return true;
        });
    assertEquals(1, payments.count());
    assertEquals(2, notifications.count());
    assertEquals("PAID", sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertEquals(2, tickets.count());
  }

  @Test
  void concurrentSyncClaimsDoNotDuplicateOutboundWork() throws Exception {
    pollingMode();
    var response = booking.bookTickets(actor, request());
    var claims =
        race(
            () -> sepay.claimSync(response.paymentSessionId(), actor),
            () -> sepay.claimSync(response.paymentSessionId(), actor));
    assertEquals(
        1, claims.stream().map(value -> (Optional<?>) value).filter(Optional::isPresent).count());
    assertEquals(
        NOW.plusSeconds(15),
        sessions.findById(response.paymentSessionId()).orElseThrow().getNextCheckAt());
  }

  @Test
  void failedSyncCommitRollsBackPaymentAndAuditAndCanRetryAfterLease() {
    pollingMode();
    var response = booking.bookTickets(actor, request());
    var claim = sepay.claimSync(response.paymentSessionId(), actor).orElseThrow();
    var snapshot = result(ipn(response.paymentSessionId()));
    doThrow(new IllegalStateException("storage failure")).when(payments).save(any());
    assertThrows(IllegalStateException.class, () -> sepay.completeSync(claim, snapshot));
    var pending = sessions.findById(response.paymentSessionId()).orElseThrow();
    assertEquals("PENDING", pending.getStatus());
    assertEquals(claim.leaseToken(), pending.getSyncLeaseToken());
    assertNull(pending.getLastCheckedAt());
    assertEquals(0, payments.count());
    assertEquals(0, notifications.count());
    assertFalse(invoices.findById(response.invoiceId()).orElseThrow().getIsPaid());
    reset(payments);
    when(clock.instant()).thenReturn(NOW.plusSeconds(16));
    var retry = sepay.claimSync(response.paymentSessionId(), actor).orElseThrow();
    assertNotEquals(claim.leaseToken(), retry.leaseToken());
    sepay.completeSync(retry, snapshot);
    assertEquals(1, payments.count());
    assertTrue(invoices.findById(response.invoiceId()).orElseThrow().getIsPaid());
  }

  @Test
  void pollingAfterCancelCannotReissueSeats() {
    pollingMode();
    var response = booking.bookTickets(actor, request());
    var snapshot = result(ipn(response.paymentSessionId()));
    var claim = sepay.claimSync(response.paymentSessionId(), actor).orElseThrow();
    sepay.cancelInvoice(response.invoiceId());
    sepay.completeSync(claim, snapshot);
    assertEquals(
        "REVIEW_REQUIRED",
        sessions.findById(response.paymentSessionId()).orElseThrow().getStatus());
    assertEquals(0, tickets.count());
    assertEquals(0, payments.count());
  }

  @Test
  void persistedPollingScheduleCanBeRecoveredWithoutBrowser() {
    pollingMode();
    var response = booking.bookTickets(actor, request());
    assertEquals(
        List.of(response.paymentSessionId()),
        sessions.findDuePollingIds(
            NOW, NOW.minusSeconds(86400), org.springframework.data.domain.PageRequest.of(0, 20)));
    sepay.claimSync(response.paymentSessionId(), actor).orElseThrow();
    assertTrue(
        sessions
            .findDuePollingIds(
                NOW, NOW.minusSeconds(86400), org.springframework.data.domain.PageRequest.of(0, 20))
            .isEmpty());
    assertEquals(
        List.of(response.paymentSessionId()),
        sessions.findDuePollingIds(
            NOW.plusSeconds(16),
            NOW.minusSeconds(86400),
            org.springframework.data.domain.PageRequest.of(0, 20)));
  }

  private List<Object> race(Callable<Object> first, Callable<Object> second) throws Exception {
    CountDownLatch start = new CountDownLatch(1);
    try (ExecutorService pool = Executors.newFixedThreadPool(2)) {
      Future<Object> a =
          pool.submit(
              () -> {
                start.await();
                return first.call();
              });
      Future<Object> b =
          pool.submit(
              () -> {
                start.await();
                return second.call();
              });
      start.countDown();
      return List.of(a.get(20, TimeUnit.SECONDS), b.get(20, TimeUnit.SECONDS));
    }
  }
}
