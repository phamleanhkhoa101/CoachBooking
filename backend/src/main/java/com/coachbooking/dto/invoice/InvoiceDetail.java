package com.coachbooking.dto.invoice;

import com.coachbooking.dto.payment.SepayStatus;
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
    PaymentRow payment,
    SepayStatus sepay) {
  public InvoiceDetail(
      Integer invoiceId,
      String customerName,
      String customerPhone,
      String employeeName,
      LocalDateTime createdAt,
      BigDecimal totalAmount,
      Boolean isPaid,
      List<TicketRow> tickets,
      PaymentRow payment) {
    this(
        invoiceId,
        customerName,
        customerPhone,
        employeeName,
        createdAt,
        totalAmount,
        isPaid,
        tickets,
        payment,
        null);
  }

  public InvoiceDetail withPayment(SepayStatus status) {
    return new InvoiceDetail(
        invoiceId,
        customerName,
        customerPhone,
        employeeName,
        createdAt,
        totalAmount,
        isPaid,
        tickets,
        payment,
        status);
  }
}
