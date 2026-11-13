package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.dto.invoice.*;
import com.coachbooking.repo.*;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class InvoiceServiceTest {

  private InvoiceRepository invoiceRepository;
  private TicketRepository ticketRepository;
  private PaymentRepository paymentRepository;
  private CustomerRepository customerRepository;
  private EmployeeRepository employeeRepository;
  private TripRepository tripRepository;
  private RouteRepository routeRepository;
  private CurrentUserService currentUserService;
  private SeatStatusService seatStatusService;
  private InvoiceService service;

  private Account adminAccount;
  private Account customerAccount;
  private Customer customer;
  private Employee employee;

  @BeforeEach
  void setUp() {
    invoiceRepository = mock(InvoiceRepository.class);
    ticketRepository = mock(TicketRepository.class);
    paymentRepository = mock(PaymentRepository.class);
    customerRepository = mock(CustomerRepository.class);
    employeeRepository = mock(EmployeeRepository.class);
    tripRepository = mock(TripRepository.class);
    routeRepository = mock(RouteRepository.class);
    currentUserService = mock(CurrentUserService.class);
    seatStatusService = mock(SeatStatusService.class);

    service =
        new InvoiceService(
            invoiceRepository,
            ticketRepository,
            paymentRepository,
            customerRepository,
            employeeRepository,
            tripRepository,
            routeRepository,
            currentUserService,
            seatStatusService);

    adminAccount = new Account();
    adminAccount.setAccountId(1);
    adminAccount.setUsername("admin");

    customerAccount = new Account();
    customerAccount.setAccountId(2);
    customerAccount.setUsername("customer");

    customer = new Customer();
    customer.setCustomerId(10);
    customer.setAccountId(2);
    customer.setFullName("Nguyen Customer");
    customer.setPhone("0901234567");

    employee = new Employee();
    employee.setEmployeeId(20);
    employee.setFullName("Tran Employee");

    when(currentUserService.isCustomer(adminAccount)).thenReturn(false);
    when(currentUserService.isCustomer(customerAccount)).thenReturn(true);
    when(customerRepository.findByAccountId(customerAccount.getAccountId()))
        .thenReturn(Optional.of(customer));
  }

  @Test
  void listWithFilters() {
    Invoice inv1 = new Invoice();
    inv1.setInvoiceId(1);
    inv1.setCustomerId(10);
    inv1.setEmployeeId(20);
    inv1.setCreatedAt(LocalDateTime.of(2026, 5, 10, 10, 0));
    inv1.setTotalAmount(BigDecimal.valueOf(300_000));
    inv1.setIsPaid(true);

    Invoice inv2 = new Invoice();
    inv2.setInvoiceId(2);
    inv2.setCustomerId(10);
    inv2.setCreatedAt(LocalDateTime.of(2026, 5, 12, 10, 0));
    inv2.setTotalAmount(BigDecimal.valueOf(150_000));
    inv2.setIsPaid(false);

    when(invoiceRepository.findAllByOrderByCreatedAtDesc()).thenReturn(List.of(inv2, inv1));
    when(ticketRepository.findByInvoiceIdIn(anyList())).thenReturn(List.of());
    when(paymentRepository.findByInvoiceIdIn(anyList())).thenReturn(List.of());
    when(customerRepository.findAllById(anyList())).thenReturn(List.of(customer));
    when(employeeRepository.findAllById(anyList())).thenReturn(List.of(employee));

    List<InvoiceRow> rows = service.list(LocalDate.of(2026, 5, 1), LocalDate.of(2026, 5, 15), true);
    assertEquals(1, rows.size());
    assertEquals(1, rows.get(0).invoiceId());
    assertEquals("Nguyen Customer", rows.get(0).customerName());
    assertEquals("Tran Employee", rows.get(0).employeeName());

    // list without filter
    List<InvoiceRow> allRows = service.list(null, null, null);
    assertEquals(2, allRows.size());

    // empty list
    when(invoiceRepository.findAllByOrderByCreatedAtDesc()).thenReturn(List.of());
    List<InvoiceRow> emptyRows = service.list(null, null, null);
    assertTrue(emptyRows.isEmpty());
  }

  @Test
  void myInvoicesReturnsCustomerInvoices() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(10);
    inv.setCreatedAt(LocalDateTime.now());
    inv.setTotalAmount(BigDecimal.valueOf(150_000));
    inv.setIsPaid(true);

    when(invoiceRepository.findByCustomerIdOrderByCreatedAtDesc(10)).thenReturn(List.of(inv));
    when(ticketRepository.findByInvoiceIdIn(List.of(1))).thenReturn(List.of());
    when(paymentRepository.findByInvoiceIdIn(List.of(1))).thenReturn(List.of());
    when(customerRepository.findAllById(anyList())).thenReturn(List.of(customer));

    List<InvoiceRow> rows = service.myInvoices(customerAccount);
    assertEquals(1, rows.size());
    assertEquals(1, rows.get(0).invoiceId());

    // customer not found returns empty
    when(customerRepository.findByAccountId(customerAccount.getAccountId()))
        .thenReturn(Optional.empty());
    List<InvoiceRow> noCustRows = service.myInvoices(customerAccount);
    assertTrue(noCustRows.isEmpty());
  }

  @Test
  void detailSuccessForAdminAndCustomer() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(10);
    inv.setEmployeeId(20);
    inv.setCreatedAt(LocalDateTime.now());
    inv.setTotalAmount(BigDecimal.valueOf(150_000));
    inv.setIsPaid(true);

    when(invoiceRepository.findById(1)).thenReturn(Optional.of(inv));
    when(customerRepository.findById(10)).thenReturn(Optional.of(customer));
    when(employeeRepository.findById(20)).thenReturn(Optional.of(employee));

    Ticket ticket = new Ticket();
    ticket.setTicketId(101);
    ticket.setInvoiceId(1);
    ticket.setTripId(50);
    ticket.setSeatNumber(5);
    ticket.setPrice(BigDecimal.valueOf(150_000));
    when(ticketRepository.findByInvoiceId(1)).thenReturn(List.of(ticket));

    Trip trip = new Trip();
    trip.setTripId(50);
    trip.setRouteId(90);
    trip.setDepartureTime(LocalDateTime.now().plusDays(1));
    when(tripRepository.findAllById(List.of(50))).thenReturn(List.of(trip));

    Route route = new Route();
    route.setRouteId(90);
    route.setDeparture("Sài Gòn");
    route.setDestination("Đà Lạt");
    when(routeRepository.findAllById(List.of(90))).thenReturn(List.of(route));

    Payment payment = new Payment();
    payment.setPaymentId(501);
    payment.setInvoiceId(1);
    payment.setAmount(BigDecimal.valueOf(150_000));
    payment.setPaymentDate(LocalDateTime.now());
    payment.setPaymentMethod("Chuyển khoản");
    when(paymentRepository.findByInvoiceId(1)).thenReturn(Optional.of(payment));

    InvoiceDetail detail = service.detail(1, customerAccount);
    assertNotNull(detail);
    assertEquals(1, detail.invoiceId());
    assertEquals("Nguyen Customer", detail.customerName());
    assertEquals("0901234567", detail.customerPhone());
    assertEquals("Tran Employee", detail.employeeName());
    assertEquals(1, detail.tickets().size());
    assertEquals("Sài Gòn", detail.tickets().get(0).departure());
    assertEquals("Đà Lạt", detail.tickets().get(0).destination());
    assertNotNull(detail.payment());
    assertEquals("Chuyển khoản", detail.payment().paymentMethod());
  }

  @Test
  void detailRejectsForbiddenAccess() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(999); // different customer

    when(invoiceRepository.findById(1)).thenReturn(Optional.of(inv));

    ApiException ex = assertThrows(ApiException.class, () -> service.detail(1, customerAccount));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
  }

  @Test
  void detailThrowsNotFoundWhenInvoiceMissing() {
    when(invoiceRepository.findById(99)).thenReturn(Optional.empty());

    ApiException ex = assertThrows(ApiException.class, () -> service.detail(99, adminAccount));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void lookupSuccessAndFailures() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(10);
    inv.setCreatedAt(LocalDateTime.now());
    inv.setTotalAmount(BigDecimal.valueOf(150_000));
    inv.setIsPaid(true);

    when(invoiceRepository.findById(1)).thenReturn(Optional.of(inv));
    when(customerRepository.findById(10)).thenReturn(Optional.of(customer));
    when(ticketRepository.findByInvoiceId(1)).thenReturn(List.of());
    when(paymentRepository.findByInvoiceId(1)).thenReturn(Optional.empty());

    // Valid lookup (matching phone with formatting)
    InvoiceDetail detail = service.lookup(1, "090-123-4567");
    assertNotNull(detail);
    assertEquals(1, detail.invoiceId());

    // Invalid params
    ApiException badReq =
        assertThrows(ApiException.class, () -> service.lookup(null, "0901234567"));
    assertEquals(HttpStatus.BAD_REQUEST, badReq.getStatus());

    // Phone mismatch
    ApiException notFound = assertThrows(ApiException.class, () -> service.lookup(1, "0999999999"));
    assertEquals(HttpStatus.NOT_FOUND, notFound.getStatus());
  }

  @Test
  void paySuccessAndValidation() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(10);
    inv.setIsPaid(false);

    when(invoiceRepository.findById(1)).thenReturn(Optional.of(inv));

    Ticket t1 = new Ticket();
    t1.setPrice(BigDecimal.valueOf(100_000));
    Ticket t2 = new Ticket();
    t2.setPrice(BigDecimal.valueOf(50_000));
    when(ticketRepository.findByInvoiceId(1)).thenReturn(List.of(t1, t2));

    // invalid method
    ApiException badMethod =
        assertThrows(ApiException.class, () -> service.pay(1, "Bitcoin", adminAccount));
    assertEquals(HttpStatus.BAD_REQUEST, badMethod.getStatus());

    // successful pay
    Map<String, String> res = service.pay(1, "Tiền mặt", adminAccount);
    assertEquals("Thanh toán hóa đơn thành công.", res.get("message"));
    verify(paymentRepository).save(any(Payment.class));
    verify(invoiceRepository)
        .save(argThat(i -> i.getIsPaid() && i.getTotalAmount().intValue() == 150_000));

    // already paid throws conflict
    inv.setIsPaid(true);
    ApiException conflict =
        assertThrows(ApiException.class, () -> service.pay(1, "Tiền mặt", adminAccount));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());
  }

  @Test
  void cancelSuccessAndPaidRejection() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(10);
    inv.setIsPaid(false);

    when(invoiceRepository.findById(1)).thenReturn(Optional.of(inv));
    Ticket t = new Ticket();
    t.setTripId(11);
    when(ticketRepository.findByInvoiceId(1)).thenReturn(List.of(t));

    Map<String, String> res = service.cancel(1, adminAccount);
    assertEquals("Hủy hóa đơn thành công.", res.get("message"));
    verify(ticketRepository).deleteByInvoiceId(1);
    verify(invoiceRepository).delete(inv);
    verify(seatStatusService).publishAfterCommit(11);

    // paid invoice cancel throws conflict
    inv.setIsPaid(true);
    ApiException conflict = assertThrows(ApiException.class, () -> service.cancel(1, adminAccount));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());
  }

  @Test
  void cancelTicketSuccessAndDepartedRejection() {
    Invoice inv = new Invoice();
    inv.setInvoiceId(1);
    inv.setCustomerId(10);
    inv.setIsPaid(false);

    Ticket ticket = new Ticket();
    ticket.setTicketId(101);
    ticket.setInvoiceId(1);
    ticket.setTripId(11);
    ticket.setPrice(BigDecimal.valueOf(100_000));

    Trip trip = new Trip();
    trip.setTripId(11);
    trip.setDepartureTime(LocalDateTime.now().plusDays(1));

    when(ticketRepository.findById(101)).thenReturn(Optional.of(ticket));
    when(invoiceRepository.findById(1)).thenReturn(Optional.of(inv));
    when(tripRepository.findById(11)).thenReturn(Optional.of(trip));

    // CASE: remaining tickets exist
    Ticket remainingTicket = new Ticket();
    remainingTicket.setTicketId(102);
    remainingTicket.setPrice(BigDecimal.valueOf(50_000));
    when(ticketRepository.findByInvoiceId(1)).thenReturn(List.of(remainingTicket));

    Map<String, String> res = service.cancelTicket(101, adminAccount);
    assertEquals("Hủy vé thành công.", res.get("message"));
    verify(ticketRepository).delete(ticket);
    verify(seatStatusService).publishAfterCommit(11);
    verify(invoiceRepository).save(argThat(i -> i.getTotalAmount().intValue() == 50_000));

    // CASE: departed trip throws conflict
    trip.setDepartureTime(LocalDateTime.now().minusHours(1));
    ApiException departed =
        assertThrows(ApiException.class, () -> service.cancelTicket(101, adminAccount));
    assertEquals(HttpStatus.CONFLICT, departed.getStatus());

    // CASE: last remaining ticket deletes entire invoice
    trip.setDepartureTime(LocalDateTime.now().plusDays(1));
    when(ticketRepository.findByInvoiceId(1)).thenReturn(List.of());
    Map<String, String> resLast = service.cancelTicket(101, adminAccount);
    assertEquals("Đã hủy vé cuối cùng và xóa hóa đơn.", resLast.get("message"));
    verify(invoiceRepository).delete(inv);
  }
}
