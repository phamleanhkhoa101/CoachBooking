package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.LocalDateTime;
import java.util.UUID;

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

  public Long getHoldId() {
    return holdId;
  }

  public UUID getHoldToken() {
    return holdToken;
  }

  public void setHoldToken(UUID holdToken) {
    this.holdToken = holdToken;
  }

  public Integer getAccountId() {
    return accountId;
  }

  public void setAccountId(Integer accountId) {
    this.accountId = accountId;
  }

  public Integer getTripId() {
    return tripId;
  }

  public void setTripId(Integer tripId) {
    this.tripId = tripId;
  }

  public Integer getSeatNumber() {
    return seatNumber;
  }

  public void setSeatNumber(Integer seatNumber) {
    this.seatNumber = seatNumber;
  }

  public LocalDateTime getCreatedAt() {
    return createdAt;
  }

  public void setCreatedAt(LocalDateTime createdAt) {
    this.createdAt = createdAt;
  }

  public LocalDateTime getExpiresAt() {
    return expiresAt;
  }

  public void setExpiresAt(LocalDateTime expiresAt) {
    this.expiresAt = expiresAt;
  }
}
