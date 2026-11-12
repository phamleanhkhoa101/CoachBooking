package com.coachbooking.dto.booking;

import jakarta.validation.constraints.NotNull;
import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

public record BookingRequest(
    @NotNull(message = "Vui lòng chọn chuyến xe.") Integer tripId,
    List<Integer> seatNumbers,
    String customerName,
    String phone,
    LocalDate dateOfBirth,
    Boolean gender,
    String paymentMethod,
    UUID holdToken) {}
