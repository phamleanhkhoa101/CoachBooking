package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.TaiXe;
import com.coachbooking.repo.ChuyenXeRepository;
import com.coachbooking.repo.TaiXeRepository;
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
@RequestMapping("/api/tai-xe")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class TaiXeController {

  private final TaiXeRepository taiXeRepository;
  private final ChuyenXeRepository chuyenXeRepository;

  public TaiXeController(TaiXeRepository taiXeRepository, ChuyenXeRepository chuyenXeRepository) {
    this.taiXeRepository = taiXeRepository;
    this.chuyenXeRepository = chuyenXeRepository;
  }

  @GetMapping
  public List<TaiXe> list() {
    return taiXeRepository.findAll();
  }

  @GetMapping("/{id}")
  public TaiXe get(@PathVariable Integer id) {
    return taiXeRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
  }

  @PostMapping
  @PreAuthorize("hasRole('Admin')")
  public TaiXe create(@Valid @RequestBody Request request) {
    TaiXe taiXe = new TaiXe();
    apply(taiXe, request);
    return taiXeRepository.save(taiXe);
  }

  @PutMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public TaiXe update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    TaiXe taiXe = get(id);
    apply(taiXe, request);
    return taiXeRepository.save(taiXe);
  }

  @DeleteMapping("/{id}")
  @PreAuthorize("hasRole('Admin')")
  public Map<String, String> delete(@PathVariable Integer id) {
    TaiXe taiXe = get(id);
    if (chuyenXeRepository.existsByMaTx(id)) {
      throw ApiException.conflict("Không thể xóa tài xế đã có chuyến xe.");
    }
    taiXeRepository.delete(taiXe);
    return Map.of("message", "Xóa tài xế thành công.");
  }

  private void apply(TaiXe taiXe, Request request) {
    if (request.namSinh().isAfter(LocalDate.now().minusYears(18))) {
      throw ApiException.badRequest("Tài xế phải đủ 18 tuổi.");
    }
    taiXe.setTenTx(request.tenTx() == null ? null : request.tenTx().trim());
    taiXe.setNamSinh(request.namSinh());
    taiXe.setCccd(request.cccd().trim());
    if (request.gioiTinh() != null) {
      taiXe.setGioiTinh(request.gioiTinh());
    }
    taiXe.setSdt(request.sdt().trim());
    taiXe.setDiaChi(request.diaChi().trim());
    taiXe.setBangLai(request.bangLai().trim());
    taiXe.setNgayCap(request.ngayCap());
    if (request.trangThai() != null) {
      taiXe.setTrangThai(request.trangThai());
    }
  }

  public record Request(
      String tenTx,
      @NotNull(message = "Vui lòng nhập ngày sinh.") LocalDate namSinh,
      @NotBlank(message = "Vui lòng nhập CCCD.") String cccd,
      Boolean gioiTinh,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") String sdt,
      @NotBlank(message = "Vui lòng nhập địa chỉ.") String diaChi,
      @NotBlank(message = "Vui lòng nhập bằng lái.") String bangLai,
      @NotNull(message = "Vui lòng nhập ngày cấp.") LocalDate ngayCap,
      Boolean trangThai) {}
}
