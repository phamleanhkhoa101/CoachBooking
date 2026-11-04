package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.ChuyenXe;
import com.coachbooking.domain.HoaDon;
import com.coachbooking.domain.TuyenDuong;
import com.coachbooking.domain.VeXe;
import com.coachbooking.repo.ChuyenXeRepository;
import com.coachbooking.repo.HoaDonRepository;
import com.coachbooking.repo.TuyenDuongRepository;
import com.coachbooking.repo.VeXeRepository;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;
import java.util.stream.Collectors;
import org.springframework.stereotype.Service;

@Service
public class StatisticService {

  private final HoaDonRepository hoaDonRepository;
  private final VeXeRepository veXeRepository;
  private final ChuyenXeRepository chuyenXeRepository;
  private final TuyenDuongRepository tuyenDuongRepository;

  public StatisticService(
      HoaDonRepository hoaDonRepository,
      VeXeRepository veXeRepository,
      ChuyenXeRepository chuyenXeRepository,
      TuyenDuongRepository tuyenDuongRepository) {
    this.hoaDonRepository = hoaDonRepository;
    this.veXeRepository = veXeRepository;
    this.chuyenXeRepository = chuyenXeRepository;
    this.tuyenDuongRepository = tuyenDuongRepository;
  }

  public ThongKeResponse thongKe(LocalDate tu, LocalDate den) {
    LocalDate denThucTe = den == null ? LocalDate.now() : den;
    LocalDate tuThucTe = tu == null ? denThucTe.minusDays(29) : tu;
    if (tuThucTe.isAfter(denThucTe)) {
      throw ApiException.badRequest("Ngày bắt đầu phải trước ngày kết thúc.");
    }

    List<HoaDon> hoaDons =
        hoaDonRepository.findByTrangThaiTrueAndNgayLapGreaterThanEqualAndNgayLapLessThan(
            tuThucTe.atStartOfDay(), denThucTe.plusDays(1).atStartOfDay());

    long soKhachHang =
        hoaDons.stream().map(HoaDon::getMaKh).filter(java.util.Objects::nonNull).distinct().count();
    BigDecimal tongDoanhThu =
        hoaDons.stream().map(HoaDon::getTongTien).reduce(BigDecimal.ZERO, BigDecimal::add);
    long soChuyenXe =
        chuyenXeRepository.countByNgayGioKhoiHanhGreaterThanEqualAndNgayGioKhoiHanhLessThan(
            tuThucTe.atStartOfDay(), denThucTe.plusDays(1).atStartOfDay());

    List<Integer> hoaDonIds = hoaDons.stream().map(HoaDon::getMaHd).toList();
    List<VeXe> ves = hoaDonIds.isEmpty() ? List.of() : veXeRepository.findByMaHdIn(hoaDonIds);

    Map<Integer, LocalDate> ngayTheoHoaDon =
        hoaDons.stream()
            .collect(
                Collectors.toMap(HoaDon::getMaHd, hoaDon -> hoaDon.getNgayLap().toLocalDate()));

    Map<LocalDate, BigDecimal> doanhThuTheoNgay = new LinkedHashMap<>();
    Map<LocalDate, Long> soVeTheoNgay = new LinkedHashMap<>();
    for (LocalDate ngay = tuThucTe; !ngay.isAfter(denThucTe); ngay = ngay.plusDays(1)) {
      doanhThuTheoNgay.put(ngay, BigDecimal.ZERO);
      soVeTheoNgay.put(ngay, 0L);
    }
    for (HoaDon hoaDon : hoaDons) {
      LocalDate ngay = hoaDon.getNgayLap().toLocalDate();
      doanhThuTheoNgay.computeIfPresent(ngay, (key, value) -> value.add(hoaDon.getTongTien()));
    }
    for (VeXe ve : ves) {
      LocalDate ngay = ngayTheoHoaDon.get(ve.getMaHd());
      if (ngay != null) {
        soVeTheoNgay.computeIfPresent(ngay, (key, value) -> value + 1);
      }
    }

    List<DiemDoanhThu> doanhThuList = new ArrayList<>();
    doanhThuTheoNgay.forEach(
        (ngay, doanhThu) ->
            doanhThuList.add(
                new DiemDoanhThu(ngay.toString(), doanhThu, soVeTheoNgay.getOrDefault(ngay, 0L))));

    return new ThongKeResponse(
        soKhachHang, soChuyenXe, tongDoanhThu, ves.size(), doanhThuList, tuyenPhoBien(ves));
  }

  private List<TuyenPhoBien> tuyenPhoBien(List<VeXe> ves) {
    if (ves.isEmpty()) {
      return List.of();
    }
    Map<Integer, Long> soVeTheoChuyen =
        ves.stream().collect(Collectors.groupingBy(VeXe::getMaChuyen, Collectors.counting()));
    Map<Integer, ChuyenXe> chuyenMap =
        chuyenXeRepository.findAllById(soVeTheoChuyen.keySet()).stream()
            .collect(Collectors.toMap(ChuyenXe::getMaChuyen, Function.identity()));
    Map<Integer, TuyenDuong> tuyenMap =
        tuyenDuongRepository
            .findAllById(chuyenMap.values().stream().map(ChuyenXe::getMaTuyen).distinct().toList())
            .stream()
            .collect(Collectors.toMap(TuyenDuong::getMaTuyen, Function.identity()));

    Map<String, Long> soVeTheoTuyen = new HashMap<>();
    soVeTheoChuyen.forEach(
        (maChuyen, soVe) -> {
          ChuyenXe chuyenXe = chuyenMap.get(maChuyen);
          if (chuyenXe == null) {
            return;
          }
          TuyenDuong tuyen = tuyenMap.get(chuyenXe.getMaTuyen());
          if (tuyen == null) {
            return;
          }
          String tenTuyen = tuyen.getDiemDi() + " → " + tuyen.getDiemDen();
          soVeTheoTuyen.merge(tenTuyen, soVe, Long::sum);
        });

    return soVeTheoTuyen.entrySet().stream()
        .sorted(Map.Entry.<String, Long>comparingByValue().reversed())
        .limit(10)
        .map(entry -> new TuyenPhoBien(entry.getKey(), entry.getValue()))
        .toList();
  }

  public record DiemDoanhThu(String ngay, BigDecimal doanhThu, Long soVe) {}

  public record TuyenPhoBien(String tenTuyen, Long soVe) {}

  public record ThongKeResponse(
      long soKhachHang,
      long soChuyenXe,
      BigDecimal tongDoanhThu,
      long soVe,
      List<DiemDoanhThu> doanhThuTheoNgay,
      List<TuyenPhoBien> tuyenPhoBien) {}
}
