package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.dto.trip.TripRequest;
import com.coachbooking.repo.*;
import java.time.*;
import java.util.*;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
public class ScheduleOccurrenceService {
  private final TripScheduleRepository schedules;
  private final ScheduleOccurrenceRepository occurrences;
  private final TripService trips;
  private final VehicleRepository vehicles;
  private final DriverRepository drivers;
  private final RouteRepository routes;
  private final VehicleTypeRepository types;
  private final BusCompanyRepository companies;
  private final TicketPriceRepository prices;

  public record Result(LocalDate serviceDate, String status, Integer tripId, String message) {}

  @Transactional(readOnly = true)
  public Result preview(Integer scheduleId, LocalDate date) {
    TripSchedule schedule =
        schedules
            .findById(scheduleId)
            .orElseThrow(() -> ApiException.notFound("Lịch chạy không tồn tại."));
    Result skip = skip(schedule, date);
    if (skip != null) return skip;
    try {
      validate(schedule, date);
      return new Result(date, "READY", null, "Có thể tạo chuyến; sẽ kiểm tra lại khi thực hiện.");
    } catch (ApiException ex) {
      return new Result(date, "ERROR", null, ex.getMessage());
    }
  }

  @Transactional
  public Result generate(Integer scheduleId, LocalDate date) {
    TripSchedule schedule =
        schedules
            .lockById(scheduleId)
            .orElseThrow(() -> ApiException.notFound("Lịch chạy không tồn tại."));
    Result skip = skip(schedule, date);
    if (skip != null) {
      if ("EXCLUDED".equals(skip.status())
          && occurrences.findByScheduleIdAndServiceDate(scheduleId, date).isEmpty()) {
        saveOccurrence(scheduleId, date, null, "EXCLUDED");
      }
      return skip;
    }
    // TripService locks vehicle, then driver. Its transaction joins this occurrence transaction.
    Trip trip =
        trips.create(
            new TripRequest(
                schedule.getVehicleId(),
                schedule.getRouteId(),
                schedule.getDriverId(),
                date.atTime(schedule.getDepartureTime())));
    saveOccurrence(scheduleId, date, trip.getTripId(), "CREATED");
    return new Result(date, "CREATED", trip.getTripId(), "Đã tạo chuyến.");
  }

  private void saveOccurrence(Integer scheduleId, LocalDate date, Integer tripId, String status) {
    ScheduleOccurrence occurrence = new ScheduleOccurrence();
    occurrence.setScheduleId(scheduleId);
    occurrence.setServiceDate(date);
    occurrence.setTripId(tripId);
    occurrence.setStatus(status);
    occurrences.saveAndFlush(occurrence);
  }

  private Result skip(TripSchedule s, LocalDate date) {
    var existing = occurrences.findByScheduleIdAndServiceDate(s.getScheduleId(), date);
    if (existing.isPresent()) {
      var o = existing.get();
      return new Result(
          date,
          o.getStatus().equals("CREATED") ? "EXISTS" : o.getStatus(),
          o.getTripId(),
          "Ngày này đã được xử lý, không tự tạo lại.");
    }
    if (!Boolean.TRUE.equals(s.getIsActive())
        || date.isBefore(s.getValidFrom())
        || date.isAfter(s.getValidTo())
        || !Arrays.asList(s.getWeekdays().split(","))
            .contains(String.valueOf(date.getDayOfWeek().getValue()))) {
      return new Result(date, "SKIPPED", null, "Không thuộc lịch hoạt động.");
    }
    if (Arrays.asList(s.getExcludedDates().split(",")).contains(date.toString())) {
      return new Result(date, "EXCLUDED", null, "Ngày nghỉ.");
    }
    if (!date.atTime(s.getDepartureTime()).isAfter(LocalDateTime.now(TripPlanningTime.ZONE))) {
      return new Result(date, "PAST", null, "Giờ khởi hành đã qua.");
    }
    return null;
  }

  public void validate(TripSchedule s, LocalDate date) {
    Vehicle vehicle =
        vehicles
            .findById(s.getVehicleId())
            .orElseThrow(() -> ApiException.badRequest("Xe không tồn tại."));
    Driver driver =
        drivers
            .findById(s.getDriverId())
            .orElseThrow(() -> ApiException.badRequest("Tài xế không tồn tại."));
    Route route =
        routes
            .findById(s.getRouteId())
            .orElseThrow(() -> ApiException.badRequest("Tuyến không tồn tại."));
    if (!Boolean.TRUE.equals(vehicle.getIsActive())
        || !Boolean.TRUE.equals(driver.getIsActive())
        || !Boolean.TRUE.equals(route.getIsActive())
        || !types
            .findById(vehicle.getTypeId())
            .map(t -> Boolean.TRUE.equals(t.getIsActive()))
            .orElse(false)
        || !companies
            .findById(vehicle.getCompanyId())
            .map(c -> Boolean.TRUE.equals(c.getIsActive()))
            .orElse(false)) {
      throw ApiException.badRequest("Nhà xe, xe, loại xe, tuyến và tài xế phải hoạt động.");
    }
    long minutes =
        route.getEstimatedDuration() == null
            ? 360
            : route.getEstimatedDuration().toSecondOfDay() / 60;
    if (minutes <= 0) throw ApiException.badRequest("Thời gian chạy phải lớn hơn 0.");
    var price =
        prices.findByCompanyIdAndTypeIdAndRouteId(
            vehicle.getCompanyId(), vehicle.getTypeId(), s.getRouteId());
    if (price.isEmpty() || price.get().getPrice().signum() <= 0)
      throw ApiException.conflict("Chưa có giá vé hợp lệ.");
    LocalDateTime departure = date.atTime(s.getDepartureTime());
    trips.checkConflict(
        s.getVehicleId(), s.getDriverId(), departure, departure.plusMinutes(minutes), null);
  }
}
