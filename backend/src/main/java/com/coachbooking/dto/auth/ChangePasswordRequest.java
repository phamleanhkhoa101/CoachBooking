package com.coachbooking.dto.auth;

import jakarta.validation.constraints.NotBlank;

public class ChangePasswordRequest {
  @NotBlank(message = "Vui lòng nhập mật khẩu hiện tại.")
  private String oldPassword;

  @NotBlank(message = "Vui lòng nhập mật khẩu mới.")
  private String newPassword;

  public ChangePasswordRequest() {}

  public ChangePasswordRequest(String oldPassword, String newPassword) {
    this.oldPassword = oldPassword;
    this.newPassword = newPassword;
  }

  public String oldPassword() {
    return oldPassword;
  }

  public String newPassword() {
    return newPassword;
  }

  public String getOldPassword() {
    return oldPassword;
  }

  public void setOldPassword(String oldPassword) {
    if (this.oldPassword == null || !oldPassword.isBlank()) {
      this.oldPassword = oldPassword;
    }
  }

  public void setMatKhauCu(String matKhauCu) {
    if (this.oldPassword == null || this.oldPassword.isBlank()) {
      this.oldPassword = matKhauCu;
    }
  }

  public String getNewPassword() {
    return newPassword;
  }

  public void setNewPassword(String newPassword) {
    if (this.newPassword == null || !newPassword.isBlank()) {
      this.newPassword = newPassword;
    }
  }

  public void setMatKhauMoi(String matKhauMoi) {
    if (this.newPassword == null || this.newPassword.isBlank()) {
      this.newPassword = matKhauMoi;
    }
  }
}
