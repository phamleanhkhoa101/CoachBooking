package com.coachbooking.domain;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.time.Instant;
import java.util.UUID;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "sepay_sessions")
@Getter
@Setter
@NoArgsConstructor
public class SepaySession {
  @Id
  @Column(name = "session_id")
  private UUID sessionId;

  @Column(name = "invoice_id", nullable = false, unique = true)
  private Integer invoiceId;

  @Column(name = "account_id", nullable = false)
  private Integer accountId;

  @Column(name = "trip_id", nullable = false)
  private Integer tripId;

  @Column(name = "hold_token", nullable = false, unique = true)
  private UUID holdToken;

  @Column(name = "seat_numbers", nullable = false, length = 2000)
  private String seatNumbers;

  @Column(name = "ticket_count", nullable = false)
  private Integer ticketCount;

  @Column(nullable = false, precision = 18, scale = 2)
  private BigDecimal amount;

  @Column(nullable = false, unique = true, length = 100)
  private String reference;

  @Column(nullable = false, length = 30)
  private String status;

  @Column(name = "provider_transaction_id", unique = true, length = 128)
  private String providerTransactionId;

  @Column(name = "expires_at", nullable = false)
  private Instant expiresAt;

  @Column(name = "confirmation_mode", nullable = false, length = 10)
  private String confirmationMode = "IPN";

  @Column(name = "created_at", nullable = false)
  private Instant createdAt = Instant.now();

  @Column(name = "next_check_at")
  private Instant nextCheckAt;

  @Column(name = "last_checked_at")
  private Instant lastCheckedAt;

  @Column(name = "last_check_error", length = 200)
  private String lastCheckError;

  @Column(name = "sync_failures", nullable = false)
  private int syncFailures;

  @Column(name = "sync_lease_token")
  private UUID syncLeaseToken;
}
