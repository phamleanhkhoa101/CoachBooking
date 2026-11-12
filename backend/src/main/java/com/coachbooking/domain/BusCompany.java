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
@Table(name = "bus_companies")
public class BusCompany {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "company_id")
  private Integer companyId;

  @Column(name = "company_name", nullable = false, length = 100)
  private String companyName;

  @Column(name = "phone", nullable = false, unique = true, length = 15)
  private String phone;

  @Column(name = "email", nullable = false, unique = true, length = 100)
  private String email;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;
}
