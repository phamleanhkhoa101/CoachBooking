package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Min;
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
@RequestMapping({"/api/vehicle-types", "/api/loai-xe"})
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class VehicleTypeController {

  private final VehicleTypeRepository vehicleTypeRepository;
  private final VehicleRepository vehicleRepository;
  private final TicketPriceRepository ticketPriceRepository;

  public VehicleTypeController(
      VehicleTypeRepository vehicleTypeRepository,
      VehicleRepository vehicleRepository,
      TicketPriceRepository ticketPriceRepository) {
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.vehicleRepository = vehicleRepository;
    this.ticketPriceRepository = ticketPriceRepository;
  }

  @GetMapping
  public List<VehicleType> list() {
    return vehicleTypeRepository.findAll();
  }

  @GetMapping("/{id}")
  public VehicleType get(@PathVariable Integer id) {
    return vehicleTypeRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public VehicleType create(@Valid @RequestBody Request request) {
    VehicleType vehicleType = new VehicleType();
    apply(vehicleType, request);
    return vehicleTypeRepository.save(vehicleType);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public VehicleType update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    VehicleType vehicleType = get(id);
    apply(vehicleType, request);
    return vehicleTypeRepository.save(vehicleType);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    VehicleType vehicleType = get(id);
    if (vehicleRepository.existsByTypeId(id)) {
      throw ApiException.conflict("Không thể xóa loại xe đang được sử dụng.");
    }
    if (ticketPriceRepository.existsByTypeId(id)) {
      throw ApiException.conflict("Không thể xóa loại xe đang có bảng giá.");
    }
    vehicleTypeRepository.delete(vehicleType);
    return Map.of("message", "Xóa loại xe thành công.");
  }

  private void apply(VehicleType vehicleType, Request request) {
    vehicleType.setTypeName(request.typeName().trim());
    vehicleType.setSeatCapacity(request.seatCapacity());
    if (request.isActive() != null) {
      vehicleType.setIsActive(request.isActive());
    }
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập tên loại xe.") @JsonAlias({"tenLx", "typeName"})
          String typeName,
      @NotNull(message = "Vui lòng nhập số chỗ.")
          @Min(value = 1, message = "Số chỗ phải lớn hơn 0.")
          @JsonAlias({"soCho", "seatCapacity"})
          Integer seatCapacity,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}
}
