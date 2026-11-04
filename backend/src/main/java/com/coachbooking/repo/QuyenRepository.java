package com.coachbooking.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import com.coachbooking.domain.Quyen;

public interface QuyenRepository extends JpaRepository<Quyen, Integer> {

    Optional<Quyen> findByTenQuyen(String tenQuyen);
}
