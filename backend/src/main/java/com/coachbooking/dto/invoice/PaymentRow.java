package com.coachbooking.dto.invoice;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public record PaymentRow(
    Integer paymentId, BigDecimal amount, LocalDateTime paymentDate, String paymentMethod) {}
