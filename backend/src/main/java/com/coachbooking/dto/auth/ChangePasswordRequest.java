package com.coachbooking.dto.auth;

import jakarta.validation.constraints.NotBlank;

public record ChangePasswordRequest(
    @NotBlank(message = "Vui lòng nhập mật khẩu hiện tại.") String oldPassword,
    @NotBlank(message = "Vui lòng nhập mật khẩu mới.") String newPassword) {}
