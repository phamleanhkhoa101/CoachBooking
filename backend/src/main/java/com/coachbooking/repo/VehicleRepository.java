package com.coachbooking.repo;

import com.coachbooking.domain.Vehicle;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface VehicleRepository extends JpaRepository<Vehicle, Integer> {
  Optional<Vehicle> findByLicensePlate(String licensePlate);

  boolean existsByLicensePlate(String licensePlate);

  boolean existsByCompanyId(Integer companyId);

  boolean existsByTypeId(Integer typeId);
}
