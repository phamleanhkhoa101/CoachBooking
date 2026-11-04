package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
public class ChuyenXe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maChuyen;

    @Column(nullable = false)
    private Integer maXe;

    @Column(nullable = false)
    private Integer maTuyen;

    @Column(nullable = false)
    private Integer maTx;

    @Column(nullable = false)
    private LocalDateTime ngayGioKhoiHanh;

    private LocalDateTime ngayGioDenDuKien;

    @Column(precision = 12, scale = 0)
    private BigDecimal giaVeThucTe;

    public Integer getMaChuyen() {
        return maChuyen;
    }

    public void setMaChuyen(Integer maChuyen) {
        this.maChuyen = maChuyen;
    }

    public Integer getMaXe() {
        return maXe;
    }

    public void setMaXe(Integer maXe) {
        this.maXe = maXe;
    }

    public Integer getMaTuyen() {
        return maTuyen;
    }

    public void setMaTuyen(Integer maTuyen) {
        this.maTuyen = maTuyen;
    }

    public Integer getMaTx() {
        return maTx;
    }

    public void setMaTx(Integer maTx) {
        this.maTx = maTx;
    }

    public LocalDateTime getNgayGioKhoiHanh() {
        return ngayGioKhoiHanh;
    }

    public void setNgayGioKhoiHanh(LocalDateTime ngayGioKhoiHanh) {
        this.ngayGioKhoiHanh = ngayGioKhoiHanh;
    }

    public LocalDateTime getNgayGioDenDuKien() {
        return ngayGioDenDuKien;
    }

    public void setNgayGioDenDuKien(LocalDateTime ngayGioDenDuKien) {
        this.ngayGioDenDuKien = ngayGioDenDuKien;
    }

    public BigDecimal getGiaVeThucTe() {
        return giaVeThucTe;
    }

    public void setGiaVeThucTe(BigDecimal giaVeThucTe) {
        this.giaVeThucTe = giaVeThucTe;
    }
}
