package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyList;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.domain.SeatHold;
import com.coachbooking.domain.Trip;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.dto.hold.SeatHoldRequest;
import com.coachbooking.dto.hold.SeatHoldResponse;
import com.coachbooking.repo.SeatHoldRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.UUID;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class SeatHoldServiceTest {

  private TripRepository tripRepository;
  private VehicleRepository vehicleRepository;
  private VehicleTypeRepository vehicleTypeRepository;
  private TicketRepository ticketRepository;
  private SeatHoldRepository seatHoldRepository;
  private SeatStatusService seatStatusService;
  private SeatHoldService service;
  private Account account;

  @BeforeEach
  void setUp() {
    tripRepository = mock(TripRepository.class);
    vehicleRepository = mock(VehicleRepository.class);
    vehicleTypeRepository = mock(VehicleTypeRepository.class);
    ticketRepository = mock(TicketRepository.class);
    seatHoldRepository = mock(SeatHoldRepository.class);
    seatStatusService = mock(SeatStatusService.class);
    service =
        new SeatHoldService(
            tripRepository,
            vehicleRepository,
            vehicleTypeRepository,
            ticketRepository,
            seatHoldRepository,
            seatStatusService);

    account = new Account();
    account.setAccountId(7);

    Trip trip = new Trip();
    trip.setTripId(11);
    trip.setVehicleId(5);
    trip.setDepartureTime(LocalDateTime.now().plusDays(1));
    trip.setActualPrice(BigDecimal.valueOf(150_000));
    when(tripRepository.findByIdForUpdate(11)).thenReturn(Optional.of(trip));

    Vehicle vehicle = new Vehicle();
    vehicle.setVehicleId(5);
    vehicle.setTypeId(3);
    when(vehicleRepository.findById(5)).thenReturn(Optional.of(vehicle));

    VehicleType type = new VehicleType();
    type.setTypeId(3);
    type.setSeatCapacity(40);
    when(vehicleTypeRepository.findById(3)).thenReturn(Optional.of(type));
    when(ticketRepository.findSeatNumbersByTripId(11)).thenReturn(List.of());
    when(seatHoldRepository.findByTripIdAndExpiresAtAfterOrderBySeatNumber(any(), any()))
        .thenReturn(List.of());
  }

  @Test
  void createHoldsAllRequestedSeatsWithOneToken() {
    SeatHoldResponse response = service.create(account, new SeatHoldRequest(11, List.of(4, 3)));

    assertEquals(List.of(3, 4), response.seatNumbers());
    assertEquals(11, response.tripId());
    verify(seatHoldRepository).saveAllAndFlush(anyList());
    verify(seatStatusService).publishAfterCommit(11);
  }

  @Test
  void createRejectsWholeRequestWhenOneSeatIsHeldByAnotherAccount() {
    SeatHold other = new SeatHold();
    other.setAccountId(99);
    other.setTripId(11);
    other.setSeatNumber(4);
    other.setExpiresAt(LocalDateTime.now().plusMinutes(5));
    when(seatHoldRepository.findByTripIdAndExpiresAtAfterOrderBySeatNumber(any(), any()))
        .thenReturn(List.of(other));

    ApiException error =
        assertThrows(
            ApiException.class,
            () -> service.create(account, new SeatHoldRequest(11, List.of(3, 4))));

    assertEquals(HttpStatus.CONFLICT, error.getStatus());
    verify(seatHoldRepository, never()).saveAllAndFlush(anyList());
  }

  @Test
  void createRejectsDuplicateSeats() {
    ApiException error =
        assertThrows(
            ApiException.class,
            () -> service.create(account, new SeatHoldRequest(11, List.of(3, 3))));

    assertEquals(HttpStatus.BAD_REQUEST, error.getStatus());
    verify(seatHoldRepository, never()).saveAllAndFlush(anyList());
  }

  @Test
  void createRejectsDepartedTrip() {
    trip(11).setDepartureTime(LocalDateTime.now().minusMinutes(5));

    ApiException error =
        assertThrows(
            ApiException.class, () -> service.create(account, new SeatHoldRequest(11, List.of(1))));

    assertEquals(HttpStatus.CONFLICT, error.getStatus());
    verify(seatHoldRepository, never()).saveAllAndFlush(anyList());
  }

  @Test
  void createRejectsUnknownTrip() {
    when(tripRepository.findByIdForUpdate(99)).thenReturn(Optional.empty());

    ApiException error =
        assertThrows(
            ApiException.class, () -> service.create(account, new SeatHoldRequest(99, List.of(1))));

    assertEquals(HttpStatus.NOT_FOUND, error.getStatus());
  }

  @Test
  void createRejectsUnknownVehicle() {
    when(vehicleRepository.findById(5)).thenReturn(Optional.empty());

    ApiException error =
        assertThrows(
            ApiException.class, () -> service.create(account, new SeatHoldRequest(11, List.of(1))));

    assertEquals(HttpStatus.NOT_FOUND, error.getStatus());
  }

  @Test
  void createRejectsUnknownVehicleType() {
    when(vehicleTypeRepository.findById(3)).thenReturn(Optional.empty());

    ApiException error =
        assertThrows(
            ApiException.class, () -> service.create(account, new SeatHoldRequest(11, List.of(1))));

    assertEquals(HttpStatus.NOT_FOUND, error.getStatus());
  }

  @Test
  void createRejectsSeatBeyondVehicleCapacity() {
    ApiException error =
        assertThrows(
            ApiException.class,
            () -> service.create(account, new SeatHoldRequest(11, List.of(41))));

    assertEquals(HttpStatus.BAD_REQUEST, error.getStatus());
    verify(seatHoldRepository, never()).saveAllAndFlush(anyList());
  }

  @Test
  void createRejectsAlreadyBookedSeat() {
    when(ticketRepository.findSeatNumbersByTripId(11)).thenReturn(List.of(3));

    ApiException error =
        assertThrows(
            ApiException.class,
            () -> service.create(account, new SeatHoldRequest(11, List.of(3, 4))));

    assertEquals(HttpStatus.CONFLICT, error.getStatus());
    verify(seatHoldRepository, never()).saveAllAndFlush(anyList());
  }

  @Test
  void createRejectsEmptySeatList() {
    ApiException error =
        assertThrows(
            ApiException.class, () -> service.create(account, new SeatHoldRequest(11, List.of())));

    assertEquals(HttpStatus.BAD_REQUEST, error.getStatus());
  }

  @Test
  void createRejectsNullSeatNumber() {
    ApiException error =
        assertThrows(
            ApiException.class,
            () -> service.create(account, new SeatHoldRequest(11, Arrays.asList(1, null))));

    assertEquals(HttpStatus.BAD_REQUEST, error.getStatus());
  }

  @Test
  void createRejectsSeatBelowOne() {
    ApiException error =
        assertThrows(
            ApiException.class, () -> service.create(account, new SeatHoldRequest(11, List.of(0))));

    assertEquals(HttpStatus.BAD_REQUEST, error.getStatus());
  }

  @Test
  void getReturnsActiveHold() {
    UUID token = UUID.randomUUID();
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(token))
        .thenReturn(List.of(hold(token, 1), hold(token, 2)));

    SeatHoldResponse response = service.get(account, token);

    assertEquals(token, response.holdToken());
    assertEquals(11, response.tripId());
    assertEquals(List.of(1, 2), response.seatNumbers());
  }

  @Test
  void getRejectsUnknownHold() {
    UUID token = UUID.randomUUID();
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(token)).thenReturn(List.of());

    ApiException error = assertThrows(ApiException.class, () -> service.get(account, token));

    assertEquals(HttpStatus.NOT_FOUND, error.getStatus());
  }

  @Test
  void getRejectsExpiredHold() {
    UUID token = UUID.randomUUID();
    SeatHold expired = hold(token, 1);
    expired.setExpiresAt(LocalDateTime.now().minusMinutes(1));
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(token)).thenReturn(List.of(expired));

    ApiException error = assertThrows(ApiException.class, () -> service.get(account, token));

    assertEquals(HttpStatus.CONFLICT, error.getStatus());
  }

  @Test
  void getRejectsHoldOfAnotherAccount() {
    UUID token = UUID.randomUUID();
    SeatHold other = hold(token, 1);
    other.setAccountId(99);
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(token)).thenReturn(List.of(other));

    ApiException error = assertThrows(ApiException.class, () -> service.get(account, token));

    assertEquals(HttpStatus.FORBIDDEN, error.getStatus());
  }

  @Test
  void releaseDeletesHoldAndNotifiesSeatStatus() {
    UUID token = UUID.randomUUID();
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(token))
        .thenReturn(List.of(hold(token, 1)));

    service.release(account, token);

    verify(seatHoldRepository).deleteByHoldToken(token);
    verify(seatStatusService).publishAfterCommit(11);
  }

  @Test
  void releaseRejectsUnknownHold() {
    UUID token = UUID.randomUUID();
    when(seatHoldRepository.findByHoldTokenOrderBySeatNumber(token)).thenReturn(List.of());

    ApiException error = assertThrows(ApiException.class, () -> service.release(account, token));

    assertEquals(HttpStatus.NOT_FOUND, error.getStatus());
    verify(seatHoldRepository, never()).deleteByHoldToken(token);
  }

  private Trip trip(int tripId) {
    return tripRepository.findByIdForUpdate(tripId).orElseThrow();
  }

  private SeatHold hold(UUID token, int seatNumber) {
    SeatHold hold = new SeatHold();
    hold.setHoldToken(token);
    hold.setAccountId(account.getAccountId());
    hold.setTripId(11);
    hold.setSeatNumber(seatNumber);
    hold.setCreatedAt(LocalDateTime.now());
    hold.setExpiresAt(LocalDateTime.now().plusMinutes(5));
    return hold;
  }
}
