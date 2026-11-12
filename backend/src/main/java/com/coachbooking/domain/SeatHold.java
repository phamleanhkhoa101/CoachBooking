package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.LocalDateTime;
import java.util.UUID;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "seat_holds")
public class SeatHold {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "hold_id")
  private Long holdId;

  @Column(name = "hold_token", nullable = false)
  private UUID holdToken;

  @Column(name = "account_id", nullable = false)
  private Integer accountId;

  @Column(name = "trip_id", nullable = false)
  private Integer tripId;

  @Column(name = "seat_number", nullable = false)
  private Integer seatNumber;

  @Column(name = "created_at", nullable = false)
  private LocalDateTime createdAt;

  @Column(name = "expires_at", nullable = false)
  private LocalDateTime expiresAt;
}
