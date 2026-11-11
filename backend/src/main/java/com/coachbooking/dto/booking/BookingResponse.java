package com.coachbooking.dto.booking;

import java.math.BigDecimal;

public record BookingResponse(
    Integer invoiceId,
    BigDecimal totalAmount,
    Integer ticketCount,
    String paymentMethod,
    Boolean isPaid) {}
