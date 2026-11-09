package com.coachbooking.dto.auth;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public class RegisterRequest {
  @NotBlank(message = "Vui lòng nhập tên đăng nhập.")
  private String username;

  @NotBlank(message = "Vui lòng nhập email.")
  @Email(message = "Email không đúng định dạng.")
  private String email;

  @NotBlank(message = "Vui lòng nhập mật khẩu.")
  private String password;

  @NotBlank(message = "Vui lòng xác nhận mật khẩu.")
  private String confirmPassword;

  public RegisterRequest() {}

  public RegisterRequest(String username, String email, String password, String confirmPassword) {
    this.username = username;
    this.email = email;
    this.password = password;
    this.confirmPassword = confirmPassword;
  }

  public String username() {
    return username;
  }

  public String email() {
    return email;
  }

  public String password() {
    return password;
  }

  public String confirmPassword() {
    return confirmPassword;
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

  public String getConfirmPassword() {
    return confirmPassword;
  }

  public void setConfirmPassword(String confirmPassword) {
    if (this.confirmPassword == null || !confirmPassword.isBlank()) {
      this.confirmPassword = confirmPassword;
    }
  }

  public void setXacNhanMatKhau(String xacNhanMatKhau) {
    if (this.confirmPassword == null || this.confirmPassword.isBlank()) {
      this.confirmPassword = xacNhanMatKhau;
    }
  }
}
