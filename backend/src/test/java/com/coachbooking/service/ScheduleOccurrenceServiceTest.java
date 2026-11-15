package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.*;
import com.coachbooking.repo.*;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class ScheduleOccurrenceServiceTest {
  TripScheduleRepository schedules = mock(TripScheduleRepository.class);
  ScheduleOccurrenceRepository occurrences = mock(ScheduleOccurrenceRepository.class);
  TripService trips = mock(TripService.class);
  ScheduleOccurrenceService service =
      new ScheduleOccurrenceService(
          schedules,
          occurrences,
          trips,
          mock(VehicleRepository.class),
          mock(DriverRepository.class),
          mock(RouteRepository.class),
          mock(VehicleTypeRepository.class),
          mock(BusCompanyRepository.class),
          mock(TicketPriceRepository.class));
  TripSchedule schedule;
  LocalDate date =
      LocalDate.now(TripPlanningTime.ZONE).plusYears(1).withMonth(12).withDayOfMonth(31);

  @BeforeEach
  void setup() {
    schedule = new TripSchedule();
    schedule.setScheduleId(1);
    schedule.setVehicleId(1);
    schedule.setRouteId(1);
    schedule.setDriverId(1);
    schedule.setDepartureTime(LocalTime.of(8, 0));
    schedule.setWeekdays("1,2,3,4,5,6,7");
    schedule.setValidFrom(date);
    schedule.setValidTo(date.plusDays(10));
    schedule.setIsActive(true);
    schedule.setExcludedDates("");
    when(schedules.findById(1)).thenReturn(Optional.of(schedule));
    when(schedules.lockById(1)).thenReturn(Optional.of(schedule));
  }

  @Test
  void generationAcrossYearUsesCalendarDateAndRecordsOccurrence() {
    Trip trip = new Trip();
    trip.setTripId(50);
    when(trips.create(any())).thenReturn(trip);
    var result = service.generate(1, date.plusDays(1));
    assertEquals("CREATED", result.status());
    verify(trips).create(argThat(r -> r.departureTime().equals(date.plusDays(1).atTime(8, 0))));
    verify(occurrences)
        .saveAndFlush(
            argThat(o -> o.getServiceDate().equals(date.plusDays(1)) && o.getTripId() == 50));
  }

  @Test
  void existingDeletedOrExcludedDateNeverCreatesAnotherTrip() {
    for (String status : List.of("CREATED", "DELETED", "EXCLUDED")) {
      ScheduleOccurrence o = new ScheduleOccurrence();
      o.setStatus(status);
      when(occurrences.findByScheduleIdAndServiceDate(1, date)).thenReturn(Optional.of(o));
      assertNotEquals("CREATED", service.generate(1, date).status());
    }
    verifyNoInteractions(trips);
  }

  @Test
  void exclusionsArePersistentButPreviewDoesNotWrite() {
    schedule.setExcludedDates(date.toString());
    assertEquals("EXCLUDED", service.preview(1, date).status());
    verify(occurrences, never()).saveAndFlush(any());
    assertEquals("EXCLUDED", service.generate(1, date).status());
    verify(occurrences).saveAndFlush(argThat(o -> "EXCLUDED".equals(o.getStatus())));
    verifyNoInteractions(trips);
  }

  @Test
  void ignoresInactiveOutOfRangeWrongWeekdayAndPastTime() {
    schedule.setIsActive(false);
    assertEquals("SKIPPED", service.generate(1, date).status());
    schedule.setIsActive(true);
    assertEquals("SKIPPED", service.generate(1, date.minusDays(1)).status());
    schedule.setWeekdays(String.valueOf(date.plusDays(1).getDayOfWeek().getValue()));
    assertEquals("SKIPPED", service.generate(1, date).status());
    LocalDate yesterday = LocalDate.now(TripPlanningTime.ZONE).minusDays(1);
    schedule.setValidFrom(yesterday);
    schedule.setWeekdays("1,2,3,4,5,6,7");
    assertEquals("PAST", service.generate(1, yesterday).status());
    verifyNoInteractions(trips);
  }
}
