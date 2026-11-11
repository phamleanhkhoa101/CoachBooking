package com.coachbooking.dto.statistic;

public record PopularRoute(String routeName, Long ticketCount) {
  public String getTenTuyen() {
    return routeName;
  }

  public Long getSoVe() {
    return ticketCount;
  }
}
