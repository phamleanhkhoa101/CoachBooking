package vn.qlbxmd.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class NhaXe {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer maNx;

    @Column(nullable = false, length = 100)
    private String tenNx;

    @Column(nullable = false, unique = true, length = 15)
    private String sdt;

    @Column(nullable = false, unique = true, length = 100)
    private String email;

    @Column(nullable = false)
    private Boolean trangThai = true;

    public Integer getMaNx() {
        return maNx;
    }

    public void setMaNx(Integer maNx) {
        this.maNx = maNx;
    }

    public String getTenNx() {
        return tenNx;
    }

    public void setTenNx(String tenNx) {
        this.tenNx = tenNx;
    }

    public String getSdt() {
        return sdt;
    }

    public void setSdt(String sdt) {
        this.sdt = sdt;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public Boolean getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(Boolean trangThai) {
        this.trangThai = trangThai;
    }
}
