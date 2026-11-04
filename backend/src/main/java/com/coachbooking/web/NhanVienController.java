package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.NhanVien;
import com.coachbooking.repo.HoaDonRepository;
import com.coachbooking.repo.NhanVienRepository;
import com.coachbooking.repo.TaiKhoanRepository;
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
@RequestMapping("/api/nhan-vien")
@PreAuthorize("hasRole('Admin')")
public class NhanVienController {

  private final NhanVienRepository nhanVienRepository;
  private final HoaDonRepository hoaDonRepository;
  private final TaiKhoanRepository taiKhoanRepository;

  public NhanVienController(
      NhanVienRepository nhanVienRepository,
      HoaDonRepository hoaDonRepository,
      TaiKhoanRepository taiKhoanRepository) {
    this.nhanVienRepository = nhanVienRepository;
    this.hoaDonRepository = hoaDonRepository;
    this.taiKhoanRepository = taiKhoanRepository;
  }

  @GetMapping
  public List<NhanVien> list() {
    return nhanVienRepository.findAll();
  }

  @GetMapping("/{id}")
  public NhanVien get(@PathVariable Integer id) {
    return nhanVienRepository
        .findById(id)
        .orElseThrow(() -> ApiException.notFound("Nhân viên không tồn tại."));
  }

  @PostMapping
  public NhanVien create(@Valid @RequestBody Request request) {
    if (nhanVienRepository.existsBySdt(request.sdt().trim())) {
      throw ApiException.conflict("Số điện thoại đã tồn tại.");
    }
    validateAccount(null, request);
    NhanVien nhanVien = new NhanVien();
    apply(nhanVien, request);
    return nhanVienRepository.save(nhanVien);
  }

  @PutMapping("/{id}")
  public NhanVien update(@PathVariable Integer id, @Valid @RequestBody Request request) {
    NhanVien nhanVien = get(id);
    nhanVienRepository
        .findBySdt(request.sdt().trim())
        .filter(other -> !other.getMaNv().equals(id))
        .ifPresent(
            other -> {
              throw ApiException.conflict("Số điện thoại đã tồn tại.");
            });
    validateAccount(id, request);
    apply(nhanVien, request);
    return nhanVienRepository.save(nhanVien);
  }

  @DeleteMapping("/{id}")
  public Map<String, String> delete(@PathVariable Integer id) {
    NhanVien nhanVien = get(id);
    if (hoaDonRepository.existsByMaNv(id)) {
      throw ApiException.conflict("Không thể xóa nhân viên đã lập hóa đơn.");
    }
    nhanVienRepository.delete(nhanVien);
    return Map.of("message", "Xóa nhân viên thành công.");
  }

  private void validateAccount(Integer maNv, Request request) {
    if (request.maTk() != null) {
      if (!taiKhoanRepository.existsById(request.maTk())) {
        throw ApiException.notFound("Tài khoản không tồn tại.");
      }
      nhanVienRepository
          .findByMaTk(request.maTk())
          .filter(other -> !other.getMaNv().equals(maNv))
          .ifPresent(
              other -> {
                throw ApiException.conflict("Tài khoản này đã gắn với nhân viên khác.");
              });
    }
  }

  private void apply(NhanVien nhanVien, Request request) {
    if (request.namSinh().isAfter(LocalDate.now().minusYears(18))) {
      throw ApiException.badRequest("Nhân viên phải đủ 18 tuổi.");
    }
    nhanVien.setTenNv(request.tenNv().trim());
    nhanVien.setNamSinh(request.namSinh());
    nhanVien.setDiaChi(request.diaChi().trim());
    nhanVien.setSdt(request.sdt().trim());
    if (request.gioiTinh() != null) {
      nhanVien.setGioiTinh(request.gioiTinh());
    }
    if (request.trangThai() != null) {
      nhanVien.setTrangThai(request.trangThai());
    }
    nhanVien.setMaTk(request.maTk());
  }

  public record Request(
      @NotBlank(message = "Vui lòng nhập tên nhân viên.") String tenNv,
      @NotNull(message = "Vui lòng nhập ngày sinh.") LocalDate namSinh,
      @NotBlank(message = "Vui lòng nhập địa chỉ.") String diaChi,
      @NotBlank(message = "Vui lòng nhập số điện thoại.") String sdt,
      Boolean gioiTinh,
      Boolean trangThai,
      Integer maTk) {}
}
