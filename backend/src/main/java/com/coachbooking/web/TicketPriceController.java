package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.TicketPrice;
import com.coachbooking.repo.BusCompanyRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.Valid;
import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotNull;
import java.math.BigDecimal;
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
@RequestMapping({"/api/ticket-prices", "/api/gia-ve"})
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class TicketPriceController {

  private final TicketPriceRepository ticketPriceRepository;
  private final BusCompanyRepository busCompanyRepository;
  private final VehicleTypeRepository vehicleTypeRepository;
  private final RouteRepository routeRepository;

  public TicketPriceController(
      TicketPriceRepository ticketPriceRepository,
      BusCompanyRepository busCompanyRepository,
      VehicleTypeRepository vehicleTypeRepository,
      RouteRepository routeRepository) {
    this.ticketPriceRepository = ticketPriceRepository;
    this.busCompanyRepository = busCompanyRepository;
    this.vehicleTypeRepository = vehicleTypeRepository;
    this.routeRepository = routeRepository;
  }

  @GetMapping
  public List<TicketPrice> list() {
    return ticketPriceRepository.findAll();
  }

  @GetMapping("/{id}")
  public TicketPrice get(@PathVariable Integer id) {
    return ticketPriceRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Giá vé không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public TicketPrice create(@Valid @RequestBody Request request) {
    validateReferences(request);
    if (ticketPriceRepository.existsByCompanyIdAndTypeIdAndRouteId(
        request.companyId(), request.typeId(), request.routeId())) {
      throw ApiException.conflict("Giá vé cho nhà xe, loại xe và tuyến đường này đã tồn tại.");
    }
    TicketPrice ticketPrice = new TicketPrice();
    apply(ticketPrice, request);
    return ticketPriceRepository.save(ticketPrice);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public TicketPrice update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    TicketPrice ticketPrice = get(id);
    validateReferences(request);
    ticketPriceRepository
        .findByCompanyIdAndTypeIdAndRouteId(
            request.companyId(), request.typeId(), request.routeId())
        .filter(other -> !other.getPriceId().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict(
                  "Giá vé cho nhà xe, loại xe và tuyến đường này đã tồn tại.");
            });
    apply(ticketPrice, request);
    return ticketPriceRepository.save(ticketPrice);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    TicketPrice ticketPrice = get(id);
    ticketPriceRepository.delete(ticketPrice);
    return Map.of("message", "Xóa giá vé thành công.");
  }

  private void validateReferences(Request request) {
    if (!busCompanyRepository.existsById(request.companyId())) {
      throw ApiException.notFound("Nhà xe không tồn tại.");
    }
    if (!vehicleTypeRepository.existsById(request.typeId())) {
      throw ApiException.notFound("Loại xe không tồn tại.");
    }
    if (!routeRepository.existsById(request.routeId())) {
      throw ApiException.notFound("Tuyến đường không tồn tại.");
    }
  }

  private void apply(TicketPrice ticketPrice, Request request) {
    ticketPrice.setCompanyId(request.companyId());
    ticketPrice.setTypeId(request.typeId());
    ticketPrice.setRouteId(request.routeId());
    ticketPrice.setPrice(request.price());
  }

  public record Request(
      @NotNull(message = "Vui lòng chọn nhà xe.") @JsonAlias({"maNx", "companyId"})
          Integer companyId,
      @NotNull(message = "Vui lòng chọn loại xe.") @JsonAlias({"maLx", "typeId"}) Integer typeId,
      @NotNull(message = "Vui lòng chọn tuyến đường.") @JsonAlias({"maTuyen", "routeId"})
          Integer routeId,
      @NotNull(message = "Vui lòng nhập giá vé.")
          @DecimalMin(value = "1", message = "Giá vé phải lớn hơn 0.")
          @JsonAlias({"giaVe", "price"})
          BigDecimal price) {}
}
