package com.coachbooking.dto.statistic;

import java.math.BigDecimal;

public record RevenuePoint(String date, BigDecimal revenue, Long ticketCount) {
  public String getNgay() {
    return date;
  }

  public BigDecimal getDoanhThu() {
    return revenue;
  }

  public Long getSoVe() {
    return ticketCount;
  }
}
