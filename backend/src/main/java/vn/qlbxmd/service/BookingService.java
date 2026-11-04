package vn.qlbxmd.service;

import jakarta.validation.constraints.NotNull;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.Set;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import vn.qlbxmd.common.ApiException;
import vn.qlbxmd.common.Validators;
import vn.qlbxmd.domain.ChuyenXe;
import vn.qlbxmd.domain.HoaDon;
import vn.qlbxmd.domain.KhachHang;
import vn.qlbxmd.domain.LoaiXe;
import vn.qlbxmd.domain.NhanVien;
import vn.qlbxmd.domain.TaiKhoan;
import vn.qlbxmd.domain.ThanhToan;
import vn.qlbxmd.domain.VeXe;
import vn.qlbxmd.domain.Xe;
import vn.qlbxmd.repo.ChuyenXeRepository;
import vn.qlbxmd.repo.HoaDonRepository;
import vn.qlbxmd.repo.KhachHangRepository;
import vn.qlbxmd.repo.LoaiXeRepository;
import vn.qlbxmd.repo.NhanVienRepository;
import vn.qlbxmd.repo.ThanhToanRepository;
import vn.qlbxmd.repo.VeXeRepository;
import vn.qlbxmd.repo.XeRepository;

@Service
public class BookingService {

    private static final Set<String> PHUONG_THUC_HOP_LE =
            Set.of("Tiền mặt", "Chuyển khoản", "Thẻ ATM");

    private final ChuyenXeRepository chuyenXeRepository;
    private final XeRepository xeRepository;
    private final LoaiXeRepository loaiXeRepository;
    private final KhachHangRepository khachHangRepository;
    private final NhanVienRepository nhanVienRepository;
    private final HoaDonRepository hoaDonRepository;
    private final VeXeRepository veXeRepository;
    private final ThanhToanRepository thanhToanRepository;
    private final CurrentUserService currentUserService;

    public BookingService(ChuyenXeRepository chuyenXeRepository,
                          XeRepository xeRepository,
                          LoaiXeRepository loaiXeRepository,
                          KhachHangRepository khachHangRepository,
                          NhanVienRepository nhanVienRepository,
                          HoaDonRepository hoaDonRepository,
                          VeXeRepository veXeRepository,
                          ThanhToanRepository thanhToanRepository,
                          CurrentUserService currentUserService) {
        this.chuyenXeRepository = chuyenXeRepository;
        this.xeRepository = xeRepository;
        this.loaiXeRepository = loaiXeRepository;
        this.khachHangRepository = khachHangRepository;
        this.nhanVienRepository = nhanVienRepository;
        this.hoaDonRepository = hoaDonRepository;
        this.veXeRepository = veXeRepository;
        this.thanhToanRepository = thanhToanRepository;
        this.currentUserService = currentUserService;
    }

    @Transactional
    public DatVeResponse datVe(TaiKhoan actor, DatVeRequest request) {
        ChuyenXe chuyenXe = chuyenXeRepository.findById(request.maChuyen())
                .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
        LocalDateTime bayGio = LocalDateTime.now();
        if (!chuyenXe.getNgayGioKhoiHanh().isAfter(bayGio)) {
            throw ApiException.conflict("Chuyến xe đã khởi hành, không thể đặt vé.");
        }
        if (chuyenXe.getGiaVeThucTe() == null || chuyenXe.getGiaVeThucTe().signum() <= 0) {
            throw ApiException.conflict("Chuyến xe chưa có giá vé hợp lệ.");
        }

        List<Integer> choNgois = normalizeSeats(request.choNgois());
        Xe xe = xeRepository.findById(chuyenXe.getMaXe())
                .orElseThrow(() -> ApiException.notFound("Xe của chuyến không tồn tại."));
        LoaiXe loaiXe = loaiXeRepository.findById(xe.getMaLx())
                .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
        int gheCaoNhat = choNgois.stream().max(Integer::compareTo).orElse(0);
        if (gheCaoNhat > loaiXe.getSoCho()) {
            throw ApiException.badRequest("Ghế chọn vượt quá số chỗ của xe (" + loaiXe.getSoCho() + " chỗ).");
        }

        List<Integer> daDat = veXeRepository.findChoNgoiByMaChuyen(chuyenXe.getMaChuyen());
        List<Integer> trung = choNgois.stream().filter(daDat::contains).toList();
        if (!trung.isEmpty()) {
            throw ApiException.conflict("Ghế số " + joinSeats(trung) + " đã được đặt. Vui lòng chọn ghế khác.");
        }

        KhachHang khachHang = resolveKhachHang(actor, request);

        boolean thanhToanNgay = !Validators.isBlank(request.phuongThuc())
                && !"Thanh toán sau".equalsIgnoreCase(request.phuongThuc().trim());
        String phuongThuc = thanhToanNgay ? request.phuongThuc().trim() : null;
        if (thanhToanNgay && !PHUONG_THUC_HOP_LE.contains(phuongThuc)) {
            throw ApiException.badRequest("Phương thức thanh toán không hợp lệ.");
        }

        BigDecimal tongTien = chuyenXe.getGiaVeThucTe().multiply(BigDecimal.valueOf(choNgois.size()));

        HoaDon hoaDon = new HoaDon();
        hoaDon.setMaKh(khachHang.getMaKh());
        hoaDon.setMaNv(maNhanVien(actor));
        hoaDon.setNgayLap(bayGio);
        hoaDon.setTongTien(tongTien);
        hoaDon.setTrangThai(thanhToanNgay);
        HoaDon saved = hoaDonRepository.save(hoaDon);

        List<VeXe> ves = new ArrayList<>();
        for (Integer choNgoi : choNgois) {
            VeXe ve = new VeXe();
            ve.setMaChuyen(chuyenXe.getMaChuyen());
            ve.setMaHd(saved.getMaHd());
            ve.setChoNgoi(choNgoi);
            ve.setGiaVe(chuyenXe.getGiaVeThucTe());
            ves.add(ve);
        }
        veXeRepository.saveAll(ves);

        if (thanhToanNgay) {
            ThanhToan thanhToan = new ThanhToan();
            thanhToan.setMaHd(saved.getMaHd());
            thanhToan.setTongTien(tongTien);
            thanhToan.setNgayTt(bayGio);
            thanhToan.setPhuongThuc(phuongThuc);
            thanhToanRepository.save(thanhToan);
        }

        return new DatVeResponse(saved.getMaHd(), tongTien, choNgois.size(),
                thanhToanNgay ? phuongThuc : "Thanh toán sau", thanhToanNgay);
    }

