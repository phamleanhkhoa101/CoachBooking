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
@Table(name = "accounts")
public class Account {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "account_id")
  private Integer accountId;

  @Column(name = "username", nullable = false, unique = true, length = 50)
  private String username;

  @Column(name = "password", nullable = false, length = 100)
  private String password;

  @Column(name = "email", nullable = false, unique = true, length = 100)
  private String email;

  @Column(name = "role_id", nullable = false)
  private Integer roleId;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;
}
