package com.coachbooking.dto.auth;

public record UserResponse(
    Integer accountId,
    String username,
    String email,
    String role,
    Integer roleId,
    Integer employeeId,
    Integer customerId,
    String fullName,
    Boolean isActive) {}
