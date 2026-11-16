package com.coachbooking.dto.payment;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;
import jakarta.validation.Valid;
import jakarta.validation.constraints.*;
import java.math.BigDecimal;

@JsonIgnoreProperties(ignoreUnknown = true)
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public record SepayIpn(
    @NotBlank @Size(max = 50) String notificationType,
    @NotNull @Valid Order order,
    @NotNull @Valid Transaction transaction) {
  @JsonIgnoreProperties(ignoreUnknown = true)
  @JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
  public record Order(
      @NotBlank @Size(max = 100) String orderInvoiceNumber,
      @Size(max = 30) String orderStatus,
      @Digits(integer = 16, fraction = 2) BigDecimal orderAmount,
      @Size(max = 10) String orderCurrency) {}

  @JsonIgnoreProperties(ignoreUnknown = true)
  @JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
  public record Transaction(
      @NotBlank @Size(max = 128) String id,
      @Size(max = 30) String transactionStatus,
      @Size(max = 30) String transactionType,
      @Digits(integer = 16, fraction = 2) BigDecimal transactionAmount,
      @Size(max = 10) String transactionCurrency,
      @Size(max = 30) String paymentMethod) {}
}
