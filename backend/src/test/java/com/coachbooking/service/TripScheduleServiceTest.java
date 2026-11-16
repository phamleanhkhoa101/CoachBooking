package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.dto.trip.ScheduleRequest;
import com.coachbooking.repo.*;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.SimpleTransactionStatus;

class TripScheduleServiceTest {
  TripScheduleRepository schedules = mock(TripScheduleRepository.class);
  VehicleRepository vehicles = mock(VehicleRepository.class);
  DriverRepository drivers = mock(DriverRepository.class);
  RouteRepository routes = mock(RouteRepository.class);
  ScheduleOccurrenceService worker = mock(ScheduleOccurrenceService.class);
  ObjectMapper mapper = mock(ObjectMapper.class);
  PlatformTransactionManager transactions = mock(PlatformTransactionManager.class);
  TripScheduleService service =
      new TripScheduleService(schedules, vehicles, drivers, routes, worker, mapper, transactions);
  LocalDate start = LocalDate.of(2030, 1, 1);
  TripSchedule schedule = new TripSchedule();

  @BeforeEach
  void setup() {
    schedule.setScheduleId(7);
    when(transactions.getTransaction(any())).thenAnswer(i -> new SimpleTransactionStatus());
    when(schedules.save(any())).thenAnswer(i -> i.getArgument(0));
  }

  ScheduleRequest request(LocalDate from, LocalDate to, Set<LocalDate> excluded) {
    return new ScheduleRequest(
        "  Sai Gon - Da Lat  ",
        1,
        2,
        3,
        LocalTime.of(8, 30),
        Set.of(7, 1, 3),
        from,
        to,
        true,
        excluded);
  }

  void resourcesExist() {
    when(vehicles.findById(1)).thenReturn(Optional.of(new Vehicle()));
    when(drivers.existsById(3)).thenReturn(true);
    when(routes.existsById(2)).thenReturn(true);
  }

  @Test
  void createsAndUpdatesScheduleWithSortedDatesAndWeekdays() {
    resourcesExist();
    var request = request(start, start.plusDays(5), Set.of(start.plusDays(3), start));
    var created = service.save(null, request);
    assertEquals("Sai Gon - Da Lat", created.getName());
    assertEquals("1,3,7", created.getWeekdays());
    assertEquals("2030-01-01,2030-01-04", created.getExcludedDates());
    assertEquals(1, created.getVehicleId());
    assertEquals(2, created.getRouteId());
    assertEquals(3, created.getDriverId());
    assertEquals(LocalTime.of(8, 30), created.getDepartureTime());
    assertEquals(start, created.getValidFrom());
    assertEquals(start.plusDays(5), created.getValidTo());
    assertTrue(created.getIsActive());
    when(schedules.lockById(7)).thenReturn(Optional.of(schedule));
    assertSame(schedule, service.save(7, request));
    assertEquals(created.getName(), schedule.getName());
    verify(transactions, times(2)).commit(any());
  }

  @Test
  void rejectsReversedValidityAndHolidaysOutsideEitherBoundary() {
    assertEquals(
        HttpStatus.BAD_REQUEST,
        assertThrows(
                ApiException.class,
                () -> service.save(null, request(start, start.minusDays(1), Set.of())))
            .getStatus());
    for (LocalDate excluded : List.of(start.minusDays(1), start.plusDays(6))) {
      assertThrows(
          ApiException.class,
          () -> service.save(null, request(start, start.plusDays(5), Set.of(excluded))));
    }
    verifyNoInteractions(vehicles, schedules, transactions);
  }

  @Test
  void rejectsMissingResourcesWithoutSaving() {
    var request = request(start, start, Set.of());
    assertThrows(ApiException.class, () -> service.save(null, request));
    when(vehicles.findById(1)).thenReturn(Optional.of(new Vehicle()));
    assertThrows(ApiException.class, () -> service.save(null, request));
    when(drivers.existsById(3)).thenReturn(true);
    assertThrows(ApiException.class, () -> service.save(null, request));
    verify(schedules, never()).save(any());
  }

