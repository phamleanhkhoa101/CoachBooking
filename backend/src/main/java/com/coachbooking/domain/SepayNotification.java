package com.coachbooking.domain;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.time.Instant;
import java.util.UUID;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "sepay_notifications")
@Getter
@Setter
@NoArgsConstructor
public class SepayNotification {
  @Id
  @Column(name = "event_key", length = 64)
  private String eventKey;

  @Column(name = "session_id")
  private UUID sessionId;

  @Column(nullable = false, length = 100)
  private String reference;

  @Column(name = "provider_transaction_id", nullable = false, length = 128)
  private String providerTransactionId;

  @Column(name = "notification_type", nullable = false, length = 50)
  private String notificationType;

  @Column(precision = 18, scale = 2)
  private BigDecimal amount;

  @Column(length = 10)
  private String currency;

  @Column(nullable = false, length = 50)
  private String outcome;

  @Column(name = "received_at", nullable = false)
  private Instant receivedAt;

  @Column(nullable = false, length = 10)
  private String source = "IPN";
}
