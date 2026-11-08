package com.coachbooking.repo;

import com.coachbooking.domain.Employee;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface EmployeeRepository extends JpaRepository<Employee, Integer> {
  Optional<Employee> findByPhone(String phone);

  Optional<Employee> findByAccountId(Integer accountId);

  boolean existsByPhone(String phone);

  boolean existsByAccountId(Integer accountId);
}
