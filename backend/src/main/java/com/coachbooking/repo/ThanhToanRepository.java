package com.coachbooking.repo;

import com.coachbooking.domain.ThanhToan;
import java.util.Collection;
import java.util.List;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ThanhToanRepository extends JpaRepository<ThanhToan, Integer> {

  Optional<ThanhToan> findByMaHd(Integer maHd);

  List<ThanhToan> findByMaHdIn(Collection<Integer> maHds);
}
