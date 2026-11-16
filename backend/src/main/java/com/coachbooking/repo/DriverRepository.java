package com.coachbooking.repo;

import com.coachbooking.domain.Driver;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface DriverRepository extends JpaRepository<Driver, Integer> {
  @org.springframework.data.jpa.repository.Lock(jakarta.persistence.LockModeType.PESSIMISTIC_WRITE)
  @org.springframework.data.jpa.repository.Query("select e from Driver e where e.driverId = :id")
  Optional<Driver> lockById(@org.springframework.data.repository.query.Param("id") Integer id);

  Optional<Driver> findByCitizenId(String citizenId);

  Optional<Driver> findByPhone(String phone);

  boolean existsByCitizenId(String citizenId);

  boolean existsByPhone(String phone);
}
