package com.coachbooking.service;

import com.coachbooking.repo.SeatHoldRepository;
import java.time.LocalDateTime;
import java.util.List;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SeatHoldCleanupService {

  private final SeatHoldRepository seatHoldRepository;
  private final SeatStatusService seatStatusService;

  public SeatHoldCleanupService(
      SeatHoldRepository seatHoldRepository, SeatStatusService seatStatusService) {
    this.seatHoldRepository = seatHoldRepository;
    this.seatStatusService = seatStatusService;
  }

  @Scheduled(fixedDelay = 60_000)
  @Transactional
  public void cleanupExpiredHolds() {
    LocalDateTime now = LocalDateTime.now();
    List<Integer> tripIds = seatHoldRepository.findTripIdsWithExpiredHolds(now);
    for (Integer tripId : tripIds) {
      if (seatHoldRepository.deleteExpiredByTripId(tripId, now) > 0) {
        seatStatusService.publishAfterCommit(tripId);
      }
    }
  }
}
