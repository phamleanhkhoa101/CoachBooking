package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

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

  public String getCompanyName() {
    return companyName;
  }

  public void setCompanyName(String companyName) {
    this.companyName = companyName;
  }

  @JsonProperty("tenNx")
  public String getTenNx() {
    return companyName;
  }

  @JsonProperty("tenNx")
  public void setTenNx(String tenNx) {
    this.companyName = tenNx;
  }

  public String getPhone() {
    return phone;
  }

  public void setPhone(String phone) {
    this.phone = phone;
  }

  @JsonProperty("sdt")
  public String getSdt() {
    return phone;
  }

  @JsonProperty("sdt")
  public void setSdt(String sdt) {
    this.phone = sdt;
  }

  public String getEmail() {
    return email;
  }

  public void setEmail(String email) {
    this.email = email;
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
