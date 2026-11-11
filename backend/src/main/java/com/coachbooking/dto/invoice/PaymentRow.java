package com.coachbooking.dto.invoice;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public record PaymentRow(
    Integer paymentId, BigDecimal amount, LocalDateTime paymentDate, String paymentMethod) {
  public Integer getMaTt() {
    return paymentId;
  }

  public BigDecimal getTongTien() {
    return amount;
  }

  public LocalDateTime getNgayTt() {
    return paymentDate;
  }

  public String getPhuongThuc() {
    return paymentMethod;
  }
}
