package vn.qlbxmd.config;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;
import vn.qlbxmd.domain.ChuyenXe;
import vn.qlbxmd.domain.HoaDon;
import vn.qlbxmd.domain.KhachHang;
import vn.qlbxmd.domain.NhanVien;
import vn.qlbxmd.domain.TaiKhoan;
import vn.qlbxmd.domain.ThanhToan;
import vn.qlbxmd.domain.VeXe;
import vn.qlbxmd.repo.ChuyenXeRepository;
import vn.qlbxmd.repo.HoaDonRepository;
import vn.qlbxmd.repo.KhachHangRepository;
import vn.qlbxmd.repo.NhanVienRepository;
import vn.qlbxmd.repo.TaiKhoanRepository;
import vn.qlbxmd.repo.ThanhToanRepository;
import vn.qlbxmd.repo.VeXeRepository;

@Component
public class DataInitializer implements CommandLineRunner {

    private static final Logger log = LoggerFactory.getLogger(DataInitializer.class);

    private final TaiKhoanRepository taiKhoanRepository;
    private final NhanVienRepository nhanVienRepository;
    private final KhachHangRepository khachHangRepository;
    private final ChuyenXeRepository chuyenXeRepository;
    private final HoaDonRepository hoaDonRepository;
    private final VeXeRepository veXeRepository;
    private final ThanhToanRepository thanhToanRepository;
    private final PasswordEncoder passwordEncoder;

    public DataInitializer(TaiKhoanRepository taiKhoanRepository,
                           NhanVienRepository nhanVienRepository,
                           KhachHangRepository khachHangRepository,
                           ChuyenXeRepository chuyenXeRepository,
                           HoaDonRepository hoaDonRepository,
                           VeXeRepository veXeRepository,
                           ThanhToanRepository thanhToanRepository,
                           PasswordEncoder passwordEncoder) {
        this.taiKhoanRepository = taiKhoanRepository;
        this.nhanVienRepository = nhanVienRepository;
        this.khachHangRepository = khachHangRepository;
        this.chuyenXeRepository = chuyenXeRepository;
        this.hoaDonRepository = hoaDonRepository;
        this.veXeRepository = veXeRepository;
        this.thanhToanRepository = thanhToanRepository;
        this.passwordEncoder = passwordEncoder;
    }

    @Override
    @Transactional
    public void run(String... args) {
        TaiKhoan admin = ensureAccount("admin", "admin@qlbxmd.vn", "Admin@123", 1);
        TaiKhoan nv1 = ensureAccount("nv1", "nv1@qlbxmd.vn", "NhanVien@123", 2);
        TaiKhoan nv2 = ensureAccount("nv2", "nv2@qlbxmd.vn", "NhanVien@123", 2);
        TaiKhoan kh1 = ensureAccount("kh1", "4@gmail.com", "KhachHang@123", 3);
        TaiKhoan kh2 = ensureAccount("kh2", "5@gmail.com", "KhachHang@123", 3);

        linkNhanVien("0912345678", admin);
        linkNhanVien("0912345679", nv1);
        linkNhanVien("0912345680", nv2);
        linkKhachHang("0981111111", kh1);
        linkKhachHang("0981111112", kh2);

        seedDemoInvoices();
    }

    private TaiKhoan ensureAccount(String tenDangNhap, String email, String matKhau, int maQuyen) {
        return taiKhoanRepository.findByTenDangNhap(tenDangNhap).orElseGet(() -> {
            TaiKhoan taiKhoan = new TaiKhoan();
            taiKhoan.setTenDangNhap(tenDangNhap);
            taiKhoan.setEmail(email);
            taiKhoan.setMatKhau(passwordEncoder.encode(matKhau));
            taiKhoan.setMaQuyen(maQuyen);
            taiKhoan.setTrangThai(true);
            TaiKhoan saved = taiKhoanRepository.save(taiKhoan);
            log.info("Đã tạo tài khoản mẫu: {}", tenDangNhap);
            return saved;
        });
    }

    private void linkNhanVien(String sdt, TaiKhoan taiKhoan) {
        nhanVienRepository.findBySdt(sdt).ifPresent(nhanVien -> {
            if (nhanVien.getMaTk() == null) {
                nhanVien.setMaTk(taiKhoan.getMaTk());
                nhanVienRepository.save(nhanVien);
            }
        });
    }

    private void linkKhachHang(String sdt, TaiKhoan taiKhoan) {
        khachHangRepository.findBySdt(sdt).ifPresent(khachHang -> {
            if (khachHang.getMaTk() == null) {
                khachHang.setMaTk(taiKhoan.getMaTk());
                khachHangRepository.save(khachHang);
            }
        });
    }

    private void seedDemoInvoices() {
        if (hoaDonRepository.count() > 0) {
            return;
        }
        List<ChuyenXe> chuyenXes = chuyenXeRepository.findAllByOrderByNgayGioKhoiHanhAsc().stream()
                .filter(chuyen -> chuyen.getNgayGioKhoiHanh().isAfter(LocalDateTime.now()))
                .limit(3)
                .toList();
        List<KhachHang> khachHangs = khachHangRepository.findAll().stream().limit(3).toList();
        if (chuyenXes.isEmpty() || khachHangs.isEmpty()) {
            return;
        }

        NhanVien nhanVien = nhanVienRepository.findAll().stream().findFirst().orElse(null);
        for (int i = 0; i < Math.min(chuyenXes.size(), khachHangs.size()); i++) {
            ChuyenXe chuyen = chuyenXes.get(i);
            if (chuyen.getGiaVeThucTe() == null || chuyen.getGiaVeThucTe().signum() <= 0) {
                continue;
            }
            KhachHang khachHang = khachHangs.get(i);
            HoaDon hoaDon = new HoaDon();
            hoaDon.setMaKh(khachHang.getMaKh());
            hoaDon.setMaNv(nhanVien == null ? null : nhanVien.getMaNv());
            hoaDon.setNgayLap(LocalDateTime.now().minusDays(3L - i));
            hoaDon.setTrangThai(true);

            List<VeXe> ves = new java.util.ArrayList<>();
            int soVe = (i % 2) + 1;
            for (int seat = 1; seat <= soVe; seat++) {
                VeXe ve = new VeXe();
                ve.setMaChuyen(chuyen.getMaChuyen());
                ve.setChoNgoi(seat);
                ve.setGiaVe(chuyen.getGiaVeThucTe());
                ves.add(ve);
            }
            BigDecimal tongTien = chuyen.getGiaVeThucTe().multiply(BigDecimal.valueOf(soVe));
            hoaDon.setTongTien(tongTien);
            HoaDon saved = hoaDonRepository.save(hoaDon);

            for (VeXe ve : ves) {
                ve.setMaHd(saved.getMaHd());
            }
            veXeRepository.saveAll(ves);

            ThanhToan thanhToan = new ThanhToan();
            thanhToan.setMaHd(saved.getMaHd());
            thanhToan.setTongTien(tongTien);
            thanhToan.setNgayTt(saved.getNgayLap().plusMinutes(5));
            thanhToan.setPhuongThuc("Tiền mặt");
            thanhToanRepository.save(thanhToan);
        }
        log.info("Đã tạo hóa đơn mẫu cho thống kê và lịch sử mua vé.");
    }
}
