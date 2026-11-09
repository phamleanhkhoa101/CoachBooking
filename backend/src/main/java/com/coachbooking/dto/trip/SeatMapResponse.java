package com.coachbooking.dto.trip;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

public record SeatMapResponse(
    Integer tripId,
    String typeName,
    Integer seatCapacity,
    BigDecimal price,
    List<Integer> bookedSeats,
    String departure,
    String destination,
    LocalDateTime departureTime) {
  public Integer getMaChuyen() {
    return tripId;
  }

  public String getTenLoaiXe() {
    return typeName;
  }

  public Integer getSoCho() {
    return seatCapacity;
  }

  public BigDecimal getGiaVe() {
    return price;
  }

  public List<Integer> getGheDaDat() {
    return bookedSeats;
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
}