  @Test
  void missingScheduleRollsBackUpdateAndDisable() {
    resourcesExist();
    assertEquals(
        HttpStatus.NOT_FOUND,
        assertThrows(ApiException.class, () -> service.save(7, request(start, start, Set.of())))
            .getStatus());
    assertThrows(ApiException.class, () -> service.disable(7));
    verify(transactions, times(2)).rollback(any());
    verify(schedules, never()).save(any());
  }

  @Test
  void listsAndDisablesWithoutGeneratingOrDeletingTrips() {
    when(schedules.findAll()).thenReturn(List.of(schedule));
    assertEquals(List.of(schedule), service.list());
    when(schedules.lockById(7)).thenReturn(Optional.of(schedule));
    service.disable(7);
    assertFalse(schedule.getIsActive());
    verify(schedules).save(schedule);
    verifyNoInteractions(worker);
  }

  @Test
  void rejectsInvalidGenerationRangesAndMissingSchedule() {
    assertThrows(ApiException.class, () -> service.run(7, null, start, true));
    assertThrows(ApiException.class, () -> service.run(7, start, null, true));
    assertThrows(ApiException.class, () -> service.run(7, start, start.minusDays(1), true));
    assertThrows(ApiException.class, () -> service.run(7, start, start.plusDays(30), true));
    assertEquals(
        HttpStatus.NOT_FOUND,
        assertThrows(ApiException.class, () -> service.run(7, start, start, true)).getStatus());
    verifyNoInteractions(worker);
  }

  @Test
  void previewIncludesBothEndsOfThirtyDayRangeAndDoesNotWriteAudit() {
    when(schedules.existsById(7)).thenReturn(true);
    when(worker.preview(eq(7), any()))
        .thenAnswer(
            i -> new ScheduleOccurrenceService.Result(i.getArgument(1), "READY", null, "ready"));
    var results = service.run(7, start, start.plusDays(29), true);
    assertEquals(30, results.size());
    assertEquals(start, results.getFirst().serviceDate());
    assertEquals(start.plusDays(29), results.getLast().serviceDate());
    verify(worker, times(30)).preview(eq(7), any());
    verify(worker, never()).generate(any(), any());
    verifyNoInteractions(mapper, transactions);
    verify(schedules, never()).save(any());
  }

  @Test
  void generationContinuesAfterErrorsAndPersistsAudit() throws Exception {
    when(schedules.existsById(7)).thenReturn(true);
    when(schedules.lockById(7)).thenReturn(Optional.of(schedule));
    when(worker.generate(7, start)).thenThrow(ApiException.conflict("Vehicle busy"));
    when(worker.generate(7, start.plusDays(1)))
        .thenThrow(new IllegalStateException("private details"));
    var created = new ScheduleOccurrenceService.Result(start.plusDays(2), "CREATED", 50, "created");
    when(worker.generate(7, start.plusDays(2))).thenReturn(created);
    when(mapper.writeValueAsString(any())).thenReturn("audit-json");
    var results = service.run(7, start, start.plusDays(2), false);
    assertEquals(
        List.of("ERROR", "ERROR", "CREATED"), results.stream().map(r -> r.status()).toList());
    assertEquals("Vehicle busy", results.getFirst().message());
    assertFalse(results.get(1).message().contains("private details"));
    assertEquals(start.plusDays(1), results.get(1).serviceDate());
    assertSame(created, results.getLast());
    assertEquals("audit-json", schedule.getLastResult());
    assertNotNull(schedule.getLastRunAt());
    verify(mapper).writeValueAsString(results);
    verify(schedules).save(schedule);
    verify(worker, never()).preview(any(), any());
    verify(transactions).commit(any());
  }

  @Test
  void auditSerializationFailureIsReportedWithoutSavingFalseSuccess() throws Exception {
    when(schedules.existsById(7)).thenReturn(true);
    when(worker.generate(7, start))
        .thenReturn(new ScheduleOccurrenceService.Result(start, "CREATED", 50, "ok"));
    var failure = new JsonProcessingException("serialization failed") {};
    when(mapper.writeValueAsString(any())).thenThrow(failure);
    assertSame(
        failure,
        assertThrows(IllegalStateException.class, () -> service.run(7, start, start, false))
            .getCause());
    verify(schedules, never()).save(any());
  }
}
