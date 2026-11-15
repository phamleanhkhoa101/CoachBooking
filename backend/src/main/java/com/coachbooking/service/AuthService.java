package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Employee;
import com.coachbooking.domain.PasswordResetToken;
import com.coachbooking.domain.Role;
import com.coachbooking.dto.auth.AuthResponse;
import com.coachbooking.dto.auth.ChangePasswordRequest;
import com.coachbooking.dto.auth.ForgotPasswordRequest;
import com.coachbooking.dto.auth.LoginRequest;
import com.coachbooking.dto.auth.RegisterRequest;
import com.coachbooking.dto.auth.ResetPasswordRequest;
import com.coachbooking.dto.auth.UserResponse;
import com.coachbooking.dto.auth.VerifyResetOtpRequest;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.PasswordResetTokenRepository;
import com.coachbooking.repo.RoleRepository;
import com.coachbooking.security.JwtService;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.time.LocalDateTime;
import java.util.HexFormat;
import java.util.Map;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class AuthService {

  private static final int ROLE_CUSTOMER_ID = 3;
  private static final SecureRandom SECURE_RANDOM = new SecureRandom();

  private final AccountRepository accountRepository;
  private final RoleRepository roleRepository;
  private final EmployeeRepository employeeRepository;
  private final CustomerRepository customerRepository;
  private final PasswordResetTokenRepository passwordResetTokenRepository;
  private final EmailService emailService;
  private final PasswordEncoder passwordEncoder;
  private final JwtService jwtService;
  private final int resetExpirationMinutes;
  private final int resetCooldownSeconds;
  private final int resetMaxAttempts;

  public AuthService(
      AccountRepository accountRepository,
      RoleRepository roleRepository,
      EmployeeRepository employeeRepository,
      CustomerRepository customerRepository,
      PasswordResetTokenRepository passwordResetTokenRepository,
      EmailService emailService,
      PasswordEncoder passwordEncoder,
      JwtService jwtService) {
    this(
        accountRepository,
        roleRepository,
        employeeRepository,
        customerRepository,
        passwordResetTokenRepository,
        emailService,
        passwordEncoder,
        jwtService,
        15,
        60,
        5);
  }

  @Autowired
  public AuthService(
      AccountRepository accountRepository,
      RoleRepository roleRepository,
      EmployeeRepository employeeRepository,
      CustomerRepository customerRepository,
      PasswordResetTokenRepository passwordResetTokenRepository,
      EmailService emailService,
      PasswordEncoder passwordEncoder,
      JwtService jwtService,
      @Value("${app.password-reset.expiration-minutes:15}") int resetExpirationMinutes,
      @Value("${app.password-reset.cooldown-seconds:60}") int resetCooldownSeconds,
      @Value("${app.password-reset.max-attempts:5}") int resetMaxAttempts) {
    this.accountRepository = accountRepository;
    this.roleRepository = roleRepository;
    this.employeeRepository = employeeRepository;
    this.customerRepository = customerRepository;
    this.passwordResetTokenRepository = passwordResetTokenRepository;
    this.emailService = emailService;
    this.passwordEncoder = passwordEncoder;
    this.jwtService = jwtService;
    this.resetExpirationMinutes = resetExpirationMinutes;
    this.resetCooldownSeconds = resetCooldownSeconds;
    this.resetMaxAttempts = resetMaxAttempts;
  }

  private Account authenticate(LoginRequest request) {
    Account account =
        accountRepository
            .findByUsername(request.username().trim())
            .orElseThrow(() -> ApiException.badRequest("Tên đăng nhập hoặc mật khẩu không đúng."));
    if (account.getPassword() == null
        || !passwordEncoder.matches(request.password(), account.getPassword())) {
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

  @Transactional
  public AuthResponse loginWithGoogle(String googleSub, String email, boolean emailVerified) {
    if (Validators.isBlank(googleSub) || Validators.isBlank(email) || !emailVerified) {
      throw ApiException.badRequest("Tài khoản Google chưa xác thực email hoặc thiếu thông tin.");
    }
    String normalizedEmail = email.trim();
    String normalizedSub = googleSub.trim();

    Account account =
        accountRepository
            .findByGoogleSub(normalizedSub)
            .orElseGet(() -> accountRepository.findByEmailIgnoreCase(normalizedEmail).orElse(null));

    if (account == null) {
      account = new Account();
      account.setUsername(generateUniqueUsernameFromEmail(normalizedEmail));
      account.setEmail(normalizedEmail);
      account.setPassword(null);
      account.setRoleId(ROLE_CUSTOMER_ID);
      account.setIsActive(true);
      account.setAuthProvider("GOOGLE");
      account.setGoogleSub(normalizedSub);
      account = accountRepository.save(account);
    }

    if (!Boolean.TRUE.equals(account.getIsActive())) {
      throw ApiException.forbidden("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
    }
    String roleName = roleName(account);
    if (!"KhachHang".equalsIgnoreCase(roleName) && !"Customer".equalsIgnoreCase(roleName)) {
      throw ApiException.forbidden(
          "Tài khoản quản trị/nhân viên vui lòng đăng nhập tại cổng Quản trị viên.");
    }
    if (Validators.isBlank(account.getGoogleSub())) {
      account.setGoogleSub(normalizedSub);
      account = accountRepository.save(account);
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
    Account account = requireActiveAccountByEmail(request.email());
    LocalDateTime now = LocalDateTime.now();

    passwordResetTokenRepository
        .findTopByAccountIdOrderByCreatedAtDesc(account.getAccountId())
        .ifPresent(
            latest -> {
              if (latest.getCreatedAt().plusSeconds(resetCooldownSeconds).isAfter(now)) {
                throw ApiException.badRequest(
                    "Vui lòng đợi ít nhất "
                        + resetCooldownSeconds
                        + " giây trước khi yêu cầu mã OTP mới.");
              }
            });

    passwordResetTokenRepository.invalidateActiveTokens(account.getAccountId(), now);

    String otp = generateOtp();
    PasswordResetToken token = new PasswordResetToken();
    token.setAccountId(account.getAccountId());
    token.setTokenHash(sha256Hex(otp));
    token.setCreatedAt(now);
    token.setExpiresAt(now.plusMinutes(resetExpirationMinutes));
    token.setAttempts(0);
    passwordResetTokenRepository.save(token);

    emailService.sendPasswordResetOtp(
        account.getEmail(), account.getUsername(), otp, resetExpirationMinutes);

    return Map.of(
        "message", "Mã xác nhận OTP đã được gửi tới email của bạn. Vui lòng kiểm tra hộp thư.");
  }

  @Transactional(noRollbackFor = ApiException.class)
  public Map<String, String> verifyResetOtp(VerifyResetOtpRequest request) {
    Account account = requireActiveAccountByEmail(request.email());
    if (Validators.isBlank(request.otp()) || !request.otp().trim().matches("\\d{6}")) {
      throw ApiException.badRequest("Mã OTP phải gồm 6 chữ số.");
    }

    LocalDateTime now = LocalDateTime.now();
    PasswordResetToken token = requireActiveResetToken(account.getAccountId(), now);

    byte[] expectedHash = token.getTokenHash().getBytes(StandardCharsets.UTF_8);
    byte[] actualHash = sha256Hex(request.otp().trim()).getBytes(StandardCharsets.UTF_8);
    if (!MessageDigest.isEqual(expectedHash, actualHash)) {
      int attempts = (token.getAttempts() == null ? 0 : token.getAttempts()) + 1;
      token.setAttempts(attempts);
      if (attempts >= resetMaxAttempts) {
        token.setUsedAt(now);
        passwordResetTokenRepository.save(token);
        throw ApiException.badRequest(
            "Bạn đã nhập sai mã OTP quá số lần cho phép. Vui lòng yêu cầu mã mới.");
      }
      passwordResetTokenRepository.save(token);
      throw ApiException.badRequest("Mã OTP không chính xác.");
    }

    String resetToken = generateResetToken();
    token.setVerifiedAt(now);
    token.setResetTokenHash(sha256Hex(resetToken));
    passwordResetTokenRepository.save(token);

    return Map.of("message", "Xác thực OTP thành công.", "resetToken", resetToken);
  }

  @Transactional
  public Map<String, String> resetPassword(ResetPasswordRequest request) {
    Account account = requireActiveAccountByEmail(request.email());
    if (Validators.isBlank(request.resetToken())) {
      throw ApiException.badRequest("Phiên xác thực OTP không hợp lệ.");
    }
    Validators.requirePassword(request.newPassword());

    LocalDateTime now = LocalDateTime.now();
    PasswordResetToken token = requireActiveResetToken(account.getAccountId(), now);
    if (token.getVerifiedAt() == null || token.getResetTokenHash() == null) {
      throw ApiException.badRequest("Vui lòng xác thực mã OTP trước khi đặt lại mật khẩu.");
    }

    byte[] expectedHash = token.getResetTokenHash().getBytes(StandardCharsets.UTF_8);
    byte[] actualHash = sha256Hex(request.resetToken().trim()).getBytes(StandardCharsets.UTF_8);
    if (!MessageDigest.isEqual(expectedHash, actualHash)) {
      throw ApiException.badRequest("Phiên xác thực không hợp lệ hoặc đã hết hạn.");
    }

    account.setPassword(passwordEncoder.encode(request.newPassword()));
    accountRepository.save(account);
    token.setUsedAt(now);
    passwordResetTokenRepository.save(token);

    return Map.of(
        "message", "Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới.");
  }

  private Account requireActiveAccountByEmail(String email) {
    Validators.requireEmail(email);
    Account account =
        accountRepository
            .findByEmailIgnoreCase(email.trim())
            .orElseThrow(() -> ApiException.notFound("Email không tồn tại trong hệ thống."));
    if (!Boolean.TRUE.equals(account.getIsActive())) {
      throw ApiException.forbidden("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
    }
    return account;
  }

  private PasswordResetToken requireActiveResetToken(Integer accountId, LocalDateTime now) {
    PasswordResetToken token =
        passwordResetTokenRepository
            .findTopByAccountIdAndUsedAtIsNullOrderByCreatedAtDesc(accountId)
            .orElseThrow(
                () ->
                    ApiException.badRequest(
                        "Mã OTP không hợp lệ hoặc đã hết hạn. Vui lòng yêu cầu mã mới."));
    if (token.getExpiresAt().isBefore(now)) {
      token.setUsedAt(now);
      passwordResetTokenRepository.save(token);
      throw ApiException.badRequest("Mã OTP đã hết hạn. Vui lòng yêu cầu mã mới.");
    }
    return token;
  }

  public UserResponse me(Account account) {
    return toUserResponse(account, roleName(account));
  }

  @Transactional
  public Map<String, String> changePassword(Account account, ChangePasswordRequest request) {
    if (account.getPassword() != null
        && !passwordEncoder.matches(request.oldPassword(), account.getPassword())) {
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

  private String generateUniqueUsernameFromEmail(String email) {
    String prefix = email.split("@", 2)[0].replaceAll("[^a-zA-Z0-9._-]", "");
    if (prefix.length() < 3) {
      prefix = "google_" + prefix;
    }
    String base = prefix.length() > 40 ? prefix.substring(0, 40) : prefix;
    String candidate = base;
    int suffix = 1;
    while (accountRepository.existsByUsername(candidate)) {
      candidate = base + "_" + suffix++;
    }
    return candidate;
  }

  private String generateOtp() {
    return String.format("%06d", SECURE_RANDOM.nextInt(1_000_000));
  }

  private String generateResetToken() {
    byte[] bytes = new byte[32];
    SECURE_RANDOM.nextBytes(bytes);
    return HexFormat.of().formatHex(bytes);
  }

  private String sha256Hex(String value) {
    try {
      MessageDigest digest = MessageDigest.getInstance("SHA-256");
      return HexFormat.of().formatHex(digest.digest(value.getBytes(StandardCharsets.UTF_8)));
    } catch (NoSuchAlgorithmException ex) {
      throw new IllegalStateException("SHA-256 algorithm unavailable", ex);
    }
  }
}
