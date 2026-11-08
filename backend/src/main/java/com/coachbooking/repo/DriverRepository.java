package com.coachbooking.repo;

import com.coachbooking.domain.Driver;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface DriverRepository extends JpaRepository<Driver, Integer> {
  Optional<Driver> findByCitizenId(String citizenId);

  Optional<Driver> findByPhone(String phone);

  boolean existsByCitizenId(String citizenId);

  boolean existsByPhone(String phone);
}
