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
@Table(name = "drivers")
public class Driver {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "driver_id")
  private Integer driverId;

  @Column(name = "full_name", length = 100)
  private String fullName;

  @Column(name = "date_of_birth", nullable = false)
  private LocalDate dateOfBirth;

  @Column(name = "citizen_id", nullable = false, unique = true, length = 12)
  private String citizenId;

  @Column(name = "gender", nullable = false)
  private Boolean gender = false;

  @Column(name = "phone", nullable = false, unique = true, length = 11)
  private String phone;

  @Column(name = "address", nullable = false, length = 100)
  private String address;

  @Column(name = "license_number", nullable = false, length = 10)
  private String licenseNumber;

  @Column(name = "issue_date", nullable = false)
  private LocalDate issueDate;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;

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

  public String getFullName() {
    return fullName;
  }

  public void setFullName(String fullName) {
    this.fullName = fullName;
  }

  @JsonProperty("tenTx")
  public String getTenTx() {
    return fullName;
  }

  @JsonProperty("tenTx")
  public void setTenTx(String tenTx) {
    this.fullName = tenTx;
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

  public String getCitizenId() {
    return citizenId;
  }

  public void setCitizenId(String citizenId) {
    this.citizenId = citizenId;
  }

  @JsonProperty("cccd")
  public String getCccd() {
    return citizenId;
  }

  @JsonProperty("cccd")
  public void setCccd(String cccd) {
    this.citizenId = cccd;
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

  public String getLicenseNumber() {
    return licenseNumber;
  }

  public void setLicenseNumber(String licenseNumber) {
    this.licenseNumber = licenseNumber;
  }

  @JsonProperty("bangLai")
  public String getBangLai() {
    return licenseNumber;
  }

  @JsonProperty("bangLai")
  public void setBangLai(String bangLai) {
    this.licenseNumber = bangLai;
  }

  public LocalDate getIssueDate() {
    return issueDate;
  }

  public void setIssueDate(LocalDate issueDate) {
    this.issueDate = issueDate;
  }

  @JsonProperty("ngayCap")
  public LocalDate getNgayCap() {
    return issueDate;
  }

  @JsonProperty("ngayCap")
  public void setNgayCap(LocalDate ngayCap) {
    this.issueDate = ngayCap;
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
