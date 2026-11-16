package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.repo.*;
import java.math.BigDecimal;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;

class ScheduleOccurrenceServiceTest {
  TripScheduleRepository schedules = mock(TripScheduleRepository.class);
  ScheduleOccurrenceRepository occurrences = mock(ScheduleOccurrenceRepository.class);
  TripService trips = mock(TripService.class);
  VehicleRepository vehicles = mock(VehicleRepository.class);
  DriverRepository drivers = mock(DriverRepository.class);
  RouteRepository routes = mock(RouteRepository.class);
  VehicleTypeRepository types = mock(VehicleTypeRepository.class);
  BusCompanyRepository companies = mock(BusCompanyRepository.class);
  TicketPriceRepository prices = mock(TicketPriceRepository.class);
  ScheduleOccurrenceService service =
      new ScheduleOccurrenceService(
          schedules, occurrences, trips, vehicles, drivers, routes, types, companies, prices);
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
    assertEquals("SKIPPED", service.generate(1, date.plusDays(11)).status());
    schedule.setWeekdays(String.valueOf(date.plusDays(1).getDayOfWeek().getValue()));
    assertEquals("SKIPPED", service.generate(1, date).status());
    LocalDate yesterday = LocalDate.now(TripPlanningTime.ZONE).minusDays(1);
    schedule.setValidFrom(yesterday);
    schedule.setWeekdays("1,2,3,4,5,6,7");
    assertEquals("PAST", service.generate(1, yesterday).status());
    verifyNoInteractions(trips);
  }

  void validResources() {
    Vehicle vehicle = new Vehicle();
    vehicle.setIsActive(true);
    vehicle.setTypeId(2);
    vehicle.setCompanyId(3);
    Driver driver = new Driver();
    driver.setIsActive(true);
    Route route = new Route();
    route.setIsActive(true);
    route.setEstimatedDuration(LocalTime.of(4, 30));
    VehicleType type = new VehicleType();
    type.setIsActive(true);
    BusCompany company = new BusCompany();
    company.setIsActive(true);
    TicketPrice price = new TicketPrice();
    price.setPrice(BigDecimal.valueOf(200000));
    when(vehicles.findById(1)).thenReturn(Optional.of(vehicle));
    when(drivers.findById(1)).thenReturn(Optional.of(driver));
    when(routes.findById(1)).thenReturn(Optional.of(route));
    when(types.findById(2)).thenReturn(Optional.of(type));
    when(companies.findById(3)).thenReturn(Optional.of(company));
    when(prices.findByCompanyIdAndTypeIdAndRouteId(3, 2, 1)).thenReturn(Optional.of(price));
  }

  @Test
  void previewChecksConflictUsingRouteDurationWithoutCreatingTrip() {
    validResources();
    assertEquals("READY", service.preview(1, date).status());
    verify(trips).checkConflict(1, 1, date.atTime(8, 0), date.atTime(12, 30), null);
    verify(trips, never()).create(any());
    verify(occurrences, never()).saveAndFlush(any());
  }

  @Test
  void missingDurationUsesSixHoursAndConflictBecomesPreviewError() {
    validResources();
    routes.findById(1).orElseThrow().setEstimatedDuration(null);
    doThrow(ApiException.conflict("Driver busy"))
        .when(trips)
        .checkConflict(1, 1, date.atTime(8, 0), date.atTime(14, 0), null);
    var result = service.preview(1, date);
    assertEquals("ERROR", result.status());
    assertEquals("Driver busy", result.message());
    verify(occurrences, never()).saveAndFlush(any());
  }

  @ParameterizedTest
  @ValueSource(strings = {"vehicle", "driver", "route", "type", "company"})
  void rejectsMissingResources(String resource) {
    validResources();
    switch (resource) {
      case "vehicle" -> when(vehicles.findById(1)).thenReturn(Optional.empty());
      case "driver" -> when(drivers.findById(1)).thenReturn(Optional.empty());
      case "route" -> when(routes.findById(1)).thenReturn(Optional.empty());
      case "type" -> when(types.findById(2)).thenReturn(Optional.empty());
      case "company" -> when(companies.findById(3)).thenReturn(Optional.empty());
    }
    assertEquals("ERROR", service.preview(1, date).status());
    verifyNoInteractions(trips);
  }

  @ParameterizedTest
  @ValueSource(strings = {"vehicle", "driver", "route", "type", "company"})
  void rejectsInactiveResources(String resource) {
    validResources();
    switch (resource) {
      case "vehicle" -> vehicles.findById(1).orElseThrow().setIsActive(false);
      case "driver" -> drivers.findById(1).orElseThrow().setIsActive(false);
      case "route" -> routes.findById(1).orElseThrow().setIsActive(false);
      case "type" -> types.findById(2).orElseThrow().setIsActive(false);
      case "company" -> companies.findById(3).orElseThrow().setIsActive(false);
    }
    assertThrows(ApiException.class, () -> service.validate(schedule, date));
    verifyNoInteractions(trips);
  }

  @Test
  void rejectsZeroDurationAndMissingOrNonPositiveFare() {
    validResources();
    Route route = routes.findById(1).orElseThrow();
    route.setEstimatedDuration(LocalTime.MIDNIGHT);
    assertThrows(ApiException.class, () -> service.validate(schedule, date));
    route.setEstimatedDuration(LocalTime.of(1, 0));
    when(prices.findByCompanyIdAndTypeIdAndRouteId(3, 2, 1)).thenReturn(Optional.empty());
    assertThrows(ApiException.class, () -> service.validate(schedule, date));
    for (BigDecimal fare : List.of(BigDecimal.ZERO, BigDecimal.valueOf(-1))) {
      TicketPrice price = new TicketPrice();
      price.setPrice(fare);
      when(prices.findByCompanyIdAndTypeIdAndRouteId(3, 2, 1)).thenReturn(Optional.of(price));
      assertThrows(ApiException.class, () -> service.validate(schedule, date));
    }
    verifyNoInteractions(trips);
  }

  @Test
  void missingScheduleFailsBothPreviewAndGeneration() {
    assertThrows(ApiException.class, () -> service.preview(99, date));
    assertThrows(ApiException.class, () -> service.generate(99, date));
    verifyNoInteractions(trips, occurrences);
  }

  @Test
  void failedTripCreationDoesNotRecordSuccessfulOccurrence() {
    doThrow(ApiException.conflict("Vehicle busy")).when(trips).create(any());
    assertThrows(ApiException.class, () -> service.generate(1, date));
    verify(occurrences, never()).saveAndFlush(any());
  }
}
