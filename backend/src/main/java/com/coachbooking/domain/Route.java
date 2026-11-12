package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.LocalTime;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "routes")
public class Route {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "route_id")
  private Integer routeId;

  @Column(name = "departure", nullable = false)
  private String departure;

  @Column(name = "destination", nullable = false)
  private String destination;

  @Column(name = "distance")
  private Integer distance;

  @Column(name = "estimated_duration")
  private LocalTime estimatedDuration;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;
}