    private List<Integer> normalizeSeats(List<Integer> choNgois) {
        if (choNgois == null || choNgois.isEmpty()) {
            throw ApiException.badRequest("Vui lòng chọn ít nhất một ghế.");
        }
        List<Integer> seats = choNgois.stream()
                .filter(Objects::nonNull)
                .sorted()
                .toList();
        if (seats.size() != choNgois.size()) {
            throw ApiException.badRequest("Danh sách ghế không hợp lệ.");
        }
        if (seats.get(0) < 1) {
            throw ApiException.badRequest("Số ghế phải bắt đầu từ 1.");
        }
        if (seats.stream().distinct().count() != seats.size()) {
            throw ApiException.badRequest("Danh sách ghế bị trùng.");
        }
        return seats;
    }

    private KhachHang resolveKhachHang(TaiKhoan actor, DatVeRequest request) {
        if (currentUserService.isCustomer(actor)) {
            KhachHang khachHang = khachHangRepository.findByMaTk(actor.getMaTk()).orElse(null);
            if (khachHang != null) {
                return khachHang;
            }
            Validators.requireText(request.tenKhachHang(), "Vui lòng nhập họ tên để hoàn tất hồ sơ.");
            Validators.requirePhone(request.sdt());
            KhachHang trungSdt = khachHangRepository.findBySdt(request.sdt().trim()).orElse(null);
            if (trungSdt != null) {
                if (trungSdt.getMaTk() != null && !trungSdt.getMaTk().equals(actor.getMaTk())) {
                    throw ApiException.conflict("Số điện thoại này đã được gắn với tài khoản khác.");
                }
                trungSdt.setMaTk(actor.getMaTk());
                return khachHangRepository.save(trungSdt);
            }
            return khachHangRepository.save(newKhachHang(request, actor.getMaTk()));
        }

        Validators.requireText(request.tenKhachHang(), "Vui lòng nhập tên khách hàng.");
        Validators.requirePhone(request.sdt());
        if (request.namSinh() == null) {
            throw ApiException.badRequest("Vui lòng nhập ngày sinh khách hàng.");
        }
        return khachHangRepository.findBySdt(request.sdt().trim())
                .orElseGet(() -> khachHangRepository.save(newKhachHang(request, null)));
    }

    private KhachHang newKhachHang(DatVeRequest request, Integer maTk) {
        if (request.namSinh() == null || request.namSinh().isAfter(LocalDate.now())) {
            throw ApiException.badRequest("Ngày sinh khách hàng không hợp lệ.");
        }
        KhachHang khachHang = new KhachHang();
        khachHang.setTenKh(request.tenKhachHang().trim());
        khachHang.setSdt(request.sdt().trim());
        khachHang.setNamSinh(request.namSinh());
        khachHang.setGioiTinh(Boolean.TRUE.equals(request.gioiTinh()));
        khachHang.setMaTk(maTk);
        return khachHang;
    }

    private Integer maNhanVien(TaiKhoan actor) {
        return nhanVienRepository.findByMaTk(actor.getMaTk())
                .map(NhanVien::getMaNv)
                .orElse(null);
    }

    private String joinSeats(List<Integer> seats) {
        return String.join(", ", seats.stream().map(String::valueOf).toList());
    }

    public record DatVeRequest(
            @NotNull(message = "Vui lòng chọn chuyến xe.") Integer maChuyen,
            List<Integer> choNgois,
            String tenKhachHang,
            String sdt,
            LocalDate namSinh,
            Boolean gioiTinh,
            String phuongThuc) {
    }

    public record DatVeResponse(
            Integer maHd,
            BigDecimal tongTien,
            Integer soVe,
            String phuongThuc,
            Boolean trangThai) {
    }
}
