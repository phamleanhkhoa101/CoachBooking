package vn.qlbxmd.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class Xe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maXe;

    @Column(nullable = false)
    private Integer maNx;

    @Column(nullable = false)
    private Integer maLx;

    @Column(nullable = false, unique = true, length = 20)
    private String bienSoXe;

    @Column(nullable = false)
    private Boolean trangThai = true;

    public Integer getMaXe() {
        return maXe;
    }

    public void setMaXe(Integer maXe) {
        this.maXe = maXe;
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

    public String getBienSoXe() {
        return bienSoXe;
    }

    public void setBienSoXe(String bienSoXe) {
        this.bienSoXe = bienSoXe;
    }

    public Boolean getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(Boolean trangThai) {
        this.trangThai = trangThai;
    }
}
