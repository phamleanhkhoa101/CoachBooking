package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "roles")
public class Role {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "role_id")
  private Integer roleId;

  @Column(name = "role_name", nullable = false, unique = true, length = 50)
  private String roleName;

  @Column(name = "description")
  private String description;

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

  public String getRoleName() {
    return roleName;
  }

  public void setRoleName(String roleName) {
    this.roleName = roleName;
  }

  @JsonProperty("tenQuyen")
  public String getTenQuyen() {
    return roleName;
  }

  @JsonProperty("tenQuyen")
  public void setTenQuyen(String tenQuyen) {
    this.roleName = tenQuyen;
  }

  public String getDescription() {
    return description;
  }

  public void setDescription(String description) {
    this.description = description;
  }

  @JsonProperty("moTa")
  public String getMoTa() {
    return description;
  }

  @JsonProperty("moTa")
  public void setMoTa(String moTa) {
    this.description = moTa;
  }
}
