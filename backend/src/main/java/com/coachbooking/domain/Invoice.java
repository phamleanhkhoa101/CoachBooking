package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Table(name = "invoices")
public class Invoice {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "invoice_id")
  private Integer invoiceId;

  @Column(name = "customer_id")
  private Integer customerId;

  @Column(name = "employee_id")
  private Integer employeeId;

  @Column(name = "created_at", nullable = false)
  private LocalDateTime createdAt;

  @Column(name = "total_amount", nullable = false)
  private BigDecimal totalAmount = BigDecimal.ZERO;

  @Column(name = "is_paid", nullable = false)
  private Boolean isPaid = false;

  public Integer getInvoiceId() {
    return invoiceId;
  }

  public void setInvoiceId(Integer invoiceId) {
    this.invoiceId = invoiceId;
  }

  @JsonProperty("maHd")
  public Integer getMaHd() {
    return invoiceId;
  }

  @JsonProperty("maHd")
  public void setMaHd(Integer maHd) {
    this.invoiceId = maHd;
  }

  public Integer getCustomerId() {
    return customerId;
  }

  public void setCustomerId(Integer customerId) {
    this.customerId = customerId;
  }

  @JsonProperty("maKh")
  public Integer getMaKh() {
    return customerId;
  }

  @JsonProperty("maKh")
  public void setMaKh(Integer maKh) {
    this.customerId = maKh;
  }

  public Integer getEmployeeId() {
    return employeeId;
  }

  public void setEmployeeId(Integer employeeId) {
    this.employeeId = employeeId;
  }

  @JsonProperty("maNv")
  public Integer getMaNv() {
    return employeeId;
  }

  @JsonProperty("maNv")
  public void setMaNv(Integer maNv) {
    this.employeeId = maNv;
  }

  public LocalDateTime getCreatedAt() {
    return createdAt;
  }

  public void setCreatedAt(LocalDateTime createdAt) {
    this.createdAt = createdAt;
  }

  @JsonProperty("ngayLap")
  public LocalDateTime getNgayLap() {
    return createdAt;
  }

  @JsonProperty("ngayLap")
  public void setNgayLap(LocalDateTime ngayLap) {
    this.createdAt = ngayLap;
  }

  public BigDecimal getTotalAmount() {
    return totalAmount;
  }

  public void setTotalAmount(BigDecimal totalAmount) {
    this.totalAmount = totalAmount;
  }

  @JsonProperty("tongTien")
  public BigDecimal getTongTien() {
    return totalAmount;
  }

  @JsonProperty("tongTien")
  public void setTongTien(BigDecimal tongTien) {
    this.totalAmount = tongTien;
  }

  public Boolean getIsPaid() {
    return isPaid;
  }

  public void setIsPaid(Boolean isPaid) {
    this.isPaid = isPaid;
  }

  @JsonProperty("trangThai")
  public Boolean getTrangThai() {
    return isPaid;
  }

  @JsonProperty("trangThai")
  public void setTrangThai(Boolean trangThai) {
    this.isActiveOrPaid(trangThai);
  }

  private void isActiveOrPaid(Boolean value) {
    this.isPaid = value;
  }
}
