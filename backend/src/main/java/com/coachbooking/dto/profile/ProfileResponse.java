package com.coachbooking.dto.profile;

import java.time.LocalDate;

public record ProfileResponse(
    String type,
    Integer customerId,
    Integer employeeId,
    String fullName,
    LocalDate dateOfBirth,
    String phone,
    Boolean gender,
    String address,
    String email,
    String username,
    String role) {
  public String getLoai() {
    return type;
  }

  public Integer getMaKh() {
    return customerId;
  }

  public Integer getMaNv() {
    return employeeId;
  }

  public String getHoTen() {
    return fullName;
  }

  public LocalDate getNamSinh() {
    return dateOfBirth;
  }

  public String getSdt() {
    return phone;
  }

  public Boolean getGioiTinh() {
    return gender;
  }

  public String getDiaChi() {
    return address;
  }

  public String getTenDangNhap() {
    return username;
  }

  public String getQuyen() {
    return role;
  }
}
