package com.coachbooking.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import com.coachbooking.domain.TuyenDuong;

public interface TuyenDuongRepository extends JpaRepository<TuyenDuong, Integer> {

    boolean existsByDiemDiAndDiemDen(String diemDi, String diemDen);

    Optional<TuyenDuong> findByDiemDiAndDiemDen(String diemDi, String diemDen);
}
