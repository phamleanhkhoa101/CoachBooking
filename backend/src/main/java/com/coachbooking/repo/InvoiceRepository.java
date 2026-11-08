package com.coachbooking.repo;

import com.coachbooking.domain.Invoice;
import java.time.LocalDateTime;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;

public interface InvoiceRepository extends JpaRepository<Invoice, Integer> {

  List<Invoice> findAllByOrderByCreatedAtDesc();

  List<Invoice> findByCustomerIdOrderByCreatedAtDesc(Integer customerId);

  List<Invoice> findByEmployeeIdOrderByCreatedAtDesc(Integer employeeId);

  List<Invoice> findByIsPaidTrueAndCreatedAtGreaterThanEqualAndCreatedAtLessThan(
      LocalDateTime from, LocalDateTime to);

  boolean existsByCustomerId(Integer customerId);

  boolean existsByEmployeeId(Integer employeeId);
}
