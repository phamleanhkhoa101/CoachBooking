package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.assertSame;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.dto.trip.SeatMapResponse;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.service.SeatStatusService;
import com.coachbooking.service.TripService;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;
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
}
