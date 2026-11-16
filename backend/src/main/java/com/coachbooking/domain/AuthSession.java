package com.coachbooking.domain;

import jakarta.persistence.*;
import java.time.Instant;
import java.util.UUID;
import lombok.Getter;
import lombok.Setter;

@Entity
@Table(name = "auth_sessions")
@Getter
@Setter
public class AuthSession {
  @Id private UUID sessionId;

  @Column(nullable = false)
  private Integer accountId;

  @Column(nullable = false)
  private Integer roleId;

  @Column(nullable = false)
  private boolean rememberMe;

  @Column(nullable = false)
  private Instant createdAt;

  @Column(nullable = false)
  private Instant lastActiveAt;

  @Column(nullable = false)
  private Instant expiresAt;

  private Instant revokedAt;
}
