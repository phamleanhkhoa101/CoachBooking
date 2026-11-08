package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

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

  public String getTypeName() {
    return typeName;
  }

  public void setTypeName(String typeName) {
    this.typeName = typeName;
  }

  @JsonProperty("tenLx")
  public String getTenLx() {
    return typeName;
  }

  @JsonProperty("tenLx")
  public void setTenLx(String tenLx) {
    this.typeName = tenLx;
  }

  public Integer getSeatCapacity() {
    return seatCapacity;
  }

  public void setSeatCapacity(Integer seatCapacity) {
    this.seatCapacity = seatCapacity;
  }

  @JsonProperty("soCho")
  public Integer getSoCho() {
    return seatCapacity;
  }

  @JsonProperty("soCho")
  public void setSoCho(Integer soCho) {
    this.seatCapacity = soCho;
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
