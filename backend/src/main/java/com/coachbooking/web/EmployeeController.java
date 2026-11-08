package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Employee;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.EmployeeRepository;
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
@RequestMapping({"/api/employees", "/api/nhan-vien"})
@PreAuthorize("hasRole('Admin')")
public class EmployeeController {

  private final EmployeeRepository employeeRepository;
  private final InvoiceRepository invoiceRepository;
  private final AccountRepository accountRepository;

  public EmployeeController(
      EmployeeRepository employeeRepository,
      InvoiceRepository invoiceRepository,
      AccountRepository accountRepository) {
    this.employeeRepository = employeeRepository;
    this.invoiceRepository = invoiceRepository;
    this.accountRepository = accountRepository;
  }

  @GetMapping
  public List<Employee> list() {
    return employeeRepository.findAll();
  }

  @GetMapping("/{id}")
  public Employee get(@PathVariable Integer id) {
    return employeeRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Nhân viên không tồn tại."));
  }

  @PostMapping
  public Employee create(@Valid @RequestBody Request request) {
    if (employeeRepository.existsByPhone(request.phone().trim())) {
      throw ApiException.conflict("Số điện thoại đã tồn tại.");
    }
    validateAccount(null, request);
    Employee employee = new Employee();
    apply(employee, request);
    return employeeRepository.save(employee);
  }

  @PutMapping("/{id}")
  public Employee update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    Employee employee = get(id);
    employeeRepository
        .findByPhone(request.phone().trim())
        .filter(other -> !other.getEmployeeId().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict("Số điện thoại đã tồn tại.");
            });
    validateAccount(id, request);
    apply(employee, request);
    return employeeRepository.save(employee);
  }

  @DeleteMapping("/{id}")
  public Map<String, String> delete(@PathVariable Integer id) {
    Employee employee = get(id);
    if (invoiceRepository.existsByEmployeeId(id)) {
      throw ApiException.conflict("Không thể xóa nhân viên đã lập hóa đơn.");
    }
    employeeRepository.delete(employee);
    return Map.of("message", "Xóa nhân viên thành công.");
  }

  private void validateAccount(Integer employeeId, Request request) {
    if (request.accountId() != null) {
      if (!accountRepository.existsById(request.accountId())) {
        throw ApiException.notFound("Tài khoản không tồn tại.");
      }
      employeeRepository
          .findByAccountId(request.accountId())
          .filter(other -> !other.getEmployeeId().equals(employeeId))
          .ifPresent(
              other -> {
                throw ApiException.conflict("Tài khoản này đã gắn với nhân viên khác.");
              });
    }
  }

  private void apply(Employee employee, Request request) {
    if (request.dateOfBirth().isAfter(LocalDate.now().minusYears(18))) {
      throw ApiException.badRequest("Nhân viên phải đủ 18 tuổi.");
    }
    employee.setFullName(request.fullName().trim());
    employee.setDateOfBirth(request.dateOfBirth());
    employee.setAddress(request.address().trim());
    employee.setPhone(request.phone().trim());
    if (request.gender() != null) {
      employee.setGender(request.gender());
    }
    if (request.isActive() != null) {
      employee.setIsActive(request.isActive());
    }
    employee.setAccountId(request.accountId());
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập tên nhân viên.") @JsonAlias({"tenNv", "fullName"})
          String fullName,
      @NotNull(message = "Vui lòng nhập ngày sinh.") @JsonAlias({"namSinh", "dateOfBirth"})
          LocalDate dateOfBirth,
      @NotBlank(message = "Vui lòng nhập địa chỉ.") @JsonAlias({"diaChi", "address"})
          String address,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") @JsonAlias({"sdt", "phone"}) String phone,
      @JsonAlias({"gioiTinh", "gender"}) Boolean gender,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive,
      @JsonAlias({"maTk", "accountId"}) Integer accountId) {}
}
