package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.RoleRepository;
import java.util.Optional;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;

class CurrentUserServiceTest {

  private AccountRepository accountRepository;
  private RoleRepository roleRepository;
  private CurrentUserService service;

  @BeforeEach
  void setUp() {
    accountRepository = mock(AccountRepository.class);
    roleRepository = mock(RoleRepository.class);
    service = new CurrentUserService(accountRepository, roleRepository);
  }

  @AfterEach
  void tearDown() {
    SecurityContextHolder.clearContext();
  }

  @Test
  void requireAccountSuccess() {
    SecurityContextHolder.getContext()
        .setAuthentication(
            new UsernamePasswordAuthenticationToken("alice", "pwd", java.util.List.of()));

    Account account = new Account();
    account.setAccountId(1);
    account.setUsername("alice");
    when(accountRepository.findByUsername("alice")).thenReturn(Optional.of(account));

    Account result = service.requireAccount();
    assertNotNull(result);
    assertEquals("alice", result.getUsername());
  }

  @Test
  void requireAccountThrowsUnauthorizedWhenNotAuthenticated() {
    SecurityContextHolder.clearContext();
    ApiException ex = assertThrows(ApiException.class, () -> service.requireAccount());
    assertEquals(HttpStatus.UNAUTHORIZED, ex.getStatus());

    SecurityContextHolder.getContext()
        .setAuthentication(new UsernamePasswordAuthenticationToken("anonymousUser", "pwd"));
    ApiException anonEx = assertThrows(ApiException.class, () -> service.requireAccount());
    assertEquals(HttpStatus.UNAUTHORIZED, anonEx.getStatus());
  }

  @Test
  void roleNameAndIsCustomer() {
    Account account = new Account();
    account.setRoleId(1);

    Role role = new Role();
    role.setRoleId(1);
    role.setRoleName("Admin");
    when(roleRepository.findById(1)).thenReturn(Optional.of(role));

    assertEquals("Admin", service.roleName(account));
    assertFalse(service.isCustomer(account));

    when(roleRepository.findById(1)).thenReturn(Optional.empty());
    assertEquals("KhachHang", service.roleName(account));
    assertTrue(service.isCustomer(account));
  }
}
