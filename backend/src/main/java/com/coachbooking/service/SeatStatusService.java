package com.coachbooking.service;

import com.coachbooking.dto.hold.SeatStatusResponse;
import com.coachbooking.repo.SeatHoldRepository;
import com.coachbooking.repo.TicketRepository;
import java.io.IOException;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.CopyOnWriteArrayList;
import org.springframework.stereotype.Service;
import org.springframework.transaction.support.TransactionSynchronization;
import org.springframework.transaction.support.TransactionSynchronizationManager;
import org.springframework.web.servlet.mvc.method.annotation.SseEmitter;

@Service
public class SeatStatusService {

  private final TicketRepository ticketRepository;
  private final SeatHoldRepository seatHoldRepository;
  private final Map<Integer, CopyOnWriteArrayList<SseEmitter>> emitters = new ConcurrentHashMap<>();

  public SeatStatusService(
      TicketRepository ticketRepository, SeatHoldRepository seatHoldRepository) {
    this.ticketRepository = ticketRepository;
    this.seatHoldRepository = seatHoldRepository;
  }

  public SeatStatusResponse snapshot(Integer tripId) {
    LocalDateTime now = LocalDateTime.now();
    return new SeatStatusResponse(
        tripId,
        ticketRepository.findSeatNumbersByTripId(tripId),
        seatHoldRepository.findByTripIdAndExpiresAtAfterOrderBySeatNumber(tripId, now).stream()
            .map(hold -> hold.getSeatNumber())
            .toList(),
        now);
  }

  public SseEmitter subscribe(Integer tripId) {
    SseEmitter emitter = new SseEmitter(0L);
    emitters.computeIfAbsent(tripId, ignored -> new CopyOnWriteArrayList<>()).add(emitter);
    emitter.onCompletion(() -> remove(tripId, emitter));
    emitter.onTimeout(() -> remove(tripId, emitter));
    emitter.onError(ignored -> remove(tripId, emitter));
    try {
      emitter.send(SseEmitter.event().name("seat-status").data(snapshot(tripId)));
    } catch (IOException ex) {
      remove(tripId, emitter);
      emitter.completeWithError(ex);
    }
    return emitter;
  }

  public void publishAfterCommit(Integer tripId) {
    if (TransactionSynchronizationManager.isSynchronizationActive()) {
      TransactionSynchronizationManager.registerSynchronization(
          new TransactionSynchronization() {
            @Override
            public void afterCommit() {
              publish(tripId);
            }
          });
    } else {
      publish(tripId);
    }
  }

  public void publish(Integer tripId) {
    List<SseEmitter> tripEmitters = emitters.get(tripId);
    if (tripEmitters == null || tripEmitters.isEmpty()) {
      return;
    }
    SeatStatusResponse status = snapshot(tripId);
    tripEmitters.forEach(
        emitter -> {
          try {
            emitter.send(SseEmitter.event().name("seat-status").data(status));
          } catch (IOException | IllegalStateException ex) {
            remove(tripId, emitter);
            emitter.complete();
          }
        });
  }

  private void remove(Integer tripId, SseEmitter emitter) {
    List<SseEmitter> tripEmitters = emitters.get(tripId);
    if (tripEmitters != null) {
      tripEmitters.remove(emitter);
      if (tripEmitters.isEmpty()) {
        emitters.remove(tripId, tripEmitters);
      }
    }
  }
}
