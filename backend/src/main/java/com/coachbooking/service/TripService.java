package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.common.Validators;
import com.coachbooking.domain.ChuyenXe;
import com.coachbooking.domain.GiaVe;
import com.coachbooking.domain.LoaiXe;
import com.coachbooking.domain.TaiXe;
import com.coachbooking.domain.TuyenDuong;
import com.coachbooking.domain.Xe;
import com.coachbooking.repo.ChuyenXeRepository;
import com.coachbooking.repo.GiaVeRepository;
import com.coachbooking.repo.LoaiXeRepository;
import com.coachbooking.repo.TaiXeRepository;
import com.coachbooking.repo.TuyenDuongRepository;
import com.coachbooking.repo.VeXeRepository;
import com.coachbooking.repo.XeRepository;
import jakarta.validation.constraints.NotNull;
import java.math.BigDecimal;
import java.time.Duration;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class TripService {

  private final ChuyenXeRepository chuyenXeRepository;
  private final XeRepository xeRepository;
  private final LoaiXeRepository loaiXeRepository;
  private final TuyenDuongRepository tuyenDuongRepository;
  private final TaiXeRepository taiXeRepository;
  private final GiaVeRepository giaVeRepository;
  private final VeXeRepository veXeRepository;

  public TripService(
      ChuyenXeRepository chuyenXeRepository,
      XeRepository xeRepository,
      LoaiXeRepository loaiXeRepository,
      TuyenDuongRepository tuyenDuongRepository,
      TaiXeRepository taiXeRepository,
      GiaVeRepository giaVeRepository,
      VeXeRepository veXeRepository) {
    this.chuyenXeRepository = chuyenXeRepository;
    this.xeRepository = xeRepository;
    this.loaiXeRepository = loaiXeRepository;
    this.tuyenDuongRepository = tuyenDuongRepository;
    this.taiXeRepository = taiXeRepository;
    this.giaVeRepository = giaVeRepository;
    this.veXeRepository = veXeRepository;
  }

  public List<ChuyenXeRepository.TripRow> listAll() {
    return chuyenXeRepository.findAllRows();
  }

  public List<ChuyenXeRepository.TripRow> search(
      String diemDi, String diemDen, LocalDate ngay, Integer tuGio, Integer denGio) {
    LocalDate date = ngay == null ? LocalDate.now() : ngay;
    if (tuGio != null && denGio != null && tuGio > denGio) {
      throw ApiException.badRequest("Khung giờ không hợp lệ.");
    }
    LocalDateTime start = date.atStartOfDay();
    LocalDateTime end = date.plusDays(1).atStartOfDay();
    LocalDateTime now = LocalDateTime.now();
    return chuyenXeRepository.search(blankToNull(diemDi), blankToNull(diemDen), start, end).stream()
        .filter(row -> row.getGiaVeThucTe() != null && row.getGiaVeThucTe().signum() > 0)
        .filter(row -> row.getNgayGioKhoiHanh().isAfter(now))
        .filter(row -> tuGio == null || row.getNgayGioKhoiHanh().getHour() >= tuGio)
        .filter(row -> denGio == null || row.getNgayGioKhoiHanh().getHour() <= denGio)
        .toList();
  }

  public SeatMapResponse seatMap(Integer maChuyen) {
    ChuyenXe chuyenXe = findTrip(maChuyen);
    Xe xe =
        xeRepository
            .findById(chuyenXe.getMaXe())
            .orElseThrow(() -> ApiException.notFound("Xe của chuyến không tồn tại."));
    LoaiXe loaiXe =
        loaiXeRepository
            .findById(xe.getMaLx())
            .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
    TuyenDuong tuyen =
        tuyenDuongRepository
            .findById(chuyenXe.getMaTuyen())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    return new SeatMapResponse(
        chuyenXe.getMaChuyen(),
        loaiXe.getTenLx(),
        loaiXe.getSoCho(),
        chuyenXe.getGiaVeThucTe(),
        veXeRepository.findChoNgoiByMaChuyen(maChuyen),
        tuyen.getDiemDi(),
        tuyen.getDiemDen(),
        chuyenXe.getNgayGioKhoiHanh());
  }

  @Transactional
  public ChuyenXe create(TripRequest request) {
    Xe xe =
        xeRepository
            .findById(request.maXe())
            .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    TuyenDuong tuyen =
        tuyenDuongRepository
            .findById(request.maTuyen())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    TaiXe taiXe =
        taiXeRepository
            .findById(request.maTx())
            .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
    LocalDateTime khoiHanh = requireDeparture(request.ngayGioKhoiHanh());
    checkConflict(xe.getMaXe(), khoiHanh, null);

    GiaVe giaVe =
        giaVeRepository
            .findByMaNxAndMaLxAndMaTuyen(xe.getMaNx(), xe.getMaLx(), tuyen.getMaTuyen())
            .orElseThrow(
                () ->
                    ApiException.conflict(
                        "Chưa có bảng giá cho nhà xe, loại xe và tuyến đường này."));

    ChuyenXe chuyenXe = new ChuyenXe();
    apply(chuyenXe, xe, tuyen, taiXe, khoiHanh, giaVe.getGiaVe());
    return chuyenXeRepository.save(chuyenXe);
  }

  @Transactional
  public ChuyenXe update(Integer maChuyen, TripRequest request) {
    ChuyenXe chuyenXe = findTrip(maChuyen);
    Xe xe =
        xeRepository
            .findById(request.maXe())
            .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    TuyenDuong tuyen =
        tuyenDuongRepository
            .findById(request.maTuyen())
            .orElseThrow(() -> ApiException.notFound("Tuyến đường không tồn tại."));
    TaiXe taiXe =
        taiXeRepository
            .findById(request.maTx())
            .orElseThrow(() -> ApiException.notFound("Tài xế không tồn tại."));
    LocalDateTime khoiHanh = requireDeparture(request.ngayGioKhoiHanh());
    if (veXeRepository.existsByMaChuyen(maChuyen)) {
      boolean doiThongTinChuyen =
          !chuyenXe.getMaXe().equals(xe.getMaXe())
              || !chuyenXe.getMaTuyen().equals(tuyen.getMaTuyen())
              || !chuyenXe.getNgayGioKhoiHanh().equals(khoiHanh);
      if (doiThongTinChuyen) {
        throw ApiException.conflict(
            "Chuyến đã bán vé, chỉ có thể đổi tài xế. Không thể đổi xe, tuyến hoặc giờ khởi hành.");
      }
    }
    checkConflict(xe.getMaXe(), khoiHanh, maChuyen);

    GiaVe giaVe =
        giaVeRepository
            .findByMaNxAndMaLxAndMaTuyen(xe.getMaNx(), xe.getMaLx(), tuyen.getMaTuyen())
            .orElseThrow(
                () ->
                    ApiException.conflict(
                        "Chưa có bảng giá cho nhà xe, loại xe và tuyến đường này."));

    apply(chuyenXe, xe, tuyen, taiXe, khoiHanh, giaVe.getGiaVe());
    return chuyenXeRepository.save(chuyenXe);
  }

  @Transactional
  public Map<String, String> delete(Integer maChuyen) {
    ChuyenXe chuyenXe = findTrip(maChuyen);
    if (veXeRepository.existsByMaChuyen(maChuyen)) {
      throw ApiException.conflict("Không thể xóa chuyến xe đã có vé.");
    }
    chuyenXeRepository.delete(chuyenXe);
    return Map.of("message", "Xóa chuyến xe thành công.");
  }

  public ChuyenXe findTrip(Integer maChuyen) {
    return chuyenXeRepository
        .findById(maChuyen)
        .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
  }

  private void apply(
      ChuyenXe chuyenXe,
      Xe xe,
      TuyenDuong tuyen,
      TaiXe taiXe,
      LocalDateTime khoiHanh,
      BigDecimal giaVe) {
    chuyenXe.setMaXe(xe.getMaXe());
    chuyenXe.setMaTuyen(tuyen.getMaTuyen());
    chuyenXe.setMaTx(taiXe.getMaTx());
    chuyenXe.setNgayGioKhoiHanh(khoiHanh);
    chuyenXe.setNgayGioDenDuKien(khoiHanh.plus(durationOf(tuyen)));
    chuyenXe.setGiaVeThucTe(giaVe);
  }

  private LocalDateTime requireDeparture(LocalDateTime khoiHanh) {
    if (khoiHanh == null) {
      throw ApiException.badRequest("Vui lòng chọn ngày giờ khởi hành.");
    }
    if (!khoiHanh.isAfter(LocalDateTime.now())) {
      throw ApiException.badRequest("Ngày giờ khởi hành phải sau thời điểm hiện tại.");
    }
    return khoiHanh;
  }

  private void checkConflict(Integer maXe, LocalDateTime khoiHanh, Integer excludeId) {
    LocalDateTime start = khoiHanh.withSecond(0).withNano(0);
    LocalDateTime end = start.plusMinutes(1);
    boolean conflict =
        excludeId == null
            ? chuyenXeRepository.existsByMaXeAndNgayGioKhoiHanhBetween(maXe, start, end)
            : chuyenXeRepository.existsByMaXeAndNgayGioKhoiHanhBetweenAndMaChuyenNot(
                maXe, start, end, excludeId);
    if (conflict) {
      throw ApiException.conflict(
          "Xe này đã được lên chuyến vào cùng thời điểm. Vui lòng chọn xe hoặc giờ khác.");
    }
  }

  private Duration durationOf(TuyenDuong tuyen) {
    return tuyen.getThoiGianDuKien() == null
        ? Duration.ofHours(6)
        : Duration.ofSeconds(tuyen.getThoiGianDuKien().toSecondOfDay());
  }

  private String blankToNull(String value) {
    return Validators.isBlank(value) ? null : value.trim();
  }

  public record TripRequest(
      @NotNull(message = "Vui lòng chọn xe.") Integer maXe,
      @NotNull(message = "Vui lòng chọn tuyến đường.") Integer maTuyen,
      @NotNull(message = "Vui lòng chọn tài xế.") Integer maTx,
      @NotNull(message = "Vui lòng chọn ngày giờ khởi hành.") LocalDateTime ngayGioKhoiHanh) {}

  public record SeatMapResponse(
      Integer maChuyen,
      String tenLoaiXe,
      Integer soCho,
      BigDecimal giaVe,
      List<Integer> gheDaDat,
      String diemDi,
      String diemDen,
      LocalDateTime ngayGioKhoiHanh) {}
}
