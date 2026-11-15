package com.coachbooking.dto.auth;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public record ResetPasswordRequest(
    @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
        String email,
    @NotBlank(message = "Thiếu phiên xác thực đặt lại mật khẩu.") String resetToken,
    @NotBlank(message = "Vui lòng nhập mật khẩu mới.") String newPassword) {}
