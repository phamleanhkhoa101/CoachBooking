package com.coachbooking.repo;

import com.coachbooking.domain.AuthRefreshToken;
import java.util.Optional;
import java.util.UUID;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface AuthRefreshTokenRepository extends JpaRepository<AuthRefreshToken, String> {
  @Query("select t.sessionId from AuthRefreshToken t where t.tokenHash = :hash")
  Optional<UUID> findSessionId(@Param("hash") String hash);
}
