package vn.qlbxmd.web;

import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import java.util.List;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.crypto.password.PasswordEncoder;
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
import vn.qlbxmd.domain.TaiKhoan;
import vn.qlbxmd.repo.KhachHangRepository;
import vn.qlbxmd.repo.NhanVienRepository;
import vn.qlbxmd.repo.QuyenRepository;
import vn.qlbxmd.repo.TaiKhoanRepository;

@RestController
@RequestMapping("/api/tai-khoan")
@PreAuthorize("hasRole('Admin')")
public class TaiKhoanController {

    private final TaiKhoanRepository taiKhoanRepository;
    private final QuyenRepository quyenRepository;
    private final NhanVienRepository nhanVienRepository;
    private final KhachHangRepository khachHangRepository;
    private final PasswordEncoder passwordEncoder;

    public TaiKhoanController(TaiKhoanRepository taiKhoanRepository,
                              QuyenRepository quyenRepository,
                              NhanVienRepository nhanVienRepository,
                              KhachHangRepository khachHangRepository,
                              PasswordEncoder passwordEncoder) {
        this.taiKhoanRepository = taiKhoanRepository;
        this.quyenRepository = quyenRepository;
        this.nhanVienRepository = nhanVienRepository;
        this.khachHangRepository = khachHangRepository;
        this.passwordEncoder = passwordEncoder;
    }

    @GetMapping
    public List<TaiKhoan> list() {
        return taiKhoanRepository.findAll();
    }

    @GetMapping("/{id}")
    public TaiKhoan get(@PathVariable Integer id) {
        return taiKhoanRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Tài khoản không tồn tại."));
    }

    @PostMapping
    public TaiKhoan create(@Valid @RequestBody CreateRequest request) {
        String tenDangNhap = request.tenDangNhap().trim();
        if (tenDangNhap.length() < 6) {
            throw ApiException.badRequest("Tên đăng nhập phải có ít nhất 6 ký tự.");
        }
        Validators.requirePassword(request.matKhau());
        if (taiKhoanRepository.existsByTenDangNhap(tenDangNhap)) {
            throw ApiException.conflict("Tên đăng nhập đã được sử dụng.");
        }
        if (taiKhoanRepository.existsByEmail(request.email().trim())) {
            throw ApiException.conflict("Email đã được sử dụng.");
        }
        if (!quyenRepository.existsById(request.maQuyen())) {
            throw ApiException.notFound("Quyền không tồn tại.");
        }

        TaiKhoan taiKhoan = new TaiKhoan();
        taiKhoan.setTenDangNhap(tenDangNhap);
        taiKhoan.setEmail(request.email().trim());
        taiKhoan.setMatKhau(passwordEncoder.encode(request.matKhau()));
        taiKhoan.setMaQuyen(request.maQuyen());
        taiKhoan.setTrangThai(request.trangThai() == null || request.trangThai());
        return taiKhoanRepository.save(taiKhoan);
    }

    @PutMapping("/{id}")
    public TaiKhoan update(@PathVariable Integer id, @Valid @RequestBody UpdateRequest request) {
        TaiKhoan taiKhoan = get(id);
        if (!quyenRepository.existsById(request.maQuyen())) {
            throw ApiException.notFound("Quyền không tồn tại.");
        }
        if (!taiKhoan.getEmail().equalsIgnoreCase(request.email().trim())
                && taiKhoanRepository.existsByEmail(request.email().trim())) {
            throw ApiException.conflict("Email đã được sử dụng.");
        }
        taiKhoan.setEmail(request.email().trim());
        taiKhoan.setMaQuyen(request.maQuyen());
        if (request.trangThai() != null) {
            taiKhoan.setTrangThai(request.trangThai());
        }
        if (!Validators.isBlank(request.matKhau())) {
            Validators.requirePassword(request.matKhau());
            taiKhoan.setMatKhau(passwordEncoder.encode(request.matKhau()));
        }
        return taiKhoanRepository.save(taiKhoan);
    }

    @DeleteMapping("/{id}")
    public Map<String, String> delete(@PathVariable Integer id) {
        TaiKhoan taiKhoan = get(id);
        if (nhanVienRepository.existsByMaTk(id) || khachHangRepository.existsByMaTk(id)) {
            throw ApiException.conflict("Không thể xóa tài khoản đang gắn với nhân viên hoặc khách hàng.");
        }
        taiKhoanRepository.delete(taiKhoan);
        return Map.of("message", "Xóa tài khoản thành công.");
    }

    public record CreateRequest(
            @NotBlank(message = "Vui lòng nhập tên đăng nhập.") String tenDangNhap,
            @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.") String email,
            @NotBlank(message = "Vui lòng nhập mật khẩu.") String matKhau,
            @NotNull(message = "Vui lòng chọn quyền.") Integer maQuyen,
            Boolean trangThai) {
    }

    public record UpdateRequest(
            @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.") String email,
            String matKhau,
            @NotNull(message = "Vui lòng chọn quyền.") Integer maQuyen,
            Boolean trangThai) {
    }
}
