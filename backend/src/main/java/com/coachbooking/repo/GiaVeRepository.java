package com.coachbooking.repo;

import com.coachbooking.domain.GiaVe;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface GiaVeRepository extends JpaRepository<GiaVe, Integer> {

  Optional<GiaVe> findByMaNxAndMaLxAndMaTuyen(Integer maNx, Integer maLx, Integer maTuyen);

  boolean existsByMaNxAndMaLxAndMaTuyen(Integer maNx, Integer maLx, Integer maTuyen);

  boolean existsByMaNx(Integer maNx);

  boolean existsByMaLx(Integer maLx);

  boolean existsByMaTuyen(Integer maTuyen);
}
