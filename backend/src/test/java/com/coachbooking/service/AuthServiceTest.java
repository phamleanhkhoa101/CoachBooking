package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Customer;
import com.coachbooking.domain.Role;
import com.coachbooking.dto.auth.*;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.RoleRepository;
import com.coachbooking.security.JwtService;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.security.crypto.password.PasswordEncoder;

class AuthServiceTest {

  private AccountRepository accountRepository;
  private RoleRepository roleRepository;
  private EmployeeRepository employeeRepository;
  private CustomerRepository customerRepository;
  private PasswordEncoder passwordEncoder;
  private JwtService jwtService;
  private AuthService authService;

  @BeforeEach
  void setUp() {
    accountRepository = mock(AccountRepository.class);
    roleRepository = mock(RoleRepository.class);
    employeeRepository = mock(EmployeeRepository.class);
    customerRepository = mock(CustomerRepository.class);
    passwordEncoder = mock(PasswordEncoder.class);
    jwtService = mock(JwtService.class);

    authService =
        new AuthService(
            accountRepository,
            roleRepository,
            employeeRepository,
            customerRepository,
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
    ForgotPasswordRequest req = new ForgotPasswordRequest("validuser", "user@test.com");
    Account acc = createAccount(1, "validuser", 3, true);
    when(accountRepository.findByUsernameAndEmail("validuser", "user@test.com"))
        .thenReturn(Optional.of(acc));
    when(passwordEncoder.encode(anyString())).thenReturn("new_encoded");

    Map<String, String> res = authService.forgotPassword(req);
    assertEquals(
        "Đã đặt lại mật khẩu. Môi trường demo trả mật khẩu tạm trực tiếp.", res.get("message"));
    assertNotNull(res.get("temporaryPassword"));
    verify(accountRepository).save(acc);

    // user not found
    when(accountRepository.findByUsernameAndEmail("validuser", "user@test.com"))
        .thenReturn(Optional.empty());
    ApiException notFound = assertThrows(ApiException.class, () -> authService.forgotPassword(req));
    assertEquals(HttpStatus.NOT_FOUND, notFound.getStatus());
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
