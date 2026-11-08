package com.coachbooking.web;

import com.coachbooking.domain.Trip;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.service.TripService;
import jakarta.validation.Valid;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/trips", "/api/chuyen-xe"})
public class TripController {

  private final TripRepository tripRepository;
  private final TripService tripService;

  public TripController(TripRepository tripRepository, TripService tripService) {
    this.tripRepository = tripRepository;
    this.tripService = tripService;
  }

  @GetMapping
  @PreAuthorize("hasAnyRole('Admin','NhanVien')")
  public List<TripRepository.TripRow> list() {
    return tripService.listAll();
  }

  @GetMapping({"/search", "/tim"})
  public List<TripRepository.TripRow> search(
      @RequestParam(value = "departure", required = false) String departure,
      @RequestParam(value = "diemDi", required = false) String diemDi,
      @RequestParam(value = "destination", required = false) String destination,
      @RequestParam(value = "diemDen", required = false) String diemDen,
      @RequestParam(value = "date", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate date,
      @RequestParam(value = "ngay", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate ngay,
      @RequestParam(value = "fromHour", required = false) Integer fromHour,
      @RequestParam(value = "tuGio", required = false) Integer tuGio,
      @RequestParam(value = "toHour", required = false) Integer toHour,
      @RequestParam(value = "denGio", required = false) Integer denGio) {
    String actualDep = departure != null ? departure : diemDi;
    String actualDest = destination != null ? destination : diemDen;
    LocalDate actualDate = date != null ? date : ngay;
    Integer actualFromH = fromHour != null ? fromHour : tuGio;
    Integer actualToH = toHour != null ? toHour : denGio;
    return tripService.search(actualDep, actualDest, actualDate, actualFromH, actualToH);
  }

  @GetMapping({"/{id}/seats", "/{id}/ghe"})
  @PreAuthorize("isAuthenticated()")
  public TripService.SeatMapResponse seatMap(@PathVariable Integer id) {
    return tripService.seatMap(id);
  }

  @GetMapping("/{id}")
  @PreAuthorize("hasAnyRole('Admin','NhanVien')")
  public Trip get(@PathVariable Integer id) {
    return tripRepository
        .findById(id)
        .orElseThrow(
            () -> com.coachbooking.common.ApiException.notFound("Chuyến xe không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public Trip create(@Valid @RequestBody TripService.TripRequest request) {
    return tripService.create(request);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Trip update(
      @PathVariable Integer id, @Valid @RequestBody TripService.TripRequest request) {
    return tripService.update(id, request);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    return tripService.delete(id);
  }
}
