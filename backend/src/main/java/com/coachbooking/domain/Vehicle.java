package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

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

  public Integer getVehicleId() {
    return vehicleId;
  }

  public void setVehicleId(Integer vehicleId) {
    this.vehicleId = vehicleId;
  }

  @JsonProperty("maXe")
  public Integer getMaXe() {
    return vehicleId;
  }

  @JsonProperty("maXe")
  public void setMaXe(Integer maXe) {
    this.vehicleId = maXe;
  }

  public Integer getCompanyId() {
    return companyId;
  }

  public void setCompanyId(Integer companyId) {
    this.companyId = companyId;
  }

  @JsonProperty("maNx")
  public Integer getMaNx() {
    return companyId;
  }

  @JsonProperty("maNx")
  public void setMaNx(Integer maNx) {
    this.companyId = maNx;
  }

  public Integer getTypeId() {
    return typeId;
  }

  public void setTypeId(Integer typeId) {
    this.typeId = typeId;
  }

  @JsonProperty("maLx")
  public Integer getMaLx() {
    return typeId;
  }

  @JsonProperty("maLx")
  public void setMaLx(Integer maLx) {
    this.typeId = maLx;
  }

  public String getLicensePlate() {
    return licensePlate;
  }

  public void setLicensePlate(String licensePlate) {
    this.licensePlate = licensePlate;
  }

  @JsonProperty("bienSoXe")
  public String getBienSoXe() {
    return licensePlate;
  }

  @JsonProperty("bienSoXe")
  public void setBienSoXe(String bienSoXe) {
    this.licensePlate = bienSoXe;
  }

  public Boolean getIsActive() {
    return isActive;
  }

  public void setIsActive(Boolean isActive) {
    this.isActive = isActive;
  }

  @JsonProperty("trangThai")
  public Boolean getTrangThai() {
    return isActive;
  }

  @JsonProperty("trangThai")
  public void setTrangThai(Boolean trangThai) {
    this.isActive = trangThai;
  }
}
