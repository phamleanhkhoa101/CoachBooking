package com.coachbooking.dto.auth;

import com.fasterxml.jackson.annotation.JsonIgnore;
import java.time.Instant;

public record AuthResponse(
    String token,
    UserResponse user,
    long expiresIn,
    @JsonIgnore String refreshToken,
    @JsonIgnore boolean rememberMe,
    @JsonIgnore Instant refreshExpiresAt) {
  public AuthResponse(String token, UserResponse user) {
    this(token, user, 900, null, false, null);
  }
}
