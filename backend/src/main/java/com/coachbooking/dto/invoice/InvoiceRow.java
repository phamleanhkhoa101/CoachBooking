package com.coachbooking.dto.invoice;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public record InvoiceRow(
    Integer invoiceId,
    String customerName,
    String employeeName,
    LocalDateTime createdAt,
    BigDecimal totalAmount,
    Boolean isPaid,
    Long ticketCount,
    String paymentMethod) {
  public Integer getMaHd() {
    return invoiceId;
  }

  public String getTenKhachHang() {
    return customerName;
  }

  public String getTenNhanVien() {
    return employeeName;
  }

  public LocalDateTime getNgayLap() {
    return createdAt;
  }

  public BigDecimal getTongTien() {
    return totalAmount;
  }

  public Boolean getTrangThai() {
    return isPaid;
  }

  public Long getSoVe() {
    return ticketCount;
  }

  public String getPhuongThuc() {
    return paymentMethod;
  }
}
