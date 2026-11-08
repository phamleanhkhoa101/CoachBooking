package com.coachbooking.repo;

import com.coachbooking.domain.BusCompany;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface BusCompanyRepository extends JpaRepository<BusCompany, Integer> {
  Optional<BusCompany> findByCompanyName(String companyName);

  boolean existsByCompanyName(String companyName);

  boolean existsByPhone(String phone);

  boolean existsByEmail(String email);
}
