package com.coachbooking.security;

import com.coachbooking.domain.Quyen;
import com.coachbooking.domain.TaiKhoan;
import com.coachbooking.repo.QuyenRepository;
import com.coachbooking.repo.TaiKhoanRepository;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

@Service
public class AppUserDetailsService implements UserDetailsService {

  private final TaiKhoanRepository taiKhoanRepository;
  private final QuyenRepository quyenRepository;

  public AppUserDetailsService(
      TaiKhoanRepository taiKhoanRepository, QuyenRepository quyenRepository) {
    this.taiKhoanRepository = taiKhoanRepository;
    this.quyenRepository = quyenRepository;
  }

  @Override
  public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
    TaiKhoan taiKhoan =
        taiKhoanRepository
            .findByTenDangNhap(username)
            .orElseThrow(
                () -> new UsernameNotFoundException("Không tìm thấy tài khoản: " + username));
    String tenQuyen =
        quyenRepository.findById(taiKhoan.getMaQuyen()).map(Quyen::getTenQuyen).orElse("KhachHang");
    return User.withUsername(taiKhoan.getTenDangNhap())
        .password(taiKhoan.getMatKhau())
        .authorities("ROLE_" + tenQuyen)
        .disabled(!Boolean.TRUE.equals(taiKhoan.getTrangThai()))
        .build();
  }
}
