package com.coachbooking.dto.payment;

/** Internal HTTP result, with safe user-facing messages only. */
public record SepayLookupResult(
    SepayOrderResponse.Order order, String error, Long retryAfterSeconds) {}
