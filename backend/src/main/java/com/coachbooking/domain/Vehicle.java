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
@Table(name = "vehicles")
public class Vehicle {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "vehicle_id")
  private Integer vehicleId;

  @Column(name = "company_id", nullable = false)
  private Integer companyId;

  @Column(name = "type_id", nullable = false)
  private Integer typeId;

  @Column(name = "license_plate", nullable = false, unique = true, length = 20)
  private String licensePlate;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;
}
