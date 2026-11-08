package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.LocalTime;

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

  public Integer getRouteId() {
    return routeId;
  }

  public void setRouteId(Integer routeId) {
    this.routeId = routeId;
  }

  @JsonProperty("maTuyen")
  public Integer getMaTuyen() {
    return routeId;
  }

  @JsonProperty("maTuyen")
  public void setMaTuyen(Integer maTuyen) {
    this.routeId = maTuyen;
  }

  public String getDeparture() {
    return departure;
  }

  public void setDeparture(String departure) {
    this.departure = departure;
  }

  @JsonProperty("diemDi")
  public String getDiemDi() {
    return departure;
  }

  @JsonProperty("diemDi")
  public void setDiemDi(String diemDi) {
    this.departure = diemDi;
  }

  public String getDestination() {
    return destination;
  }

  public void setDestination(String destination) {
    this.destination = destination;
  }

  @JsonProperty("diemDen")
  public String getDiemDen() {
    return destination;
  }

  @JsonProperty("diemDen")
  public void setDiemDen(String diemDen) {
    this.destination = diemDen;
  }

  public Integer getDistance() {
    return distance;
  }

  public void setDistance(Integer distance) {
    this.distance = distance;
  }

  @JsonProperty("quangDuong")
  public Integer getQuangDuong() {
    return distance;
  }

  @JsonProperty("quangDuong")
  public void setQuangDuong(Integer quangDuong) {
    this.distance = quangDuong;
  }

  public LocalTime getEstimatedDuration() {
    return estimatedDuration;
  }

  public void setEstimatedDuration(LocalTime estimatedDuration) {
    this.estimatedDuration = estimatedDuration;
  }

  @JsonProperty("thoiGianDuKien")
  public LocalTime getThoiGianDuKien() {
    return estimatedDuration;
  }

  @JsonProperty("thoiGianDuKien")
  public void setThoiGianDuKien(LocalTime thoiGianDuKien) {
    this.estimatedDuration = thoiGianDuKien;
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
