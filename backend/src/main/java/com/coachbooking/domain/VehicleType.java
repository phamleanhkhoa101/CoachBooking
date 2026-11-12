package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "vehicle_types")
public class VehicleType {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "type_id")
  private Integer typeId;

  @Column(name = "type_name", nullable = false, length = 100)
  private String typeName;

  @Column(name = "seat_capacity", nullable = false)
  private Integer seatCapacity;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;
}
