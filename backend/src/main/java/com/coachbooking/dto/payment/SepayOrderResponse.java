package com.coachbooking.dto.payment;

import com.coachbooking.dto.payment.SepayIpn.Transaction;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;
import jakarta.validation.Valid;
import jakarta.validation.constraints.*;
import java.math.BigDecimal;
import java.util.List;

/** Only fields used for reconciliation; customer and card information are ignored. */
@JsonIgnoreProperties(ignoreUnknown = true)
public record SepayOrderResponse(@NotNull @Valid Order data) {
  @JsonIgnoreProperties(ignoreUnknown = true)
  @JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
  public record Order(
      @NotBlank @Size(max = 100) String orderInvoiceNumber,
      @NotBlank @Size(max = 30) String orderStatus,
      @NotNull @Digits(integer = 16, fraction = 2) BigDecimal orderAmount,
      @NotBlank @Size(max = 10) String orderCurrency,
      @Size(max = 100) List<@NotNull @Valid Transaction> transactions) {}
}
