package com.coachbooking.domain;

import jakarta.persistence.*;
import java.time.Instant;
import java.util.UUID;
import lombok.Getter;
import lombok.Setter;

@Entity
@Table(name = "auth_refresh_tokens")
@Getter
@Setter
public class AuthRefreshToken {
  @Id
  @Column(length = 64)
  private String tokenHash;

  @Column(nullable = false)
  private UUID sessionId;

  private Instant usedAt;
}
