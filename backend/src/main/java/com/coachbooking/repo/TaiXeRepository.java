package com.coachbooking.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import com.coachbooking.domain.TaiXe;

public interface TaiXeRepository extends JpaRepository<TaiXe, Integer> {

    boolean existsByCccd(String cccd);

    boolean existsBySdt(String sdt);
}
