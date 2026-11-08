package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Table(name = "trips")
public class Trip {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "trip_id")
  private Integer tripId;

  @Column(name = "vehicle_id", nullable = false)
  private Integer vehicleId;

  @Column(name = "route_id", nullable = false)
  private Integer routeId;

  @Column(name = "driver_id", nullable = false)
  private Integer driverId;

  @Column(name = "departure_time", nullable = false)
  private LocalDateTime departureTime;

  @Column(name = "estimated_arrival_time")
  private LocalDateTime estimatedArrivalTime;

  @Column(name = "actual_price")
  private BigDecimal actualPrice;

  public Integer getTripId() {
    return tripId;
  }

  public void setTripId(Integer tripId) {
    this.tripId = tripId;
  }

  @JsonProperty("maChuyen")
  public Integer getMaChuyen() {
    return tripId;
  }

  @JsonProperty("maChuyen")
  public void setMaChuyen(Integer maChuyen) {
    this.tripId = maChuyen;
  }

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

  public Integer getDriverId() {
    return driverId;
  }

  public void setDriverId(Integer driverId) {
    this.driverId = driverId;
  }

  @JsonProperty("maTx")
  public Integer getMaTx() {
    return driverId;
  }

  @JsonProperty("maTx")
  public void setMaTx(Integer maTx) {
    this.driverId = maTx;
  }

  public LocalDateTime getDepartureTime() {
    return departureTime;
  }

  public void setDepartureTime(LocalDateTime departureTime) {
    this.departureTime = departureTime;
  }

  @JsonProperty("ngayGioKhoiHanh")
  public LocalDateTime getNgayGioKhoiHanh() {
    return departureTime;
  }

  @JsonProperty("ngayGioKhoiHanh")
  public void setNgayGioKhoiHanh(LocalDateTime ngayGioKhoiHanh) {
    this.departureTime = ngayGioKhoiHanh;
  }

  public LocalDateTime getEstimatedArrivalTime() {
    return estimatedArrivalTime;
  }

  public void setEstimatedArrivalTime(LocalDateTime estimatedArrivalTime) {
    this.estimatedArrivalTime = estimatedArrivalTime;
  }

  @JsonProperty("ngayGioDenDuKien")
  public LocalDateTime getNgayGioDenDuKien() {
    return estimatedArrivalTime;
  }

  @JsonProperty("ngayGioDenDuKien")
  public void setNgayGioDenDuKien(LocalDateTime ngayGioDenDuKien) {
    this.estimatedArrivalTime = ngayGioDenDuKien;
  }

  public BigDecimal getActualPrice() {
    return actualPrice;
  }

  public void setActualPrice(BigDecimal actualPrice) {
    this.actualPrice = actualPrice;
  }

  @JsonProperty("giaVeThucTe")
  public BigDecimal getGiaVeThucTe() {
    return actualPrice;
  }

  @JsonProperty("giaVeThucTe")
  public void setGiaVeThucTe(BigDecimal giaVeThucTe) {
    this.actualPrice = giaVeThucTe;
  }
}
