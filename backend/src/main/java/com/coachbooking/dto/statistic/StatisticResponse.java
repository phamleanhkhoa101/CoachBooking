package com.coachbooking.dto.statistic;

import java.math.BigDecimal;
import java.util.List;

public record StatisticResponse(
    long customerCount,
    long tripCount,
    BigDecimal totalRevenue,
    long ticketCount,
    List<RevenuePoint> dailyRevenue,
    List<PopularRoute> popularRoutes) {}
