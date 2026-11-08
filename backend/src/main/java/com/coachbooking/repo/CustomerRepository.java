package com.coachbooking.repo;

import com.coachbooking.domain.Customer;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CustomerRepository extends JpaRepository<Customer, Integer> {
  Optional<Customer> findByPhone(String phone);

  Optional<Customer> findByAccountId(Integer accountId);

  boolean existsByPhone(String phone);

  boolean existsByAccountId(Integer accountId);
}
