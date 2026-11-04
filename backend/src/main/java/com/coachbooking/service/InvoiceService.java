package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.ChuyenXe;
import com.coachbooking.domain.HoaDon;
import com.coachbooking.domain.KhachHang;
import com.coachbooking.domain.NhanVien;
import com.coachbooking.domain.TaiKhoan;
import com.coachbooking.domain.ThanhToan;
import com.coachbooking.domain.TuyenDuong;
import com.coachbooking.domain.VeXe;
import com.coachbooking.repo.ChuyenXeRepository;
import com.coachbooking.repo.HoaDonRepository;
import com.coachbooking.repo.KhachHangRepository;
import com.coachbooking.repo.NhanVienRepository;
import com.coachbooking.repo.ThanhToanRepository;
import com.coachbooking.repo.TuyenDuongRepository;
import com.coachbooking.repo.VeXeRepository;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.function.Function;
import java.util.stream.Collectors;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class InvoiceService {

  private static final Set<String> PHUONG_THUC_HOP_LE =
      Set.of("Tiền mặt", "Chuyển khoản", "Thẻ ATM");

  private final HoaDonRepository hoaDonRepository;
  private final VeXeRepository veXeRepository;
  private final ThanhToanRepository thanhToanRepository;
  private final KhachHangRepository khachHangRepository;
  private final NhanVienRepository nhanVienRepository;
  private final ChuyenXeRepository chuyenXeRepository;
  private final TuyenDuongRepository tuyenDuongRepository;
  private final CurrentUserService currentUserService;

  public InvoiceService(
      HoaDonRepository hoaDonRepository,
      VeXeRepository veXeRepository,
      ThanhToanRepository thanhToanRepository,
      KhachHangRepository khachHangRepository,
      NhanVienRepository nhanVienRepository,
      ChuyenXeRepository chuyenXeRepository,
      TuyenDuongRepository tuyenDuongRepository,
      CurrentUserService currentUserService) {
    this.hoaDonRepository = hoaDonRepository;
    this.veXeRepository = veXeRepository;
    this.thanhToanRepository = thanhToanRepository;
    this.khachHangRepository = khachHangRepository;
    this.nhanVienRepository = nhanVienRepository;
    this.chuyenXeRepository = chuyenXeRepository;
    this.tuyenDuongRepository = tuyenDuongRepository;
    this.currentUserService = currentUserService;
  }

  public List<HoaDonRow> list(LocalDate tu, LocalDate den, Boolean trangThai) {
    List<HoaDon> hoaDons =
        hoaDonRepository.findAllByOrderByNgayLapDesc().stream()
            .filter(hoaDon -> trangThai == null || trangThai.equals(hoaDon.getTrangThai()))
            .filter(hoaDon -> tu == null || !hoaDon.getNgayLap().toLocalDate().isBefore(tu))
            .filter(hoaDon -> den == null || !hoaDon.getNgayLap().toLocalDate().isAfter(den))
            .toList();
    return toRows(hoaDons);
  }

  public List<HoaDonRow> myInvoices(TaiKhoan taiKhoan) {
    KhachHang khachHang = khachHangRepository.findByMaTk(taiKhoan.getMaTk()).orElse(null);
    if (khachHang == null) {
      return List.of();
    }
    return toRows(hoaDonRepository.findByMaKhOrderByNgayLapDesc(khachHang.getMaKh()));
  }

  public HoaDonDetail detail(Integer maHd, TaiKhoan taiKhoan) {
    HoaDon hoaDon = findInvoice(maHd);
    requireOwnership(taiKhoan, hoaDon);

    KhachHang khachHang =
        hoaDon.getMaKh() == null
            ? null
            : khachHangRepository.findById(hoaDon.getMaKh()).orElse(null);
    NhanVien nhanVien =
        hoaDon.getMaNv() == null
            ? null
            : nhanVienRepository.findById(hoaDon.getMaNv()).orElse(null);

    List<VeXe> ves = veXeRepository.findByMaHd(maHd);
    Map<Integer, ChuyenXe> chuyenMap = loadTrips(ves);
    Map<Integer, TuyenDuong> tuyenMap = loadRoutes(chuyenMap.values());
    List<VeRow> veRows =
        ves.stream().map(ve -> toVeRow(ve, chuyenMap.get(ve.getMaChuyen()), tuyenMap)).toList();
    ThanhToan thanhToan = thanhToanRepository.findByMaHd(maHd).orElse(null);

    return new HoaDonDetail(
        hoaDon.getMaHd(),
        khachHang == null ? null : khachHang.getTenKh(),
        khachHang == null ? null : khachHang.getSdt(),
        nhanVien == null ? null : nhanVien.getTenNv(),
        hoaDon.getNgayLap(),
        hoaDon.getTongTien(),
        hoaDon.getTrangThai(),
        veRows,
        thanhToan == null
            ? null
            : new ThanhToanRow(
                thanhToan.getMaTt(),
                thanhToan.getTongTien(),
                thanhToan.getNgayTt(),
                thanhToan.getPhuongThuc()));
  }

  @Transactional
  public Map<String, String> pay(Integer maHd, String phuongThuc, TaiKhoan taiKhoan) {
    if (Validators.isBlank(phuongThuc) || !PHUONG_THUC_HOP_LE.contains(phuongThuc.trim())) {
      throw ApiException.badRequest("Phương thức thanh toán không hợp lệ.");
    }
    HoaDon hoaDon = findInvoice(maHd);
    requireOwnership(taiKhoan, hoaDon);
    if (Boolean.TRUE.equals(hoaDon.getTrangThai())) {
      throw ApiException.conflict("Hóa đơn đã được thanh toán.");
    }
    List<VeXe> ves = veXeRepository.findByMaHd(maHd);
    BigDecimal tongTien = ves.stream().map(VeXe::getGiaVe).reduce(BigDecimal.ZERO, BigDecimal::add);

    ThanhToan thanhToan = new ThanhToan();
    thanhToan.setMaHd(maHd);
    thanhToan.setTongTien(tongTien);
    thanhToan.setNgayTt(LocalDateTime.now());
    thanhToan.setPhuongThuc(phuongThuc.trim());
    thanhToanRepository.save(thanhToan);

    hoaDon.setTongTien(tongTien);
    hoaDon.setTrangThai(true);
    hoaDonRepository.save(hoaDon);
    return Map.of("message", "Thanh toán hóa đơn thành công.");
  }

  @Transactional
  public Map<String, String> cancel(Integer maHd, TaiKhoan taiKhoan) {
    HoaDon hoaDon = findInvoice(maHd);
    requireOwnership(taiKhoan, hoaDon);
    if (Boolean.TRUE.equals(hoaDon.getTrangThai())) {
      throw ApiException.conflict("Không thể hủy hóa đơn đã thanh toán.");
    }
    veXeRepository.deleteByMaHd(maHd);
    hoaDonRepository.delete(hoaDon);
    return Map.of("message", "Hủy hóa đơn thành công.");
  }

  @Transactional
  public Map<String, String> cancelTicket(Integer maVe, TaiKhoan taiKhoan) {
    VeXe ve =
        veXeRepository.findById(maVe).orElseThrow(() -> ApiException.notFound("Vé không tồn tại."));
    HoaDon hoaDon = findInvoice(ve.getMaHd());
    requireOwnership(taiKhoan, hoaDon);
    if (Boolean.TRUE.equals(hoaDon.getTrangThai())) {
      throw ApiException.conflict("Không thể hủy vé của hóa đơn đã thanh toán.");
    }
    ChuyenXe chuyenXe =
        chuyenXeRepository
            .findById(ve.getMaChuyen())
            .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    if (!chuyenXe.getNgayGioKhoiHanh().isAfter(LocalDateTime.now())) {
      throw ApiException.conflict("Chuyến xe đã khởi hành, không thể hủy vé.");
    }

    veXeRepository.delete(ve);
    veXeRepository.flush();

    List<VeXe> conLai = veXeRepository.findByMaHd(hoaDon.getMaHd());
    if (conLai.isEmpty()) {
      hoaDonRepository.delete(hoaDon);
      return Map.of("message", "Đã hủy vé cuối cùng và xóa hóa đơn.");
    }
    BigDecimal tongTien =
        conLai.stream().map(VeXe::getGiaVe).reduce(BigDecimal.ZERO, BigDecimal::add);
    hoaDon.setTongTien(tongTien);
    hoaDonRepository.save(hoaDon);
    return Map.of("message", "Hủy vé thành công.");
  }

  private List<HoaDonRow> toRows(List<HoaDon> hoaDons) {
    if (hoaDons.isEmpty()) {
      return List.of();
    }
    List<Integer> ids = hoaDons.stream().map(HoaDon::getMaHd).toList();
    Map<Integer, Long> soVe =
        veXeRepository.findByMaHdIn(ids).stream()
            .collect(Collectors.groupingBy(VeXe::getMaHd, Collectors.counting()));
    Map<Integer, ThanhToan> thanhToan =
        thanhToanRepository.findByMaHdIn(ids).stream()
            .collect(Collectors.toMap(ThanhToan::getMaHd, Function.identity()));
    Map<Integer, String> tenKhachHang =
        loadNames(
            hoaDons.stream().map(HoaDon::getMaKh).filter(java.util.Objects::nonNull).toList(),
            khachHangRepository.findAllById(
                hoaDons.stream().map(HoaDon::getMaKh).filter(java.util.Objects::nonNull).toList()),
            KhachHang::getMaKh,
            KhachHang::getTenKh);
    Map<Integer, String> tenNhanVien =
        loadNames(
            hoaDons.stream().map(HoaDon::getMaNv).filter(java.util.Objects::nonNull).toList(),
            nhanVienRepository.findAllById(
                hoaDons.stream().map(HoaDon::getMaNv).filter(java.util.Objects::nonNull).toList()),
            NhanVien::getMaNv,
            NhanVien::getTenNv);

    return hoaDons.stream()
        .map(
            hoaDon ->
                new HoaDonRow(
                    hoaDon.getMaHd(),
                    hoaDon.getMaKh() == null ? null : tenKhachHang.get(hoaDon.getMaKh()),
                    hoaDon.getMaNv() == null ? null : tenNhanVien.get(hoaDon.getMaNv()),
                    hoaDon.getNgayLap(),
                    hoaDon.getTongTien(),
                    hoaDon.getTrangThai(),
                    soVe.getOrDefault(hoaDon.getMaHd(), 0L),
                    thanhToan.containsKey(hoaDon.getMaHd())
                        ? thanhToan.get(hoaDon.getMaHd()).getPhuongThuc()
                        : null))
        .toList();
  }

  private <T> Map<Integer, String> loadNames(
      List<Integer> ids,
      Collection<T> items,
      Function<T, Integer> idGetter,
      Function<T, String> nameGetter) {
    if (ids.isEmpty()) {
      return Map.of();
    }
    return items.stream().collect(Collectors.toMap(idGetter, nameGetter));
  }

  private Map<Integer, ChuyenXe> loadTrips(List<VeXe> ves) {
    List<Integer> ids = ves.stream().map(VeXe::getMaChuyen).distinct().toList();
    if (ids.isEmpty()) {
      return Map.of();
    }
    return chuyenXeRepository.findAllById(ids).stream()
        .collect(Collectors.toMap(ChuyenXe::getMaChuyen, Function.identity()));
  }

  private Map<Integer, TuyenDuong> loadRoutes(Collection<ChuyenXe> chuyenXes) {
    List<Integer> ids = chuyenXes.stream().map(ChuyenXe::getMaTuyen).distinct().toList();
    if (ids.isEmpty()) {
      return Map.of();
    }
    return tuyenDuongRepository.findAllById(ids).stream()
        .collect(Collectors.toMap(TuyenDuong::getMaTuyen, Function.identity()));
  }

  private VeRow toVeRow(VeXe ve, ChuyenXe chuyenXe, Map<Integer, TuyenDuong> tuyenMap) {
    TuyenDuong tuyen = chuyenXe == null ? null : tuyenMap.get(chuyenXe.getMaTuyen());
    return new VeRow(
        ve.getMaVe(),
        ve.getMaChuyen(),
        tuyen == null ? null : tuyen.getDiemDi(),
        tuyen == null ? null : tuyen.getDiemDen(),
        chuyenXe == null ? null : chuyenXe.getNgayGioKhoiHanh(),
        ve.getChoNgoi(),
        ve.getGiaVe());
  }

  private HoaDon findInvoice(Integer maHd) {
    return hoaDonRepository
        .findById(maHd)
        .orElseThrow(() -> ApiException.notFound("Hóa đơn không tồn tại."));
  }

  private void requireOwnership(TaiKhoan taiKhoan, HoaDon hoaDon) {
    if (!currentUserService.isCustomer(taiKhoan)) {
      return;
    }
    Integer maKh =
        khachHangRepository.findByMaTk(taiKhoan.getMaTk()).map(KhachHang::getMaKh).orElse(null);
    if (maKh == null || !maKh.equals(hoaDon.getMaKh())) {
      throw ApiException.forbidden("Bạn không có quyền thao tác trên hóa đơn này.");
    }
  }

  public record HoaDonRow(
      Integer maHd,
      String tenKhachHang,
      String tenNhanVien,
      LocalDateTime ngayLap,
      BigDecimal tongTien,
      Boolean trangThai,
      Long soVe,
      String phuongThuc) {}

  public record VeRow(
      Integer maVe,
      Integer maChuyen,
      String diemDi,
      String diemDen,
      LocalDateTime ngayGioKhoiHanh,
      Integer choNgoi,
      BigDecimal giaVe) {}

  public record ThanhToanRow(
      Integer maTt, BigDecimal tongTien, LocalDateTime ngayTt, String phuongThuc) {}

  public record HoaDonDetail(
      Integer maHd,
      String tenKhachHang,
      String sdtKhachHang,
      String tenNhanVien,
      LocalDateTime ngayLap,
      BigDecimal tongTien,
      Boolean trangThai,
      List<VeRow> veXes,
      ThanhToanRow thanhToan) {}
}
