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
@Table(name = "employees")
public class Employee {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "employee_id")
  private Integer employeeId;

  @Column(name = "full_name", nullable = false, length = 100)
  private String fullName;

  @Column(name = "date_of_birth", nullable = false)
  private LocalDate dateOfBirth;

  @Column(name = "address", nullable = false, length = 100)
  private String address;

  @Column(name = "phone", nullable = false, unique = true, length = 11)
  private String phone;

  @Column(name = "gender", nullable = false)
  private Boolean gender = false;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;

  @Column(name = "account_id", unique = true)
  private Integer accountId;

  public Integer getEmployeeId() {
    return employeeId;
  }

  public void setEmployeeId(Integer employeeId) {
    this.employeeId = employeeId;
  }

  @JsonProperty("maNv")
  public Integer getMaNv() {
    return employeeId;
  }

  @JsonProperty("maNv")
  public void setMaNv(Integer maNv) {
    this.employeeId = maNv;
  }

  public String getFullName() {
    return fullName;
  }

  public void setFullName(String fullName) {
    this.fullName = fullName;
  }

  @JsonProperty("tenNv")
  public String getTenNv() {
    return fullName;
  }

  @JsonProperty("tenNv")
  public void setTenNv(String tenNv) {
    this.fullName = tenNv;
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

  public String getAddress() {
    return address;
  }

  public void setAddress(String address) {
    this.address = address;
  }

  @JsonProperty("diaChi")
  public String getDiaChi() {
    return address;
  }

  @JsonProperty("diaChi")
  public void setDiaChi(String diaChi) {
    this.address = diaChi;
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
