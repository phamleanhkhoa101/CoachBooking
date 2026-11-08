package com.coachbooking.config;

import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Payment;
import com.coachbooking.domain.Ticket;
import com.coachbooking.domain.Trip;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.InvoiceRepository;
import com.coachbooking.repo.PaymentRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

@Component
public class DataInitializer implements CommandLineRunner {

  private static final Logger log = LoggerFactory.getLogger(DataInitializer.class);

  private final AccountRepository accountRepository;
  private final EmployeeRepository employeeRepository;
  private final CustomerRepository customerRepository;
  private final TripRepository tripRepository;
  private final InvoiceRepository invoiceRepository;
  private final TicketRepository ticketRepository;
  private final PaymentRepository paymentRepository;
  private final PasswordEncoder passwordEncoder;

  public DataInitializer(
      AccountRepository accountRepository,
      EmployeeRepository employeeRepository,
      CustomerRepository customerRepository,
      TripRepository tripRepository,
      InvoiceRepository invoiceRepository,
      TicketRepository ticketRepository,
      PaymentRepository paymentRepository,
      PasswordEncoder passwordEncoder) {
    this.accountRepository = accountRepository;
    this.employeeRepository = employeeRepository;
    this.customerRepository = customerRepository;
    this.tripRepository = tripRepository;
    this.invoiceRepository = invoiceRepository;
    this.ticketRepository = ticketRepository;
    this.paymentRepository = paymentRepository;
    this.passwordEncoder = passwordEncoder;
  }

  @Override
  @Transactional
  public void run(String... args) {
    Account admin = ensureAccount("admin", "admin@gmail.com", "Abc@123", 1);
    Account nv1 = ensureAccount("nv1", "nv1@gmail.com", "Abc@123", 2);
    Account nv2 = ensureAccount("nv2", "nv2@gmail.com", "Abc@123", 2);
    Account kh1 = ensureAccount("kh1", "kh1@gmail.com", "Abc@123", 3);
    Account kh2 = ensureAccount("kh2", "kh2@gmail.com", "Abc@123", 3);

    linkEmployee("0912345678", admin);
    linkEmployee("0912345679", nv1);
    linkEmployee("0912345680", nv2);
    linkCustomer("0981111111", kh1);
    linkCustomer("0981111112", kh2);

    seedDemoInvoices();
  }

  private Account ensureAccount(String username, String email, String password, int roleId) {
    return accountRepository
        .findByUsername(username)
        .orElseGet(
            () -> {
              Account account = new Account();
              account.setUsername(username);
              account.setEmail(email);
              account.setPassword(passwordEncoder.encode(password));
              account.setRoleId(roleId);
              account.setIsActive(true);
              Account saved = accountRepository.save(account);
              log.info("Đã tạo tài khoản mẫu: {}", username);
              return saved;
            });
  }

  private void linkEmployee(String phone, Account account) {
    employeeRepository
        .findByPhone(phone)
        .ifPresent(
            employee -> {
              if (employee.getAccountId() == null) {
                employee.setAccountId(account.getAccountId());
                employeeRepository.save(employee);
              }
            });
  }

  private void linkCustomer(String phone, Account account) {
    customerRepository
        .findByPhone(phone)
        .ifPresent(
            customer -> {
              if (customer.getAccountId() == null) {
                customer.setAccountId(account.getAccountId());
                customerRepository.save(customer);
              }
            });
  }

  private void seedDemoInvoices() {
    if (invoiceRepository.count() > 0) {
      return;
    }
    List<Trip> trips =
        tripRepository.findAllByOrderByDepartureTimeAsc().stream()
            .filter(trip -> trip.getDepartureTime().isAfter(LocalDateTime.now()))
            .limit(3)
            .toList();
    List<Customer> customers = customerRepository.findAll().stream().limit(3).toList();
    if (trips.isEmpty() || customers.isEmpty()) {
      return;
    }

    Employee employee = employeeRepository.findAll().stream().findFirst().orElse(null);
    for (int i = 0; i < Math.min(trips.size(), customers.size()); i++) {
      Trip trip = trips.get(i);
      if (trip.getActualPrice() == null || trip.getActualPrice().signum() <= 0) {
        continue;
      }
      Customer customer = customers.get(i);
      Invoice invoice = new Invoice();
      invoice.setCustomerId(customer.getCustomerId());
      invoice.setEmployeeId(employee == null ? null : employee.getEmployeeId());
      invoice.setCreatedAt(LocalDateTime.now().minusDays(3L - i));
      invoice.setIsPaid(true);

      List<Ticket> tickets = new ArrayList<>();
      int ticketCount = (i % 2) + 1;
      for (int seat = 1; seat <= ticketCount; seat++) {
        Ticket ticket = new Ticket();
        ticket.setTripId(trip.getTripId());
        ticket.setSeatNumber(seat);
        ticket.setPrice(trip.getActualPrice());
        tickets.add(ticket);
      }
      BigDecimal totalAmount = trip.getActualPrice().multiply(BigDecimal.valueOf(ticketCount));
      invoice.setTotalAmount(totalAmount);
      Invoice savedInvoice = invoiceRepository.save(invoice);

      for (Ticket ticket : tickets) {
        ticket.setInvoiceId(savedInvoice.getInvoiceId());
      }
      ticketRepository.saveAll(tickets);

      Payment payment = new Payment();
      payment.setInvoiceId(savedInvoice.getInvoiceId());
      payment.setAmount(totalAmount);
      payment.setPaymentDate(savedInvoice.getCreatedAt().plusMinutes(5));
      payment.setPaymentMethod("Tiền mặt");
      paymentRepository.save(payment);
    }
    log.info("Đã tạo hóa đơn mẫu cho thống kê và lịch sử mua vé.");
  }
}
