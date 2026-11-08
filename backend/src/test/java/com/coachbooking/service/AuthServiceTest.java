package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.RoleRepository;
import com.coachbooking.security.JwtService;
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

    AuthService.AuthResponse response =
        authService.loginUser(new AuthService.LoginRequest("kh1", "pass123"));

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
            ApiException.class,
            () -> authService.loginUser(new AuthService.LoginRequest("admin", "pass123")));
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

    AuthService.AuthResponse response =
        authService.loginAdmin(new AuthService.LoginRequest("admin", "pass123"));

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
            ApiException.class,
            () -> authService.loginAdmin(new AuthService.LoginRequest("kh1", "pass123")));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
    assertTrue(ex.getMessage().contains("không có quyền truy cập quản trị"));
  }

  @Test
  void authenticate_badRequest_whenWrongPassword() {
    Account acc = createAccount(1, "kh1", 3, true);
    when(accountRepository.findByUsername("kh1")).thenReturn(Optional.of(acc));
    when(passwordEncoder.matches("wrong", "encoded_secret")).thenReturn(false);

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> authService.login(new AuthService.LoginRequest("kh1", "wrong")));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
  }
}
