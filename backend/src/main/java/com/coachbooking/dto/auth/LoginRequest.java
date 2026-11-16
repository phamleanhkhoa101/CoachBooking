package com.coachbooking.dto.auth;

import jakarta.validation.constraints.NotBlank;

public record LoginRequest(
    @NotBlank(message = "Vui lĂ²ng nháº­p tĂªn Ä‘Äƒng nháº­p.") String username,
    @NotBlank(message = "Vui lĂ²ng nháº­p máº­t kháº©u.") String password,
    boolean rememberMe) {
  public LoginRequest(String username, String password) {
    this(username, password, false);
  }
}
