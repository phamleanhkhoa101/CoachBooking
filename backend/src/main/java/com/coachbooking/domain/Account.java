package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "accounts")
public class Account {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "account_id")
  private Integer accountId;

  @Column(name = "username", nullable = false, unique = true, length = 50)
  private String username;

  @Column(name = "password", nullable = false, length = 100)
  private String password;

  @Column(name = "email", nullable = false, unique = true, length = 100)
  private String email;

  @Column(name = "role_id", nullable = false)
  private Integer roleId;

  @Column(name = "is_active", nullable = false)
  private Boolean isActive = true;

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

  public String getUsername() {
    return username;
  }

  public void setUsername(String username) {
    this.username = username;
  }

  @JsonProperty("tenDangNhap")
  public String getTenDangNhap() {
    return username;
  }

  @JsonProperty("tenDangNhap")
  public void setTenDangNhap(String tenDangNhap) {
    this.username = tenDangNhap;
  }

  public String getPassword() {
    return password;
  }

  public void setPassword(String password) {
    this.password = password;
  }

  @JsonProperty("matKhau")
  public String getMatKhau() {
    return password;
  }

  @JsonProperty("matKhau")
  public void setMatKhau(String matKhau) {
    this.password = matKhau;
  }

  public String getEmail() {
    return email;
  }

  public void setEmail(String email) {
    this.email = email;
  }

  public Integer getRoleId() {
    return roleId;
  }

  public void setRoleId(Integer roleId) {
    this.roleId = roleId;
  }

  @JsonProperty("maQuyen")
  public Integer getMaQuyen() {
    return roleId;
  }

  @JsonProperty("maQuyen")
  public void setMaQuyen(Integer maQuyen) {
    this.roleId = maQuyen;
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
