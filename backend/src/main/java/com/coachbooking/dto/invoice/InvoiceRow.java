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
    String paymentMethod) {}
