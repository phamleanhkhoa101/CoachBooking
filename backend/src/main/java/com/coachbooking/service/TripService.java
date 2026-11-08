package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Driver;
import com.coachbooking.domain.Route;
import com.coachbooking.domain.TicketPrice;
import com.coachbooking.domain.Trip;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.repo.DriverRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import jakarta.validation.constraints.NotNull;
import java.math.BigDecimal;
import java.time.Duration;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class TripService {

  private final TripRepository tripRepository;
  private final VehicleRepository vehicleRepository;
  private final VehicleTypeRepository vehicleTypeRepository;
  private final RouteRepository routeRepository;
  private final DriverRepository driverRepository;
  private final TicketPriceRepository ticketPriceRepository;
  private final TicketRepository ticketRepository;

  public TripService(
      TripRepository tripRepository,
      VehicleRepository vehicleRepository,
      VehicleTypeRepository vehicleTypeRepository,
      RouteRepository routeRepository,
      DriverRepository driverRepository,
      TicketPriceRepository ticketPriceRepository,
      TicketRepository ticketRepository) {
    this.tripRepository = tripRepository;
    this.vehicleRepository = vehicleRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.routeRepository = routeRepository;
    this.driverRepository = driverRepository;
    this.ticketPriceRepository = ticketPriceRepository;
    this.ticketRepository = ticketRepository;
  }

  public List<TripRepository.TripRow> listAll() {
    return tripRepository.findAllRows();
  }

  public List<TripRepository.TripRow> search(
      String departure, String destination, LocalDate date, Integer fromHour, Integer toHour) {
    LocalDate searchDate = date == null ? LocalDate.now() : date;
    if (fromHour != null && toHour != null && fromHour > toHour) {
      throw ApiException.badRequest("Khung giờ không hợp lệ.");
    }
    LocalDateTime start = searchDate.atStartOfDay();
    LocalDateTime end = searchDate.plusDays(1).atStartOfDay();
    LocalDateTime now = LocalDateTime.now();
    return tripRepository
        .search(blankToNull(departure), blankToNull(destination), start, end)
        .stream()
        .filter(row -> row.getActualPrice() != null && row.getActualPrice().signum() > 0)
        .filter(row -> row.getDepartureTime().isAfter(now))
        .filter(row -> fromHour == null || row.getDepartureTime().getHour() >= fromHour)
        .filter(row -> toHour == null || row.getDepartureTime().getHour() <= toHour)
        .toList();
  }

  public SeatMapResponse seatMap(Integer tripId) {
    Trip trip = findTrip(tripId);
    Vehicle vehicle =
        vehicleRepository
            .findById(trip.getVehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe của chuyến không tồn tại."));
    VehicleType vehicleType =
        vehicleTypeRepository
            .findById(vehicle.getTypeId())
            .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
    Route route =
        routeRepository
            .findById(trip.getRouteId())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    return new SeatMapResponse(
        trip.getTripId(),
        vehicleType.getTypeName(),
        vehicleType.getSeatCapacity(),
        trip.getActualPrice(),
        ticketRepository.findSeatNumbersByTripId(tripId),
        route.getDeparture(),
        route.getDestination(),
        trip.getDepartureTime());
  }

  @Transactional
  public Trip create(TripRequest request) {
    Vehicle vehicle =
        vehicleRepository
            .findById(request.vehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    Route route =
        routeRepository
            .findById(request.routeId())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    Driver driver =
        driverRepository
            .findById(request.driverId())
            .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
    LocalDateTime departure = requireDeparture(request.departureTime());
    checkConflict(vehicle.getVehicleId(), departure, null);

    TicketPrice ticketPrice =
        ticketPriceRepository
            .findByCompanyIdAndTypeIdAndRouteId(
                vehicle.getCompanyId(), vehicle.getTypeId(), route.getRouteId())
            .orElseThrow(
                () ->
                    ApiException.conflict(
                        "Chưa có bảng giá cho nhà xe, loại xe và tuyến đường này."));

    Trip trip = new Trip();
    apply(trip, vehicle, route, driver, departure, ticketPrice.getPrice());
    return tripRepository.save(trip);
  }

  @Transactional
  public Trip update(Integer tripId, TripRequest request) {
    Trip trip = findTrip(tripId);
    Vehicle vehicle =
        vehicleRepository
            .findById(request.vehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    Route route =
        routeRepository
            .findById(request.routeId())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    Driver driver =
        driverRepository
            .findById(request.driverId())
            .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
    LocalDateTime departure = requireDeparture(request.departureTime());
    if (ticketRepository.existsByTripId(tripId)) {
      boolean changedCoreInfo =
          !trip.getVehicleId().equals(vehicle.getVehicleId())
              || !trip.getRouteId().equals(route.getRouteId())
              || !trip.getDepartureTime().equals(departure);
      if (changedCoreInfo) {
        throw ApiException.conflict(
            "Chuyến đã bán vé, chỉ có thể đổi tài xế. Không thể đổi xe, tuyến hoặc giờ khởi hành.");
      }
    }
    checkConflict(vehicle.getVehicleId(), departure, tripId);

    TicketPrice ticketPrice =
        ticketPriceRepository
            .findByCompanyIdAndTypeIdAndRouteId(
                vehicle.getCompanyId(), vehicle.getTypeId(), route.getRouteId())
            .orElseThrow(
                () ->
                    ApiException.conflict(
                        "Chưa có bảng giá cho nhà xe, loại xe và tuyến đường này."));

    apply(trip, vehicle, route, driver, departure, ticketPrice.getPrice());
    return tripRepository.save(trip);
  }

  @Transactional
  public Map<String, String> delete(Integer tripId) {
    Trip trip = findTrip(tripId);
    if (ticketRepository.existsByTripId(tripId)) {
      throw ApiException.conflict("Không thể xóa chuyến xe đã có vé.");
    }
    tripRepository.delete(trip);
    return Map.of("message", "Xóa chuyến xe thành công.");
  }

  public Trip findTrip(Integer tripId) {
    return tripRepository
        .findById(tripId)
        .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
  }

  private void apply(
      Trip trip,
      Vehicle vehicle,
      Route route,
      Driver driver,
      LocalDateTime departure,
      BigDecimal price) {
    trip.setVehicleId(vehicle.getVehicleId());
    trip.setRouteId(route.getRouteId());
    trip.setDriverId(driver.getDriverId());
    trip.setDepartureTime(departure);
    trip.setEstimatedArrivalTime(departure.plus(durationOf(route)));
    trip.setActualPrice(price);
  }

  private LocalDateTime requireDeparture(LocalDateTime departure) {
    if (departure == null) {
      throw ApiException.badRequest("Vui lòng chọn ngày giờ khởi hành.");
    }
    if (!departure.isAfter(LocalDateTime.now())) {
      throw ApiException.badRequest("Ngày giờ khởi hành phải sau thời điểm hiện tại.");
    }
    return departure;
  }

  private void checkConflict(Integer vehicleId, LocalDateTime departure, Integer excludeId) {
    LocalDateTime start = departure.withSecond(0).withNano(0);
    LocalDateTime end = start.plusMinutes(1);
    boolean conflict =
        excludeId == null
            ? tripRepository.existsByVehicleIdAndDepartureTimeBetween(vehicleId, start, end)
            : tripRepository.existsByVehicleIdAndDepartureTimeBetweenAndTripIdNot(
                vehicleId, start, end, excludeId);
    if (conflict) {
      throw ApiException.conflict(
          "Xe này đã được lên chuyến vào cùng thời điểm. Vui lòng chọn xe hoặc giờ khác.");
    }
  }

  private Duration durationOf(Route route) {
    return route.getEstimatedDuration() == null
        ? Duration.ofHours(6)
        : Duration.ofSeconds(route.getEstimatedDuration().toSecondOfDay());
  }

  private String blankToNull(String value) {
    return Validators.isBlank(value) ? null : value.trim();
  }

  public record TripRequest(
      @NotNull(message = "Vui lòng chọn xe.")
          @com.fasterxml.jackson.annotation.JsonAlias({"vehicleId", "maXe"})
          Integer vehicleId,
      @NotNull(message = "Vui lòng chọn tuyến đường.")
          @com.fasterxml.jackson.annotation.JsonAlias({"routeId", "maTuyen"})
          Integer routeId,
      @NotNull(message = "Vui lòng chọn tài xế.")
          @com.fasterxml.jackson.annotation.JsonAlias({"driverId", "maTx"})
          Integer driverId,
      @NotNull(message = "Vui lòng chọn ngày giờ khởi hành.")
          @com.fasterxml.jackson.annotation.JsonAlias({"departureTime", "ngayGioKhoiHanh"})
          LocalDateTime departureTime) {}

  public record SeatMapResponse(
      Integer tripId,
      String typeName,
      Integer seatCapacity,
      BigDecimal price,
      List<Integer> bookedSeats,
      String departure,
      String destination,
      LocalDateTime departureTime) {
    public Integer getMaChuyen() {
      return tripId;
    }

    public String getTenLoaiXe() {
      return typeName;
    }

    public Integer getSoCho() {
      return seatCapacity;
    }

    public BigDecimal getGiaVe() {
      return price;
    }

    public List<Integer> getGheDaDat() {
      return bookedSeats;
    }

    public String getDiemDi() {
      return departure;
    }

    public String getDiemDen() {
      return destination;
    }

    public LocalDateTime getNgayGioKhoiHanh() {
      return departureTime;
    }
  }
}
