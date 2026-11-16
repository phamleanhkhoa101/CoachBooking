package com.coachbooking.repo;

import com.coachbooking.domain.Vehicle;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface VehicleRepository extends JpaRepository<Vehicle, Integer> {
  @org.springframework.data.jpa.repository.Lock(jakarta.persistence.LockModeType.PESSIMISTIC_WRITE)
  @org.springframework.data.jpa.repository.Query("select e from Vehicle e where e.vehicleId = :id")
  Optional<Vehicle> lockById(@org.springframework.data.repository.query.Param("id") Integer id);

  Optional<Vehicle> findByLicensePlate(String licensePlate);

  boolean existsByLicensePlate(String licensePlate);

  boolean existsByCompanyId(Integer companyId);

  boolean existsByTypeId(Integer typeId);
}
