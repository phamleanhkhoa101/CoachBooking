package com.coachbooking.repo;

import com.coachbooking.domain.Ticket;
import java.util.Collection;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface TicketRepository extends JpaRepository<Ticket, Integer> {

  List<Ticket> findByTripId(Integer tripId);

  List<Ticket> findByInvoiceId(Integer invoiceId);

  List<Ticket> findByInvoiceIdIn(Collection<Integer> invoiceIds);

  boolean existsByTripId(Integer tripId);

  void deleteByInvoiceId(Integer invoiceId);

  @Query("select t.seatNumber from Ticket t where t.tripId = :tripId")
  List<Integer> findSeatNumbersByTripId(@Param("tripId") Integer tripId);
}
