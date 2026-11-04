package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import java.math.BigDecimal;

@Entity
public class GiaVe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maGv;

    @Column(nullable = false)
    private Integer maNx;

    @Column(nullable = false)
    private Integer maLx;

    @Column(nullable = false)
    private Integer maTuyen;

    @Column(nullable = false, precision = 12, scale = 0)
    private BigDecimal giaVe;

    public Integer getMaGv() {
        return maGv;
    }

    public void setMaGv(Integer maGv) {
        this.maGv = maGv;
    }

    public Integer getMaNx() {
        return maNx;
    }

    public void setMaNx(Integer maNx) {
        this.maNx = maNx;
    }

    public Integer getMaLx() {
        return maLx;
    }

    public void setMaLx(Integer maLx) {
        this.maLx = maLx;
    }

    public Integer getMaTuyen() {
        return maTuyen;
    }

    public void setMaTuyen(Integer maTuyen) {
        this.maTuyen = maTuyen;
    }

    public BigDecimal getGiaVe() {
        return giaVe;
    }

    public void setGiaVe(BigDecimal giaVe) {
        this.giaVe = giaVe;
    }
}
