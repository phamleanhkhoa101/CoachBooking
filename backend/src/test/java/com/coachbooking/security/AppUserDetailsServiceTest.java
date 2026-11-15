package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Account;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.RoleRepository;
import java.util.Optional;
import org.junit.jupiter.api.Test;
import org.springframework.security.core.userdetails.UserDetails;

class AppUserDetailsServiceTest {

  @Test
  void loadUserByUsernameHandlesNullPasswordForGoogleAccounts() {
    AccountRepository accountRepo = mock(AccountRepository.class);
    RoleRepository roleRepo = mock(RoleRepository.class);
    AppUserDetailsService service = new AppUserDetailsService(accountRepo, roleRepo);

    Account googleAccount = new Account();
    googleAccount.setAccountId(1);
    googleAccount.setUsername("googleuser");
    googleAccount.setPassword(null);
    googleAccount.setRoleId(3);
    googleAccount.setIsActive(true);

    Role role = new Role();
    role.setRoleId(3);
    role.setRoleName("KhachHang");

    when(accountRepo.findByUsername("googleuser")).thenReturn(Optional.of(googleAccount));
    when(roleRepo.findById(3)).thenReturn(Optional.of(role));

    UserDetails userDetails = service.loadUserByUsername("googleuser");
    assertEquals("googleuser", userDetails.getUsername());
    assertEquals("", userDetails.getPassword());
    assertTrue(userDetails.isEnabled());
    assertTrue(
        userDetails.getAuthorities().stream()
            .anyMatch(a -> "ROLE_KhachHang".equals(a.getAuthority())));
  }
}
