package com.coachbooking.repo;

import com.coachbooking.domain.PasswordResetToken;
import java.time.LocalDateTime;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface PasswordResetTokenRepository extends JpaRepository<PasswordResetToken, Long> {

  Optional<PasswordResetToken> findTopByAccountIdAndUsedAtIsNullOrderByCreatedAtDesc(
      Integer accountId);

  Optional<PasswordResetToken> findTopByAccountIdOrderByCreatedAtDesc(Integer accountId);

  @Modifying
  @Query(
      "update PasswordResetToken t set t.usedAt = :now "
          + "where t.accountId = :accountId and t.usedAt is null")
  int invalidateActiveTokens(
      @Param("accountId") Integer accountId, @Param("now") LocalDateTime now);
}
