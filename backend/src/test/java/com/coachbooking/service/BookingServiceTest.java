package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyList;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Payment;
import com.coachbooking.domain.SeatHold;
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
import java.util.Optional;
import java.util.UUID;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
import org.springframework.http.HttpStatus;

class BookingServiceTest {

  private static final int TRIP_ID = 11;
  private static final int VEHICLE_ID = 5;
  private static final int VEHICLE_TYPE_ID = 3;
  private static final int CUSTOMER_ID = 22;
  private static final String PHONE = "0901234567";
  private static final BigDecimal SEAT_PRICE = BigDecimal.valueOf(150_000);
  private static final UUID HOLD_TOKEN = UUID.randomUUID();

  private TripRepository tripRepository;
  private VehicleRepository vehicleRepository;
  private VehicleTypeRepository vehicleTypeRepository;
  private CustomerRepository customerRepository;
  private EmployeeRepository employeeRepository;
  private InvoiceRepository invoiceRepository;
  private TicketRepository ticketRepository;
  private PaymentRepository paymentRepository;
  private CurrentUserService currentUserService;
  private SeatHoldRepository seatHoldRepository;
  private SeatStatusService seatStatusService;
  private BookingService service;

  private Account employee;
  private Customer customer;

  @BeforeEach
  void setUp() {
    tripRepository = mock(TripRepository.class);
    vehicleRepository = mock(VehicleRepository.class);
    vehicleTypeRepository = mock(VehicleTypeRepository.class);
    customerRepository = mock(CustomerRepository.class);
    employeeRepository = mock(EmployeeRepository.class);
    invoiceRepository = mock(InvoiceRepository.class);
    ticketRepository = mock(TicketRepository.class);
    paymentRepository = mock(PaymentRepository.class);
    currentUserService = mock(CurrentUserService.class);
    seatHoldRepository = mock(SeatHoldRepository.class);
    seatStatusService = mock(SeatStatusService.class);

    service =
        new BookingService(
            tripRepository,
            vehicleRepository,
            vehicleTypeRepository,
            customerRepository,
            employeeRepository,
            invoiceRepository,
            ticketRepository,
            paymentRepository,
            currentUserService,
            seatHoldRepository,
            seatStatusService);

    employee = account(7, "nhanvien");
    customer = new Customer();
    customer.setCustomerId(CUSTOMER_ID);
    customer.setAccountId(employee.getAccountId());
    customer.setFullName("Nguyen A");
    customer.setPhone(PHONE);
    customer.setDateOfBirth(LocalDate.of(1990, 1, 1));

    when(currentUserService.isCustomer(employee)).thenReturn(false);
    stubTrip();
    stubVehicle();
    stubNoOccupiedSeats();
    when(customerRepository.findByPhone(PHONE)).thenReturn(Optional.of(customer));
    when(invoiceRepository.save(any(Invoice.class)))
        .thenAnswer(
            invocation -> {
              Invoice invoice = invocation.getArgument(0);
              invoice.setInvoiceId(100);
              return invoice;
            });
    when(ticketRepository.saveAllAndFlush(anyList()))
        .thenAnswer(invocation -> invocation.getArgument(0));
  }

  /** CASE: bán vé thành công cho nhân viên, giá vé lấy từ chuyến xe. */
  @Test
  void bookSuccessForEmployee() {
    BookingResponse response = service.bookTickets(employee, request(List.of(2, 1), "Cash"));

    assertNotNull(response);
    assertEquals(100, response.invoiceId());
    assertEquals(2, response.ticketCount());
    assertEquals(SEAT_PRICE.multiply(BigDecimal.valueOf(2)), response.totalAmount());
    assertEquals("Cash", response.paymentMethod());
    assertTrue(response.isPaid());

    ArgumentCaptor<Invoice> invoice = ArgumentCaptor.forClass(Invoice.class);
    verify(invoiceRepository).save(invoice.capture());
    assertEquals(CUSTOMER_ID, invoice.getValue().getCustomerId());
    assertEquals(SEAT_PRICE.multiply(BigDecimal.valueOf(2)), invoice.getValue().getTotalAmount());
    assertTrue(invoice.getValue().getIsPaid());

    verify(ticketRepository).saveAllAndFlush(anyList());
    verify(paymentRepository).save(any(Payment.class));
    verify(seatStatusService, times(1)).publishAfterCommit(TRIP_ID);
  }

