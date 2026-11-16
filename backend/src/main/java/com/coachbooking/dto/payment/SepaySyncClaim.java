package com.coachbooking.dto.payment;

import java.util.UUID;

/** Internal lease; never sent to the browser. */
public record SepaySyncClaim(UUID sessionId, UUID leaseToken, String reference) {}
