package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.Role;
import com.coachbooking.repo.AccountRepository;
import com.coachbooking.repo.RoleRepository;
import org.springframework.http.HttpStatus;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

@Service
public class CurrentUserService {

  private final AccountRepository accountRepository;
  private final RoleRepository roleRepository;

  public CurrentUserService(AccountRepository accountRepository, RoleRepository roleRepository) {
    this.accountRepository = accountRepository;
    this.roleRepository = roleRepository;
  }

  public Account requireAccount() {
    Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
    if (authentication == null
        || !authentication.isAuthenticated()
        || "anonymousUser".equals(authentication.getName())) {
      throw new ApiException(HttpStatus.UNAUTHORIZED, "Vui lòng đăng nhập để tiếp tục.");
    }
    return accountRepository
        .findByUsername(authentication.getName())
        .orElseThrow(() -> new ApiException(HttpStatus.UNAUTHORIZED, "Tài khoản không tồn tại."));
  }

  public String roleName(Account account) {
    return roleRepository.findById(account.getRoleId()).map(Role::getRoleName).orElse("KhachHang");
  }

  public boolean isCustomer(Account account) {
    return "KhachHang".equals(roleName(account));
  }
}
