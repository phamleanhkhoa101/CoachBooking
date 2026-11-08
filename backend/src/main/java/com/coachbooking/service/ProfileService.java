package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.constraints.NotBlank;
import java.time.LocalDate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class ProfileService {

  private final CustomerRepository customerRepository;
  private final EmployeeRepository employeeRepository;
  private final CurrentUserService currentUserService;

  public ProfileService(
      CustomerRepository customerRepository,
      EmployeeRepository employeeRepository,
      CurrentUserService currentUserService) {
    this.customerRepository = customerRepository;
    this.employeeRepository = employeeRepository;
    this.currentUserService = currentUserService;
  }

  public ProfileResponse get(Account account) {
    String role = currentUserService.roleName(account);
    Customer customer = customerRepository.findByAccountId(account.getAccountId()).orElse(null);
    Employee employee = employeeRepository.findByAccountId(account.getAccountId()).orElse(null);

    if (customer != null) {
      return new ProfileResponse(
          "CUSTOMER",
          customer.getCustomerId(),
          null,
          customer.getFullName(),
          customer.getDateOfBirth(),
          customer.getPhone(),
          customer.getGender(),
          null,
          account.getEmail(),
          account.getUsername(),
          role);
    }
    if (employee != null) {
      return new ProfileResponse(
          "EMPLOYEE",
          null,
          employee.getEmployeeId(),
          employee.getFullName(),
          employee.getDateOfBirth(),
          employee.getPhone(),
          employee.getGender(),
          employee.getAddress(),
          account.getEmail(),
          account.getUsername(),
          role);
    }
    return new ProfileResponse(
        "UNASSIGNED",
        null,
        null,
        account.getUsername(),
        null,
        null,
        null,
        null,
        account.getEmail(),
        account.getUsername(),
        role);
  }

  @Transactional
  public ProfileResponse update(Account account, UpdateProfileRequest request) {
    Validators.requireText(request.fullName(), "Vui lòng nhập họ tên.");
    Validators.requirePhone(request.phone());
    if (request.dateOfBirth() == null) {
      throw ApiException.badRequest("Vui lòng nhập ngày sinh.");
    }
    if (request.dateOfBirth().isAfter(LocalDate.now())) {
      throw ApiException.badRequest("Ngày sinh không hợp lệ.");
    }

    Customer customer = customerRepository.findByAccountId(account.getAccountId()).orElse(null);
    if (customer != null) {
      customerRepository
          .findByPhone(request.phone().trim())
          .filter(other -> !other.getCustomerId().equals(customer.getCustomerId()))
          .ifPresent(
              other -> {
                throw ApiException.conflict("Số điện thoại đã được sử dụng.");
              });
      customer.setFullName(request.fullName().trim());
      customer.setPhone(request.phone().trim());
      customer.setDateOfBirth(request.dateOfBirth());
      customer.setGender(Boolean.TRUE.equals(request.gender()));
      customerRepository.save(customer);
      return get(account);
    }

    Employee employee = employeeRepository.findByAccountId(account.getAccountId()).orElse(null);
    if (employee != null) {
      if (Validators.isBlank(request.address())) {
        throw ApiException.badRequest("Vui lòng nhập địa chỉ.");
      }
      employeeRepository
          .findByPhone(request.phone().trim())
          .filter(other -> !other.getEmployeeId().equals(employee.getEmployeeId()))
          .ifPresent(
              other -> {
                throw ApiException.conflict("Số điện thoại đã được sử dụng.");
              });
      employee.setFullName(request.fullName().trim());
      employee.setPhone(request.phone().trim());
      employee.setDateOfBirth(request.dateOfBirth());
      employee.setGender(Boolean.TRUE.equals(request.gender()));
      employee.setAddress(request.address().trim());
      employeeRepository.save(employee);
      return get(account);
    }

    if (!currentUserService.isCustomer(account)) {
      throw ApiException.badRequest("Tài khoản chưa được gắn với nhân viên hoặc khách hàng.");
    }
    Customer existingPhone = customerRepository.findByPhone(request.phone().trim()).orElse(null);
    if (existingPhone != null) {
      if (existingPhone.getAccountId() != null
          && !existingPhone.getAccountId().equals(account.getAccountId())) {
        throw ApiException.conflict("Số điện thoại đã được sử dụng.");
      }
      existingPhone.setFullName(request.fullName().trim());
      existingPhone.setDateOfBirth(request.dateOfBirth());
      existingPhone.setGender(Boolean.TRUE.equals(request.gender()));
      existingPhone.setAccountId(account.getAccountId());
      customerRepository.save(existingPhone);
      return get(account);
    }
    Customer newCust = new Customer();
    newCust.setFullName(request.fullName().trim());
    newCust.setPhone(request.phone().trim());
    newCust.setDateOfBirth(request.dateOfBirth());
    newCust.setGender(Boolean.TRUE.equals(request.gender()));
    newCust.setAccountId(account.getAccountId());
    customerRepository.save(newCust);
    return get(account);
  }

  public record UpdateProfileRequest(
      @NotBlank(message = "Vui lòng nhập họ tên.") @JsonAlias({"hoTen", "fullName"})
          String fullName,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") @JsonAlias({"sdt", "phone"}) String phone,
      @JsonAlias({"namSinh", "dateOfBirth"}) LocalDate dateOfBirth,
      @JsonAlias({"gioiTinh", "gender"}) Boolean gender,
      @JsonAlias({"diaChi", "address"}) String address) {}

  public record ProfileResponse(
      String type,
      Integer customerId,
      Integer employeeId,
      String fullName,
      LocalDate dateOfBirth,
      String phone,
      Boolean gender,
      String address,
      String email,
      String username,
      String role) {
    public String getLoai() {
      return type;
    }

    public Integer getMaKh() {
      return customerId;
    }

    public Integer getMaNv() {
      return employeeId;
    }

    public String getHoTen() {
      return fullName;
    }

    public LocalDate getNamSinh() {
      return dateOfBirth;
    }

    public String getSdt() {
      return phone;
    }

    public Boolean getGioiTinh() {
      return gender;
    }

    public String getDiaChi() {
      return address;
    }

    public String getTenDangNhap() {
      return username;
    }

    public String getQuyen() {
      return role;
    }
  }
}
