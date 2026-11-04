package com.coachbooking.repo;

import com.coachbooking.domain.Quyen;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface QuyenRepository extends JpaRepository<Quyen, Integer> {

  Optional<Quyen> findByTenQuyen(String tenQuyen);
}
