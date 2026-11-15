package com.coachbooking.domain;

import jakarta.persistence.*;
import java.time.*;
import lombok.*;

@Getter
@Setter
@NoArgsConstructor
@Entity
@Table(name = "trip_schedules")
public class TripSchedule {
  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "schedule_id")
  private Integer scheduleId;

  @Column(name = "name")
  private String name;

  @Column(name = "vehicle_id")
  private Integer vehicleId;

  @Column(name = "route_id")
  private Integer routeId;

  @Column(name = "driver_id")
  private Integer driverId;

  @Column(name = "departure_time")
  private LocalTime departureTime;

  @Column(name = "weekdays")
  private String weekdays;

  @Column(name = "valid_from")
  private LocalDate validFrom;

  @Column(name = "valid_to")
  private LocalDate validTo;

  @Column(name = "is_active")
  private Boolean isActive;

  @Column(name = "excluded_dates")
  private String excludedDates;

  @Column(name = "last_result")
  private String lastResult;

  @Column(name = "last_run_at")
  private LocalDateTime lastRunAt;
}
