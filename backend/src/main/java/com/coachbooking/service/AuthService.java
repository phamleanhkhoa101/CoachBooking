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

  public static class LoginRequest {
    @NotBlank(message = "Vui lòng nhập tên đăng nhập.")
    private String username;

    @NotBlank(message = "Vui lòng nhập mật khẩu.")
    private String password;

    public LoginRequest() {}

    public LoginRequest(String username, String password) {
      this.username = username;
      this.password = password;
    }

    public String username() {
      return username;
    }

    public String password() {
      return password;
    }

    public String getUsername() {
      return username;
    }

    public void setUsername(String username) {
      if (this.username == null || !username.isBlank()) {
        this.username = username;
      }
    }

    public void setTenDangNhap(String tenDangNhap) {
      if (this.username == null || this.username.isBlank()) {
        this.username = tenDangNhap;
      }
    }

    public String getPassword() {
      return password;
    }

    public void setPassword(String password) {
      if (this.password == null || !password.isBlank()) {
        this.password = password;
      }
    }

    public void setMatKhau(String matKhau) {
      if (this.password == null || this.password.isBlank()) {
        this.password = matKhau;
      }
    }
  }

  public static class RegisterRequest {
    @NotBlank(message = "Vui lòng nhập tên đăng nhập.")
    private String username;

    @NotBlank(message = "Vui lòng nhập email.")
    @Email(message = "Email không đúng định dạng.")
    private String email;

    @NotBlank(message = "Vui lòng nhập mật khẩu.")
    private String password;

    @NotBlank(message = "Vui lòng xác nhận mật khẩu.")
    private String confirmPassword;

    public RegisterRequest() {}

    public RegisterRequest(String username, String email, String password, String confirmPassword) {
      this.username = username;
      this.email = email;
      this.password = password;
      this.confirmPassword = confirmPassword;
    }

    public String username() {
      return username;
    }

    public String email() {
      return email;
    }

    public String password() {
      return password;
    }

    public String confirmPassword() {
      return confirmPassword;
    }

    public String getUsername() {
      return username;
    }

    public void setUsername(String username) {
      if (this.username == null || !username.isBlank()) {
        this.username = username;
      }
    }

    public void setTenDangNhap(String tenDangNhap) {
      if (this.username == null || this.username.isBlank()) {
        this.username = tenDangNhap;
      }
    }

    public String getEmail() {
      return email;
    }

    public void setEmail(String email) {
      this.email = email;
    }

    public String getPassword() {
      return password;
    }

    public void setPassword(String password) {
      if (this.password == null || !password.isBlank()) {
        this.password = password;
      }
    }

    public void setMatKhau(String matKhau) {
      if (this.password == null || this.password.isBlank()) {
        this.password = matKhau;
      }
    }

    public String getConfirmPassword() {
      return confirmPassword;
    }

    public void setConfirmPassword(String confirmPassword) {
      if (this.confirmPassword == null || !confirmPassword.isBlank()) {
        this.confirmPassword = confirmPassword;
      }
    }

    public void setXacNhanMatKhau(String xacNhanMatKhau) {
      if (this.confirmPassword == null || this.confirmPassword.isBlank()) {
        this.confirmPassword = xacNhanMatKhau;
      }
    }
  }

  public static class ForgotPasswordRequest {
    @NotBlank(message = "Vui lòng nhập tên đăng nhập.")
    private String username;

    @NotBlank(message = "Vui lòng nhập email.")
    @Email(message = "Email không đúng định dạng.")
    private String email;

    public ForgotPasswordRequest() {}

    public ForgotPasswordRequest(String username, String email) {
      this.username = username;
      this.email = email;
    }

    public String username() {
      return username;
    }

    public String email() {
      return email;
    }

    public String getUsername() {
      return username;
    }

    public void setUsername(String username) {
      if (this.username == null || !username.isBlank()) {
        this.username = username;
      }
    }

    public void setTenDangNhap(String tenDangNhap) {
      if (this.username == null || this.username.isBlank()) {
        this.username = tenDangNhap;
      }
    }

    public String getEmail() {
      return email;
    }

    public void setEmail(String email) {
      this.email = email;
    }
  }

  public static class ChangePasswordRequest {
    @NotBlank(message = "Vui lòng nhập mật khẩu hiện tại.")
    private String oldPassword;

    @NotBlank(message = "Vui lòng nhập mật khẩu mới.")
    private String newPassword;

    public ChangePasswordRequest() {}

    public ChangePasswordRequest(String oldPassword, String newPassword) {
      this.oldPassword = oldPassword;
      this.newPassword = newPassword;
    }

    public String oldPassword() {
      return oldPassword;
    }

    public String newPassword() {
      return newPassword;
    }

    public String getOldPassword() {
      return oldPassword;
    }

    public void setOldPassword(String oldPassword) {
      if (this.oldPassword == null || !oldPassword.isBlank()) {
        this.oldPassword = oldPassword;
      }
    }

    public void setMatKhauCu(String matKhauCu) {
      if (this.oldPassword == null || this.oldPassword.isBlank()) {
        this.oldPassword = matKhauCu;
      }
    }

    public String getNewPassword() {
      return newPassword;
    }

    public void setNewPassword(String newPassword) {
      if (this.newPassword == null || !newPassword.isBlank()) {
        this.newPassword = newPassword;
      }
    }

    public void setMatKhauMoi(String matKhauMoi) {
      if (this.newPassword == null || this.newPassword.isBlank()) {
        this.newPassword = matKhauMoi;
      }
    }
  }

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
