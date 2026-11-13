package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.repo.SeatHoldRepository;
import java.time.LocalDateTime;
import java.util.List;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class SeatHoldCleanupServiceTest {

  private SeatHoldRepository seatHoldRepository;
  private SeatStatusService seatStatusService;
  private SeatHoldCleanupService service;

  @BeforeEach
  void setUp() {
    seatHoldRepository = mock(SeatHoldRepository.class);
    seatStatusService = mock(SeatStatusService.class);
    service = new SeatHoldCleanupService(seatHoldRepository, seatStatusService);
  }

  @Test
  void cleanupPublishesOnlyForTripsWithDeletedHolds() {
    when(seatHoldRepository.findTripIdsWithExpiredHolds(any(LocalDateTime.class)))
        .thenReturn(List.of(11, 12));
    when(seatHoldRepository.deleteExpiredByTripId(anyInt(), any(LocalDateTime.class)))
        .thenReturn(2)
        .thenReturn(0);

    service.cleanupExpiredHolds();

    verify(seatStatusService).publishAfterCommit(11);
    verify(seatStatusService, never()).publishAfterCommit(12);
  }

  @Test
  void cleanupDoesNothingWhenNoExpiredHolds() {
    when(seatHoldRepository.findTripIdsWithExpiredHolds(any(LocalDateTime.class)))
        .thenReturn(List.of());

    assertDoesNotThrow(() -> service.cleanupExpiredHolds());

    verify(seatHoldRepository, never()).deleteExpiredByTripId(anyInt(), any(LocalDateTime.class));
    verify(seatStatusService, never()).publishAfterCommit(any());
  }
}
