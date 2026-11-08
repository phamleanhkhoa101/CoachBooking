package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Customer;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.InvoiceRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/customers", "/api/khach-hang"})
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class CustomerController {

  private final CustomerRepository customerRepository;
  private final InvoiceRepository invoiceRepository;
  private final AccountRepository accountRepository;

  public CustomerController(
      CustomerRepository customerRepository,
      InvoiceRepository invoiceRepository,
      AccountRepository accountRepository) {
    this.customerRepository = customerRepository;
    this.invoiceRepository = invoiceRepository;
    this.accountRepository = accountRepository;
  }

  @GetMapping
  public List<Customer> list() {
    return customerRepository.findAll();
  }

  @GetMapping("/{id}")
  public Customer get(@PathVariable Integer id) {
    return customerRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Khách hàng không tồn tại."));
  }

  @PostMapping
  public Customer create(@Valid @RequestBody Request request) {
    if (customerRepository.existsByPhone(request.phone().trim())) {
      throw ApiException.conflict("Số điện thoại đã tồn tại.");
    }
    if (request.accountId() != null && !accountRepository.existsById(request.accountId())) {
      throw ApiException.notFound("Tài khoản không tồn tại.");
    }
    Customer customer = new Customer();
    apply(customer, request);
    return customerRepository.save(customer);
  }

  @PutMapping("/{id}")
  public Customer update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    Customer customer = get(id);
    Validators.requirePhone(request.phone());
    customerRepository
        .findByPhone(request.phone().trim())
        .filter(other -> !other.getCustomerId().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict("Số điện thoại đã tồn tại.");
            });
    if (request.accountId() != null && !accountRepository.existsById(request.accountId())) {
      throw ApiException.notFound("Tài khoản không tồn tại.");
    }
    apply(customer, request);
    return customerRepository.save(customer);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    Customer customer = get(id);
    if (invoiceRepository.existsByCustomerId(id)) {
      throw ApiException.conflict("Không thể xóa khách hàng đã có hóa đơn.");
    }
    customerRepository.delete(customer);
    return Map.of("message", "Xóa khách hàng thành công.");
  }

  private void apply(Customer customer, Request request) {
    if (request.dateOfBirth() == null || request.dateOfBirth().isAfter(LocalDate.now())) {
      throw ApiException.badRequest("Ngày sinh không hợp lệ.");
    }
    Validators.requirePhone(request.phone());
    customer.setFullName(request.fullName().trim());
    customer.setDateOfBirth(request.dateOfBirth());
    customer.setPhone(request.phone().trim());
    if (request.gender() != null) {
      customer.setGender(request.gender());
    }
    customer.setAccountId(request.accountId());
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập tên khách hàng.") @JsonAlias({"tenKh", "fullName"})
          String fullName,
      @NotNull(message = "Vui lòng nhập ngày sinh.") @JsonAlias({"namSinh", "dateOfBirth"})
          LocalDate dateOfBirth,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") @JsonAlias({"sdt", "phone"}) String phone,
      @JsonAlias({"gioiTinh", "gender"}) Boolean gender,
      @JsonAlias({"maTk", "accountId"}) Integer accountId) {}
}
