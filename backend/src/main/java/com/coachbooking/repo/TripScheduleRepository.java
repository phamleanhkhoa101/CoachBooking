package com.coachbooking.repo;

import com.coachbooking.domain.TripSchedule;
import org.springframework.data.jpa.repository.JpaRepository;

public interface TripScheduleRepository extends JpaRepository<TripSchedule, Integer> {
  @org.springframework.data.jpa.repository.Lock(jakarta.persistence.LockModeType.PESSIMISTIC_WRITE)
  @org.springframework.data.jpa.repository.Query(
      "select s from TripSchedule s where s.scheduleId = :id")
  java.util.Optional<TripSchedule> lockById(
      @org.springframework.data.repository.query.Param("id") Integer id);
}
