package com.coachbooking.dto.trip;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

public record SeatMapResponse(
    Integer tripId,
    String typeName,
    Integer seatCapacity,
    BigDecimal price,
    List<Integer> bookedSeats,
    List<Integer> heldSeats,
    String departure,
    String destination,
    LocalDateTime departureTime) {}
