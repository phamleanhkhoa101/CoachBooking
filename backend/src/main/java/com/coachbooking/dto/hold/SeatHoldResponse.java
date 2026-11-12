package com.coachbooking.dto.hold;

import java.time.OffsetDateTime;
import java.util.List;
import java.util.UUID;

public record SeatHoldResponse(
    UUID holdToken, Integer tripId, List<Integer> seatNumbers, OffsetDateTime expiresAt) {}

