package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.PasswordResetToken;
import com.coachbooking.domain.Role;
import com.coachbooking.dto.auth.*;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.PasswordResetTokenRepository;
import com.coachbooking.repo.RoleRepository;
import com.coachbooking.security.JwtService;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.time.LocalDateTime;
import java.util.HexFormat;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
import org.springframework.http.HttpStatus;
import org.springframework.security.crypto.password.PasswordEncoder;

class AuthServiceTest {

  private AccountRepository accountRepository;
  private RoleRepository roleRepository;
  private EmployeeRepository employeeRepository;
  private CustomerRepository customerRepository;
  private PasswordResetTokenRepository passwordResetTokenRepository;
  private EmailService emailService;
  private PasswordEncoder passwordEncoder;
  private JwtService jwtService;
  private AuthService authService;

  @BeforeEach
  void setUp() {
    accountRepository = mock(AccountRepository.class);
    roleRepository = mock(RoleRepository.class);
    employeeRepository = mock(EmployeeRepository.class);
    customerRepository = mock(CustomerRepository.class);
    passwordResetTokenRepository = mock(PasswordResetTokenRepository.class);
    emailService = mock(EmailService.class);
    passwordEncoder = mock(PasswordEncoder.class);
    jwtService = mock(JwtService.class);

    authService =
        new AuthService(
            accountRepository,
            roleRepository,
            employeeRepository,
            customerRepository,
            passwordResetTokenRepository,
            emailService,
            passwordEncoder,
            jwtService);
  }

  private Account createAccount(int accountId, String username, int roleId, boolean isActive) {
    Account acc = new Account();
    acc.setAccountId(accountId);
    acc.setUsername(username);
    acc.setEmail(username + "@example.com");
    acc.setPassword("encoded_secret");
    acc.setRoleId(roleId);
    acc.setIsActive(isActive);
    return acc;
  }

