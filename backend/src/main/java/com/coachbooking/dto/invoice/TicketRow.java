package com.coachbooking.dto.invoice;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public record TicketRow(
    Integer ticketId,
    Integer tripId,
    String departure,
    String destination,
    LocalDateTime departureTime,
    Integer seatNumber,
    BigDecimal price) {}
