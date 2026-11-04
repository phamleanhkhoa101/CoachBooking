package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import java.time.LocalDate;

@Entity
public class TaiXe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maTx;

    @Column(length = 100)
    private String tenTx;

    @Column(nullable = false)
    private LocalDate namSinh;

    @Column(nullable = false, unique = true, length = 12)
    private String cccd;

    @Column(nullable = false)
    private Boolean gioiTinh = false;

    @Column(nullable = false, unique = true, length = 11)
    private String sdt;

    @Column(nullable = false, length = 100)
    private String diaChi;

    @Column(nullable = false, length = 5)
    private String bangLai;

    @Column(nullable = false)
    private LocalDate ngayCap;

    @Column(nullable = false)
    private Boolean trangThai = true;

    public Integer getMaTx() {
        return maTx;
    }

    public void setMaTx(Integer maTx) {
        this.maTx = maTx;
    }

    public String getTenTx() {
        return tenTx;
    }

    public void setTenTx(String tenTx) {
        this.tenTx = tenTx;
    }

    public LocalDate getNamSinh() {
        return namSinh;
    }

    public void setNamSinh(LocalDate namSinh) {
        this.namSinh = namSinh;
    }

    public String getCccd() {
        return cccd;
    }

    public void setCccd(String cccd) {
        this.cccd = cccd;
    }

    public Boolean getGioiTinh() {
        return gioiTinh;
    }

    public void setGioiTinh(Boolean gioiTinh) {
        this.gioiTinh = gioiTinh;
    }

    public String getSdt() {
        return sdt;
    }

    public void setSdt(String sdt) {
        this.sdt = sdt;
    }

    public String getDiaChi() {
        return diaChi;
    }

    public void setDiaChi(String diaChi) {
        this.diaChi = diaChi;
    }

    public String getBangLai() {
        return bangLai;
    }

    public void setBangLai(String bangLai) {
        this.bangLai = bangLai;
    }

    public LocalDate getNgayCap() {
        return ngayCap;
    }

    public void setNgayCap(LocalDate ngayCap) {
        this.ngayCap = ngayCap;
    }

    public Boolean getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(Boolean trangThai) {
        this.trangThai = trangThai;
    }
}