  @Test
  void loginUser_success_whenCustomer() {
    Account acc = createAccount(1, "kh1", 3, true);
    when(accountRepository.findByUsername("kh1")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Role role = new Role();
    role.setRoleId(3);
    role.setRoleName("KhachHang");
    when(roleRepository.findById(3)).thenReturn(Optional.of(role));
    when(jwtService.generate(acc, "KhachHang")).thenReturn("mock_token");

    AuthResponse response = authService.loginUser(new LoginRequest("kh1", "pass123"));

    assertNotNull(response);
    assertEquals("mock_token", response.token());
    assertEquals("KhachHang", response.user().role());
  }

  @Test
  void loginUser_forbidden_whenAdmin() {
    Account acc = createAccount(2, "admin", 1, true);
    when(accountRepository.findByUsername("admin")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Role role = new Role();
    role.setRoleId(1);
    role.setRoleName("Admin");
    when(roleRepository.findById(1)).thenReturn(Optional.of(role));

    ApiException ex =
        assertThrows(
            ApiException.class, () -> authService.loginUser(new LoginRequest("admin", "pass123")));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
    assertTrue(ex.getMessage().contains("Quản trị viên"));
  }

  @Test
  void loginAdmin_success_whenAdminOrEmployee() {
    Account acc = createAccount(2, "admin", 1, true);
    when(accountRepository.findByUsername("admin")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Role role = new Role();
    role.setRoleId(1);
    role.setRoleName("Admin");
    when(roleRepository.findById(1)).thenReturn(Optional.of(role));
    when(jwtService.generate(acc, "Admin")).thenReturn("admin_token");

    AuthResponse response = authService.loginAdmin(new LoginRequest("admin", "pass123"));

    assertNotNull(response);
    assertEquals("admin_token", response.token());
    assertEquals("Admin", response.user().role());
  }

  @Test
  void loginAdmin_forbidden_whenCustomer() {
    Account acc = createAccount(1, "kh1", 3, true);
    when(accountRepository.findByUsername("kh1")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Role role = new Role();
    role.setRoleId(3);
    role.setRoleName("KhachHang");
    when(roleRepository.findById(3)).thenReturn(Optional.of(role));

    ApiException ex =
        assertThrows(
            ApiException.class, () -> authService.loginAdmin(new LoginRequest("kh1", "pass123")));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
    assertTrue(ex.getMessage().contains("không có quyền truy cập quản trị"));
  }

  @Test
  void authenticate_badRequest_whenWrongPassword() {
    Account acc = createAccount(1, "kh1", 3, true);
    when(accountRepository.findByUsername("kh1")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("wrong", "encoded_secret")).thenReturn(false);

    ApiException ex =
        assertThrows(ApiException.class, () -> authService.login(new LoginRequest("kh1", "wrong")));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
  }

  @Test
  void authenticate_forbidden_whenAccountInactive() {
    Account acc = createAccount(1, "kh1", 3, false);
    when(accountRepository.findByUsername("kh1")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    ApiException ex =
        assertThrows(
            ApiException.class, () -> authService.login(new LoginRequest("kh1", "pass123")));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
  }

  @Test
  void registerSuccessAndValidations() {
    RegisterRequest req =
        new RegisterRequest("newuser", "user@test.com", "Password@123", "Password@123");
    when(passwordEncoder.encode("Password@123")).thenReturn("encoded");
    when(accountRepository.existsByUsername("newuser")).thenReturn(false);
    when(accountRepository.existsByEmail("user@test.com")).thenReturn(false);

    Map<String, String> res = authService.register(req);
    assertEquals("Đăng ký thành công.", res.get("message"));
    verify(accountRepository).save(any(Account.class));

    // username short
    RegisterRequest shortUser =
        new RegisterRequest("user", "user@test.com", "Password@123", "Password@123");
    ApiException exShort = assertThrows(ApiException.class, () -> authService.register(shortUser));
    assertEquals(HttpStatus.BAD_REQUEST, exShort.getStatus());

    // password mismatch
    RegisterRequest mismatch =
        new RegisterRequest("newuser", "user@test.com", "Password@123", "Wrong@123");
    ApiException exMis = assertThrows(ApiException.class, () -> authService.register(mismatch));
    assertEquals(HttpStatus.BAD_REQUEST, exMis.getStatus());

    // duplicate username
    when(accountRepository.existsByUsername("newuser")).thenReturn(true);
    ApiException exDup = assertThrows(ApiException.class, () -> authService.register(req));
    assertEquals(HttpStatus.CONFLICT, exDup.getStatus());
  }

  @Test
  void forgotPasswordSuccessAndValidation() {
    ForgotPasswordRequest req = new ForgotPasswordRequest("user@test.com");
    Account acc = createAccount(1, "validuser", 3, true);
    acc.setEmail("user@test.com");
    when(accountRepository.findByEmailIgnoreCase("user@test.com")).thenReturn(Optional.of(acc));
    when(passwordResetTokenRepository.findTopByAccountIdOrderByCreatedAtDesc(1))
        .thenReturn(Optional.empty());

    Map<String, String> res = authService.forgotPassword(req);
    assertEquals(
        "Mã xác nhận OTP đã được gửi tới email của bạn. Vui lòng kiểm tra hộp thư.",
        res.get("message"));
    assertNull(res.get("temporaryPassword"));
    assertNull(res.get("matKhauTam"));

    ArgumentCaptor<String> otpCaptor = ArgumentCaptor.forClass(String.class);
    verify(passwordResetTokenRepository).invalidateActiveTokens(eq(1), any(LocalDateTime.class));
    verify(passwordResetTokenRepository).save(any(PasswordResetToken.class));
    verify(emailService)
        .sendPasswordResetOtp(eq("user@test.com"), eq("validuser"), otpCaptor.capture(), eq(15));
    assertTrue(otpCaptor.getValue().matches("\\d{6}"));

    // cooldown check
    PasswordResetToken recent = new PasswordResetToken();
    recent.setCreatedAt(LocalDateTime.now().minusSeconds(10));
    when(passwordResetTokenRepository.findTopByAccountIdOrderByCreatedAtDesc(1))
        .thenReturn(Optional.of(recent));
    ApiException cooldown = assertThrows(ApiException.class, () -> authService.forgotPassword(req));
    assertEquals(HttpStatus.BAD_REQUEST, cooldown.getStatus());

    // inactive account
    acc.setIsActive(false);
    ApiException locked = assertThrows(ApiException.class, () -> authService.forgotPassword(req));
    assertEquals(HttpStatus.FORBIDDEN, locked.getStatus());

    // email not found
    when(accountRepository.findByEmailIgnoreCase("user@test.com")).thenReturn(Optional.empty());
    ApiException notFound = assertThrows(ApiException.class, () -> authService.forgotPassword(req));
    assertEquals(HttpStatus.NOT_FOUND, notFound.getStatus());
  }

  @Test
  void verifyOtpAndResetPasswordFlow() throws Exception {
    Account acc = createAccount(1, "validuser", 3, true);
    acc.setEmail("user@test.com");
    when(accountRepository.findByEmailIgnoreCase("user@test.com")).thenReturn(Optional.of(acc));

    String otp = "123456";
    String otpHash =
        HexFormat.of()
            .formatHex(
                MessageDigest.getInstance("SHA-256").digest(otp.getBytes(StandardCharsets.UTF_8)));

    PasswordResetToken token = new PasswordResetToken();
    token.setTokenId(10L);
    token.setAccountId(1);
    token.setTokenHash(otpHash);
    token.setCreatedAt(LocalDateTime.now().minusMinutes(1));
    token.setExpiresAt(LocalDateTime.now().plusMinutes(14));
    token.setAttempts(0);

    when(passwordResetTokenRepository.findTopByAccountIdAndUsedAtIsNullOrderByCreatedAtDesc(1))
        .thenReturn(Optional.of(token));

    // Step 2: verify OTP succeeds and issues resetToken
    Map<String, String> verifyRes =
        authService.verifyResetOtp(new VerifyResetOtpRequest("user@test.com", otp));
    assertEquals("Xác thực OTP thành công.", verifyRes.get("message"));
    String resetToken = verifyRes.get("resetToken");
    assertNotNull(resetToken);
    assertNotNull(token.getVerifiedAt());
    assertNotNull(token.getResetTokenHash());
    assertNull(token.getUsedAt());

    // Step 3: reset password with invalid resetToken fails
    ResetPasswordRequest badTokenReq =
        new ResetPasswordRequest("user@test.com", "wrong-token", "NewPass@123");
    ApiException badTokenEx =
        assertThrows(ApiException.class, () -> authService.resetPassword(badTokenReq));
    assertEquals(HttpStatus.BAD_REQUEST, badTokenEx.getStatus());

    // Step 3: reset password with valid resetToken succeeds
    when(passwordEncoder.encode("NewPass@123")).thenReturn("new_hash");
    ResetPasswordRequest validResetReq =
        new ResetPasswordRequest("user@test.com", resetToken, "NewPass@123");
    Map<String, String> resetRes = authService.resetPassword(validResetReq);
    assertEquals(
        "Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới.",
        resetRes.get("message"));
    assertEquals("new_hash", acc.getPassword());
    assertNotNull(token.getUsedAt());
    verify(accountRepository).save(acc);

    // wrong OTP increments attempts and locks after maxAttempts (5)
    PasswordResetToken tokenWrong = new PasswordResetToken();
    tokenWrong.setTokenId(11L);
    tokenWrong.setAccountId(1);
    tokenWrong.setTokenHash(otpHash);
    tokenWrong.setCreatedAt(LocalDateTime.now());
    tokenWrong.setExpiresAt(LocalDateTime.now().plusMinutes(10));
    tokenWrong.setAttempts(4);
    when(passwordResetTokenRepository.findTopByAccountIdAndUsedAtIsNullOrderByCreatedAtDesc(1))
        .thenReturn(Optional.of(tokenWrong));

    VerifyResetOtpRequest wrongOtpReq = new VerifyResetOtpRequest("user@test.com", "654321");
    ApiException tooMany =
        assertThrows(ApiException.class, () -> authService.verifyResetOtp(wrongOtpReq));
    assertEquals(HttpStatus.BAD_REQUEST, tooMany.getStatus());
    assertEquals(5, tokenWrong.getAttempts());
    assertNotNull(tokenWrong.getUsedAt());

    // expired token
    PasswordResetToken expired = new PasswordResetToken();
    expired.setTokenId(12L);
    expired.setAccountId(1);
    expired.setTokenHash(otpHash);
    expired.setCreatedAt(LocalDateTime.now().minusMinutes(20));
    expired.setExpiresAt(LocalDateTime.now().minusMinutes(5));
    expired.setAttempts(0);
    when(passwordResetTokenRepository.findTopByAccountIdAndUsedAtIsNullOrderByCreatedAtDesc(1))
        .thenReturn(Optional.of(expired));
    ApiException expiredEx =
        assertThrows(
            ApiException.class,
            () -> authService.verifyResetOtp(new VerifyResetOtpRequest("user@test.com", otp)));
    assertEquals(HttpStatus.BAD_REQUEST, expiredEx.getStatus());
    assertNotNull(expired.getUsedAt());
  }

  @Test
  void changePasswordSuccessAndValidation() {
    Account acc = createAccount(1, "validuser", 3, true);
    ChangePasswordRequest req = new ChangePasswordRequest("old_secret", "NewPass@123");
    when(passwordEncoder.matches("old_secret", "encoded_secret")).thenReturn(true);
    when(passwordEncoder.encode("NewPass@123")).thenReturn("new_hash");

    Map<String, String> res = authService.changePassword(acc, req);
    assertEquals("Đổi mật khẩu thành công.", res.get("message"));
    verify(accountRepository).save(acc);

    // wrong old password
    ChangePasswordRequest wrongOld = new ChangePasswordRequest("wrong_old", "NewPass@123");
    ApiException ex =
        assertThrows(ApiException.class, () -> authService.changePassword(acc, wrongOld));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());

    // Google account with null password sets local password
    Account googleAcc = createAccount(2, "googleuser", 3, true);
    googleAcc.setPassword(null);
    Map<String, String> googleRes =
        authService.changePassword(googleAcc, new ChangePasswordRequest("", "NewPass@123"));
    assertEquals("Đổi mật khẩu thành công.", googleRes.get("message"));
    assertEquals("new_hash", googleAcc.getPassword());
  }

  @Test
  void loginWithGoogleCreatesNewCustomerAndLinksExistingCustomer() {
    // 1. New Google user auto-registers as KhachHang
    when(accountRepository.findByGoogleSub("sub-123")).thenReturn(Optional.empty());
    when(accountRepository.findByEmailIgnoreCase("newgoogle@gmail.com"))
        .thenReturn(Optional.empty());
    when(accountRepository.existsByUsername("newgoogle")).thenReturn(false);
    when(accountRepository.save(any(Account.class)))
        .thenAnswer(
            inv -> {
              Account saved = inv.getArgument(0);
              saved.setAccountId(99);
              return saved;
            });
    Role role = new Role();
    role.setRoleName("KhachHang");
    when(roleRepository.findById(3)).thenReturn(Optional.of(role));
    when(jwtService.generate(any(Account.class), eq("KhachHang"))).thenReturn("google-jwt");

    AuthResponse res = authService.loginWithGoogle("sub-123", "newgoogle@gmail.com", true);
    assertEquals("google-jwt", res.token());
    assertEquals("GOOGLE", Account.class.cast(verifyAndCaptureSavedAccount()).getAuthProvider());

    // 2. Existing local KhachHang with same email gets linked without losing local credentials
    Account existingLocal = createAccount(50, "local_kh", 3, true);
    existingLocal.setEmail("local@gmail.com");
    existingLocal.setAuthProvider("LOCAL");
    existingLocal.setGoogleSub(null);
    when(accountRepository.findByGoogleSub("sub-existing")).thenReturn(Optional.empty());
    when(accountRepository.findByEmailIgnoreCase("local@gmail.com"))
        .thenReturn(Optional.of(existingLocal));
    when(accountRepository.save(existingLocal)).thenReturn(existingLocal);

    AuthResponse linkedRes = authService.loginWithGoogle("sub-existing", "local@gmail.com", true);
    assertEquals("google-jwt", linkedRes.token());
    assertEquals("sub-existing", existingLocal.getGoogleSub());
    assertEquals("LOCAL", existingLocal.getAuthProvider());
    assertEquals("encoded_secret", existingLocal.getPassword());

    // 3. Short/long prefix and duplicate username collision when generating username
    when(accountRepository.findByGoogleSub("sub-short")).thenReturn(Optional.empty());
    when(accountRepository.findByEmailIgnoreCase("a!@gmail.com")).thenReturn(Optional.empty());
    when(accountRepository.existsByUsername("google_a")).thenReturn(true);
    when(accountRepository.existsByUsername("google_a_1")).thenReturn(false);

    AuthResponse shortRes = authService.loginWithGoogle("sub-short", "a!@gmail.com", true);
    assertEquals("google-jwt", shortRes.token());

    String longPrefix = "a".repeat(45) + "@gmail.com";
    when(accountRepository.findByGoogleSub("sub-long")).thenReturn(Optional.empty());
    when(accountRepository.findByEmailIgnoreCase(longPrefix)).thenReturn(Optional.empty());
    when(accountRepository.existsByUsername("a".repeat(40))).thenReturn(false);

    AuthResponse longRes = authService.loginWithGoogle("sub-long", longPrefix, true);
    assertEquals("google-jwt", longRes.token());
  }

  private Account verifyAndCaptureSavedAccount() {
    org.mockito.ArgumentCaptor<Account> captor = org.mockito.ArgumentCaptor.forClass(Account.class);
    verify(accountRepository, atLeastOnce()).save(captor.capture());
    return captor.getValue();
  }

  @Test
  void loginWithGoogleRejectsUnverifiedLockedAndAdminAccounts() {
    ApiException unverified =
        assertThrows(
            ApiException.class, () -> authService.loginWithGoogle("sub-1", "a@gmail.com", false));
    assertEquals(HttpStatus.BAD_REQUEST, unverified.getStatus());

    Account locked = createAccount(10, "locked", 3, false);
    when(accountRepository.findByGoogleSub("sub-locked")).thenReturn(Optional.of(locked));
    ApiException lockedEx =
        assertThrows(
            ApiException.class,
            () -> authService.loginWithGoogle("sub-locked", "locked@gmail.com", true));
    assertEquals(HttpStatus.FORBIDDEN, lockedEx.getStatus());

    Account adminAcc = createAccount(11, "admin", 1, true);
    Role adminRole = new Role();
    adminRole.setRoleName("Admin");
    when(accountRepository.findByGoogleSub("sub-admin")).thenReturn(Optional.of(adminAcc));
    when(roleRepository.findById(1)).thenReturn(Optional.of(adminRole));
    ApiException adminEx =
        assertThrows(
            ApiException.class,
            () -> authService.loginWithGoogle("sub-admin", "admin@gmail.com", true));
    assertEquals(HttpStatus.FORBIDDEN, adminEx.getStatus());
  }

  @Test
  void passwordLoginRejectsGoogleAccountWithoutLocalPassword() {
    Account googleOnly = createAccount(12, "guser", 3, true);
    googleOnly.setPassword(null);
    when(accountRepository.findByUsername("guser")).thenReturn(Optional.of(googleOnly));

    ApiException ex =
        assertThrows(
            ApiException.class, () -> authService.login(new LoginRequest("guser", "SomePass@1")));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
  }

  @Test
  void meReturnsExpectedUserResponse() {
    Account acc = createAccount(1, "validuser", 3, true);
    Customer cust = new Customer();
    cust.setCustomerId(5);
    cust.setFullName("Customer Full Name");
    when(customerRepository.findByAccountId(1)).thenReturn(Optional.of(cust));

    UserResponse me = authService.me(acc);
    assertNotNull(me);
    assertEquals(1, me.accountId());
    assertEquals("validuser", me.username());
    assertEquals("Customer Full Name", me.fullName());
    assertEquals(5, me.customerId());
  }
}
