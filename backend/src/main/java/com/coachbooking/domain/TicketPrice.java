package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;

@Entity
@Table(name = "ticket_prices")
public class TicketPrice {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "price_id")
  private Integer priceId;

  @Column(name = "company_id", nullable = false)
  private Integer companyId;

  @Column(name = "type_id", nullable = false)
  private Integer typeId;

  @Column(name = "route_id", nullable = false)
  private Integer routeId;

  @Column(name = "price", nullable = false)
  private BigDecimal price;

  public Integer getPriceId() {
    return priceId;
  }

  public void setPriceId(Integer priceId) {
    this.priceId = priceId;
  }

  @JsonProperty("maGv")
  public Integer getMaGv() {
    return priceId;
  }

  @JsonProperty("maGv")
  public void setMaGv(Integer maGv) {
    this.priceId = maGv;
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

  public BigDecimal getPrice() {
    return price;
  }

  public void setPrice(BigDecimal price) {
    this.price = price;
  }

  @JsonProperty("giaVe")
  public BigDecimal getGiaVe() {
    return price;
  }

  @JsonProperty("giaVe")
  public void setGiaVe(BigDecimal giaVe) {
    this.price = giaVe;
  }
}
