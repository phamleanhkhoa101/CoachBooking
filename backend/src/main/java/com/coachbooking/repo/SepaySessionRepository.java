package com.coachbooking.repo;

import com.coachbooking.domain.SepaySession;
import jakarta.persistence.LockModeType;
import java.time.Instant;
import java.util.*;
import org.springframework.data.jpa.repository.*;
import org.springframework.data.repository.query.Param;

public interface SepaySessionRepository extends JpaRepository<SepaySession, UUID> {
  Optional<SepaySession> findByHoldToken(UUID holdToken);

  Optional<SepaySession> findByInvoiceId(Integer invoiceId);

  Optional<SepaySession> findByReference(String reference);

  boolean existsByProviderTransactionId(String providerTransactionId);

  @Query("select s.tripId from SepaySession s where s.sessionId = :id")
  Optional<Integer> findTripId(@Param("id") UUID id);

  @Query("select s.sessionId from SepaySession s where s.reference = :reference")
  Optional<UUID> findSessionIdByReference(@Param("reference") String reference);

  @Query("select s.sessionId from SepaySession s where s.invoiceId = :invoiceId")
  Optional<UUID> findSessionIdByInvoiceId(@Param("invoiceId") Integer invoiceId);

  @Lock(LockModeType.PESSIMISTIC_WRITE)
  @Query("select s from SepaySession s where s.sessionId = :id")
  Optional<SepaySession> findByIdForUpdate(@Param("id") UUID id);

  List<SepaySession> findByStatusAndExpiresAtLessThanEqual(String status, Instant now);

  @Query(
      "select s.sessionId from SepaySession s where s.confirmationMode = 'POLLING' "
          + "and s.status <> 'PAID' and s.createdAt > :cutoff "
          + "and (s.nextCheckAt is null or s.nextCheckAt <= :now) order by s.nextCheckAt, s.sessionId")
  List<UUID> findDuePollingIds(
      @Param("now") Instant now,
      @Param("cutoff") Instant cutoff,
      org.springframework.data.domain.Pageable pageable);
}
