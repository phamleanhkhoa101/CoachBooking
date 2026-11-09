package com.coachbooking.dto.auth;

import jakarta.validation.constraints.NotBlank;

public class LoginRequest {
  @NotBlank(message = "Vui lòng nhập tên đăng nhập.")
  private String username;

  @NotBlank(message = "Vui lòng nhập mật khẩu.")
  private String password;

  public LoginRequest() {}

  public LoginRequest(String username, String password) {
    this.username = username;
    this.password = password;
  }

  public String username() {
    return username;
  }

  public String password() {
    return password;
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

  public String getPassword() {
    return password;
  }

  public void setPassword(String password) {
    if (this.password == null || !password.isBlank()) {
      this.password = password;
    }
  }

  public void setMatKhau(String matKhau) {
    if (this.password == null || this.password.isBlank()) {
      this.password = matKhau;
    }
  }
}
