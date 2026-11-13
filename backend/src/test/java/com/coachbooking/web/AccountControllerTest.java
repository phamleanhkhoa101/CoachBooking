package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.CustomerRepository;
import com.coachbooking.repo.EmployeeRepository;
import com.coachbooking.repo.RoleRepository;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.security.crypto.password.PasswordEncoder;

class AccountControllerTest {

  private AccountRepository accountRepository;
  private RoleRepository roleRepository;
  private EmployeeRepository employeeRepository;
  private CustomerRepository customerRepository;
  private PasswordEncoder passwordEncoder;
  private AccountController controller;

  @BeforeEach
  void setUp() {
    accountRepository = mock(AccountRepository.class);
    roleRepository = mock(RoleRepository.class);
    employeeRepository = mock(EmployeeRepository.class);
    customerRepository = mock(CustomerRepository.class);
    passwordEncoder = mock(PasswordEncoder.class);
    controller =
        new AccountController(
            accountRepository,
            roleRepository,
            employeeRepository,
            customerRepository,
            passwordEncoder);
  }

  @Test
  void listAndGetSuccess() {
    Account acc = new Account();
    acc.setAccountId(1);
    acc.setUsername("user1");
    when(accountRepository.findAll()).thenReturn(List.of(acc));
    when(accountRepository.findById(1)).thenReturn(Optional.of(acc));

    assertEquals(1, controller.list().size());
    assertEquals(1, controller.get(1).getAccountId());

    when(accountRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndValidation() {
    AccountController.CreateRequest req =
        new AccountController.CreateRequest("newuser", "user@test.com", "Password@123", 2, true);

    when(accountRepository.existsByUsername("newuser")).thenReturn(false);
    when(accountRepository.existsByEmail("user@test.com")).thenReturn(false);
    when(roleRepository.existsById(2)).thenReturn(true);
    when(passwordEncoder.encode("Password@123")).thenReturn("encoded_pass");
    when(accountRepository.save(any(Account.class))).thenAnswer(i -> i.getArgument(0));

    Account created = controller.create(req);
    assertNotNull(created);
    assertEquals("newuser", created.getUsername());
    assertEquals("encoded_pass", created.getPassword());

    // short username
    AccountController.CreateRequest shortUser =
        new AccountController.CreateRequest("user", "user@test.com", "Password@123", 2, true);
    ApiException shortEx = assertThrows(ApiException.class, () -> controller.create(shortUser));
    assertEquals(HttpStatus.BAD_REQUEST, shortEx.getStatus());

    // role not found
    when(roleRepository.existsById(2)).thenReturn(false);
    ApiException roleEx = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.NOT_FOUND, roleEx.getStatus());
  }

  @Test
  void updateSuccessAndValidation() {
    Account acc = new Account();
    acc.setAccountId(1);
    acc.setEmail("old@test.com");
    acc.setRoleId(1);
    when(accountRepository.findById(1)).thenReturn(Optional.of(acc));
    when(roleRepository.existsById(2)).thenReturn(true);
    when(accountRepository.existsByEmail("new@test.com")).thenReturn(false);
    when(passwordEncoder.encode("NewPass@123")).thenReturn("new_hash");
    when(accountRepository.save(any(Account.class))).thenAnswer(i -> i.getArgument(0));

    AccountController.UpdateRequest req =
        new AccountController.UpdateRequest("new@test.com", "NewPass@123", 2, true);
    Account updated = controller.update(1, req);
    assertEquals("new@test.com", updated.getEmail());
    assertEquals("new_hash", updated.getPassword());

    // duplicate email
    when(accountRepository.existsByEmail("dup@test.com")).thenReturn(true);
    AccountController.UpdateRequest dupReq =
        new AccountController.UpdateRequest("dup@test.com", null, 2, true);
    ApiException dupEx = assertThrows(ApiException.class, () -> controller.update(1, dupReq));
    assertEquals(HttpStatus.CONFLICT, dupEx.getStatus());
  }

  @Test
  void deleteSuccessAndRestrictions() {
    Account acc = new Account();
    acc.setAccountId(1);
    when(accountRepository.findById(1)).thenReturn(Optional.of(acc));
    when(employeeRepository.existsByAccountId(1)).thenReturn(false);
    when(customerRepository.existsByAccountId(1)).thenReturn(false);

    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa tài khoản thành công.", res.get("message"));
    verify(accountRepository).delete(acc);

    // cannot delete if attached to customer
    when(customerRepository.existsByAccountId(1)).thenReturn(true);
    ApiException conflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());
  }
}
