package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.LocalDate;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "drivers")
public class Driver {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "driver_id")
  private Integer driverId;

  @Column(name = "full_name", length = 100)
  private String fullName;

  @Column(name = "date_of_birth", nullable = false)
  private LocalDate dateOfBirth;

  @Column(name = "citizen_id", nullable = false, unique = true, length = 12)
  private String citizenId;

  @Column(name = "gender", nullable = false)
  private Boolean gender = false;

  @Column(name = "phone", nullable = false, unique = true, length = 11)
  private String phone;

  @Column(name = "address", nullable = false, length = 100)
  private String address;

  @Column(name = "license_number", nullable = false, length = 10)
  private String licenseNumber;

  @Column(name = "issue_date", nullable = false)
  private LocalDate issueDate;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;
}
