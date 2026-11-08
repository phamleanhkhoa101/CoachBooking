package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Payment;
import com.coachbooking.domain.Route;
import com.coachbooking.domain.Ticket;
import com.coachbooking.domain.Trip;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.InvoiceRepository;
import com.coachbooking.repo.PaymentRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.function.Function;
import java.util.stream.Collectors;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class InvoiceService {

  private static final Set<String> VALID_PAYMENT_METHODS =
      Set.of("Tiền mặt", "Chuyển khoản", "Thẻ ATM", "Cash", "Bank Transfer", "ATM Card");

  private final InvoiceRepository invoiceRepository;
  private final TicketRepository ticketRepository;
  private final PaymentRepository paymentRepository;
  private final CustomerRepository customerRepository;
  private final EmployeeRepository employeeRepository;
  private final TripRepository tripRepository;
  private final RouteRepository routeRepository;
  private final CurrentUserService currentUserService;

  public InvoiceService(
      InvoiceRepository invoiceRepository,
      TicketRepository ticketRepository,
      PaymentRepository paymentRepository,
      CustomerRepository customerRepository,
      EmployeeRepository employeeRepository,
      TripRepository tripRepository,
      RouteRepository routeRepository,
      CurrentUserService currentUserService) {
    this.invoiceRepository = invoiceRepository;
    this.ticketRepository = ticketRepository;
    this.paymentRepository = paymentRepository;
    this.customerRepository = customerRepository;
    this.employeeRepository = employeeRepository;
    this.tripRepository = tripRepository;
    this.routeRepository = routeRepository;
    this.currentUserService = currentUserService;
  }

  public List<InvoiceRow> list(LocalDate fromDate, LocalDate toDate, Boolean isPaid) {
    List<Invoice> invoices =
        invoiceRepository.findAllByOrderByCreatedAtDesc().stream()
            .filter(invoice -> isPaid == null || isPaid.equals(invoice.getIsPaid()))
            .filter(
                invoice ->
                    fromDate == null || !invoice.getCreatedAt().toLocalDate().isBefore(fromDate))
            .filter(
                invoice -> toDate == null || !invoice.getCreatedAt().toLocalDate().isAfter(toDate))
            .toList();
    return toRows(invoices);
  }

  public List<InvoiceRow> myInvoices(Account account) {
    Customer customer = customerRepository.findByAccountId(account.getAccountId()).orElse(null);
    if (customer == null) {
      return List.of();
    }
    return toRows(invoiceRepository.findByCustomerIdOrderByCreatedAtDesc(customer.getCustomerId()));
  }

  public InvoiceDetail detail(Integer invoiceId, Account account) {
    Invoice invoice = findInvoice(invoiceId);
    requireOwnership(account, invoice);

    Customer customer =
        invoice.getCustomerId() == null
            ? null
            : customerRepository.findById(invoice.getCustomerId()).orElse(null);
    Employee employee =
        invoice.getEmployeeId() == null
            ? null
            : employeeRepository.findById(invoice.getEmployeeId()).orElse(null);

    List<Ticket> tickets = ticketRepository.findByInvoiceId(invoiceId);
    Map<Integer, Trip> tripMap = loadTrips(tickets);
    Map<Integer, Route> routeMap = loadRoutes(tripMap.values());
    List<TicketRow> ticketRows =
        tickets.stream()
            .map(ticket -> toTicketRow(ticket, tripMap.get(ticket.getTripId()), routeMap))
            .toList();
    Payment payment = paymentRepository.findByInvoiceId(invoiceId).orElse(null);

    return new InvoiceDetail(
        invoice.getInvoiceId(),
        customer == null ? null : customer.getFullName(),
        customer == null ? null : customer.getPhone(),
        employee == null ? null : employee.getFullName(),
        invoice.getCreatedAt(),
        invoice.getTotalAmount(),
        invoice.getIsPaid(),
        ticketRows,
        payment == null
            ? null
            : new PaymentRow(
                payment.getPaymentId(),
                payment.getAmount(),
                payment.getPaymentDate(),
                payment.getPaymentMethod()));
  }

  @Transactional
  public Map<String, String> pay(Integer invoiceId, String paymentMethod, Account account) {
    if (Validators.isBlank(paymentMethod)
        || !VALID_PAYMENT_METHODS.contains(paymentMethod.trim())) {
      throw ApiException.badRequest("Phương thức thanh toán không hợp lệ.");
    }
    Invoice invoice = findInvoice(invoiceId);
    requireOwnership(account, invoice);
    if (Boolean.TRUE.equals(invoice.getIsPaid())) {
      throw ApiException.conflict("Hóa đơn đã được thanh toán.");
    }
    List<Ticket> tickets = ticketRepository.findByInvoiceId(invoiceId);
    BigDecimal totalAmount =
        tickets.stream().map(Ticket::getPrice).reduce(BigDecimal.ZERO, BigDecimal::add);

    Payment payment = new Payment();
    payment.setInvoiceId(invoiceId);
    payment.setAmount(totalAmount);
    payment.setPaymentDate(LocalDateTime.now());
    payment.setPaymentMethod(paymentMethod.trim());
    paymentRepository.save(payment);

    invoice.setTotalAmount(totalAmount);
    invoice.setIsPaid(true);
    invoiceRepository.save(invoice);
    return Map.of("message", "Thanh toán hóa đơn thành công.");
  }

  @Transactional
  public Map<String, String> cancel(Integer invoiceId, Account account) {
    Invoice invoice = findInvoice(invoiceId);
    requireOwnership(account, invoice);
    if (Boolean.TRUE.equals(invoice.getIsPaid())) {
      throw ApiException.conflict("Không thể hủy hóa đơn đã thanh toán.");
    }
    ticketRepository.deleteByInvoiceId(invoiceId);
    invoiceRepository.delete(invoice);
    return Map.of("message", "Hủy hóa đơn thành công.");
  }

  @Transactional
  public Map<String, String> cancelTicket(Integer ticketId, Account account) {
    Ticket ticket =
        ticketRepository
            .findById(ticketId)
            .orElseThrow(() -> ApiException.notFound("Vé không tồn tại."));
    Invoice invoice = findInvoice(ticket.getInvoiceId());
    requireOwnership(account, invoice);
    if (Boolean.TRUE.equals(invoice.getIsPaid())) {
      throw ApiException.conflict("Không thể hủy vé của hóa đơn đã thanh toán.");
    }
    Trip trip =
        tripRepository
            .findById(ticket.getTripId())
            .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    if (!trip.getDepartureTime().isAfter(LocalDateTime.now())) {
      throw ApiException.conflict("Chuyến xe đã khởi hành, không thể hủy vé.");
    }

    ticketRepository.delete(ticket);
    ticketRepository.flush();

    List<Ticket> remaining = ticketRepository.findByInvoiceId(invoice.getInvoiceId());
    if (remaining.isEmpty()) {
      invoiceRepository.delete(invoice);
      return Map.of("message", "Đã hủy vé cuối cùng và xóa hóa đơn.");
    }
    BigDecimal totalAmount =
        remaining.stream().map(Ticket::getPrice).reduce(BigDecimal.ZERO, BigDecimal::add);
    invoice.setTotalAmount(totalAmount);
    invoiceRepository.save(invoice);
    return Map.of("message", "Hủy vé thành công.");
  }

  private List<InvoiceRow> toRows(List<Invoice> invoices) {
    if (invoices.isEmpty()) {
      return List.of();
    }
    List<Integer> ids = invoices.stream().map(Invoice::getInvoiceId).toList();
    Map<Integer, Long> ticketCounts =
        ticketRepository.findByInvoiceIdIn(ids).stream()
            .collect(Collectors.groupingBy(Ticket::getInvoiceId, Collectors.counting()));
    Map<Integer, Payment> payments =
        paymentRepository.findByInvoiceIdIn(ids).stream()
            .collect(Collectors.toMap(Payment::getInvoiceId, Function.identity()));
    Map<Integer, String> customerNames =
        loadNames(
            invoices.stream()
                .map(Invoice::getCustomerId)
                .filter(java.util.Objects::nonNull)
                .toList(),
            customerRepository.findAllById(
                invoices.stream()
                    .map(Invoice::getCustomerId)
                    .filter(java.util.Objects::nonNull)
                    .toList()),
            Customer::getCustomerId,
            Customer::getFullName);
    Map<Integer, String> employeeNames =
        loadNames(
            invoices.stream()
                .map(Invoice::getEmployeeId)
                .filter(java.util.Objects::nonNull)
                .toList(),
            employeeRepository.findAllById(
                invoices.stream()
                    .map(Invoice::getEmployeeId)
                    .filter(java.util.Objects::nonNull)
                    .toList()),
            Employee::getEmployeeId,
            Employee::getFullName);

    return invoices.stream()
        .map(
            invoice ->
                new InvoiceRow(
                    invoice.getInvoiceId(),
                    invoice.getCustomerId() == null
                        ? null
                        : customerNames.get(invoice.getCustomerId()),
                    invoice.getEmployeeId() == null
                        ? null
                        : employeeNames.get(invoice.getEmployeeId()),
                    invoice.getCreatedAt(),
                    invoice.getTotalAmount(),
                    invoice.getIsPaid(),
                    ticketCounts.getOrDefault(invoice.getInvoiceId(), 0L),
                    payments.containsKey(invoice.getInvoiceId())
                        ? payments.get(invoice.getInvoiceId()).getPaymentMethod()
                        : null))
        .toList();
  }

  private <T> Map<Integer, String> loadNames(
      List<Integer> ids,
      Collection<T> items,
      Function<T, Integer> idGetter,
      Function<T, String> nameGetter) {
    if (ids.isEmpty()) {
      return Map.of();
    }
    return items.stream().collect(Collectors.toMap(idGetter, nameGetter));
  }

  private Map<Integer, Trip> loadTrips(List<Ticket> tickets) {
    List<Integer> ids = tickets.stream().map(Ticket::getTripId).distinct().toList();
    if (ids.isEmpty()) {
      return Map.of();
    }
    return tripRepository.findAllById(ids).stream()
        .collect(Collectors.toMap(Trip::getTripId, Function.identity()));
  }

  private Map<Integer, Route> loadRoutes(Collection<Trip> trips) {
    List<Integer> ids = trips.stream().map(Trip::getRouteId).distinct().toList();
    if (ids.isEmpty()) {
      return Map.of();
    }
    return routeRepository.findAllById(ids).stream()
        .collect(Collectors.toMap(Route::getRouteId, Function.identity()));
  }

  private TicketRow toTicketRow(Ticket ticket, Trip trip, Map<Integer, Route> routeMap) {
    Route route = trip == null ? null : routeMap.get(trip.getRouteId());
    return new TicketRow(
        ticket.getTicketId(),
        ticket.getTripId(),
        route == null ? null : route.getDeparture(),
        route == null ? null : route.getDestination(),
        trip == null ? null : trip.getDepartureTime(),
        ticket.getSeatNumber(),
        ticket.getPrice());
  }

  private Invoice findInvoice(Integer invoiceId) {
    return invoiceRepository
        .findById(invoiceId)
        .orElseThrow(() -> ApiException.notFound("Hóa đơn không tồn tại."));
  }

  private void requireOwnership(Account account, Invoice invoice) {
    if (!currentUserService.isCustomer(account)) {
      return;
    }
    Integer customerId =
        customerRepository
            .findByAccountId(account.getAccountId())
            .map(Customer::getCustomerId)
            .orElse(null);
    if (customerId == null || !customerId.equals(invoice.getCustomerId())) {
      throw ApiException.forbidden("Bạn không có quyền thao tác trên hóa đơn này.");
    }
  }

  public record InvoiceRow(
      Integer invoiceId,
      String customerName,
      String employeeName,
      LocalDateTime createdAt,
      BigDecimal totalAmount,
      Boolean isPaid,
      Long ticketCount,
      String paymentMethod) {
    // Backwards compatibility getters
    public Integer getMaHd() {
      return invoiceId;
    }

    public String getTenKhachHang() {
      return customerName;
    }

    public String getTenNhanVien() {
      return employeeName;
    }

    public LocalDateTime getNgayLap() {
      return createdAt;
    }

    public BigDecimal getTongTien() {
      return totalAmount;
    }

    public Boolean getTrangThai() {
      return isPaid;
    }

    public Long getSoVe() {
      return ticketCount;
    }

    public String getPhuongThuc() {
      return paymentMethod;
    }
  }

  public record TicketRow(
      Integer ticketId,
      Integer tripId,
      String departure,
      String destination,
      LocalDateTime departureTime,
      Integer seatNumber,
      BigDecimal price) {
    public Integer getMaVe() {
      return ticketId;
    }

    public Integer getMaChuyen() {
      return tripId;
    }

    public String getDiemDi() {
      return departure;
    }

    public String getDiemDen() {
      return destination;
    }

    public LocalDateTime getNgayGioKhoiHanh() {
      return departureTime;
    }

    public Integer getChoNgoi() {
      return seatNumber;
    }

    public BigDecimal getGiaVe() {
      return price;
    }
  }

  public record PaymentRow(
      Integer paymentId, BigDecimal amount, LocalDateTime paymentDate, String paymentMethod) {
    public Integer getMaTt() {
      return paymentId;
    }

    public BigDecimal getTongTien() {
      return amount;
    }

    public LocalDateTime getNgayTt() {
      return paymentDate;
    }

    public String getPhuongThuc() {
      return paymentMethod;
    }
  }

  public record InvoiceDetail(
      Integer invoiceId,
      String customerName,
      String customerPhone,
      String employeeName,
      LocalDateTime createdAt,
      BigDecimal totalAmount,
      Boolean isPaid,
      List<TicketRow> tickets,
      PaymentRow payment) {
    public Integer getMaHd() {
      return invoiceId;
    }

    public String getTenKhachHang() {
      return customerName;
    }

    public String getSdtKhachHang() {
      return customerPhone;
    }

    public String getTenNhanVien() {
      return employeeName;
    }

    public LocalDateTime getNgayLap() {
      return createdAt;
    }

    public BigDecimal getTongTien() {
      return totalAmount;
    }

    public Boolean getTrangThai() {
      return isPaid;
    }

    public List<TicketRow> getVeXes() {
      return tickets;
    }

    public PaymentRow getThanhToan() {
      return payment;
    }
  }
}
