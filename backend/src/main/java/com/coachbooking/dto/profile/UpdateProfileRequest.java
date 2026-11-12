package com.coachbooking.dto.profile;

import jakarta.validation.constraints.NotBlank;
import java.time.LocalDate;

public record UpdateProfileRequest(
    @NotBlank(message = "Vui lòng nhập họ tên.") String fullName,
    @NotBlank(message = "Vui lòng nhập số điện thoại.") String phone,
    LocalDate dateOfBirth,
    Boolean gender,
    String address) {}
