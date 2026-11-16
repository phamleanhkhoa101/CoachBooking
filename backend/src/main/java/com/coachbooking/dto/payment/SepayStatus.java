package com.coachbooking.dto.payment;

import com.coachbooking.domain.SepaySession;
import java.math.BigDecimal;
import java.time.Instant;
import java.util.UUID;

public record SepayStatus(
    UUID paymentSessionId,
    Integer invoiceId,
    String paymentStatus,
    BigDecimal totalAmount,
    Instant expiresAt,
    String confirmationMode,
    Instant lastCheckedAt,
    String syncError) {
  public SepayStatus(
      UUID paymentSessionId,
      Integer invoiceId,
      String paymentStatus,
      BigDecimal totalAmount,
      Instant expiresAt) {
    this(paymentSessionId, invoiceId, paymentStatus, totalAmount, expiresAt, "IPN", null, null);
  }

  public static SepayStatus from(SepaySession session) {
    return new SepayStatus(
        session.getSessionId(),
        session.getInvoiceId(),
        session.getStatus(),
        session.getAmount(),
        session.getExpiresAt(),
        session.getConfirmationMode(),
        session.getLastCheckedAt(),
        session.getLastCheckError());
  }
}
