package com.coachbooking.service;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.KhachHang;
import com.coachbooking.domain.NhanVien;
import com.coachbooking.domain.Quyen;
import com.coachbooking.domain.TaiKhoan;
import com.coachbooking.repo.KhachHangRepository;
import com.coachbooking.repo.NhanVienRepository;
import com.coachbooking.repo.QuyenRepository;
import com.coachbooking.repo.TaiKhoanRepository;
import com.coachbooking.security.JwtService;

@Service
public class AuthService {

    private static final int MA_QUYEN_KHACH_HANG = 3;
    private static final String UPPER = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private static final String LOWER = "abcdefghijkmnopqrstuvwxyz";
    private static final String DIGITS = "23456789";
    private static final String SPECIAL = "!@#$%^&*";

    private final TaiKhoanRepository taiKhoanRepository;
    private final QuyenRepository quyenRepository;
    private final NhanVienRepository nhanVienRepository;
    private final KhachHangRepository khachHangRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtService jwtService;

    public AuthService(TaiKhoanRepository taiKhoanRepository,
                       QuyenRepository quyenRepository,
                       NhanVienRepository nhanVienRepository,
                       KhachHangRepository khachHangRepository,
                       PasswordEncoder passwordEncoder,
                       JwtService jwtService) {
        this.taiKhoanRepository = taiKhoanRepository;
        this.quyenRepository = quyenRepository;
        this.nhanVienRepository = nhanVienRepository;
        this.khachHangRepository = khachHangRepository;
        this.passwordEncoder = passwordEncoder;
        this.jwtService = jwtService;
    }

