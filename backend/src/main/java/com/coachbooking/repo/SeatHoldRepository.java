package com.coachbooking.repo;

import com.coachbooking.domain.SeatHold;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface SeatHoldRepository extends JpaRepository<SeatHold, Long> {

  List<SeatHold> findByHoldTokenOrderBySeatNumber(UUID holdToken);

  List<SeatHold> findByTripIdAndExpiresAtAfterOrderBySeatNumber(Integer tripId, LocalDateTime now);

  List<SeatHold> findByTripIdAndAccountIdAndExpiresAtAfter(
      Integer tripId, Integer accountId, LocalDateTime now);

  @Modifying
  @Query("delete from SeatHold h where h.holdToken = :holdToken")
  int deleteByHoldToken(@Param("holdToken") UUID holdToken);

  @Modifying
  @Query("delete from SeatHold h where h.tripId = :tripId and h.accountId = :accountId")
  int deleteByTripIdAndAccountId(
      @Param("tripId") Integer tripId, @Param("accountId") Integer accountId);

  @Modifying
  @Query("delete from SeatHold h where h.tripId = :tripId and h.expiresAt <= :now")
  int deleteExpiredByTripId(@Param("tripId") Integer tripId, @Param("now") LocalDateTime now);

  @Query("select distinct h.tripId from SeatHold h where h.expiresAt <= :now")
  List<Integer> findTripIdsWithExpiredHolds(@Param("now") LocalDateTime now);
}
