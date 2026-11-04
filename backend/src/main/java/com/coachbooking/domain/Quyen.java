package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class Quyen {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Integer maQuyen;

  @Column(nullable = false, unique = true, length = 50)
  private String tenQuyen;

  @Column(length = 255)
  private String moTa;

  public Integer getMaQuyen() {
    return maQuyen;
  }

  public void setMaQuyen(Integer maQuyen) {
    this.maQuyen = maQuyen;
  }

  public String getTenQuyen() {
    return tenQuyen;
  }

  public void setTenQuyen(String tenQuyen) {
    this.tenQuyen = tenQuyen;
  }

  public String getMoTa() {
    return moTa;
  }

  public void setMoTa(String moTa) {
    this.moTa = moTa;
  }
}