  /** CASE: khách hàng đặt vé bằng lượt giữ ghế hợp lệ. */
  @Test
  void bookSuccessForCustomerConsumesHold() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    when(customerRepository.findByAccountId(employee.getAccountId()))
        .thenReturn(Optional.of(customer));
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(hold(1), hold(2)));

    BookingResponse response =
        service.bookTickets(employee, request(List.of(1, 2), "Chuyển khoản"));

    assertEquals(2, response.ticketCount());
    assertEquals("Chuyển khoản", response.paymentMethod());
    verify(seatHoldRepository).deleteByHoldToken(HOLD_TOKEN);
    verify(seatStatusService, times(1)).publishAfterCommit(TRIP_ID);
  }

  /** CASE: chuyến xe không tồn tại. */
  @Test
  void tripNotFoundThrowsNotFound() {
    when(tripRepository.findByIdForUpdate(99)).thenReturn(Optional.empty());

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(99, List.of(1), "Cash")));

    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: xe của chuyến không tồn tại. */
  @Test
  void vehicleNotFoundThrowsNotFound() {
    when(vehicleRepository.findById(VEHICLE_ID)).thenReturn(Optional.empty());

    ApiException ex =
        assertThrows(
            ApiException.class, () -> service.bookTickets(employee, request(List.of(1), "Cash")));

    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: loại xe không tồn tại. */
  @Test
  void vehicleTypeNotFoundThrowsNotFound() {
    when(vehicleTypeRepository.findById(VEHICLE_TYPE_ID)).thenReturn(Optional.empty());

    ApiException ex =
        assertThrows(
            ApiException.class, () -> service.bookTickets(employee, request(List.of(1), "Cash")));

    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: số ghế vượt quá sức chứa của xe. */
  @Test
  void seatsExceedingCapacityThrowBadRequest() {
    ApiException ex =
        assertThrows(
            ApiException.class, () -> service.bookTickets(employee, request(List.of(41), "Cash")));

    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: ghế đã được đặt trước đó. */
  @Test
  void alreadyBookedSeatsThrowConflict() {
    when(ticketRepository.findSeatNumbersByTripId(TRIP_ID)).thenReturn(List.of(1));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1, 2), "Cash")));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: ghế đang được khách khác giữ. */
  @Test
  void heldSeatsThrowConflict() {
    when(seatHoldRepository.findByTripIdAndExpiresAtAfterOrderBySeatNumber(any(), any()))
        .thenReturn(List.of(hold(2)));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1, 2), "Cash")));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: phương thức thanh toán không hợp lệ. */
  @Test
  void invalidPaymentMethodThrowsBadRequest() {
    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1), "InvalidMethod")));

    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
    verify(ticketRepository, never()).saveAllAndFlush(anyList());
  }

  /** CASE: khách hàng không được chọn phương thức khác chuyển khoản. */
  @Test
  void customerWithCashPaymentThrowsBadRequest() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    when(customerRepository.findByAccountId(employee.getAccountId()))
        .thenReturn(Optional.of(customer));
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(hold(1)));

    ApiException ex =
        assertThrows(
            ApiException.class, () -> service.bookTickets(employee, request(List.of(1), "Cash")));

    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: khách hàng chưa giữ ghế. */
  @Test
  void customerWithoutHoldTokenThrowsConflict() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);

    ApiException ex =
        assertThrows(
            ApiException.class,
            () ->
                service.bookTickets(
                    employee, request(TRIP_ID, List.of(1), "Chuyển khoản", (UUID) null)));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
    verify(invoiceRepository, never()).save(any());
  }

  /** CASE: lượt giữ ghế của tài khoản khác. */
  @Test
  void holdOfAnotherAccountThrowsForbidden() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    SeatHold another = hold(1);
    another.setAccountId(99);
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(another));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1), "Chuyển khoản")));

    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
  }

  /** CASE: lượt giữ ghế không khớp danh sách ghế. */
  @Test
  void holdSeatsMismatchThrowsConflict() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(hold(1), hold(3)));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1, 2), "Chuyển khoản")));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
  }

  /** CASE: lượt giữ ghế đã hết hạn. */
  @Test
  void expiredHoldThrowsConflict() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    SeatHold expired = hold(1);
    expired.setExpiresAt(LocalDateTime.now().minusMinutes(1));
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(expired));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1), "Chuyển khoản")));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
  }

  /** CASE: lượt giữ ghế thuộc chuyến khác. */
  @Test
  void holdOfAnotherTripThrowsConflict() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    SeatHold otherTrip = hold(1);
    otherTrip.setTripId(77);
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(otherTrip));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(1), "Chuyển khoản")));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
  }

  /** CASE: danh sách ghế rỗng hoặc trùng. */
  @Test
  void invalidSeatNumbersThrowBadRequest() {
    ApiException empty =
        assertThrows(
            ApiException.class, () -> service.bookTickets(employee, request(List.of(), "Cash")));
    assertEquals(HttpStatus.BAD_REQUEST, empty.getStatus());

    ApiException duplicate =
        assertThrows(
            ApiException.class,
            () -> service.bookTickets(employee, request(List.of(2, 2), "Cash")));
    assertEquals(HttpStatus.BAD_REQUEST, duplicate.getStatus());
  }

  /** CASE: chuyến đã khởi hành thì không thể bán vé. */
  @Test
  void departedTripThrowsConflict() {
    trip().setDepartureTime(LocalDateTime.now().minusHours(1));

    ApiException ex =
        assertThrows(
            ApiException.class, () -> service.bookTickets(employee, request(List.of(1), "Cash")));

    assertEquals(HttpStatus.CONFLICT, ex.getStatus());
  }

  /**
   * CASE: lỗi khi lưu ticket phải dừng toàn bộ thao tác, không phát sinh payment và không phát sự
   * kiện cập nhật ghế.
   */
  @Test
  void ticketSaveFailureAbortsBooking() {
    when(ticketRepository.saveAllAndFlush(anyList()))
        .thenThrow(new IllegalStateException("DB error"));

    IllegalStateException ex =
        assertThrows(
            IllegalStateException.class,
            () -> service.bookTickets(employee, request(List.of(1, 2), "Cash")));

    assertEquals("DB error", ex.getMessage());
    verify(invoiceRepository).save(any(Invoice.class));
    verify(paymentRepository, never()).save(any(Payment.class));
    verify(seatStatusService, never()).publishAfterCommit(any());
  }

  /** CASE: lỗi khi lưu invoice phải chặn mọi bước ghi tiếp theo. */
  @Test
  void invoiceSaveFailureAbortsBooking() {
    when(invoiceRepository.save(any(Invoice.class)))
        .thenThrow(new IllegalStateException("DB error"));

    assertThrows(
        IllegalStateException.class,
        () -> service.bookTickets(employee, request(List.of(1, 2), "Cash")));

    verify(ticketRepository, never()).saveAllAndFlush(anyList());
    verify(paymentRepository, never()).save(any(Payment.class));
    verify(seatStatusService, never()).publishAfterCommit(any());
  }

  /** CASE: lỗi khi lưu payment phải chặn publish và giữ nguyên lượt giữ ghế. */
  @Test
  void paymentSaveFailureAbortsBooking() {
    when(currentUserService.isCustomer(employee)).thenReturn(true);
    when(customerRepository.findByAccountId(employee.getAccountId()))
        .thenReturn(Optional.of(customer));
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(HOLD_TOKEN))
        .thenReturn(List.of(hold(1)));
    when(paymentRepository.save(any(Payment.class)))
        .thenThrow(new IllegalStateException("DB error"));

    assertThrows(
        IllegalStateException.class,
        () -> service.bookTickets(employee, request(List.of(1), "Chuyển khoản")));

    verify(seatHoldRepository, never()).deleteByHoldToken(any());
    verify(seatStatusService, never()).publishAfterCommit(any());
  }

  /** CASE: đặt vé "thanh toán sau" không tạo payment. */
  @Test
  void payLaterCreatesNoPayment() {
    BookingResponse response = service.bookTickets(employee, request(List.of(1), "Thanh toán sau"));

    assertFalse(response.isPaid());
    assertEquals("Thanh toán sau", response.paymentMethod());
    verify(paymentRepository, never()).save(any(Payment.class));
    verify(seatStatusService, times(1)).publishAfterCommit(TRIP_ID);
  }

  private void stubTrip() {
    Trip trip = new Trip();
    trip.setTripId(TRIP_ID);
    trip.setVehicleId(VEHICLE_ID);
    trip.setRouteId(1);
    trip.setDriverId(1);
    trip.setDepartureTime(LocalDateTime.now().plusDays(1));
    trip.setActualPrice(SEAT_PRICE);
    when(tripRepository.findByIdForUpdate(TRIP_ID)).thenReturn(Optional.of(trip));
  }

  private Trip trip() {
    return tripRepository.findByIdForUpdate(TRIP_ID).orElseThrow();
  }

  private void stubVehicle() {
    Vehicle vehicle = new Vehicle();
    vehicle.setVehicleId(VEHICLE_ID);
    vehicle.setCompanyId(1);
    vehicle.setTypeId(VEHICLE_TYPE_ID);
    vehicle.setLicensePlate("51B-12345");
    when(vehicleRepository.findById(VEHICLE_ID)).thenReturn(Optional.of(vehicle));

    VehicleType vehicleType = new VehicleType();
    vehicleType.setTypeId(VEHICLE_TYPE_ID);
    vehicleType.setTypeName("Giường nằm 40 chỗ");
    vehicleType.setSeatCapacity(40);
    when(vehicleTypeRepository.findById(VEHICLE_TYPE_ID)).thenReturn(Optional.of(vehicleType));
  }

  private void stubNoOccupiedSeats() {
    when(ticketRepository.findSeatNumbersByTripId(TRIP_ID)).thenReturn(List.of());
    when(seatHoldRepository.findByTripIdAndExpiresAtAfterOrderBySeatNumber(any(), any()))
        .thenReturn(List.of());
  }

  private SeatHold hold(int seatNumber) {
    SeatHold hold = new SeatHold();
    hold.setHoldToken(HOLD_TOKEN);
    hold.setTripId(TRIP_ID);
    hold.setAccountId(employee.getAccountId());
    hold.setSeatNumber(seatNumber);
    hold.setExpiresAt(LocalDateTime.now().plusMinutes(10));
    return hold;
  }

  private Account account(int accountId, String username) {
    Account account = new Account();
    account.setAccountId(accountId);
    account.setUsername(username);
    account.setRoleId(1);
    return account;
  }

  private BookingRequest request(List<Integer> seats, String paymentMethod) {
    return request(TRIP_ID, seats, paymentMethod, HOLD_TOKEN);
  }

  private BookingRequest request(int tripId, List<Integer> seats, String paymentMethod) {
    return request(tripId, seats, paymentMethod, HOLD_TOKEN);
  }

  private BookingRequest request(
      int tripId, List<Integer> seats, String paymentMethod, UUID holdToken) {
    return new BookingRequest(
        tripId, seats, "Nguyen A", PHONE, LocalDate.of(1990, 1, 1), true, paymentMethod, holdToken);
  }
}
