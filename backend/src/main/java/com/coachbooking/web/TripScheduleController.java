package com.coachbooking.web;

import com.coachbooking.domain.TripSchedule;
import com.coachbooking.dto.trip.ScheduleRequest;
import com.coachbooking.service.*;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import lombok.RequiredArgsConstructor;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/trip-schedules")
@PreAuthorize("hasRole('Admin')")
@RequiredArgsConstructor
public class TripScheduleController {
  private final TripScheduleService service;

  public record Range(@NotNull LocalDate from, @NotNull LocalDate to) {}

  @GetMapping
  public List<TripSchedule> list() {
    return service.list();
  }

  @PostMapping
  public TripSchedule create(@Valid @RequestBody ScheduleRequest request) {
    return service.save(null, request);
  }

  @PutMapping("/{id}")
  public TripSchedule update(
      @PathVariable Integer id, @Valid @RequestBody ScheduleRequest request) {
    return service.save(id, request);
  }

  @DeleteMapping("/{id}")
  public Map<String, String> disable(@PathVariable Integer id) {
    service.disable(id);
    return Map.of("message", "Đã tắt lịch; các chuyến đã tạo được giữ nguyên.");
  }

  @PostMapping("/{id}/preview")
  public List<ScheduleOccurrenceService.Result> preview(
      @PathVariable Integer id, @Valid @RequestBody Range range) {
    return service.run(id, range.from(), range.to(), true);
  }

  @PostMapping("/{id}/generate")
  public List<ScheduleOccurrenceService.Result> generate(
      @PathVariable Integer id, @Valid @RequestBody Range range) {
    return service.run(id, range.from(), range.to(), false);
  }
}
