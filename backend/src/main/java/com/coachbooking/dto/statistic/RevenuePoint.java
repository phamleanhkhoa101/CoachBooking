package com.coachbooking.dto.statistic;

import java.math.BigDecimal;

public record RevenuePoint(String date, BigDecimal revenue, Long ticketCount) {}
