package com.coachbooking.dto.auth;

public record AuthResponse(String token, UserResponse user) {}
