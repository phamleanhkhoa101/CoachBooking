package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.RoleRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import java.util.List;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/accounts", "/api/tai-khoan"})
@PreAuthorize("hasRole('Admin')")
public class AccountController {

  private final AccountRepository accountRepository;
  private final RoleRepository roleRepository;
  private final EmployeeRepository employeeRepository;
  private final CustomerRepository customerRepository;
  private final PasswordEncoder passwordEncoder;

  public AccountController(
      AccountRepository accountRepository,
      RoleRepository roleRepository,
      EmployeeRepository employeeRepository,
      CustomerRepository customerRepository,
      PasswordEncoder passwordEncoder) {
    this.accountRepository = accountRepository;
    this.roleRepository = roleRepository;
    this.employeeRepository = employeeRepository;
    this.customerRepository = customerRepository;
    this.passwordEncoder = passwordEncoder;
  }

  @GetMapping
  public List<Account> list() {
    return accountRepository.findAll();
  }

  @GetMapping("/{id}")
  public Account get(@PathVariable Integer id) {
    return accountRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Tài khoản không tồn tại."));
  }

  @PostMapping
  public Account create(@Valid @RequestBody CreateRequest request) {
    String username = request.username().trim();
    if (username.length() < 6) {
      throw ApiException.badRequest("Tên đăng nhập phải có ít nhất 6 ký tự.");
    }
    Validators.requirePassword(request.password());
    if (accountRepository.existsByUsername(username)) {
      throw ApiException.conflict("Tên đăng nhập đã được sử dụng.");
    }
    if (accountRepository.existsByEmail(request.email().trim())) {
      throw ApiException.conflict("Email đã được sử dụng.");
    }
    if (!roleRepository.existsById(request.roleId())) {
      throw ApiException.notFound("Quyền không tồn tại.");
    }

    Account account = new Account();
    account.setUsername(username);
    account.setEmail(request.email().trim());
    account.setPassword(passwordEncoder.encode(request.password()));
    account.setRoleId(request.roleId());
    account.setIsActive(request.isActive() == null || request.isActive());
    return accountRepository.save(account);
  }

  @PutMapping("/{id}")
  public Account update(@PathVariable Integer id, @Valid @RequestBody UpdateRequest request) {
    Account account = get(id);
    if (!roleRepository.existsById(request.roleId())) {
      throw ApiException.notFound("Quyền không tồn tại.");
    }
    if (!account.getEmail().equalsIgnoreCase(request.email().trim())
        && accountRepository.existsByEmail(request.email().trim())) {
      throw ApiException.conflict("Email đã được sử dụng.");
    }
    account.setEmail(request.email().trim());
    account.setRoleId(request.roleId());
    if (request.isActive() != null) {
      account.setIsActive(request.isActive());
    }
    if (!Validators.isBlank(request.password())) {
      Validators.requirePassword(request.password());
      account.setPassword(passwordEncoder.encode(request.password()));
    }
    return accountRepository.save(account);
  }

  @DeleteMapping("/{id}")
  public Map<String, String> delete(@PathVariable Integer id) {
    Account account = get(id);
    if (employeeRepository.existsByAccountId(id) || customerRepository.existsByAccountId(id)) {
      throw ApiException.conflict(
          "Không thể xóa tài khoản đang gắn với nhân viên hoặc khách hàng.");
    }
    accountRepository.delete(account);
    return Map.of("message", "Xóa tài khoản thành công.");
  }

  public record CreateRequest(
      @NotBlank(message = "Vui lòng nhập tên đăng nhập.") @JsonAlias({"tenDangNhap", "username"})
          String username,
      @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
          String email,
      @NotBlank(message = "Vui lòng nhập mật khẩu.") @JsonAlias({"matKhau", "password"})
          String password,
      @NotNull(message = "Vui lòng chọn quyền.") @JsonAlias({"maQuyen", "roleId"}) Integer roleId,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}

  public record UpdateRequest(
      @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
          String email,
      @JsonAlias({"matKhau", "password"}) String password,
      @NotNull(message = "Vui lòng chọn quyền.") @JsonAlias({"maQuyen", "roleId"}) Integer roleId,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}
}
