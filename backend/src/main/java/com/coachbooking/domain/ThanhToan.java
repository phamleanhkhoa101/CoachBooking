package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
public class ThanhToan {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Integer maTt;

  @Column(nullable = false, unique = true)
  private Integer maHd;

  @Column(nullable = false, precision = 12, scale = 0)
  private BigDecimal tongTien = BigDecimal.ZERO;

  @Column(nullable = false)
  private LocalDateTime ngayTt;

  @Column(nullable = false, length = 50)
  private String phuongThuc;

  public Integer getMaTt() {
    return maTt;
  }

  public void setMaTt(Integer maTt) {
    this.maTt = maTt;
  }

  public Integer getMaHd() {
    return maHd;
  }

  public void setMaHd(Integer maHd) {
    this.maHd = maHd;
  }

  public BigDecimal getTongTien() {
    return tongTien;
  }

  public void setTongTien(BigDecimal tongTien) {
    this.tongTien = tongTien;
  }

  public LocalDateTime getNgayTt() {
    return ngayTt;
  }

  public void setNgayTt(LocalDateTime ngayTt) {
    this.ngayTt = ngayTt;
  }

  public String getPhuongThuc() {
    return phuongThuc;
  }

  public void setPhuongThuc(String phuongThuc) {
    this.phuongThuc = phuongThuc;
  }
}
