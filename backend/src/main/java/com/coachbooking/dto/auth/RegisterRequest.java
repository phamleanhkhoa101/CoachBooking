package com.coachbooking.dto.auth;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public record RegisterRequest(
    @NotBlank(message = "Vui lòng nhập tên đăng nhập.") String username,
    @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
        String email,
    @NotBlank(message = "Vui lòng nhập mật khẩu.") String password,
    @NotBlank(message = "Vui lòng xác nhận mật khẩu.") String confirmPassword) {}
