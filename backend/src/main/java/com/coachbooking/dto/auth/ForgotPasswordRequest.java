package com.coachbooking.dto.auth;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public record ForgotPasswordRequest(
    @NotBlank(message = "Vui lòng nhập tên đăng nhập.") String username,
    @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
        String email) {}
