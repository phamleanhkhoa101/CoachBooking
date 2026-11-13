package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.domain.SeatHold;
import com.coachbooking.dto.hold.SeatStatusResponse;
import com.coachbooking.repo.SeatHoldRepository;
import com.coachbooking.repo.TicketRepository;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.transaction.support.TransactionSynchronization;
import org.springframework.transaction.support.TransactionSynchronizationManager;
import org.springframework.web.servlet.mvc.method.annotation.SseEmitter;

class SeatStatusServiceTest {

  private static final int TRIP_ID = 11;

  private TicketRepository ticketRepository;
  private SeatHoldRepository seatHoldRepository;
  private SeatStatusService service;
  private RecordingSeatStatusService recording;

  static class RecordingSeatStatusService extends SeatStatusService {

    private final List<Integer> published = new ArrayList<>();

    RecordingSeatStatusService(
        TicketRepository ticketRepository, SeatHoldRepository seatHoldRepository) {
      super(ticketRepository, seatHoldRepository);
    }

    @Override
    public void publish(Integer tripId) {
      published.add(tripId);
    }
  }

  @BeforeEach
  void setUp() {
    ticketRepository = mock(TicketRepository.class);
    seatHoldRepository = mock(SeatHoldRepository.class);
    service = new SeatStatusService(ticketRepository, seatHoldRepository);
    recording = new RecordingSeatStatusService(ticketRepository, seatHoldRepository);
    when(ticketRepository.findSeatNumbersByTripId(TRIP_ID)).thenReturn(List.of(1, 2));
    when(seatHoldRepository.findByTripIdAndExpiresAtAfterOrderBySeatNumber(any(), any()))
        .thenReturn(List.of(hold(5)));
  }

  @Test
  void snapshotReturnsBookedAndHeldSeats() {
    SeatStatusResponse status = service.snapshot(TRIP_ID);

    assertEquals(TRIP_ID, status.tripId());
    assertEquals(List.of(1, 2), status.bookedSeats());
    assertEquals(List.of(5), status.heldSeats());
    assertNotNull(status.occurredAt());
  }

  @Test
  void publishAfterCommitRunsImmediatelyWithoutTransaction() {
    recording.publishAfterCommit(TRIP_ID);

    assertEquals(List.of(TRIP_ID), recording.published);
  }

  @Test
  void publishAfterCommitWaitsForCommitWhenTransactionIsActive() {
    TransactionSynchronizationManager.initSynchronization();
    try {
      recording.publishAfterCommit(TRIP_ID);
      assertTrue(recording.published.isEmpty(), "Chưa được publish trước khi commit");

      for (TransactionSynchronization synchronization :
          TransactionSynchronizationManager.getSynchronizations()) {
        synchronization.afterCommit();
      }

      assertEquals(List.of(TRIP_ID), recording.published);
    } finally {
      TransactionSynchronizationManager.clearSynchronization();
    }
  }

  @Test
  void subscribeSendsCurrentSnapshot() {
    SseEmitter emitter = service.subscribe(TRIP_ID);

    assertNotNull(emitter);
    verify(ticketRepository).findSeatNumbersByTripId(TRIP_ID);
    verify(seatHoldRepository).findByTripIdAndExpiresAtAfterOrderBySeatNumber(any(), any());
    emitter.complete();
  }

  @Test
  void publishNotifiesSubscribersWithFreshSnapshot() {
    service.subscribe(TRIP_ID);

    service.publish(TRIP_ID);

    verify(ticketRepository, times(2)).findSeatNumbersByTripId(TRIP_ID);
  }

  @Test
  void publishIsNoOpWithoutSubscribers() {
    service.publish(TRIP_ID);

    verify(ticketRepository, never()).findSeatNumbersByTripId(TRIP_ID);
  }

  private SeatHold hold(int seatNumber) {
    SeatHold hold = new SeatHold();
    hold.setTripId(TRIP_ID);
    hold.setAccountId(7);
    hold.setSeatNumber(seatNumber);
    hold.setExpiresAt(LocalDateTime.now().plusMinutes(10));
    return hold;
  }
}
