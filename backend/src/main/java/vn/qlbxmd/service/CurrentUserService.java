package vn.qlbxmd.service;

import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;
import vn.qlbxmd.common.ApiException;
import vn.qlbxmd.domain.Quyen;
import vn.qlbxmd.domain.TaiKhoan;
import vn.qlbxmd.repo.QuyenRepository;
import vn.qlbxmd.repo.TaiKhoanRepository;

@Service
public class CurrentUserService {

    private final TaiKhoanRepository taiKhoanRepository;
    private final QuyenRepository quyenRepository;

    public CurrentUserService(TaiKhoanRepository taiKhoanRepository, QuyenRepository quyenRepository) {
        this.taiKhoanRepository = taiKhoanRepository;
        this.quyenRepository = quyenRepository;
    }

    public TaiKhoan requireTaiKhoan() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated()
                || "anonymousUser".equals(authentication.getName())) {
            throw new ApiException(org.springframework.http.HttpStatus.UNAUTHORIZED, "Vui lòng đăng nhập để tiếp tục.");
        }
        return taiKhoanRepository.findByTenDangNhap(authentication.getName())
                .orElseThrow(() -> new ApiException(org.springframework.http.HttpStatus.UNAUTHORIZED,
                        "Tài khoản không tồn tại."));
    }

    public String roleName(TaiKhoan taiKhoan) {
        return quyenRepository.findById(taiKhoan.getMaQuyen())
                .map(Quyen::getTenQuyen)
                .orElse("KhachHang");
    }

    public boolean isCustomer(TaiKhoan taiKhoan) {
        return "KhachHang".equals(roleName(taiKhoan));
    }
}
