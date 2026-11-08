package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.BusCompany;
import com.coachbooking.repo.BusCompanyRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.VehicleRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
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
@RequestMapping({"/api/bus-companies", "/api/nha-xe"})
public class BusCompanyController {

  private final BusCompanyRepository busCompanyRepository;
  private final VehicleRepository vehicleRepository;
  private final TicketPriceRepository ticketPriceRepository;

  public BusCompanyController(
      BusCompanyRepository busCompanyRepository,
      VehicleRepository vehicleRepository,
      TicketPriceRepository ticketPriceRepository) {
    this.busCompanyRepository = busCompanyRepository;
    this.vehicleRepository = vehicleRepository;
    this.ticketPriceRepository = ticketPriceRepository;
  }

  @GetMapping
  public List<BusCompany> list() {
    return busCompanyRepository.findAll();
  }

  @GetMapping("/{id}")
  public BusCompany get(@PathVariable Integer id) {
    return busCompanyRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Nhà xe không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public BusCompany create(@Valid @RequestBody Request request) {
    BusCompany busCompany = new BusCompany();
    apply(busCompany, request);
    return busCompanyRepository.save(busCompany);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public BusCompany update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    BusCompany busCompany = get(id);
    apply(busCompany, request);
    return busCompanyRepository.save(busCompany);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    BusCompany busCompany = get(id);
    if (vehicleRepository.existsByCompanyId(id)) {
      throw ApiException.conflict("Không thể xóa nhà xe đang có xe.");
    }
    if (ticketPriceRepository.existsByCompanyId(id)) {
      throw ApiException.conflict("Không thể xóa nhà xe đang có bảng giá.");
    }
    busCompanyRepository.delete(busCompany);
    return Map.of("message", "Xóa nhà xe thành công.");
  }

  private void apply(BusCompany busCompany, Request request) {
    busCompany.setCompanyName(request.companyName().trim());
    busCompany.setPhone(request.phone().trim());
    busCompany.setEmail(request.email().trim());
    if (request.isActive() != null) {
      busCompany.setIsActive(request.isActive());
    }
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập tên nhà xe.") @JsonAlias({"tenNx", "companyName"})
          String companyName,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") @JsonAlias({"sdt", "phone"}) String phone,
      @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.")
          String email,
      @JsonAlias({"trangThai", "isActive"}) Boolean isActive) {}
}
