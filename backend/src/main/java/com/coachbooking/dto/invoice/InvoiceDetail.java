package com.coachbooking.dto.invoice;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

public record InvoiceDetail(
    Integer invoiceId,
    String customerName,
    String customerPhone,
    String employeeName,
    LocalDateTime createdAt,
    BigDecimal totalAmount,
    Boolean isPaid,
    List<TicketRow> tickets,
    PaymentRow payment) {
  public Integer getMaHd() {
    return invoiceId;
  }

  public String getTenKhachHang() {
    return customerName;
  }

  public String getSdtKhachHang() {
    return customerPhone;
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

  public List<TicketRow> getVeXes() {
    return tickets;
  }

  public PaymentRow getThanhToan() {
    return payment;
  }
}
