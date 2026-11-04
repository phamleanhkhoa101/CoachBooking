package vn.qlbxmd.web;

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
import vn.qlbxmd.common.ApiException;
import vn.qlbxmd.common.Validators;
import vn.qlbxmd.domain.KhachHang;
import vn.qlbxmd.repo.HoaDonRepository;
import vn.qlbxmd.repo.KhachHangRepository;
import vn.qlbxmd.repo.TaiKhoanRepository;

@RestController
@RequestMapping("/api/khach-hang")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class KhachHangController {

    private final KhachHangRepository khachHangRepository;
    private final HoaDonRepository hoaDonRepository;
    private final TaiKhoanRepository taiKhoanRepository;

    public KhachHangController(KhachHangRepository khachHangRepository,
                               HoaDonRepository hoaDonRepository,
                               TaiKhoanRepository taiKhoanRepository) {
        this.khachHangRepository = khachHangRepository;
        this.hoaDonRepository = hoaDonRepository;
        this.taiKhoanRepository = taiKhoanRepository;
    }

    @GetMapping
    public List<KhachHang> list() {
        return khachHangRepository.findAll();
    }

    @GetMapping("/{id}")
    public KhachHang get(@PathVariable Integer id) {
        return khachHangRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Khách hàng không tồn tại."));
    }

    @PostMapping
    public KhachHang create(@Valid @RequestBody Request request) {
        if (khachHangRepository.existsBySdt(request.sdt().trim())) {
            throw ApiException.conflict("Số điện thoại đã tồn tại.");
        }
        if (request.maTk() != null && !taiKhoanRepository.existsById(request.maTk())) {
            throw ApiException.notFound("Tài khoản không tồn tại.");
        }
        KhachHang khachHang = new KhachHang();
        apply(khachHang, request);
        return khachHangRepository.save(khachHang);
    }

    @PutMapping("/{id}")
    public KhachHang update(@PathVariable Integer id, @Valid @RequestBody Request request) {
        KhachHang khachHang = get(id);
        Validators.requirePhone(request.sdt());
        khachHangRepository.findBySdt(request.sdt().trim())
                .filter(other -> !other.getMaKh().equals(id))
                .ifPresent(other -> {
                    throw ApiException.conflict("Số điện thoại đã tồn tại.");
                });
        if (request.maTk() != null && !taiKhoanRepository.existsById(request.maTk())) {
            throw ApiException.notFound("Tài khoản không tồn tại.");
        }
        apply(khachHang, request);
        return khachHangRepository.save(khachHang);
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Map<String, String> delete(@PathVariable Integer id) {
        KhachHang khachHang = get(id);
        if (hoaDonRepository.existsByMaKh(id)) {
            throw ApiException.conflict("Không thể xóa khách hàng đã có hóa đơn.");
        }
        khachHangRepository.delete(khachHang);
        return Map.of("message", "Xóa khách hàng thành công.");
    }

    private void apply(KhachHang khachHang, Request request) {
        if (request.namSinh() == null || request.namSinh().isAfter(LocalDate.now())) {
            throw ApiException.badRequest("Ngày sinh không hợp lệ.");
        }
        Validators.requirePhone(request.sdt());
        khachHang.setTenKh(request.tenKh().trim());
        khachHang.setNamSinh(request.namSinh());
        khachHang.setSdt(request.sdt().trim());
        if (request.gioiTinh() != null) {
            khachHang.setGioiTinh(request.gioiTinh());
        }
        khachHang.setMaTk(request.maTk());
    }

    public record Request(
            @NotBlank(message = "Vui lòng nhập tên khách hàng.") String tenKh,
            @NotNull(message = "Vui lòng nhập ngày sinh.") LocalDate namSinh,
            @NotBlank(message = "Vui lòng nhập số điện thoại.") String sdt,
            Boolean gioiTinh,
            Integer maTk) {
    }
}
