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
    PaymentRow payment) {}
