package com.coachbooking.domain;

import jakarta.persistence.*;
import java.time.*;
import lombok.*;

@Getter
@Setter
@NoArgsConstructor
@Entity
@Table(name = "schedule_occurrences")
public class ScheduleOccurrence {
  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "occurrence_id")
  private Integer occurrenceId;

  @Column(name = "schedule_id")
  private Integer scheduleId;

  @Column(name = "service_date")
  private LocalDate serviceDate;

  @Column(name = "trip_id")
  private Integer tripId;

  @Column(name = "status")
  private String status;
}
