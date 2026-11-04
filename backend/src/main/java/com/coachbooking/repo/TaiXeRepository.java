package com.coachbooking.repo;

import com.coachbooking.domain.TaiXe;
import org.springframework.data.jpa.repository.JpaRepository;

public interface TaiXeRepository extends JpaRepository<TaiXe, Integer> {

  boolean existsByCccd(String cccd);

  boolean existsBySdt(String sdt);
}
