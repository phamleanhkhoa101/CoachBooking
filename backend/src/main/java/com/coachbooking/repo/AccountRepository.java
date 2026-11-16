package com.coachbooking.repo;

import com.coachbooking.domain.Account;
import jakarta.persistence.LockModeType;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Lock;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface AccountRepository extends JpaRepository<Account, Integer> {
  @Lock(LockModeType.PESSIMISTIC_WRITE)
  @Query("select a from Account a where a.accountId = :id")
  Optional<Account> lockById(@Param("id") Integer id);

  @Lock(LockModeType.PESSIMISTIC_WRITE)
  @Query("select a from Account a where a.username = :username")
  Optional<Account> lockByUsername(@Param("username") String username);

  Optional<Account> findByUsername(String username);

  Optional<Account> findByEmail(String email);

  Optional<Account> findByEmailIgnoreCase(String email);

  Optional<Account> findByUsernameAndEmail(String username, String email);

  Optional<Account> findByGoogleSub(String googleSub);

  boolean existsByUsername(String username);

  boolean existsByEmail(String email);

  boolean existsByRoleId(Integer roleId);
}
