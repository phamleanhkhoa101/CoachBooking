package com.coachbooking.repo;

import com.coachbooking.domain.AuthSession;
import jakarta.persistence.LockModeType;
import java.time.Instant;
import java.util.Optional;
import java.util.UUID;
import org.springframework.data.jpa.repository.*;
import org.springframework.data.repository.query.Param;

public interface AuthSessionRepository extends JpaRepository<AuthSession, UUID> {
  @Lock(LockModeType.PESSIMISTIC_WRITE)
  @Query("select s from AuthSession s where s.sessionId = :id")
  Optional<AuthSession> lockById(@Param("id") UUID id);

  @Modifying
  @Query(
      "update AuthSession s set s.revokedAt = :now where s.accountId = :id and s.revokedAt is null")
  int revokeAll(@Param("id") Integer id, @Param("now") Instant now);

  @Modifying
  @Query(value = "delete from auth_sessions where expires_at < :now", nativeQuery = true)
  int deleteExpired(@Param("now") Instant now);
}
