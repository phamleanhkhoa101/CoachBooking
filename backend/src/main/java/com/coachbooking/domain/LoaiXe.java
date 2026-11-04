package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class LoaiXe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maLx;

    @Column(nullable = false, length = 100)
    private String tenLx;

    @Column(nullable = false)
    private Integer soCho;

    @Column(nullable = false)
    private Boolean trangThai = true;

    public Integer getMaLx() {
        return maLx;
    }

    public void setMaLx(Integer maLx) {
        this.maLx = maLx;
    }

    public String getTenLx() {
        return tenLx;
    }

    public void setTenLx(String tenLx) {
        this.tenLx = tenLx;
    }

    public Integer getSoCho() {
        return soCho;
    }

    public void setSoCho(Integer soCho) {
        this.soCho = soCho;
    }

    public Boolean getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(Boolean trangThai) {
        this.trangThai = trangThai;
    }
}
