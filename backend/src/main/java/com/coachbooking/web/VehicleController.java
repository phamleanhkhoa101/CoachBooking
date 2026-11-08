package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.repo.BusCompanyRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
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
@RequestMapping({"/api/vehicles", "/api/xe"})
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class VehicleController {

  private final VehicleRepository vehicleRepository;
  private final BusCompanyRepository busCompanyRepository;
  private final VehicleTypeRepository vehicleTypeRepository;
  private final TripRepository tripRepository;

  public VehicleController(
      VehicleRepository vehicleRepository,
      BusCompanyRepository busCompanyRepository,
      VehicleTypeRepository vehicleTypeRepository,
      TripRepository tripRepository) {
    this.vehicleRepository = vehicleRepository;
    this.busCompanyRepository = busCompanyRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.tripRepository = tripRepository;
  }

  @GetMapping
  public List<Vehicle> list() {
    return vehicleRepository.findAll();
  }

  @GetMapping("/{id}")
  public Vehicle get(@PathVariable Integer id) {
    return vehicleRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public Vehicle create(@Valid @RequestBody Request request) {
    Vehicle vehicle = new Vehicle();
    apply(vehicle, request);
    return vehicleRepository.save(vehicle);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Vehicle update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    Vehicle vehicle = get(id);
    apply(vehicle, request);
    return vehicleRepository.save(vehicle);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    Vehicle vehicle = get(id);
    if (tripRepository.existsByVehicleId(id)) {
      throw ApiException.conflict("Không thể xóa xe đã có chuyến.");
    }
    vehicleRepository.delete(vehicle);
    return Map.of("message", "Xóa xe thành công.");
  }

  private void apply(Vehicle vehicle, Request request) {
    if (!busCompanyRepository.existsById(request.companyId())) {
      throw ApiException.notFound("Nhà xe không tồn tại.");
    }
    if (!vehicleTypeRepository.existsById(request.typeId())) {
      throw ApiException.notFound("Loại xe không tồn tại.");
    }
    vehicle.setCompanyId(request.companyId());
    vehicle.setTypeId(request.typeId());
    vehicle.setLicensePlate(request.licensePlate().trim());
    if (request.isActive() != null) {
      vehicle.setIsActive(request.isActive());
    }
  }

  public record Request(
      @NotNull(message = "Vui lòng chọn nhà xe.") @JsonAlias({"maNx", "companyId"})
          Integer companyId,
      @NotNull(message = "Vui lòng chọn loại xe.") @JsonAlias({"maLx", "typeId"}) Integer typeId,
      @NotBlank(message = "Vui lòng nhập biển số xe.") @JsonAlias({"bienSoXe", "licensePlate"})
          String licensePlate,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}
}
