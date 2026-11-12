package com.coachbooking.dto.hold;

import java.time.LocalDateTime;
import java.util.List;

public record SeatStatusResponse(
    Integer tripId, List<Integer> bookedSeats, List<Integer> heldSeats, LocalDateTime occurredAt) {}
