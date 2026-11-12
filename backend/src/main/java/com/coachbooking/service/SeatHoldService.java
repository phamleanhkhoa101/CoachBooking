package com.coachbooking.service;

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
import java.time.LocalDateTime;
import java.time.OffsetDateTime;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.UUID;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SeatHoldService {

  public static final int HOLD_MINUTES = 10;
  private static final ZoneId VIETNAM_ZONE = ZoneId.of("Asia/Ho_Chi_Minh");

  private final TripRepository tripRepository;
  private final VehicleRepository vehicleRepository;
  private final VehicleTypeRepository vehicleTypeRepository;
  private final TicketRepository ticketRepository;
  private final SeatHoldRepository seatHoldRepository;
  private final SeatStatusService seatStatusService;

  public SeatHoldService(
      TripRepository tripRepository,
      VehicleRepository vehicleRepository,
      VehicleTypeRepository vehicleTypeRepository,
      TicketRepository ticketRepository,
      SeatHoldRepository seatHoldRepository,
      SeatStatusService seatStatusService) {
    this.tripRepository = tripRepository;
    this.vehicleRepository = vehicleRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.ticketRepository = ticketRepository;
    this.seatHoldRepository = seatHoldRepository;
    this.seatStatusService = seatStatusService;
  }

  @Transactional
  public SeatHoldResponse create(Account account, SeatHoldRequest request) {
    LocalDateTime now = LocalDateTime.now();
    Trip trip = lockTrip(request.tripId());
    if (!trip.getDepartureTime().isAfter(now)) {
      throw ApiException.conflict("Chuyến xe đã khởi hành, không thể giữ ghế.");
    }
    List<Integer> seats = normalizeSeats(request.seatNumbers());
    int capacity = seatCapacity(trip);
    if (seats.get(seats.size() - 1) > capacity) {
      throw ApiException.badRequest("Ghế chọn vượt quá số chỗ của xe (" + capacity + " chỗ).");
    }

    seatHoldRepository.deleteExpiredByTripId(trip.getTripId(), now);
    List<Integer> booked = ticketRepository.findSeatNumbersByTripId(trip.getTripId());
    List<Integer> held =
        seatHoldRepository
            .findByTripIdAndExpiresAtAfterOrderBySeatNumber(trip.getTripId(), now)
            .stream()
            .filter(hold -> !hold.getAccountId().equals(account.getAccountId()))
            .map(SeatHold::getSeatNumber)
            .toList();
    List<Integer> unavailable =
        seats.stream().filter(seat -> booked.contains(seat) || held.contains(seat)).toList();
    if (!unavailable.isEmpty()) {
      throw ApiException.conflict(
          "Ghế số " + joinSeats(unavailable) + " không còn trống. Vui lòng chọn ghế khác.");
    }

    seatHoldRepository.deleteByTripIdAndAccountId(trip.getTripId(), account.getAccountId());
    UUID token = UUID.randomUUID();
    LocalDateTime expiresAt = now.plusMinutes(HOLD_MINUTES);
    List<SeatHold> holds = new ArrayList<>();
    for (Integer seat : seats) {
      SeatHold hold = new SeatHold();
      hold.setHoldToken(token);
      hold.setAccountId(account.getAccountId());
      hold.setTripId(trip.getTripId());
      hold.setSeatNumber(seat);
      hold.setCreatedAt(now);
      hold.setExpiresAt(expiresAt);
      holds.add(hold);
    }
    seatHoldRepository.saveAllAndFlush(holds);
    seatStatusService.publishAfterCommit(trip.getTripId());
    return new SeatHoldResponse(
        token, trip.getTripId(), seats, expiresAt.atZone(VIETNAM_ZONE).toOffsetDateTime());
  }

  @Transactional(readOnly = true)
  public SeatHoldResponse get(Account account, UUID token) {
    List<SeatHold> holds = requireOwnedHold(account, token);
    SeatHold first = holds.get(0);
    if (!first.getExpiresAt().isAfter(LocalDateTime.now())) {
      throw ApiException.conflict("Thời gian giữ ghế đã hết. Vui lòng chọn lại ghế.");
    }
    return new SeatHoldResponse(
        token,
        first.getTripId(),
        holds.stream().map(SeatHold::getSeatNumber).toList(),
        first.getExpiresAt().atZone(VIETNAM_ZONE).toOffsetDateTime());
  }

  @Transactional
  public void release(Account account, UUID token) {
    List<SeatHold> holds = requireOwnedHold(account, token);
    Integer tripId = holds.get(0).getTripId();
    lockTrip(tripId);
    seatHoldRepository.deleteByHoldToken(token);
    seatStatusService.publishAfterCommit(tripId);
  }

  private List<SeatHold> requireOwnedHold(Account account, UUID token) {
    List<SeatHold> holds = seatHoldRepository.findByHoldTokenOrderBySeatNumber(token);
    if (holds.isEmpty()) {
      throw ApiException.notFound("Không tìm thấy lượt giữ ghế.");
    }
    if (holds.stream().anyMatch(hold -> !hold.getAccountId().equals(account.getAccountId()))) {
      throw ApiException.forbidden("Bạn không có quyền sử dụng lượt giữ ghế này.");
    }
    return holds;
  }

  private Trip lockTrip(Integer tripId) {
    return tripRepository
        .findByIdForUpdate(tripId)
        .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
  }

  private int seatCapacity(Trip trip) {
    Vehicle vehicle =
        vehicleRepository
            .findById(trip.getVehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe của chuyến không tồn tại."));
    VehicleType type =
        vehicleTypeRepository
            .findById(vehicle.getTypeId())
            .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
    return type.getSeatCapacity();
  }

  private List<Integer> normalizeSeats(List<Integer> requested) {
    if (requested == null || requested.isEmpty()) {
      throw ApiException.badRequest("Vui lòng chọn ít nhất một ghế.");
    }
    List<Integer> seats = requested.stream().filter(Objects::nonNull).sorted().toList();
    if (seats.size() != requested.size() || seats.get(0) < 1) {
      throw ApiException.badRequest("Danh sách ghế không hợp lệ.");
    }
    if (seats.stream().distinct().count() != seats.size()) {
      throw ApiException.badRequest("Danh sách ghế bị trùng.");
    }
    return seats;
  }

  private String joinSeats(List<Integer> seats) {
    return String.join(", ", seats.stream().map(String::valueOf).toList());
  }
}
