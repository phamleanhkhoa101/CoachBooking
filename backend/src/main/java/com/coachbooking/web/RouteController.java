package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Route;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.TripRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Positive;
import java.time.LocalTime;
import java.util.List;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/routes", "/api/tuyen-duong"})
public class RouteController {

  private final RouteRepository routeRepository;
  private final TripRepository tripRepository;
  private final TicketPriceRepository ticketPriceRepository;

  public RouteController(
      RouteRepository routeRepository,
      TripRepository tripRepository,
      TicketPriceRepository ticketPriceRepository) {
    this.routeRepository = routeRepository;
    this.tripRepository = tripRepository;
    this.ticketPriceRepository = ticketPriceRepository;
  }

  @GetMapping
  public List<Route> list() {
    return routeRepository.findAll();
  }

  @GetMapping("/{id}")
  public Route get(@PathVariable Integer id) {
    return routeRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public Route create(@Valid @RequestBody Request request) {
    if (routeRepository.existsByDepartureAndDestination(
        request.departure().trim(), request.destination().trim())) {
      throw ApiException.conflict("Tuyến đường này đã tồn tại.");
    }
    Route route = new Route();
    apply(route, request);
    return routeRepository.save(route);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Route update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    Route route = get(id);
    routeRepository
        .findByDepartureAndDestination(request.departure().trim(), request.destination().trim())
        .filter(other -> !other.getRouteId().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict("Tuyến đường này đã tồn tại.");
            });
    apply(route, request);
    return routeRepository.save(route);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    Route route = get(id);
    if (tripRepository.existsByRouteId(id)) {
      throw ApiException.conflict("Không thể xóa tuyến đường đã có chuyến xe.");
    }
    if (ticketPriceRepository.existsByRouteId(id)) {
      throw ApiException.conflict("Không thể xóa tuyến đường đang có bảng giá.");
    }
    routeRepository.delete(route);
    return Map.of("message", "Xóa tuyến đường thành công.");
  }

  private void apply(Route route, Request request) {
    route.setDeparture(request.departure().trim());
    route.setDestination(request.destination().trim());
    route.setDistance(request.distance());
    route.setEstimatedDuration(request.estimatedDuration());
    if (request.isActive() != null) {
      route.setIsActive(request.isActive());
    }
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập điểm đi.") @JsonAlias({"diemDi", "departure"})
          String departure,
      @NotBlank(message = "Vui lòng nhập điểm đến.") @JsonAlias({"diemDen", "destination"})
          String destination,
      @Positive(message = "Quãng đường phải lớn hơn 0.") @JsonAlias({"quangDuong", "distance"})
          Integer distance,
      @JsonAlias({"thoiGianDuKien", "estimatedDuration"}) LocalTime estimatedDuration,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}
}
