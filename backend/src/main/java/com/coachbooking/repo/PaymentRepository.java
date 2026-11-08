package com.coachbooking.repo;

import com.coachbooking.domain.Payment;
import java.util.Collection;
import java.util.List;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PaymentRepository extends JpaRepository<Payment, Integer> {

  Optional<Payment> findByInvoiceId(Integer invoiceId);

  List<Payment> findByInvoiceIdIn(Collection<Integer> invoiceIds);
}
