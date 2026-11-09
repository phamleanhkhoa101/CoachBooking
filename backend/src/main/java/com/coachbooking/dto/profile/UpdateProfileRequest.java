package com.coachbooking.dto.profile;

import jakarta.validation.constraints.NotBlank;
import java.time.LocalDate;

public class UpdateProfileRequest {
  @NotBlank(message = "Vui lòng nhập họ tên.")
  private String fullName;

  @NotBlank(message = "Vui lòng nhập số điện thoại.")
  private String phone;

  private LocalDate dateOfBirth;
  private Boolean gender;
  private String address;

  public UpdateProfileRequest() {}

  public UpdateProfileRequest(
      String fullName, String phone, LocalDate dateOfBirth, Boolean gender, String address) {
    this.fullName = fullName;
    this.phone = phone;
    this.dateOfBirth = dateOfBirth;
    this.gender = gender;
    this.address = address;
  }

  public String fullName() {
    return fullName;
  }

  public String phone() {
    return phone;
  }

  public LocalDate dateOfBirth() {
    return dateOfBirth;
  }

  public Boolean gender() {
    return gender;
  }

  public String address() {
    return address;
  }

  public String getFullName() {
    return fullName;
  }

  public void setFullName(String fullName) {
    if (this.fullName == null || !fullName.isBlank()) {
      this.fullName = fullName;
    }
  }

  public void setHoTen(String hoTen) {
    if (this.fullName == null || this.fullName.isBlank()) {
      this.fullName = hoTen;
    }
  }

  public String getPhone() {
    return phone;
  }

  public void setPhone(String phone) {
    if (this.phone == null || !phone.isBlank()) {
      this.phone = phone;
    }
  }

  public void setSdt(String sdt) {
    if (this.phone == null || this.phone.isBlank()) {
      this.phone = sdt;
    }
  }

  public LocalDate getDateOfBirth() {
    return dateOfBirth;
  }

  public void setDateOfBirth(LocalDate dateOfBirth) {
    if (this.dateOfBirth == null) {
      this.dateOfBirth = dateOfBirth;
    }
  }

  public void setNamSinh(LocalDate namSinh) {
    if (this.dateOfBirth == null) {
      this.dateOfBirth = namSinh;
    }
  }

  public Boolean getGender() {
    return gender;
  }

  public void setGender(Boolean gender) {
    if (this.gender == null) {
      this.gender = gender;
    }
  }

  public void setGioiTinh(Boolean gioiTinh) {
    if (this.gender == null) {
      this.gender = gioiTinh;
    }
  }

  public String getAddress() {
    return address;
  }

  public void setAddress(String address) {
    if (this.address == null || !address.isBlank()) {
      this.address = address;
    }
  }

  public void setDiaChi(String diaChi) {
    if (this.address == null || this.address.isBlank()) {
      this.address = diaChi;
    }
  }
}
