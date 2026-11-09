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
    BigDecimal price) {
  public Integer getMaVe() {
    return ticketId;
  }

  public Integer getMaChuyen() {
    return tripId;
  }

  public String getDiemDi() {
    return departure;
  }

  public String getDiemDen() {
    return destination;
  }

  public LocalDateTime getNgayGioKhoiHanh() {
    return departureTime;
  }

  public Integer getChoNgoi() {
    return seatNumber;
  }

  public BigDecimal getGiaVe() {
    return price;
  }
}
