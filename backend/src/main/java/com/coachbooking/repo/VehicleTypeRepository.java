package com.coachbooking.repo;

import com.coachbooking.domain.VehicleType;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface VehicleTypeRepository extends JpaRepository<VehicleType, Integer> {
  Optional<VehicleType> findByTypeName(String typeName);

  boolean existsByTypeName(String typeName);
}
