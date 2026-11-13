package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.dto.hold.SeatStatusResponse;
import com.coachbooking.dto.trip.SeatMapResponse;
import com.coachbooking.dto.trip.TripRequest;
import com.coachbooking.repo.*;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class TripServiceTest {

  private TripRepository tripRepository;
  private VehicleRepository vehicleRepository;
  private VehicleTypeRepository vehicleTypeRepository;
  private RouteRepository routeRepository;
  private DriverRepository driverRepository;
  private TicketPriceRepository ticketPriceRepository;
  private TicketRepository ticketRepository;
  private SeatStatusService seatStatusService;
  private TripService service;

  private Vehicle vehicle;
  private VehicleType vehicleType;
  private Route route;
  private Driver driver;
  private TicketPrice ticketPrice;
  private Trip trip;

  @BeforeEach
  void setUp() {
    tripRepository = mock(TripRepository.class);
    vehicleRepository = mock(VehicleRepository.class);
    vehicleTypeRepository = mock(VehicleTypeRepository.class);
    routeRepository = mock(RouteRepository.class);
    driverRepository = mock(DriverRepository.class);
    ticketPriceRepository = mock(TicketPriceRepository.class);
    ticketRepository = mock(TicketRepository.class);
    seatStatusService = mock(SeatStatusService.class);

    service =
        new TripService(
            tripRepository,
            vehicleRepository,
            vehicleTypeRepository,
            routeRepository,
            driverRepository,
            ticketPriceRepository,
            ticketRepository,
            seatStatusService);

    vehicleType = new VehicleType();
    vehicleType.setTypeId(1);
    vehicleType.setTypeName("Giường nằm 40 chỗ");
    vehicleType.setSeatCapacity(40);

    vehicle = new Vehicle();
    vehicle.setVehicleId(10);
    vehicle.setCompanyId(5);
    vehicle.setTypeId(1);

    route = new Route();
    route.setRouteId(20);
    route.setDeparture("Sài Gòn");
    route.setDestination("Nha Trang");
    route.setEstimatedDuration(LocalTime.of(8, 0));

    driver = new Driver();
    driver.setDriverId(30);

    ticketPrice = new TicketPrice();
    ticketPrice.setPrice(BigDecimal.valueOf(250_000));

    trip = new Trip();
    trip.setTripId(100);
    trip.setVehicleId(10);
    trip.setRouteId(20);
    trip.setDriverId(30);
    trip.setDepartureTime(LocalDateTime.now().plusDays(2));
    trip.setActualPrice(BigDecimal.valueOf(250_000));

    when(tripRepository.findById(100)).thenReturn(Optional.of(trip));
    when(vehicleRepository.findById(10)).thenReturn(Optional.of(vehicle));
    when(vehicleTypeRepository.findById(1)).thenReturn(Optional.of(vehicleType));
    when(routeRepository.findById(20)).thenReturn(Optional.of(route));
    when(driverRepository.findById(30)).thenReturn(Optional.of(driver));
    when(ticketPriceRepository.findByCompanyIdAndTypeIdAndRouteId(5, 1, 20))
        .thenReturn(Optional.of(ticketPrice));
  }

  @Test
  void listAllReturnsRepositoryRows() {
    TripRepository.TripRow row = mock(TripRepository.TripRow.class);
    when(tripRepository.findAllRows()).thenReturn(List.of(row));

    List<TripRepository.TripRow> result = service.listAll();
    assertEquals(1, result.size());
  }

  @Test
  void searchWithFilters() {
    TripRepository.TripRow r1 = mock(TripRepository.TripRow.class);
    when(r1.getActualPrice()).thenReturn(BigDecimal.valueOf(200_000));
    when(r1.getDepartureTime()).thenReturn(LocalDateTime.now().plusDays(1).withHour(8));

    TripRepository.TripRow r2 = mock(TripRepository.TripRow.class);
    when(r2.getActualPrice()).thenReturn(BigDecimal.valueOf(250_000));
    when(r2.getDepartureTime()).thenReturn(LocalDateTime.now().plusDays(1).withHour(20));

    when(tripRepository.search(any(), any(), any(), any())).thenReturn(List.of(r1, r2));

    List<TripRepository.TripRow> results =
        service.search("Sài Gòn", "Nha Trang", LocalDate.now().plusDays(1), 6, 12);
    assertEquals(1, results.size());
    assertEquals(8, results.get(0).getDepartureTime().getHour());

    // invalid hour range throws
    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.search("Sài Gòn", "Nha Trang", LocalDate.now().plusDays(1), 15, 10));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
  }

  @Test
  void seatMapReturnsExpectedDetails() {
    when(ticketRepository.findSeatNumbersByTripId(100)).thenReturn(List.of(1, 2));
    when(seatStatusService.snapshot(100))
        .thenReturn(new SeatStatusResponse(100, List.of(1, 2), List.of(3), LocalDateTime.now()));

    SeatMapResponse map = service.seatMap(100);
    assertNotNull(map);
    assertEquals(100, map.tripId());
    assertEquals("Giường nằm 40 chỗ", map.typeName());
    assertEquals(40, map.seatCapacity());
    assertEquals(List.of(1, 2), map.bookedSeats());
    assertEquals(List.of(3), map.heldSeats());
    assertEquals("Sài Gòn", map.departure());
    assertEquals("Nha Trang", map.destination());
  }

  @Test
  void createSuccessAndValidation() {
    TripRequest req = new TripRequest(10, 20, 30, LocalDateTime.now().plusDays(1));
    when(tripRepository.save(any(Trip.class))).thenAnswer(i -> i.getArgument(0));

    Trip created = service.create(req);
    assertNotNull(created);
    assertEquals(10, created.getVehicleId());
    assertEquals(20, created.getRouteId());
    assertEquals(30, created.getDriverId());
    assertEquals(BigDecimal.valueOf(250_000), created.getActualPrice());

    // vehicle conflict throws
    when(tripRepository.existsByVehicleIdAndDepartureTimeBetween(any(), any(), any()))
        .thenReturn(true);
    ApiException conflict = assertThrows(ApiException.class, () -> service.create(req));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());

    // departure in past throws
    TripRequest pastReq = new TripRequest(10, 20, 30, LocalDateTime.now().minusDays(1));
    ApiException badReq = assertThrows(ApiException.class, () -> service.create(pastReq));
    assertEquals(HttpStatus.BAD_REQUEST, badReq.getStatus());
  }

  @Test
  void updateSuccessAndRestrictionsWhenTicketsExist() {
    LocalDateTime newDep = LocalDateTime.now().plusDays(2);
    TripRequest updateReq = new TripRequest(10, 20, 30, newDep);
    when(tripRepository.save(any(Trip.class))).thenAnswer(i -> i.getArgument(0));

    Trip updated = service.update(100, updateReq);
    assertNotNull(updated);

    // Tickets exist: changing driver is allowed
    when(ticketRepository.existsByTripId(100)).thenReturn(true);
    Driver d2 = new Driver();
    d2.setDriverId(31);
    when(driverRepository.findById(31)).thenReturn(Optional.of(d2));
    TripRequest changeDriverReq = new TripRequest(10, 20, 31, trip.getDepartureTime());
    Trip updatedDriver = service.update(100, changeDriverReq);
    assertEquals(31, updatedDriver.getDriverId());

    // Tickets exist: changing vehicle or route throws conflict
    TripRequest changeVehicleReq = new TripRequest(11, 20, 30, trip.getDepartureTime());
    Vehicle v2 = new Vehicle();
    v2.setVehicleId(11);
    v2.setCompanyId(5);
    v2.setTypeId(1);
    when(vehicleRepository.findById(11)).thenReturn(Optional.of(v2));
    ApiException coreConflict =
        assertThrows(ApiException.class, () -> service.update(100, changeVehicleReq));
    assertEquals(HttpStatus.CONFLICT, coreConflict.getStatus());
  }

  @Test
  void deleteSuccessAndRejectionWhenTicketsExist() {
    when(ticketRepository.existsByTripId(100)).thenReturn(false);
    Map<String, String> res = service.delete(100);
    assertEquals("Xóa chuyến xe thành công.", res.get("message"));
    verify(tripRepository).delete(trip);

    // tickets exist throws conflict
    when(ticketRepository.existsByTripId(100)).thenReturn(true);
    ApiException conflict = assertThrows(ApiException.class, () -> service.delete(100));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());
  }
}
