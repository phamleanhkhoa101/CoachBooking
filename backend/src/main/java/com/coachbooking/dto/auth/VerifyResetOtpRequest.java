package com.coachbooking.dto.auth;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;

public record VerifyResetOtpRequest(
    @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
        String email,
    @NotBlank(message = "Vui lòng nhập mã OTP.")
        @Pattern(regexp = "\\d{6}", message = "Mã OTP phải gồm 6 chữ số.")
        String otp) {}
