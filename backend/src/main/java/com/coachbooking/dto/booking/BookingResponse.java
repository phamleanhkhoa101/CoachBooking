package com.coachbooking.dto.booking;

import java.math.BigDecimal;
import java.time.Instant;
import java.util.UUID;

public record BookingResponse(
    Integer invoiceId,
    BigDecimal totalAmount,
    Integer ticketCount,
    String paymentMethod,
    Boolean isPaid,
    UUID paymentSessionId,
    String paymentStatus,
    Instant expiresAt) {
  public BookingResponse(
      Integer invoiceId,
      BigDecimal totalAmount,
      Integer ticketCount,
      String paymentMethod,
      Boolean isPaid) {
    this(invoiceId, totalAmount, ticketCount, paymentMethod, isPaid, null, null, null);
  }
}
