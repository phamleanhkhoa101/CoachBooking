package com.coachbooking.dto.invoice;

import com.coachbooking.dto.payment.SepayStatus;
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
    String paymentMethod,
    SepayStatus sepay) {
  public InvoiceRow(
      Integer invoiceId,
      String customerName,
      String employeeName,
      LocalDateTime createdAt,
      BigDecimal totalAmount,
      Boolean isPaid,
      Long ticketCount,
      String paymentMethod) {
    this(
        invoiceId,
        customerName,
        employeeName,
        createdAt,
        totalAmount,
        isPaid,
        ticketCount,
        paymentMethod,
        null);
  }

  public InvoiceRow withPayment(SepayStatus status) {
    return new InvoiceRow(
        invoiceId,
        customerName,
        employeeName,
        createdAt,
        totalAmount,
        isPaid,
        ticketCount,
        paymentMethod,
        status);
  }
}
