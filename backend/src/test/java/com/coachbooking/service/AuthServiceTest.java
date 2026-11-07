package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Quyen;
import com.coachbooking.domain.TaiKhoan;
import com.coachbooking.repo.KhachHangRepository;
import com.coachbooking.repo.NhanVienRepository;
import com.coachbooking.repo.QuyenRepository;
import com.coachbooking.repo.TaiKhoanRepository;
import com.coachbooking.security.JwtService;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.security.crypto.password.PasswordEncoder;

class AuthServiceTest {

  private TaiKhoanRepository taiKhoanRepository;
  private QuyenRepository quyenRepository;
  private NhanVienRepository nhanVienRepository;
  private KhachHangRepository khachHangRepository;
  private PasswordEncoder passwordEncoder;
  private JwtService jwtService;
  private AuthService authService;

  @BeforeEach
  void setUp() {
    taiKhoanRepository = mock(TaiKhoanRepository.class);
    quyenRepository = mock(QuyenRepository.class);
    nhanVienRepository = mock(NhanVienRepository.class);
    khachHangRepository = mock(KhachHangRepository.class);
    passwordEncoder = mock(PasswordEncoder.class);
    jwtService = mock(JwtService.class);

    authService =
        new AuthService(
            taiKhoanRepository,
            quyenRepository,
            nhanVienRepository,
            khachHangRepository,
            passwordEncoder,
            jwtService);
  }

  private TaiKhoan createTaiKhoan(int maTk, String username, int maQuyen, boolean trangThai) {
    TaiKhoan tk = new TaiKhoan();
    tk.setMaTk(maTk);
    tk.setTenDangNhap(username);
    tk.setMatKhau("encoded_secret");
    tk.setMaQuyen(maQuyen);
    tk.setTrangThai(trangThai);
    return tk;
  }

  @Test
  void loginUser_success_whenKhachHang() {
    TaiKhoan tk = createTaiKhoan(1, "kh1", 3, true);
    when(taiKhoanRepository.findByTenDangNhap("kh1")).thenReturn(Optional.of(tk));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Quyen role = new Quyen();
    role.setMaQuyen(3);
    role.setTenQuyen("KhachHang");
    when(quyenRepository.findById(3)).thenReturn(Optional.of(role));
    when(jwtService.generate(tk, "KhachHang")).thenReturn("mock_token");

    AuthService.AuthResponse response =
        authService.loginUser(new AuthService.LoginRequest("kh1", "pass123"));

    assertNotNull(response);
    assertEquals("mock_token", response.token());
    assertEquals("KhachHang", response.nguoiDung().quyen());
  }

  @Test
  void loginUser_forbidden_whenAdmin() {
    TaiKhoan tk = createTaiKhoan(2, "admin", 1, true);
    when(taiKhoanRepository.findByTenDangNhap("admin")).thenReturn(Optional.of(tk));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Quyen role = new Quyen();
    role.setMaQuyen(1);
    role.setTenQuyen("Admin");
    when(quyenRepository.findById(1)).thenReturn(Optional.of(role));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> authService.loginUser(new AuthService.LoginRequest("admin", "pass123")));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
    assertTrue(ex.getMessage().contains("Quản trị viên"));
  }

  @Test
  void loginAdmin_success_whenAdminOrEmployee() {
    TaiKhoan tk = createTaiKhoan(2, "admin", 1, true);
    when(taiKhoanRepository.findByTenDangNhap("admin")).thenReturn(Optional.of(tk));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Quyen role = new Quyen();
    role.setMaQuyen(1);
    role.setTenQuyen("Admin");
    when(quyenRepository.findById(1)).thenReturn(Optional.of(role));
    when(jwtService.generate(tk, "Admin")).thenReturn("admin_token");

    AuthService.AuthResponse response =
        authService.loginAdmin(new AuthService.LoginRequest("admin", "pass123"));

    assertNotNull(response);
    assertEquals("admin_token", response.token());
    assertEquals("Admin", response.nguoiDung().quyen());
  }

  @Test
  void loginAdmin_forbidden_whenKhachHang() {
    TaiKhoan tk = createTaiKhoan(1, "kh1", 3, true);
    when(taiKhoanRepository.findByTenDangNhap("kh1")).thenReturn(Optional.of(tk));
    when(passwordEncoder.matches("pass123", "encoded_secret")).thenReturn(true);

    Quyen role = new Quyen();
    role.setMaQuyen(3);
    role.setTenQuyen("KhachHang");
    when(quyenRepository.findById(3)).thenReturn(Optional.of(role));

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> authService.loginAdmin(new AuthService.LoginRequest("kh1", "pass123")));
    assertEquals(HttpStatus.FORBIDDEN, ex.getStatus());
    assertTrue(ex.getMessage().contains("không có quyền truy cập quản trị"));
  }

  @Test
  void authenticate_badRequest_whenWrongPassword() {
    TaiKhoan tk = createTaiKhoan(1, "kh1", 3, true);
    when(taiKhoanRepository.findByTenDangNhap("kh1")).thenReturn(Optional.of(tk));
    when(passwordEncoder.matches("wrong", "encoded_secret")).thenReturn(false);

    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> authService.login(new AuthService.LoginRequest("kh1", "wrong")));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
  }
}
