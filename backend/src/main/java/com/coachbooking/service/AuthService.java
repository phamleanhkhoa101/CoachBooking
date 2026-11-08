package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.RoleRepository;
import com.coachbooking.security.JwtService;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class AuthService {

  private static final int ROLE_CUSTOMER_ID = 3;
  private static final String UPPER = "ABCDEFGHJKLMNPQRSTUVWXYZ";
  private static final String LOWER = "abcdefghijkmnopqrstuvwxyz";
  private static final String DIGITS = "23456789";
  private static final String SPECIAL = "!@#$%^&*";

  private final AccountRepository accountRepository;
  private final RoleRepository roleRepository;
  private final EmployeeRepository employeeRepository;
  private final CustomerRepository customerRepository;
  private final PasswordEncoder passwordEncoder;
  private final JwtService jwtService;

  public AuthService(
      AccountRepository accountRepository,
      RoleRepository roleRepository,
      EmployeeRepository employeeRepository,
      CustomerRepository customerRepository,
      PasswordEncoder passwordEncoder,
      JwtService jwtService) {
    this.accountRepository = accountRepository;
    this.roleRepository = roleRepository;
    this.employeeRepository = employeeRepository;
    this.customerRepository = customerRepository;
    this.passwordEncoder = passwordEncoder;
    this.jwtService = jwtService;
  }

  private Account authenticate(LoginRequest request) {
    Account account =
        accountRepository
            .findByUsername(request.username().trim())
            .orElseThrow(() -> ApiException.badRequest("Tên đăng nhập hoặc mật khẩu không đúng."));
    if (!passwordEncoder.matches(request.password(), account.getPassword())) {
      throw ApiException.badRequest("Tên đăng nhập hoặc mật khẩu không đúng.");
    }
    if (!Boolean.TRUE.equals(account.getIsActive())) {
      throw ApiException.forbidden("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
    }
    return account;
  }

  public AuthResponse login(LoginRequest request) {
    Account account = authenticate(request);
    String roleName = roleName(account);
    return new AuthResponse(
        jwtService.generate(account, roleName), toUserResponse(account, roleName));
  }

  public AuthResponse loginUser(LoginRequest request) {
    Account account = authenticate(request);
    String roleName = roleName(account);
    if (!"KhachHang".equalsIgnoreCase(roleName) && !"Customer".equalsIgnoreCase(roleName)) {
      throw ApiException.forbidden(
          "Tài khoản quản trị/nhân viên vui lòng đăng nhập tại cổng Quản trị viên.");
    }
    return new AuthResponse(
        jwtService.generate(account, roleName), toUserResponse(account, roleName));
  }

  public AuthResponse loginAdmin(LoginRequest request) {
    Account account = authenticate(request);
    String roleName = roleName(account);
    if ("KhachHang".equalsIgnoreCase(roleName) || "Customer".equalsIgnoreCase(roleName)) {
      throw ApiException.forbidden("Tài khoản khách hàng không có quyền truy cập quản trị.");
    }
    return new AuthResponse(
        jwtService.generate(account, roleName), toUserResponse(account, roleName));
  }

  @Transactional
  public Map<String, String> register(RegisterRequest request) {
    Validators.requireText(request.username(), "Vui lòng nhập tên đăng nhập.");
    if (request.username().trim().length() < 6) {
      throw ApiException.badRequest("Tên đăng nhập phải có ít nhất 6 ký tự.");
    }
    Validators.requireEmail(request.email());
    Validators.requirePassword(request.password());
    if (!request.password().equals(request.confirmPassword())) {
      throw ApiException.badRequest("Mật khẩu không khớp.");
    }
    if (accountRepository.existsByUsername(request.username().trim())) {
      throw ApiException.conflict("Tên đăng nhập đã được sử dụng.");
    }
    if (accountRepository.existsByEmail(request.email().trim())) {
      throw ApiException.conflict("Email đã được sử dụng.");
    }

    Account account = new Account();
    account.setUsername(request.username().trim());
    account.setEmail(request.email().trim());
    account.setPassword(passwordEncoder.encode(request.password()));
    account.setRoleId(ROLE_CUSTOMER_ID);
    account.setIsActive(true);
    accountRepository.save(account);
    return Map.of("message", "Đăng ký thành công.");
  }

  @Transactional
  public Map<String, String> forgotPassword(ForgotPasswordRequest request) {
    Validators.requireEmail(request.email());
    if (Validators.isBlank(request.username()) || request.username().trim().length() < 6) {
      throw ApiException.badRequest("Tên đăng nhập phải có ít nhất 6 ký tự.");
    }
    Account account =
        accountRepository
            .findByUsernameAndEmail(request.username().trim(), request.email().trim())
            .orElseThrow(() -> ApiException.notFound("Tên đăng nhập hoặc Email không tồn tại."));

    String tempPassword = generatePassword();
    account.setPassword(passwordEncoder.encode(tempPassword));
    accountRepository.save(account);
    return Map.of(
        "message",
        "Đã đặt lại mật khẩu. Môi trường demo trả mật khẩu tạm trực tiếp.",
        "matKhauTam",
        tempPassword,
        "temporaryPassword",
        tempPassword);
  }

  public UserResponse me(Account account) {
    return toUserResponse(account, roleName(account));
  }

  @Transactional
  public Map<String, String> changePassword(Account account, ChangePasswordRequest request) {
    if (!passwordEncoder.matches(request.oldPassword(), account.getPassword())) {
      throw ApiException.badRequest("Mật khẩu hiện tại không đúng.");
    }
    Validators.requirePassword(request.newPassword());
    account.setPassword(passwordEncoder.encode(request.newPassword()));
    accountRepository.save(account);
    return Map.of("message", "Đổi mật khẩu thành công.");
  }

  public UserResponse toUserResponse(Account account, String roleName) {
    Employee employee = employeeRepository.findByAccountId(account.getAccountId()).orElse(null);
    Customer customer = customerRepository.findByAccountId(account.getAccountId()).orElse(null);
    String fullName =
        employee != null
            ? employee.getFullName()
            : customer != null ? customer.getFullName() : account.getUsername();
    return new UserResponse(
        account.getAccountId(),
        account.getUsername(),
        account.getEmail(),
        roleName,
        account.getRoleId(),
        employee == null ? null : employee.getEmployeeId(),
        customer == null ? null : customer.getCustomerId(),
        fullName,
        account.getIsActive());
  }

  public String roleName(Account account) {
    return roleRepository.findById(account.getRoleId()).map(Role::getRoleName).orElse("KhachHang");
  }

  private String generatePassword() {
    SecureRandom random = new SecureRandom();
    List<Character> characters = new ArrayList<>();
    characters.add(UPPER.charAt(random.nextInt(UPPER.length())));
    characters.add(LOWER.charAt(random.nextInt(LOWER.length())));
    characters.add(DIGITS.charAt(random.nextInt(DIGITS.length())));
    characters.add(SPECIAL.charAt(random.nextInt(SPECIAL.length())));
    String all = UPPER + LOWER + DIGITS + SPECIAL;
    while (characters.size() < 10) {
      characters.add(all.charAt(random.nextInt(all.length())));
    }
    Collections.shuffle(characters, random);
    StringBuilder builder = new StringBuilder();
    characters.forEach(builder::append);
    return builder.toString();
  }

  public record LoginRequest(
      @NotBlank(message = "Vui lòng nhập tên đăng nhập.") @JsonAlias({"tenDangNhap", "username"})
          String username,
      @NotBlank(message = "Vui lòng nhập mật khẩu.") @JsonAlias({"matKhau", "password"})
          String password) {}

  public record RegisterRequest(
      @NotBlank(message = "Vui lòng nhập tên đăng nhập.") @JsonAlias({"tenDangNhap", "username"})
          String username,
      @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
          String email,
      @NotBlank(message = "Vui lòng nhập mật khẩu.") @JsonAlias({"matKhau", "password"})
          String password,
      @NotBlank(message = "Vui lòng xác nhận mật khẩu.")
          @JsonAlias({"xacNhanMatKhau", "confirmPassword"})
          String confirmPassword) {}

  public record ForgotPasswordRequest(
      @NotBlank(message = "Vui lòng nhập tên đăng nhập.") @JsonAlias({"tenDangNhap", "username"})
          String username,
      @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
          String email) {}

  public record ChangePasswordRequest(
      @NotBlank(message = "Vui lòng nhập mật khẩu hiện tại.")
          @JsonAlias({"matKhauCu", "oldPassword"})
          String oldPassword,
      @NotBlank(message = "Vui lòng nhập mật khẩu mới.") @JsonAlias({"matKhauMoi", "newPassword"})
          String newPassword) {}

  public record UserResponse(
      Integer accountId,
      String username,
      String email,
      String role,
      Integer roleId,
      Integer employeeId,
      Integer customerId,
      String fullName,
      Boolean isActive) {
    public Integer getMaTk() {
      return accountId;
    }

    public String getTenDangNhap() {
      return username;
    }

    public String getQuyen() {
      return role;
    }

    public Integer getMaQuyen() {
      return roleId;
    }

    public Integer getMaNv() {
      return employeeId;
    }

    public Integer getMaKh() {
      return customerId;
    }

    public String getHoTen() {
      return fullName;
    }

    public Boolean getTrangThai() {
      return isActive;
    }
  }

  public record AuthResponse(String token, UserResponse user) {
    public UserResponse getNguoiDung() {
      return user;
    }
  }
}
