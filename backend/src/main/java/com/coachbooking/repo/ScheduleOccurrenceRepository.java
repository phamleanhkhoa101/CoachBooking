package com.coachbooking.repo;

import com.coachbooking.domain.ScheduleOccurrence;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ScheduleOccurrenceRepository extends JpaRepository<ScheduleOccurrence, Integer> {
  java.util.Optional<ScheduleOccurrence> findByScheduleIdAndServiceDate(
      Integer scheduleId, java.time.LocalDate serviceDate);

  java.util.List<ScheduleOccurrence> findByTripId(Integer tripId);
}
