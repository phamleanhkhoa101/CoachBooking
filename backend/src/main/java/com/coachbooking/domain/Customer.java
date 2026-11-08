package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.LocalDate;

@Entity
@Table(name = "customers")
public class Customer {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "customer_id")
  private Integer customerId;

  @Column(name = "full_name", nullable = false, length = 100)
  private String fullName;

  @Column(name = "date_of_birth", nullable = false)
  private LocalDate dateOfBirth;

  @Column(name = "phone", nullable = false, unique = true, length = 11)
  private String phone;

  @Column(name = "gender", nullable = false)
  private Boolean gender = false;

  @Column(name = "account_id", unique = true)
  private Integer accountId;

  public Integer getCustomerId() {
    return customerId;
  }

  public void setCustomerId(Integer customerId) {
    this.customerId = customerId;
  }

  @JsonProperty("maKh")
  public Integer getMaKh() {
    return customerId;
  }

  @JsonProperty("maKh")
  public void setMaKh(Integer maKh) {
    this.customerId = maKh;
  }

  public String getFullName() {
    return fullName;
  }

  public void setFullName(String fullName) {
    this.fullName = fullName;
  }

  @JsonProperty("tenKh")
  public String getTenKh() {
    return fullName;
  }

  @JsonProperty("tenKh")
  public void setTenKh(String tenKh) {
    this.fullName = tenKh;
  }

  public LocalDate getDateOfBirth() {
    return dateOfBirth;
  }

  public void setDateOfBirth(LocalDate dateOfBirth) {
    this.dateOfBirth = dateOfBirth;
  }

  @JsonProperty("namSinh")
  public LocalDate getNamSinh() {
    return dateOfBirth;
  }

  @JsonProperty("namSinh")
  public void setNamSinh(LocalDate namSinh) {
    this.dateOfBirth = namSinh;
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

  public Boolean getGender() {
    return gender;
  }

  public void setGender(Boolean gender) {
    this.gender = gender;
  }

  @JsonProperty("gioiTinh")
  public Boolean getGioiTinh() {
    return gender;
  }

  @JsonProperty("gioiTinh")
  public void setGioiTinh(Boolean gioiTinh) {
    this.gender = gioiTinh;
  }

  public Integer getAccountId() {
    return accountId;
  }

  public void setAccountId(Integer accountId) {
    this.accountId = accountId;
  }

  @JsonProperty("maTk")
  public Integer getMaTk() {
    return accountId;
  }

  @JsonProperty("maTk")
  public void setMaTk(Integer maTk) {
    this.accountId = maTk;
  }
}
