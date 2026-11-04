package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonFormat;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import java.time.LocalTime;

@Entity
public class TuyenDuong {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Integer maTuyen;

  @Column(nullable = false, length = 255)
  private String diemDi;

  @Column(nullable = false, length = 255)
  private String diemDen;

  private Integer quangDuong;

  @JsonFormat(pattern = "HH:mm:ss")
  private LocalTime thoiGianDuKien;

  @Column(nullable = false)
  private Boolean trangThai = true;

  public Integer getMaTuyen() {
    return maTuyen;
  }

  public void setMaTuyen(Integer maTuyen) {
    this.maTuyen = maTuyen;
  }

  public String getDiemDi() {
    return diemDi;
  }

  public void setDiemDi(String diemDi) {
    this.diemDi = diemDi;
  }

  public String getDiemDen() {
    return diemDen;
  }

  public void setDiemDen(String diemDen) {
    this.diemDen = diemDen;
  }

  public Integer getQuangDuong() {
    return quangDuong;
  }

  public void setQuangDuong(Integer quangDuong) {
    this.quangDuong = quangDuong;
  }

  public LocalTime getThoiGianDuKien() {
    return thoiGianDuKien;
  }

  public void setThoiGianDuKien(LocalTime thoiGianDuKien) {
    this.thoiGianDuKien = thoiGianDuKien;
  }

  public Boolean getTrangThai() {
    return trangThai;
  }

  public void setTrangThai(Boolean trangThai) {
    this.trangThai = trangThai;
  }
}
