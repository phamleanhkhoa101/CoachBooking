package com.coachbooking.service;

import static org.mockito.Mockito.*;

import com.coachbooking.domain.TripSchedule;
import java.time.LocalDate;
import java.util.List;
import org.junit.jupiter.api.Test;

class TripScheduleJobTest {
  TripScheduleService service = mock(TripScheduleService.class);

  TripSchedule schedule(int id, Boolean active) {
    TripSchedule schedule = new TripSchedule();
    schedule.setScheduleId(id);
    schedule.setIsActive(active);
    return schedule;
  }

  @Test
  void replenishesActiveSchedulesForFourteenDaysAndContinuesAfterFailure() {
    LocalDate today = LocalDate.now(TripPlanningTime.ZONE);
    when(service.list())
        .thenReturn(
            List.of(
                schedule(1, false),
                schedule(2, null),
                schedule(3, true),
                schedule(4, true),
                schedule(5, true)));
    when(service.run(3, today, today.plusDays(13), false))
        .thenThrow(new IllegalStateException("database unavailable"));
    when(service.run(4, today, today.plusDays(13), false))
        .thenReturn(
            List.of(
                new ScheduleOccurrenceService.Result(today, "ERROR", null, "busy"),
                new ScheduleOccurrenceService.Result(today.plusDays(1), "CREATED", 99, "created")));
    when(service.run(5, today, today.plusDays(13), false)).thenReturn(List.of());
    new TripScheduleJob(service).replenish();
    verify(service).list();
    for (int id : List.of(3, 4, 5)) verify(service).run(id, today, today.plusDays(13), false);
    verifyNoMoreInteractions(service);
  }
}
