package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.TuyenDuong;
import com.coachbooking.repo.ChuyenXeRepository;
import com.coachbooking.repo.GiaVeRepository;
import com.coachbooking.repo.TuyenDuongRepository;
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
@RequestMapping("/api/tuyen-duong")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class TuyenDuongController {

  private final TuyenDuongRepository tuyenDuongRepository;
  private final ChuyenXeRepository chuyenXeRepository;
  private final GiaVeRepository giaVeRepository;

  public TuyenDuongController(
      TuyenDuongRepository tuyenDuongRepository,
      ChuyenXeRepository chuyenXeRepository,
      GiaVeRepository giaVeRepository) {
    this.tuyenDuongRepository = tuyenDuongRepository;
    this.chuyenXeRepository = chuyenXeRepository;
    this.giaVeRepository = giaVeRepository;
  }

  @GetMapping
  @PreAuthorize("isAuthenticated()")
  public List<TuyenDuong> list() {
    return tuyenDuongRepository.findAll();
  }

  @GetMapping("/{id}")
  @PreAuthorize("isAuthenticated()")
  public TuyenDuong get(@PathVariable Integer id) {
    return tuyenDuongRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public TuyenDuong create(@Valid @RequestBody Request request) {
    if (tuyenDuongRepository.existsByDiemDiAndDiemDen(
        request.diemDi().trim(), request.diemDen().trim())) {
      throw ApiException.conflict("Tuyến đường này đã tồn tại.");
    }
    TuyenDuong tuyenDuong = new TuyenDuong();
    apply(tuyenDuong, request);
    return tuyenDuongRepository.save(tuyenDuong);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public TuyenDuong update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    TuyenDuong tuyenDuong = get(id);
    tuyenDuongRepository
        .findByDiemDiAndDiemDen(request.diemDi().trim(), request.diemDen().trim())
        .filter(other -> !other.getMaTuyen().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict("Tuyến đường này đã tồn tại.");
            });
    apply(tuyenDuong, request);
    return tuyenDuongRepository.save(tuyenDuong);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    TuyenDuong tuyenDuong = get(id);
    if (chuyenXeRepository.existsByMaTuyen(id)) {
      throw ApiException.conflict("Không thể xóa tuyến đường đã có chuyến xe.");
    }
    if (giaVeRepository.existsByMaTuyen(id)) {
      throw ApiException.conflict("Không thể xóa tuyến đường đang có bảng giá.");
    }
    tuyenDuongRepository.delete(tuyenDuong);
    return Map.of("message", "Xóa tuyến đường thành công.");
  }

  private void apply(TuyenDuong tuyenDuong, Request request) {
    tuyenDuong.setDiemDi(request.diemDi().trim());
    tuyenDuong.setDiemDen(request.diemDen().trim());
    tuyenDuong.setQuangDuong(request.quangDuong());
    tuyenDuong.setThoiGianDuKien(request.thoiGianDuKien());
    if (request.trangThai() != null) {
      tuyenDuong.setTrangThai(request.trangThai());
    }
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập điểm đi.") String diemDi,
      @NotBlank(message = "Vui lòng nhập điểm đến.") String diemDen,
      @Positive(message = "Quãng đường phải lớn hơn 0.") Integer quangDuong,
      LocalTime thoiGianDuKien,
      Boolean trangThai) {}
}
