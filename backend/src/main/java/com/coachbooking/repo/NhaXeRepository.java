package com.coachbooking.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import com.coachbooking.domain.NhaXe;

public interface NhaXeRepository extends JpaRepository<NhaXe, Integer> {

    boolean existsBySdt(String sdt);

    boolean existsByEmail(String email);
}
