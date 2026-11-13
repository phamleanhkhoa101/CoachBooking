package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.doThrow;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.reset;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Trip;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.dto.booking.BookingRequest;
import com.coachbooking.dto.booking.BookingResponse;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.InvoiceRepository;
import com.coachbooking.repo.PaymentRepository;
import com.coachbooking.repo.SeatHoldRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
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
 * Kiểm chứng rollback trên PostgreSQL-compatible H2: mỗi lần gọi service chạy trong transaction
 * thật (test không bọc transaction) nên có thể quan sát dữ liệu đã commit hay chưa.
 */
@DataJpaTest(
    properties = {"spring.flyway.enabled=false", "spring.jpa.hibernate.ddl-auto=create-drop"})
@Import({BookingService.class, BookingServiceTransactionTest.MocksConfig.class})
@Transactional(propagation = Propagation.NOT_SUPPORTED)
class BookingServiceTransactionTest {

  private static final String PHONE = "0901234567";
  private static final BigDecimal SEAT_PRICE = BigDecimal.valueOf(150_000);

  @TestConfiguration
  static class MocksConfig {

    @Bean
    CurrentUserService currentUserService() {
      return mock(CurrentUserService.class);
    }

    @Bean
    SeatStatusService seatStatusService() {
      return mock(SeatStatusService.class);
    }
  }

  @Autowired private BookingService service;
  @Autowired private CurrentUserService currentUserService;
  @Autowired private SeatStatusService seatStatusService;
  @Autowired private TripRepository tripRepository;
  @Autowired private VehicleRepository vehicleRepository;
  @Autowired private VehicleTypeRepository vehicleTypeRepository;
  @Autowired private CustomerRepository customerRepository;
  @Autowired private EmployeeRepository employeeRepository;
  @Autowired private PaymentRepository paymentRepository;
  @Autowired private SeatHoldRepository seatHoldRepository;

  @MockitoSpyBean private InvoiceRepository invoiceRepository;
  @MockitoSpyBean private TicketRepository ticketRepository;

  private Account employee;
  private Integer tripId;

  @BeforeEach
  void setUp() {
    reset(currentUserService, seatStatusService);
    paymentRepository.deleteAll();
    ticketRepository.deleteAll();
    invoiceRepository.deleteAll();
    tripRepository.deleteAll();
    vehicleRepository.deleteAll();
    vehicleTypeRepository.deleteAll();
    customerRepository.deleteAll();

    VehicleType vehicleType = new VehicleType();
    vehicleType.setTypeName("Giường nằm 40 chỗ");
    vehicleType.setSeatCapacity(40);
    vehicleType.setIsActive(true);
    vehicleType = vehicleTypeRepository.save(vehicleType);

    Vehicle vehicle = new Vehicle();
    vehicle.setCompanyId(1);
    vehicle.setTypeId(vehicleType.getTypeId());
    vehicle.setLicensePlate("51B-12345");
    vehicle.setIsActive(true);
    vehicle = vehicleRepository.save(vehicle);

    Trip trip = new Trip();
    trip.setVehicleId(vehicle.getVehicleId());
    trip.setRouteId(1);
    trip.setDriverId(1);
    trip.setDepartureTime(LocalDateTime.now().plusDays(1));
    trip.setActualPrice(SEAT_PRICE);
    tripId = tripRepository.save(trip).getTripId();

    Customer customer = new Customer();
    customer.setFullName("Nguyen A");
    customer.setPhone(PHONE);
    customer.setDateOfBirth(LocalDate.of(1990, 1, 1));
    customer.setGender(false);
    customerRepository.save(customer);

    employee = new Account();
    employee.setAccountId(7);
    employee.setUsername("nhanvien");
    employee.setRoleId(1);
    when(currentUserService.isCustomer(employee)).thenReturn(false);
  }

  @Test
  void bookingIsCommittedToDatabase() {
    BookingResponse response = service.bookTickets(employee, request(List.of(1, 2), "Cash"));

    assertEquals(
        0,
        SEAT_PRICE.multiply(BigDecimal.valueOf(2)).compareTo(response.totalAmount()),
        "Tổng tiền phải bằng giá vé nhân số ghế");
    assertEquals(1, invoiceRepository.count());
    assertEquals(2, ticketRepository.count());
    assertEquals(1, paymentRepository.count());
    assertEquals(List.of(1, 2), ticketRepository.findSeatNumbersByTripId(tripId));
    assertTrue(invoiceRepository.findAll().get(0).getIsPaid());
    verify(seatStatusService, times(1)).publishAfterCommit(tripId);
  }

  @Test
  void ticketInsertFailureRollsBackInvoiceAndPayment() {
    doThrow(new IllegalStateException("ticket insert failed"))
        .when(ticketRepository)
        .saveAllAndFlush(any());

    assertThrows(
        IllegalStateException.class,
        () -> service.bookTickets(employee, request(List.of(1, 2), "Cash")));

    assertEquals(0, invoiceRepository.count(), "Invoice phải bị rollback");
    assertEquals(0, ticketRepository.count(), "Ticket phải bị rollback");
    assertEquals(0, paymentRepository.count(), "Payment phải bị rollback");
    assertEquals(0, seatHoldRepository.count());
    verify(seatStatusService, never()).publishAfterCommit(any());
  }

  @Test
  void invoiceInsertFailurePersistsNothing() {
    doThrow(new IllegalStateException("invoice insert failed")).when(invoiceRepository).save(any());

    assertThrows(
        IllegalStateException.class,
        () -> service.bookTickets(employee, request(List.of(1), "Cash")));

    assertEquals(0, invoiceRepository.count());
    assertEquals(0, ticketRepository.count());
    assertEquals(0, paymentRepository.count());
    verify(seatStatusService, never()).publishAfterCommit(any());
  }

  private BookingRequest request(List<Integer> seats, String paymentMethod) {
    return new BookingRequest(
        tripId, seats, "Nguyen A", PHONE, LocalDate.of(1990, 1, 1), false, paymentMethod, null);
  }
}
