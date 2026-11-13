package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Trip;
import com.coachbooking.dto.trip.SeatMapResponse;
import com.coachbooking.dto.trip.TripRequest;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.service.SeatStatusService;
import com.coachbooking.service.TripService;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.web.servlet.mvc.method.annotation.SseEmitter;

class TripControllerTest {

  private TripRepository tripRepository;
  private TripService tripService;
  private SeatStatusService seatStatusService;
  private TripController controller;

  @BeforeEach
  void setUp() {
    tripRepository = mock(TripRepository.class);
    tripService = mock(TripService.class);
    seatStatusService = mock(SeatStatusService.class);
    controller = new TripController(tripRepository, tripService, seatStatusService);
  }

  @Test
  void seatEventsValidatesSeatMapThenSubscribes() {
    SeatMapResponse seatMap =
        new SeatMapResponse(
            11,
            "Giường nằm 40 chỗ",
            40,
            BigDecimal.valueOf(150_000),
            List.of(1),
            List.of(2),
            "Bến xe Miền Đông",
            "Đà Lạt",
            LocalDateTime.now().plusDays(1));
    when(tripService.seatMap(11)).thenReturn(seatMap);
    SseEmitter emitter = mock(SseEmitter.class);
    when(seatStatusService.subscribe(11)).thenReturn(emitter);

    assertSame(emitter, controller.seatEvents(11));
    verify(tripService).seatMap(11);
    verify(seatStatusService).subscribe(11);
  }

  @Test
  void listAndSearch() {
    when(tripService.listAll()).thenReturn(List.of());
    when(tripService.search(any(), any(), any(), any(), any())).thenReturn(List.of());

    assertTrue(controller.list().isEmpty());
    assertTrue(
        controller
            .search("SG", null, "DL", null, LocalDate.now(), null, 8, null, 12, null)
            .isEmpty());
  }

  @Test
  void crudEndpoints() {
    Trip trip = new Trip();
    trip.setTripId(1);
    when(tripRepository.findById(1)).thenReturn(java.util.Optional.of(trip));
    when(tripService.findTrip(1)).thenReturn(trip);
    when(tripService.seatMap(1)).thenReturn(null);

    TripRequest req = new TripRequest(1, 1, 1, LocalDateTime.now().plusDays(1));
    when(tripService.create(req)).thenReturn(trip);
    when(tripService.update(1, req)).thenReturn(trip);
    when(tripService.delete(1)).thenReturn(Map.of("message", "ok"));

    assertEquals(1, controller.get(1).getTripId());
    assertNull(controller.seatMap(1));
    assertEquals(1, controller.create(req).getTripId());
    assertEquals(1, controller.update(1, req).getTripId());
    assertEquals("ok", controller.delete(1).get("message"));
  }
}
