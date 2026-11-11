package com.coachbooking.dto.statistic;

import java.math.BigDecimal;
import java.util.List;

public record StatisticResponse(
    long customerCount,
    long tripCount,
    BigDecimal totalRevenue,
    long ticketCount,
    List<RevenuePoint> dailyRevenue,
    List<PopularRoute> popularRoutes) {
  public long getSoKhachHang() {
    return customerCount;
  }

  public long getSoChuyenXe() {
    return tripCount;
  }

  public BigDecimal getTongDoanhThu() {
    return totalRevenue;
  }

  public long getSoVe() {
    return ticketCount;
  }

  public List<RevenuePoint> getDoanhThuTheoNgay() {
    return dailyRevenue;
  }

  public List<PopularRoute> getTuyenPhoBien() {
    return popularRoutes;
  }
}
