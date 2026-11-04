package com.coachbooking.repo;

import com.coachbooking.domain.HoaDon;
import java.time.LocalDateTime;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;

public interface HoaDonRepository extends JpaRepository<HoaDon, Integer> {

  List<HoaDon> findAllByOrderByNgayLapDesc();

  List<HoaDon> findByMaKhOrderByNgayLapDesc(Integer maKh);

  List<HoaDon> findByMaNvOrderByNgayLapDesc(Integer maNv);

  List<HoaDon> findByTrangThaiTrueAndNgayLapGreaterThanEqualAndNgayLapLessThan(
      LocalDateTime tu, LocalDateTime den);

  boolean existsByMaKh(Integer maKh);

  boolean existsByMaNv(Integer maNv);
}
