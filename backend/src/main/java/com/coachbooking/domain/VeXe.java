package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import java.math.BigDecimal;

@Entity
public class VeXe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maVe;

    @Column(nullable = false)
    private Integer maChuyen;

    @Column(nullable = false)
    private Integer maHd;

    @Column(nullable = false)
    private Integer choNgoi;

    @Column(nullable = false, precision = 12, scale = 0)
    private BigDecimal giaVe = BigDecimal.ZERO;

    public Integer getMaVe() {
        return maVe;
    }

    public void setMaVe(Integer maVe) {
        this.maVe = maVe;
    }

    public Integer getMaChuyen() {
        return maChuyen;
    }

    public void setMaChuyen(Integer maChuyen) {
        this.maChuyen = maChuyen;
    }

    public Integer getMaHd() {
        return maHd;
    }

    public void setMaHd(Integer maHd) {
        this.maHd = maHd;
    }

    public Integer getChoNgoi() {
        return choNgoi;
    }

    public void setChoNgoi(Integer choNgoi) {
        this.choNgoi = choNgoi;
    }

    public BigDecimal getGiaVe() {
        return giaVe;
    }

    public void setGiaVe(BigDecimal giaVe) {
        this.giaVe = giaVe;
    }
}
