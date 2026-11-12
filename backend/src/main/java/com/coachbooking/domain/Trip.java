package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "trips")
public class Trip {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "trip_id")
  private Integer tripId;

  @Column(name = "vehicle_id", nullable = false)
  private Integer vehicleId;

  @Column(name = "route_id", nullable = false)
  private Integer routeId;

  @Column(name = "driver_id", nullable = false)
  private Integer driverId;

  @Column(name = "departure_time", nullable = false)
  private LocalDateTime departureTime;

  @Column(name = "estimated_arrival_time")
  private LocalDateTime estimatedArrivalTime;

  @Column(name = "actual_price")
  private BigDecimal actualPrice;
}
