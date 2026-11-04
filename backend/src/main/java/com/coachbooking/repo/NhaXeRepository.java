package com.coachbooking.repo;

import com.coachbooking.domain.NhaXe;
import org.springframework.data.jpa.repository.JpaRepository;

public interface NhaXeRepository extends JpaRepository<NhaXe, Integer> {

  boolean existsBySdt(String sdt);

  boolean existsByEmail(String email);
}