    public AuthResponse login(LoginRequest request) {
        TaiKhoan taiKhoan = taiKhoanRepository.findByTenDangNhap(request.tenDangNhap().trim())
                .orElseThrow(() -> ApiException.badRequest("Tên đăng nhập hoặc mật khẩu không đúng."));
        if (!passwordEncoder.matches(request.matKhau(), taiKhoan.getMatKhau())) {
            throw ApiException.badRequest("Tên đăng nhập hoặc mật khẩu không đúng.");
        }
        if (!Boolean.TRUE.equals(taiKhoan.getTrangThai())) {
            throw ApiException.forbidden("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
        }
        String tenQuyen = roleName(taiKhoan);
        return new AuthResponse(jwtService.generate(taiKhoan, tenQuyen), toNguoiDung(taiKhoan, tenQuyen));
    }

    @Transactional
    public Map<String, String> register(RegisterRequest request) {
        Validators.requireText(request.tenDangNhap(), "Vui lòng nhập tên đăng nhập.");
        if (request.tenDangNhap().trim().length() < 6) {
            throw ApiException.badRequest("Tên đăng nhập phải có ít nhất 6 ký tự.");
        }
        Validators.requireEmail(request.email());
        Validators.requirePassword(request.matKhau());
        if (!request.matKhau().equals(request.xacNhanMatKhau())) {
            throw ApiException.badRequest("Mật khẩu không khớp.");
        }
        if (taiKhoanRepository.existsByTenDangNhap(request.tenDangNhap().trim())) {
            throw ApiException.conflict("Tên đăng nhập đã được sử dụng.");
        }
        if (taiKhoanRepository.existsByEmail(request.email().trim())) {
            throw ApiException.conflict("Email đã được sử dụng.");
        }

        TaiKhoan taiKhoan = new TaiKhoan();
        taiKhoan.setTenDangNhap(request.tenDangNhap().trim());
        taiKhoan.setEmail(request.email().trim());
        taiKhoan.setMatKhau(passwordEncoder.encode(request.matKhau()));
        taiKhoan.setMaQuyen(MA_QUYEN_KHACH_HANG);
        taiKhoan.setTrangThai(true);
        taiKhoanRepository.save(taiKhoan);
        return Map.of("message", "Đăng ký thành công.");
    }

    @Transactional
    public Map<String, String> forgotPassword(ForgotPasswordRequest request) {
        Validators.requireEmail(request.email());
        if (Validators.isBlank(request.tenDangNhap()) || request.tenDangNhap().trim().length() < 6) {
            throw ApiException.badRequest("Tên đăng nhập phải có ít nhất 6 ký tự.");
        }
        TaiKhoan taiKhoan = taiKhoanRepository
                .findByTenDangNhapAndEmail(request.tenDangNhap().trim(), request.email().trim())
                .orElseThrow(() -> ApiException.notFound("Tên đăng nhập hoặc Email không tồn tại."));

        String matKhauTam = generatePassword();
        taiKhoan.setMatKhau(passwordEncoder.encode(matKhauTam));
        taiKhoanRepository.save(taiKhoan);
        return Map.of(
                "message", "Đã đặt lại mật khẩu. Môi trường demo trả mật khẩu tạm trực tiếp.",
                "matKhauTam", matKhauTam);
    }

    public NguoiDung me(TaiKhoan taiKhoan) {
        return toNguoiDung(taiKhoan, roleName(taiKhoan));
    }

    @Transactional
    public Map<String, String> changePassword(TaiKhoan taiKhoan, ChangePasswordRequest request) {
        if (!passwordEncoder.matches(request.matKhauCu(), taiKhoan.getMatKhau())) {
            throw ApiException.badRequest("Mật khẩu hiện tại không đúng.");
        }
        Validators.requirePassword(request.matKhauMoi());
        taiKhoan.setMatKhau(passwordEncoder.encode(request.matKhauMoi()));
        taiKhoanRepository.save(taiKhoan);
        return Map.of("message", "Đổi mật khẩu thành công.");
    }

    public NguoiDung toNguoiDung(TaiKhoan taiKhoan, String tenQuyen) {
        NhanVien nhanVien = nhanVienRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);
        KhachHang khachHang = khachHangRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);
        String hoTen = nhanVien != null ? nhanVien.getTenNv()
                : khachHang != null ? khachHang.getTenKh()
                : taiKhoan.getTenDangNhap();
        return new NguoiDung(
                taiKhoan.getMaTk(),
                taiKhoan.getTenDangNhap(),
                taiKhoan.getEmail(),
                tenQuyen,
                taiKhoan.getMaQuyen(),
                nhanVien == null ? null : nhanVien.getMaNv(),
                khachHang == null ? null : khachHang.getMaKh(),
                hoTen,
                taiKhoan.getTrangThai());
    }

    public String roleName(TaiKhoan taiKhoan) {
        return quyenRepository.findById(taiKhoan.getMaQuyen())
                .map(Quyen::getTenQuyen)
                .orElse("KhachHang");
    }

    private String generatePassword() {
        SecureRandom random = new SecureRandom();
        List<Character> characters = new ArrayList<>();
        characters.add(UPPER.charAt(random.nextInt(UPPER.length())));
        characters.add(LOWER.charAt(random.nextInt(LOWER.length())));
        characters.add(DIGITS.charAt(random.nextInt(DIGITS.length())));
        characters.add(SPECIAL.charAt(random.nextInt(SPECIAL.length())));
        String all = UPPER + LOWER + DIGITS + SPECIAL;
        while (characters.size() < 10) {
            characters.add(all.charAt(random.nextInt(all.length())));
        }
        Collections.shuffle(characters, random);
        StringBuilder builder = new StringBuilder();
        characters.forEach(builder::append);
        return builder.toString();
    }

    public record LoginRequest(
            @NotBlank(message = "Vui lòng nhập tên đăng nhập.") String tenDangNhap,
            @NotBlank(message = "Vui lòng nhập mật khẩu.") String matKhau) {
    }

    public record RegisterRequest(
            @NotBlank(message = "Vui lòng nhập tên đăng nhập.") String tenDangNhap,
            @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.") String email,
            @NotBlank(message = "Vui lòng nhập mật khẩu.") String matKhau,
            @NotBlank(message = "Vui lòng xác nhận mật khẩu.") String xacNhanMatKhau) {
    }

    public record ForgotPasswordRequest(
            @NotBlank(message = "Vui lòng nhập tên đăng nhập.") String tenDangNhap,
            @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.") String email) {
    }

    public record ChangePasswordRequest(
            @NotBlank(message = "Vui lòng nhập mật khẩu hiện tại.") String matKhauCu,
            @NotBlank(message = "Vui lòng nhập mật khẩu mới.") String matKhauMoi) {
    }

    public record NguoiDung(
            Integer maTk,
            String tenDangNhap,
            String email,
            String quyen,
            Integer maQuyen,
            Integer maNv,
            Integer maKh,
            String hoTen,
            Boolean trangThai) {
    }

    public record AuthResponse(String token, NguoiDung nguoiDung) {
    }
}
