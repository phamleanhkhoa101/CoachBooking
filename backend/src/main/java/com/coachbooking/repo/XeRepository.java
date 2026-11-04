package com.coachbooking.repo;

import com.coachbooking.domain.Xe;
import org.springframework.data.jpa.repository.JpaRepository;

public interface XeRepository extends JpaRepository<Xe, Integer> {

  boolean existsByMaNx(Integer maNx);

  boolean existsByMaLx(Integer maLx);

  boolean existsByBienSoXe(String bienSoXe);
}
