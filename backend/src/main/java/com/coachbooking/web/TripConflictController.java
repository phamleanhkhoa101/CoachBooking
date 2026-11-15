package com.coachbooking.web;

import com.coachbooking.repo.TripRepository;
import java.util.List;
import lombok.RequiredArgsConstructor;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
public class TripConflictController {
  private final TripRepository trips;

  public record Conflict(Integer firstTripId, Integer secondTripId, String reason) {}

  @GetMapping("/api/trip-conflicts")
  @PreAuthorize("hasAnyRole('Admin','NhanVien')")
  public List<Conflict> list() {
    var all = trips.findAllByOrderByDepartureTimeAsc();
    java.util.ArrayList<Conflict> result = new java.util.ArrayList<>();
    for (int i = 0; i < all.size(); i++) {
      var a = all.get(i);
      if (a.getEstimatedArrivalTime() == null
          || !a.getEstimatedArrivalTime().isAfter(a.getDepartureTime())) {
        result.add(new Conflict(a.getTripId(), null, "Thiếu hoặc sai giờ đến dự kiến."));
        continue;
      }
      for (int j = i + 1; j < all.size(); j++) {
        var b = all.get(j);
        if (!b.getDepartureTime().isBefore(a.getEstimatedArrivalTime())) break;
        if (a.getVehicleId().equals(b.getVehicleId()) || a.getDriverId().equals(b.getDriverId())) {
          result.add(new Conflict(a.getTripId(), b.getTripId(), "Chồng thời gian xe hoặc tài xế."));
        }
      }
    }
    return result;
  }
}
