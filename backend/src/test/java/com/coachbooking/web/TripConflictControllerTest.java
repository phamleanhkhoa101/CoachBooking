package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Trip;
import com.coachbooking.repo.TripRepository;
import java.time.LocalDateTime;
import java.util.List;
import org.junit.jupiter.api.Test;

class TripConflictControllerTest {
  TripRepository trips = mock(TripRepository.class);
  TripConflictController controller = new TripConflictController(trips);
  LocalDateTime departure = LocalDateTime.of(2030, 1, 1, 8, 0);

  Trip trip(int id, int vehicle, int driver, int hour, Integer duration) {
    Trip trip = new Trip();
    trip.setTripId(id);
    trip.setVehicleId(vehicle);
    trip.setDriverId(driver);
    trip.setDepartureTime(departure.plusHours(hour));
    trip.setEstimatedArrivalTime(
        duration == null ? null : trip.getDepartureTime().plusHours(duration));
    return trip;
  }

  @Test
  void reportsMissingAndInvalidArrivalTimes() {
    when(trips.findAllByOrderByDepartureTimeAsc())
        .thenReturn(List.of(trip(1, 1, 1, 0, null), trip(2, 2, 2, 1, 0), trip(3, 3, 3, 2, -1)));
    var conflicts = controller.list();
    assertEquals(List.of(1, 2, 3), conflicts.stream().map(c -> c.firstTripId()).toList());
    assertTrue(conflicts.stream().allMatch(c -> c.secondTripId() == null && !c.reason().isBlank()));
  }

  @Test
  void detectsSharedVehicleOrDriverAndAllowsAdjacentTrips() {
    when(trips.findAllByOrderByDepartureTimeAsc())
        .thenReturn(
            List.of(
                trip(1, 1, 1, 0, 4),
                trip(2, 1, 2, 1, 1),
                trip(3, 3, 1, 2, 1),
                trip(4, 4, 4, 3, 1),
                trip(5, 1, 1, 4, 2)));
    var conflicts = controller.list();
    assertEquals(2, conflicts.size());
    assertEquals(List.of(2, 3), conflicts.stream().map(c -> c.secondTripId()).toList());
    assertTrue(conflicts.stream().allMatch(c -> c.firstTripId() == 1));
  }

  @Test
  void emptyTimetableHasNoConflicts() {
    when(trips.findAllByOrderByDepartureTimeAsc()).thenReturn(List.of());
    assertTrue(controller.list().isEmpty());
  }
}
