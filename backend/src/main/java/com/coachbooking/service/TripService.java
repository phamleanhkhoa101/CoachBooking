package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.Driver;
import com.coachbooking.domain.Route;
import com.coachbooking.domain.TicketPrice;
import com.coachbooking.domain.Trip;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.dto.trip.SeatMapResponse;
import com.coachbooking.dto.trip.TripRequest;
import com.coachbooking.repo.DriverRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
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
  private final SeatStatusService seatStatusService;
  private final com.coachbooking.repo.ScheduleOccurrenceRepository occurrences;
  private final com.coachbooking.repo.BusCompanyRepository companies;

  public TripService(
      TripRepository tripRepository,
      VehicleRepository vehicleRepository,
      VehicleTypeRepository vehicleTypeRepository,
      RouteRepository routeRepository,
      DriverRepository driverRepository,
      TicketPriceRepository ticketPriceRepository,
      TicketRepository ticketRepository,
      SeatStatusService seatStatusService,
      com.coachbooking.repo.ScheduleOccurrenceRepository occurrences,
      com.coachbooking.repo.BusCompanyRepository companies) {
    this.tripRepository = tripRepository;
    this.vehicleRepository = vehicleRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.routeRepository = routeRepository;
    this.driverRepository = driverRepository;
    this.ticketPriceRepository = ticketPriceRepository;
    this.ticketRepository = ticketRepository;
    this.seatStatusService = seatStatusService;
    this.occurrences = occurrences;
    this.companies = companies;
  }

  public List<TripRepository.TripRow> listAll() {
    return tripRepository.findAllRows();
  }

  public List<TripRepository.TripRow> search(
      String departure, String destination, LocalDate date, Integer fromHour, Integer toHour) {
    LocalDate searchDate = date == null ? LocalDate.now(TripPlanningTime.ZONE) : date;
    if (fromHour != null && toHour != null && fromHour > toHour) {
      throw ApiException.badRequest("Khung giờ không hợp lệ.");
    }
    LocalDateTime start = searchDate.atStartOfDay();
    LocalDateTime end = searchDate.plusDays(1).atStartOfDay();
    LocalDateTime now = LocalDateTime.now(TripPlanningTime.ZONE);
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
        seatStatusService.snapshot(tripId).heldSeats(),
        route.getDeparture(),
        route.getDestination(),
        trip.getDepartureTime());
  }

  @Transactional
  public Trip create(TripRequest request) {
    Vehicle vehicle =
        vehicleRepository
            .lockById(request.vehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    Route route =
        routeRepository
            .findById(request.routeId())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    Driver driver =
        driverRepository
            .lockById(request.driverId())
            .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
    LocalDateTime departure = requireDeparture(request.departureTime());
    validateResources(vehicle, driver, route);
    checkConflict(
        vehicle.getVehicleId(),
        driver.getDriverId(),
        departure,
        departure.plus(durationOf(route)),
        null);

    TicketPrice ticketPrice =
        ticketPriceRepository
            .findByCompanyIdAndTypeIdAndRouteId(
                vehicle.getCompanyId(), vehicle.getTypeId(), route.getRouteId())
            .orElseThrow(
                () ->
                    ApiException.conflict(
                        "Chưa có bảng giá cho nhà xe, loại xe và tuyến đường này."));

    if (ticketPrice.getPrice() == null || ticketPrice.getPrice().signum() <= 0) {
      throw ApiException.conflict("Chưa có giá vé hợp lệ.");
    }
    Trip trip = new Trip();
    apply(trip, vehicle, route, driver, departure, ticketPrice.getPrice());
    Trip saved = tripRepository.saveAndFlush(trip);
    return saved;
  }

  @Transactional
  public Trip update(Integer tripId, TripRequest request) {
    Trip trip =
        tripRepository
            .findByIdForUpdate(tripId)
            .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    Vehicle vehicle =
        vehicleRepository
            .lockById(request.vehicleId())
            .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    Route route =
        routeRepository
            .findById(request.routeId())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    Driver driver =
        driverRepository
            .lockById(request.driverId())
            .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
    LocalDateTime departure = requireDeparture(request.departureTime());
    boolean sold = ticketRepository.existsByTripId(tripId);
    if (sold) {
      boolean changedCoreInfo =
          !trip.getVehicleId().equals(vehicle.getVehicleId())
              || !trip.getRouteId().equals(route.getRouteId())
              || !trip.getDepartureTime().equals(departure);
      if (changedCoreInfo) {
        throw ApiException.conflict(
            "Chuyến đã bán vé, chỉ có thể đổi tài xế. Không thể đổi xe, tuyến hoặc giờ khởi hành.");
      }
    }
    validateResources(vehicle, driver, route);
    checkConflict(
        vehicle.getVehicleId(),
        driver.getDriverId(),
        departure,
        sold && trip.getEstimatedArrivalTime() != null
            ? trip.getEstimatedArrivalTime()
            : departure.plus(durationOf(route)),
        tripId);
    if (sold) {
      trip.setDriverId(driver.getDriverId());
      return tripRepository.save(trip);
    }

    TicketPrice ticketPrice =
        ticketPriceRepository
            .findByCompanyIdAndTypeIdAndRouteId(
                vehicle.getCompanyId(), vehicle.getTypeId(), route.getRouteId())
            .orElseThrow(
                () ->
                    ApiException.conflict(
                        "Chưa có bảng giá cho nhà xe, loại xe và tuyến đường này."));

    if (ticketPrice.getPrice() == null || ticketPrice.getPrice().signum() <= 0) {
      throw ApiException.conflict("Chưa có giá vé hợp lệ.");
    }
    apply(trip, vehicle, route, driver, departure, ticketPrice.getPrice());
    Trip saved = tripRepository.save(trip);
    return saved;
  }

  @Transactional
  public Map<String, String> delete(Integer tripId) {
    Trip trip =
        tripRepository
            .findByIdForUpdate(tripId)
            .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    if (ticketRepository.existsByTripId(tripId)) {
      throw ApiException.conflict("Không thể xóa chuyến xe đã có vé.");
    }
    for (var occurrence : occurrences.findByTripId(tripId)) {
      occurrence.setTripId(null);
      occurrence.setStatus("DELETED");
      occurrences.save(occurrence);
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
    if (!departure.isAfter(LocalDateTime.now(TripPlanningTime.ZONE))) {
      throw ApiException.badRequest("Ngày giờ khởi hành phải sau thời điểm hiện tại.");
    }
    return departure;
  }

  public void checkConflict(
      Integer vehicleId,
      Integer driverId,
      LocalDateTime departure,
      LocalDateTime arrival,
      Integer excludeId) {
    if (!tripRepository
        .findOverlapping(vehicleId, driverId, departure, arrival, excludeId)
        .isEmpty()) {
      throw ApiException.conflict("Xe hoặc tài xế đã có chuyến trong khoảng thời gian này.");
    }
  }

  private void validateResources(Vehicle vehicle, Driver driver, Route route) {
    if (!companies
        .findById(vehicle.getCompanyId())
        .map(c -> Boolean.TRUE.equals(c.getIsActive()))
        .orElse(false)) {
      throw ApiException.badRequest("Nhà xe phải đang hoạt động.");
    }
    boolean typeActive =
        vehicleTypeRepository
            .findById(vehicle.getTypeId())
            .map(t -> Boolean.TRUE.equals(t.getIsActive()))
            .orElse(false);
    if (!Boolean.TRUE.equals(vehicle.getIsActive())
        || !Boolean.TRUE.equals(driver.getIsActive())
        || !Boolean.TRUE.equals(route.getIsActive())
        || !typeActive) {
      throw ApiException.badRequest("Xe, loại xe, tài xế và tuyến phải đang hoạt động.");
    }
  }

  private Duration durationOf(Route route) {
    if (route.getEstimatedDuration() != null && route.getEstimatedDuration().toSecondOfDay() == 0) {
      throw ApiException.badRequest("Thời gian chạy phải lớn hơn 0.");
    }
    return route.getEstimatedDuration() == null
        ? Duration.ofHours(6)
        : Duration.ofSeconds(route.getEstimatedDuration().toSecondOfDay());
  }

  private String blankToNull(String value) {
    return Validators.isBlank(value) ? null : value.trim();
  }
}
