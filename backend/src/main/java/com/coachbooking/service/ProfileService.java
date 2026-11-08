package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
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

  public static class UpdateProfileRequest {
    @NotBlank(message = "Vui lòng nhập họ tên.")
    private String fullName;

    @NotBlank(message = "Vui lòng nhập số điện thoại.")
    private String phone;

    private LocalDate dateOfBirth;
    private Boolean gender;
    private String address;

    public UpdateProfileRequest() {}

    public UpdateProfileRequest(
        String fullName, String phone, LocalDate dateOfBirth, Boolean gender, String address) {
      this.fullName = fullName;
      this.phone = phone;
      this.dateOfBirth = dateOfBirth;
      this.gender = gender;
      this.address = address;
    }

    public String fullName() {
      return fullName;
    }

    public String phone() {
      return phone;
    }

    public LocalDate dateOfBirth() {
      return dateOfBirth;
    }

    public Boolean gender() {
      return gender;
    }

    public String address() {
      return address;
    }

    public String getFullName() {
      return fullName;
    }

    public void setFullName(String fullName) {
      if (this.fullName == null || !fullName.isBlank()) {
        this.fullName = fullName;
      }
    }

    public void setHoTen(String hoTen) {
      if (this.fullName == null || this.fullName.isBlank()) {
        this.fullName = hoTen;
      }
    }

    public String getPhone() {
      return phone;
    }

    public void setPhone(String phone) {
      if (this.phone == null || !phone.isBlank()) {
        this.phone = phone;
      }
    }

    public void setSdt(String sdt) {
      if (this.phone == null || this.phone.isBlank()) {
        this.phone = sdt;
      }
    }

    public LocalDate getDateOfBirth() {
      return dateOfBirth;
    }

    public void setDateOfBirth(LocalDate dateOfBirth) {
      if (this.dateOfBirth == null) {
        this.dateOfBirth = dateOfBirth;
      }
    }

    public void setNamSinh(LocalDate namSinh) {
      if (this.dateOfBirth == null) {
        this.dateOfBirth = namSinh;
      }
    }

    public Boolean getGender() {
      return gender;
    }

    public void setGender(Boolean gender) {
      if (this.gender == null) {
        this.gender = gender;
      }
    }

    public void setGioiTinh(Boolean gioiTinh) {
      if (this.gender == null) {
        this.gender = gioiTinh;
      }
    }

    public String getAddress() {
      return address;
    }

    public void setAddress(String address) {
      if (this.address == null || !address.isBlank()) {
        this.address = address;
      }
    }

    public void setDiaChi(String diaChi) {
      if (this.address == null || this.address.isBlank()) {
        this.address = diaChi;
      }
    }
  }

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
