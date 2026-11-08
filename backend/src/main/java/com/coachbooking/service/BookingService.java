package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Payment;
import com.coachbooking.domain.Ticket;
import com.coachbooking.domain.Trip;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.InvoiceRepository;
import com.coachbooking.repo.PaymentRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.constraints.NotNull;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.Set;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class BookingService {

  private static final Set<String> VALID_PAYMENT_METHODS =
      Set.of("Tiền mặt", "Chuyển khoản", "Thẻ ATM", "Cash", "Bank Transfer", "ATM Card");

  private final TripRepository tripRepository;
  private final VehicleRepository vehicleRepository;
  private final VehicleTypeRepository vehicleTypeRepository;
  private final CustomerRepository customerRepository;
  private final EmployeeRepository employeeRepository;
  private final InvoiceRepository invoiceRepository;
  private final TicketRepository ticketRepository;
  private final PaymentRepository paymentRepository;
  private final CurrentUserService currentUserService;

  public BookingService(
      TripRepository tripRepository,
      VehicleRepository vehicleRepository,
      VehicleTypeRepository vehicleTypeRepository,
      CustomerRepository customerRepository,
      EmployeeRepository employeeRepository,
      InvoiceRepository invoiceRepository,
      TicketRepository ticketRepository,
      PaymentRepository paymentRepository,
      CurrentUserService currentUserService) {
    this.tripRepository = tripRepository;
    this.vehicleRepository = vehicleRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.customerRepository = customerRepository;
    this.employeeRepository = employeeRepository;
    this.invoiceRepository = invoiceRepository;
    this.ticketRepository = ticketRepository;
    this.paymentRepository = paymentRepository;
    this.currentUserService = currentUserService;
  }

  @Transactional
  public BookingResponse bookTickets(Account actor, BookingRequest request) {
    Trip trip =
        tripRepository
            .findById(request.tripId())
            .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    LocalDateTime now = LocalDateTime.now();
    if (!trip.getDepartureTime().isAfter(now)) {
      throw ApiException.conflict("Chuyến xe đã khởi hành, không thể đặt vé.");
    }
    if (trip.getActualPrice() == null || trip.getActualPrice().signum() <= 0) {
      throw ApiException.conflict("Chuyến xe chưa có giá vé hợp lệ.");
    }

    List<Integer> seatNumbers = normalizeSeats(request.seatNumbers());
    Vehicle vehicle =
        vehicleRepository
            .findById(trip.getVehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe của chuyến không tồn tại."));
    VehicleType vehicleType =
        vehicleTypeRepository
            .findById(vehicle.getTypeId())
            .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
    int highestSeat = seatNumbers.stream().max(Integer::compareTo).orElse(0);
    if (highestSeat > vehicleType.getSeatCapacity()) {
      throw ApiException.badRequest(
          "Ghế chọn vượt quá số chỗ của xe (" + vehicleType.getSeatCapacity() + " chỗ).");
    }

    List<Integer> bookedSeats = ticketRepository.findSeatNumbersByTripId(trip.getTripId());
    List<Integer> duplicates = seatNumbers.stream().filter(bookedSeats::contains).toList();
    if (!duplicates.isEmpty()) {
      throw ApiException.conflict(
          "Ghế số " + joinSeats(duplicates) + " đã được đặt. Vui lòng chọn ghế khác.");
    }

    Customer customer = resolveCustomer(actor, request);

    boolean payNow =
        !Validators.isBlank(request.paymentMethod())
            && !"Thanh toán sau".equalsIgnoreCase(request.paymentMethod().trim())
            && !"Pay later".equalsIgnoreCase(request.paymentMethod().trim());
    String paymentMethod = payNow ? request.paymentMethod().trim() : null;
    if (payNow && !VALID_PAYMENT_METHODS.contains(paymentMethod)) {
      throw ApiException.badRequest("Phương thức thanh toán không hợp lệ.");
    }

    BigDecimal totalAmount = trip.getActualPrice().multiply(BigDecimal.valueOf(seatNumbers.size()));

    Invoice invoice = new Invoice();
    invoice.setCustomerId(customer.getCustomerId());
    invoice.setEmployeeId(getEmployeeId(actor));
    invoice.setCreatedAt(now);
    invoice.setTotalAmount(totalAmount);
    invoice.setIsPaid(payNow);
    Invoice savedInvoice = invoiceRepository.save(invoice);

    List<Ticket> tickets = new ArrayList<>();
    for (Integer seatNumber : seatNumbers) {
      Ticket ticket = new Ticket();
      ticket.setTripId(trip.getTripId());
      ticket.setInvoiceId(savedInvoice.getInvoiceId());
      ticket.setSeatNumber(seatNumber);
      ticket.setPrice(trip.getActualPrice());
      tickets.add(ticket);
    }
    ticketRepository.saveAll(tickets);

    if (payNow) {
      Payment payment = new Payment();
      payment.setInvoiceId(savedInvoice.getInvoiceId());
      payment.setAmount(totalAmount);
      payment.setPaymentDate(now);
      payment.setPaymentMethod(paymentMethod);
      paymentRepository.save(payment);
    }

    return new BookingResponse(
        savedInvoice.getInvoiceId(),
        totalAmount,
        seatNumbers.size(),
        payNow ? paymentMethod : "Thanh toán sau",
        payNow);
  }

  private List<Integer> normalizeSeats(List<Integer> seatNumbers) {
    if (seatNumbers == null || seatNumbers.isEmpty()) {
      throw ApiException.badRequest("Vui lòng chọn ít nhất một ghế.");
    }
    List<Integer> seats = seatNumbers.stream().filter(Objects::nonNull).sorted().toList();
    if (seats.size() != seatNumbers.size()) {
      throw ApiException.badRequest("Danh sách ghế không hợp lệ.");
    }
    if (seats.get(0) < 1) {
      throw ApiException.badRequest("Số ghế phải bắt đầu từ 1.");
    }
    if (seats.stream().distinct().count() != seats.size()) {
      throw ApiException.badRequest("Danh sách ghế bị trùng.");
    }
    return seats;
  }

  private Customer resolveCustomer(Account actor, BookingRequest request) {
    if (currentUserService.isCustomer(actor)) {
      Customer customer = customerRepository.findByAccountId(actor.getAccountId()).orElse(null);
      if (customer != null) {
        return customer;
      }
      Validators.requireText(request.customerName(), "Vui lòng nhập họ tên để hoàn tất hồ sơ.");
      Validators.requirePhone(request.phone());
      Customer existingPhone = customerRepository.findByPhone(request.phone().trim()).orElse(null);
      if (existingPhone != null) {
        if (existingPhone.getAccountId() != null
            && !existingPhone.getAccountId().equals(actor.getAccountId())) {
          throw ApiException.conflict("Số điện thoại này đã được gắn với tài khoản khác.");
        }
        existingPhone.setAccountId(actor.getAccountId());
        return customerRepository.save(existingPhone);
      }
      return customerRepository.save(newCustomer(request, actor.getAccountId()));
    }

    Validators.requireText(request.customerName(), "Vui lòng nhập tên khách hàng.");
    Validators.requirePhone(request.phone());
    if (request.dateOfBirth() == null) {
      throw ApiException.badRequest("Vui lòng nhập ngày sinh khách hàng.");
    }
    return customerRepository
        .findByPhone(request.phone().trim())
        .orElseGet(() -> customerRepository.save(newCustomer(request, null)));
  }

  private Customer newCustomer(BookingRequest request, Integer accountId) {
    if (request.dateOfBirth() == null || request.dateOfBirth().isAfter(LocalDate.now())) {
      throw ApiException.badRequest("Ngày sinh khách hàng không hợp lệ.");
    }
    Customer customer = new Customer();
    customer.setFullName(request.customerName().trim());
    customer.setPhone(request.phone().trim());
    customer.setDateOfBirth(request.dateOfBirth());
    customer.setGender(Boolean.TRUE.equals(request.gender()));
    customer.setAccountId(accountId);
    return customer;
  }

  private Integer getEmployeeId(Account actor) {
    return employeeRepository
        .findByAccountId(actor.getAccountId())
        .map(Employee::getEmployeeId)
        .orElse(null);
  }

  private String joinSeats(List<Integer> seats) {
    return String.join(", ", seats.stream().map(String::valueOf).toList());
  }

  public record BookingRequest(
      @NotNull(message = "Vui lòng chọn chuyến xe.") @JsonAlias({"maChuyen", "tripId"})
          Integer tripId,
      @JsonAlias({"choNgois", "seatNumbers"}) List<Integer> seatNumbers,
      @JsonAlias({"tenKhachHang", "customerName"}) String customerName,
      @JsonAlias({"sdt", "phone"}) String phone,
      @JsonAlias({"namSinh", "dateOfBirth"}) LocalDate dateOfBirth,
      @JsonAlias({"gioiTinh", "gender"}) Boolean gender,
      @JsonAlias({"phuongThuc", "paymentMethod"}) String paymentMethod) {}

  public record BookingResponse(
      Integer invoiceId,
      BigDecimal totalAmount,
      Integer ticketCount,
      String paymentMethod,
      Boolean isPaid) {
    // Backwards compatibility getters for existing frontend references if needed
    public Integer getMaHd() {
      return invoiceId;
    }

    public BigDecimal getTongTien() {
      return totalAmount;
    }

    public Integer getSoVe() {
      return ticketCount;
    }

    public String getPhuongThuc() {
      return paymentMethod;
    }

    public Boolean getTrangThai() {
      return isPaid;
    }
  }
}
