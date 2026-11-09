package com.coachbooking.dto.auth;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public class ForgotPasswordRequest {
  @NotBlank(message = "Vui lòng nhập tên đăng nhập.")
  private String username;

  @NotBlank(message = "Vui lòng nhập email.")
  @Email(message = "Email không đúng định dạng.")
  private String email;

  public ForgotPasswordRequest() {}

  public ForgotPasswordRequest(String username, String email) {
    this.username = username;
    this.email = email;
  }

  public String username() {
    return username;
  }

  public String email() {
    return email;
  }

  public String getUsername() {
    return username;
  }

  public void setUsername(String username) {
    if (this.username == null || !username.isBlank()) {
      this.username = username;
    }
  }

  public void setTenDangNhap(String tenDangNhap) {
    if (this.username == null || this.username.isBlank()) {
      this.username = tenDangNhap;
    }
  }

  public String getEmail() {
    return email;
  }

  public void setEmail(String email) {
    this.email = email;
  }
}
