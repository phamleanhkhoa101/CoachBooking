package com.coachbooking.repo;

import com.coachbooking.domain.Account;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AccountRepository extends JpaRepository<Account, Integer> {
  Optional<Account> findByUsername(String username);

  Optional<Account> findByEmail(String email);

  Optional<Account> findByUsernameAndEmail(String username, String email);

  boolean existsByUsername(String username);

  boolean existsByEmail(String email);

  boolean existsByRoleId(Integer roleId);
}
