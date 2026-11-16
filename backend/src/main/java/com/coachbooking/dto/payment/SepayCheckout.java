package com.coachbooking.dto.payment;

import java.util.List;

public record SepayCheckout(String checkoutUrl, List<Field> fields) {
  public record Field(String name, String value) {}
}
