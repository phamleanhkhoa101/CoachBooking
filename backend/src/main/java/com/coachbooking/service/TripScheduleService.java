package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.TripSchedule;
import com.coachbooking.dto.trip.ScheduleRequest;
import com.coachbooking.repo.*;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import java.time.*;
import java.time.temporal.ChronoUnit;
import java.util.*;
import java.util.stream.Collectors;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

@Service
@RequiredArgsConstructor
public class TripScheduleService {
  private final TripScheduleRepository schedules;
  private final VehicleRepository vehicles;
  private final DriverRepository drivers;
  private final RouteRepository routes;
  private final ScheduleOccurrenceService worker;
  private final ObjectMapper mapper;
  private final PlatformTransactionManager transactionManager;

  public List<TripSchedule> list() {
    return schedules.findAll();
  }

  public TripSchedule save(Integer id, ScheduleRequest r) {
    if (r.validTo().isBefore(r.validFrom()))
      throw ApiException.badRequest("Khoảng hiệu lực không hợp lệ.");
    if (r.excludedDates().stream()
        .anyMatch(d -> d.isBefore(r.validFrom()) || d.isAfter(r.validTo()))) {
      throw ApiException.badRequest("Ngày nghỉ phải nằm trong khoảng hiệu lực.");
    }
    var vehicle =
        vehicles
            .findById(r.vehicleId())
            .orElseThrow(() -> ApiException.badRequest("Xe không tồn tại."));
    if (!drivers.existsById(r.driverId()) || !routes.existsById(r.routeId()))
      throw ApiException.badRequest("Tài xế hoặc tuyến không tồn tại.");
    return new TransactionTemplate(transactionManager)
        .execute(
            tx -> {
              TripSchedule s =
                  id == null
                      ? new TripSchedule()
                      : schedules
                          .lockById(id)
                          .orElseThrow(() -> ApiException.notFound("Lịch chạy không tồn tại."));
              s.setName(r.name().trim());
              s.setVehicleId(r.vehicleId());
              s.setRouteId(r.routeId());
              s.setDriverId(r.driverId());
              s.setDepartureTime(r.departureTime());
              s.setWeekdays(
                  r.weekdays().stream()
                      .sorted()
                      .map(String::valueOf)
                      .collect(Collectors.joining(",")));
              s.setValidFrom(r.validFrom());
              s.setValidTo(r.validTo());
              s.setIsActive(r.isActive());
              s.setExcludedDates(
                  r.excludedDates().stream()
                      .sorted()
                      .map(LocalDate::toString)
                      .collect(Collectors.joining(",")));
              return schedules.save(s);
            });
  }

  public void disable(Integer id) {
    new TransactionTemplate(transactionManager)
        .executeWithoutResult(
            tx -> {
              var s =
                  schedules
                      .lockById(id)
                      .orElseThrow(() -> ApiException.notFound("Lịch chạy không tồn tại."));
              s.setIsActive(false);
              schedules.save(s);
            });
  }

  public List<ScheduleOccurrenceService.Result> run(
      Integer id, LocalDate from, LocalDate to, boolean preview) {
    if (from == null
        || to == null
        || to.isBefore(from)
        || ChronoUnit.DAYS.between(from, to) >= 30) {
      throw ApiException.badRequest("Chọn khoảng từ 1 đến 30 ngày.");
    }
    if (!schedules.existsById(id)) throw ApiException.notFound("Lịch chạy không tồn tại.");
    List<ScheduleOccurrenceService.Result> results = new ArrayList<>();
    for (LocalDate date = from; !date.isAfter(to); date = date.plusDays(1)) {
      try {
        results.add(preview ? worker.preview(id, date) : worker.generate(id, date));
      } catch (ApiException ex) {
        results.add(new ScheduleOccurrenceService.Result(date, "ERROR", null, ex.getMessage()));
      } catch (RuntimeException ex) {
        results.add(
            new ScheduleOccurrenceService.Result(
                date,
                "ERROR",
                null,
                "Không thể tạo chuyến; dữ liệu đã rollback. Kiểm tra cấu hình và thử lại."));
      }
    }
    if (!preview) {
      String result;
      try {
        result = mapper.writeValueAsString(results);
      } catch (JsonProcessingException ex) {
        throw new IllegalStateException(ex);
      }
      new TransactionTemplate(transactionManager)
          .executeWithoutResult(
              tx -> {
                var s = schedules.lockById(id).orElseThrow();
                s.setLastResult(result);
                s.setLastRunAt(LocalDateTime.now(TripPlanningTime.ZONE));
                schedules.save(s);
              });
    }
    return results;
  }
}
