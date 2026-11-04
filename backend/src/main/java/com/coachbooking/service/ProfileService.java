package com.coachbooking.service;

import jakarta.validation.constraints.NotBlank;
import java.time.LocalDate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.KhachHang;
import com.coachbooking.domain.NhanVien;
import com.coachbooking.domain.TaiKhoan;
import com.coachbooking.repo.KhachHangRepository;
import com.coachbooking.repo.NhanVienRepository;

@Service
public class ProfileService {

    private final KhachHangRepository khachHangRepository;
    private final NhanVienRepository nhanVienRepository;
    private final CurrentUserService currentUserService;

    public ProfileService(KhachHangRepository khachHangRepository,
                          NhanVienRepository nhanVienRepository,
                          CurrentUserService currentUserService) {
        this.khachHangRepository = khachHangRepository;
        this.nhanVienRepository = nhanVienRepository;
        this.currentUserService = currentUserService;
    }

    public ProfileResponse get(TaiKhoan taiKhoan) {
        String quyen = currentUserService.roleName(taiKhoan);
        KhachHang khachHang = khachHangRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);
        NhanVien nhanVien = nhanVienRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);

        if (khachHang != null) {
            return new ProfileResponse("KHACHHANG", khachHang.getMaKh(), null,
                    khachHang.getTenKh(), khachHang.getNamSinh(), khachHang.getSdt(),
                    khachHang.getGioiTinh(), null, taiKhoan.getEmail(), taiKhoan.getTenDangNhap(), quyen);
        }
        if (nhanVien != null) {
            return new ProfileResponse("NHANVIEN", null, nhanVien.getMaNv(),
                    nhanVien.getTenNv(), nhanVien.getNamSinh(), nhanVien.getSdt(),
                    nhanVien.getGioiTinh(), nhanVien.getDiaChi(), taiKhoan.getEmail(),
                    taiKhoan.getTenDangNhap(), quyen);
        }
        return new ProfileResponse("CHUA_GAN", null, null, taiKhoan.getTenDangNhap(), null, null,
                null, null, taiKhoan.getEmail(), taiKhoan.getTenDangNhap(), quyen);
    }

    @Transactional
    public ProfileResponse update(TaiKhoan taiKhoan, UpdateProfileRequest request) {
        Validators.requireText(request.hoTen(), "Vui lòng nhập họ tên.");
        Validators.requirePhone(request.sdt());
        if (request.namSinh() == null) {
            throw ApiException.badRequest("Vui lòng nhập ngày sinh.");
        }
        if (request.namSinh().isAfter(LocalDate.now())) {
            throw ApiException.badRequest("Ngày sinh không hợp lệ.");
        }

        KhachHang khachHang = khachHangRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);
        if (khachHang != null) {
            khachHangRepository.findBySdt(request.sdt().trim())
                    .filter(other -> !other.getMaKh().equals(khachHang.getMaKh()))
                    .ifPresent(other -> {
                        throw ApiException.conflict("Số điện thoại đã được sử dụng.");
                    });
            khachHang.setTenKh(request.hoTen().trim());
            khachHang.setSdt(request.sdt().trim());
            khachHang.setNamSinh(request.namSinh());
            khachHang.setGioiTinh(Boolean.TRUE.equals(request.gioiTinh()));
            khachHangRepository.save(khachHang);
            return get(taiKhoan);
        }

        NhanVien nhanVien = nhanVienRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);
        if (nhanVien != null) {
            if (Validators.isBlank(request.diaChi())) {
                throw ApiException.badRequest("Vui lòng nhập địa chỉ.");
            }
            nhanVienRepository.findBySdt(request.sdt().trim())
                    .filter(other -> !other.getMaNv().equals(nhanVien.getMaNv()))
                    .ifPresent(other -> {
                        throw ApiException.conflict("Số điện thoại đã được sử dụng.");
                    });
            nhanVien.setTenNv(request.hoTen().trim());
            nhanVien.setSdt(request.sdt().trim());
            nhanVien.setNamSinh(request.namSinh());
            nhanVien.setGioiTinh(Boolean.TRUE.equals(request.gioiTinh()));
            nhanVien.setDiaChi(request.diaChi().trim());
            nhanVienRepository.save(nhanVien);
            return get(taiKhoan);
        }

        if (!currentUserService.isCustomer(taiKhoan)) {
            throw ApiException.badRequest("Tài khoản chưa được gắn với nhân viên hoặc khách hàng.");
        }
        KhachHang trungSdt = khachHangRepository.findBySdt(request.sdt().trim()).orElse(null);
        if (trungSdt != null) {
            if (trungSdt.getMaTk() != null && !trungSdt.getMaTk().equals(taiKhoan.getMaTk())) {
                throw ApiException.conflict("Số điện thoại đã được sử dụng.");
            }
            trungSdt.setTenKh(request.hoTen().trim());
            trungSdt.setNamSinh(request.namSinh());
            trungSdt.setGioiTinh(Boolean.TRUE.equals(request.gioiTinh()));
            trungSdt.setMaTk(taiKhoan.getMaTk());
            khachHangRepository.save(trungSdt);
            return get(taiKhoan);
        }
        KhachHang moi = new KhachHang();
        moi.setTenKh(request.hoTen().trim());
        moi.setSdt(request.sdt().trim());
        moi.setNamSinh(request.namSinh());
        moi.setGioiTinh(Boolean.TRUE.equals(request.gioiTinh()));
        moi.setMaTk(taiKhoan.getMaTk());
        khachHangRepository.save(moi);
        return get(taiKhoan);
    }

    public record UpdateProfileRequest(
            @NotBlank(message = "Vui lòng nhập họ tên.") String hoTen,
            @NotBlank(message = "Vui lòng nhập số điện thoại.") String sdt,
            LocalDate namSinh,
            Boolean gioiTinh,
            String diaChi) {
    }

    public record ProfileResponse(
            String loai,
            Integer maKh,
            Integer maNv,
            String hoTen,
            LocalDate namSinh,
            String sdt,
            Boolean gioiTinh,
            String diaChi,
            String email,
            String tenDangNhap,
            String quyen) {
    }
}
