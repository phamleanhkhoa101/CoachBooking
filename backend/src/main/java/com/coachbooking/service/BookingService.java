package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Payment;
import com.coachbooking.domain.SeatHold;
import com.coachbooking.domain.Ticket;
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
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.Set;
import java.util.UUID;
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
  private final SeatHoldRepository seatHoldRepository;
  private final SeatStatusService seatStatusService;

  public BookingService(
      TripRepository tripRepository,
      VehicleRepository vehicleRepository,
      VehicleTypeRepository vehicleTypeRepository,
      CustomerRepository customerRepository,
      EmployeeRepository employeeRepository,
      InvoiceRepository invoiceRepository,
      TicketRepository ticketRepository,
      PaymentRepository paymentRepository,
      CurrentUserService currentUserService,
      SeatHoldRepository seatHoldRepository,
      SeatStatusService seatStatusService) {
    this.tripRepository = tripRepository;
    this.vehicleRepository = vehicleRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.customerRepository = customerRepository;
    this.employeeRepository = employeeRepository;
    this.invoiceRepository = invoiceRepository;
    this.ticketRepository = ticketRepository;
    this.paymentRepository = paymentRepository;
    this.currentUserService = currentUserService;
    this.seatHoldRepository = seatHoldRepository;
    this.seatStatusService = seatStatusService;
  }

  @Transactional
  public BookingResponse bookTickets(Account actor, BookingRequest request) {
    Trip trip =
        tripRepository
            .findByIdForUpdate(request.tripId())
            .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    LocalDateTime now = LocalDateTime.now();
    if (!trip.getDepartureTime().isAfter(now)) {
      throw ApiException.conflict("Chuyến xe đã khởi hành, không thể đặt vé.");
    }
    if (trip.getActualPrice() == null || trip.getActualPrice().signum() <= 0) {
      throw ApiException.conflict("Chuyến xe chưa có giá vé hợp lệ.");
    }

    List<Integer> seatNumbers = normalizeSeats(request.seatNumbers());
    boolean customerActor = currentUserService.isCustomer(actor);
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

    seatHoldRepository.deleteExpiredByTripId(trip.getTripId(), now);
    List<Integer> bookedSeats = ticketRepository.findSeatNumbersByTripId(trip.getTripId());
    List<Integer> duplicates = seatNumbers.stream().filter(bookedSeats::contains).toList();
    if (!duplicates.isEmpty()) {
      throw ApiException.conflict(
          "Ghế số " + joinSeats(duplicates) + " đã được đặt. Vui lòng chọn ghế khác.");
    }

    if (customerActor) {
      validateCustomerHold(actor, request.holdToken(), trip.getTripId(), seatNumbers, now);
    } else {
      List<Integer> heldSeats =
          seatHoldRepository
              .findByTripIdAndExpiresAtAfterOrderBySeatNumber(trip.getTripId(), now)
              .stream()
              .map(SeatHold::getSeatNumber)
              .filter(seatNumbers::contains)
              .toList();
      if (!heldSeats.isEmpty()) {
        throw ApiException.conflict(
            "Ghế số " + joinSeats(heldSeats) + " đang được khách khác giữ.");
      }
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
    if (customerActor && !"Chuyển khoản".equals(paymentMethod)) {
      throw ApiException.badRequest("Khách hàng chỉ có thể chọn phương thức Chuyển khoản.");
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
    ticketRepository.saveAllAndFlush(tickets);

    if (payNow) {
      Payment payment = new Payment();
      payment.setInvoiceId(savedInvoice.getInvoiceId());
      payment.setAmount(totalAmount);
      payment.setPaymentDate(now);
      payment.setPaymentMethod(paymentMethod);
      paymentRepository.save(payment);
    }

    if (customerActor) {
      seatHoldRepository.deleteByHoldToken(request.holdToken());
    }
    seatStatusService.publishAfterCommit(trip.getTripId());

    return new BookingResponse(
        savedInvoice.getInvoiceId(),
        totalAmount,
        seatNumbers.size(),
        payNow ? paymentMethod : "Thanh toán sau",
        payNow);
  }

  private void validateCustomerHold(
      Account actor, UUID token, Integer tripId, List<Integer> seatNumbers, LocalDateTime now) {
    if (token == null) {
      throw ApiException.conflict("Vui lòng giữ ghế trước khi xác nhận đặt vé.");
    }
    List<SeatHold> holds = seatHoldRepository.findByHoldTokenOrderBySeatNumber(token);
    if (holds.isEmpty()) {
      throw ApiException.conflict("Lượt giữ ghế không còn hiệu lực. Vui lòng chọn lại ghế.");
    }
    if (holds.stream().anyMatch(hold -> !hold.getAccountId().equals(actor.getAccountId()))) {
      throw ApiException.forbidden("Bạn không có quyền sử dụng lượt giữ ghế này.");
    }
    if (holds.stream().anyMatch(hold -> !hold.getTripId().equals(tripId))) {
      throw ApiException.conflict("Lượt giữ ghế không thuộc chuyến xe đã chọn.");
    }
    if (holds.stream().anyMatch(hold -> !hold.getExpiresAt().isAfter(now))) {
      throw ApiException.conflict("Thời gian giữ ghế đã hết. Vui lòng chọn lại ghế.");
    }
    List<Integer> heldSeats = holds.stream().map(SeatHold::getSeatNumber).sorted().toList();
    if (!heldSeats.equals(seatNumbers)) {
      throw ApiException.conflict("Danh sách ghế không khớp với lượt giữ ghế.");
    }
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
}
