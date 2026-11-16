package com.coachbooking.service;

import java.time.LocalDate;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

@Component
@RequiredArgsConstructor
public class TripScheduleJob {
  private static final Logger LOG = LoggerFactory.getLogger(TripScheduleJob.class);
  private final TripScheduleService service;

  @EventListener(ApplicationReadyEvent.class)
  @Scheduled(cron = "0 5 0 * * *", zone = "Asia/Ho_Chi_Minh")
  public void replenish() {
    LocalDate today = LocalDate.now(TripPlanningTime.ZONE);
    for (var schedule : service.list()) {
      if (!Boolean.TRUE.equals(schedule.getIsActive())) continue;
      try {
        var results = service.run(schedule.getScheduleId(), today, today.plusDays(13), false);
        long errors = results.stream().filter(r -> r.status().equals("ERROR")).count();
        if (errors > 0)
          LOG.warn(
              "Lịch {}: {} ngày lỗi; xem kết quả trong quản trị.",
              schedule.getScheduleId(),
              errors);
      } catch (RuntimeException ex) {
        LOG.error("Không thể bổ sung chuyến cho lịch {}.", schedule.getScheduleId(), ex);
      }
    }
  }
}
