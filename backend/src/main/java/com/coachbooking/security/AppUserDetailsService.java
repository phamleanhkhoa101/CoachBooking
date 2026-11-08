package com.coachbooking.security;

import com.coachbooking.domain.Account;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.RoleRepository;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

@Service
public class AppUserDetailsService implements UserDetailsService {

  private final AccountRepository accountRepository;
  private final RoleRepository roleRepository;

  public AppUserDetailsService(AccountRepository accountRepository, RoleRepository roleRepository) {
    this.accountRepository = accountRepository;
    this.roleRepository = roleRepository;
  }

  @Override
  public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
    Account account =
        accountRepository
            .findByUsername(username)
            .orElseThrow(
                () -> new UsernameNotFoundException("Không tìm thấy tài khoản: " + username));
    String roleName =
        roleRepository.findById(account.getRoleId()).map(Role::getRoleName).orElse("KhachHang");
    return User.withUsername(account.getUsername())
        .password(account.getPassword())
        .authorities("ROLE_" + roleName)
        .disabled(!Boolean.TRUE.equals(account.getIsActive()))
        .build();
  }
}
