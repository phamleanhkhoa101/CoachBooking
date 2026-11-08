package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Driver;
import com.coachbooking.repo.DriverRepository;
import com.coachbooking.repo.TripRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDate;
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
@RequestMapping({"/api/drivers", "/api/tai-xe"})
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class DriverController {

  private final DriverRepository driverRepository;
  private final TripRepository tripRepository;

  public DriverController(DriverRepository driverRepository, TripRepository tripRepository) {
    this.driverRepository = driverRepository;
    this.tripRepository = tripRepository;
  }

  @GetMapping
  public List<Driver> list() {
    return driverRepository.findAll();
  }

  @GetMapping("/{id}")
  public Driver get(@PathVariable Integer id) {
    return driverRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public Driver create(@Valid @RequestBody Request request) {
    Driver driver = new Driver();
    apply(driver, request);
    return driverRepository.save(driver);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Driver update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    Driver driver = get(id);
    apply(driver, request);
    return driverRepository.save(driver);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    Driver driver = get(id);
    if (tripRepository.existsByDriverId(id)) {
      throw ApiException.conflict("Không thể xóa tài xế đã có chuyến xe.");
    }
    driverRepository.delete(driver);
    return Map.of("message", "Xóa tài xế thành công.");
  }

  private void apply(Driver driver, Request request) {
    if (request.dateOfBirth().isAfter(LocalDate.now().minusYears(18))) {
      throw ApiException.badRequest("Tài xế phải đủ 18 tuổi.");
    }
    driver.setFullName(request.fullName() == null ? null : request.fullName().trim());
    driver.setDateOfBirth(request.dateOfBirth());
    driver.setCitizenId(request.citizenId().trim());
    if (request.gender() != null) {
      driver.setGender(request.gender());
    }
    driver.setPhone(request.phone().trim());
    driver.setAddress(request.address().trim());
    driver.setLicenseNumber(request.licenseNumber().trim());
    driver.setIssueDate(request.issueDate());
    if (request.isActive() != null) {
      driver.setIsActive(request.isActive());
    }
  }

  public record Request(
      @JsonAlias({"tenTx", "fullName"}) String fullName,
      @NotNull(message = "Vui lòng nhập ngày sinh.") @JsonAlias({"namSinh", "dateOfBirth"})
          LocalDate dateOfBirth,
      @NotBlank(message = "Vui lòng nhập CCCD.") @JsonAlias({"cccd", "citizenId"}) String citizenId,
      @JsonAlias({"gioiTinh", "gender"}) Boolean gender,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") @JsonAlias({"sdt", "phone"}) String phone,
      @NotBlank(message = "Vui lòng nhập địa chỉ.") @JsonAlias({"diaChi", "address"})
          String address,
      @NotBlank(message = "Vui lòng nhập bằng lái.") @JsonAlias({"bangLai", "licenseNumber"})
          String licenseNumber,
      @NotNull(message = "Vui lòng nhập ngày cấp.") @JsonAlias({"ngayCap", "issueDate"})
          LocalDate issueDate,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}
}
