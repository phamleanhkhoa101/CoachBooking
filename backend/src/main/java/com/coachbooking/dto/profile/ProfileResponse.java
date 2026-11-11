package com.coachbooking.dto.profile;

import java.time.LocalDate;

public record ProfileResponse(
    String type,
    Integer customerId,
    Integer employeeId,
    String fullName,
    LocalDate dateOfBirth,
    String phone,
    Boolean gender,
    String address,
    String email,
    String username,
    String role) {}
