package com.coachbooking.dto.auth;

public record UserResponse(
    Integer accountId,
    String username,
    String email,
    String role,
    Integer roleId,
    Integer employeeId,
    Integer customerId,
    String fullName,
    Boolean isActive) {
  public Integer getMaTk() {
    return accountId;
  }

  public String getTenDangNhap() {
    return username;
  }

  public String getQuyen() {
    return role;
  }

  public Integer getMaQuyen() {
    return roleId;
  }

  public Integer getMaNv() {
    return employeeId;
  }

  public Integer getMaKh() {
    return customerId;
  }

  public String getHoTen() {
    return fullName;
  }

  public Boolean getTrangThai() {
    return isActive;
  }
}
