package com.coachbooking.dto.booking;

import java.math.BigDecimal;

public record BookingResponse(
    Integer invoiceId,
    BigDecimal totalAmount,
    Integer ticketCount,
    String paymentMethod,
    Boolean isPaid) {
  public Integer getMaHd() {
    return invoiceId;
  }

  public BigDecimal getTongTien() {
    return totalAmount;
  }

  public Integer getSoVe() {
    return ticketCount;
  }

  public String getPhuongThuc() {
    return paymentMethod;
  }

  public Boolean getTrangThai() {
    return isPaid;
  }
}
