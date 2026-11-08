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
@Table(name = "payments")
public class Payment {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "payment_id")
  private Integer paymentId;

  @Column(name = "invoice_id", nullable = false, unique = true)
  private Integer invoiceId;

  @Column(name = "amount", nullable = false)
  private BigDecimal amount = BigDecimal.ZERO;

  @Column(name = "payment_date", nullable = false)
  private LocalDateTime paymentDate;

  @Column(name = "payment_method", nullable = false, length = 50)
  private String paymentMethod;

  public Integer getPaymentId() {
    return paymentId;
  }

  public void setPaymentId(Integer paymentId) {
    this.paymentId = paymentId;
  }

  @JsonProperty("maTt")
  public Integer getMaTt() {
    return paymentId;
  }

  @JsonProperty("maTt")
  public void setMaTt(Integer maTt) {
    this.paymentId = maTt;
  }

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

  public BigDecimal getAmount() {
    return amount;
  }

  public void setAmount(BigDecimal amount) {
    this.amount = amount;
  }

  @JsonProperty("tongTien")
  public BigDecimal getTongTien() {
    return amount;
  }

  @JsonProperty("tongTien")
  public void setTongTien(BigDecimal tongTien) {
    this.amount = tongTien;
  }

  public LocalDateTime getPaymentDate() {
    return paymentDate;
  }

  public void setPaymentDate(LocalDateTime paymentDate) {
    this.paymentDate = paymentDate;
  }

  @JsonProperty("ngayTt")
  public LocalDateTime getNgayTt() {
    return paymentDate;
  }

  @JsonProperty("ngayTt")
  public void setNgayTt(LocalDateTime ngayTt) {
    this.paymentDate = ngayTt;
  }

  public String getPaymentMethod() {
    return paymentMethod;
  }

  public void setPaymentMethod(String paymentMethod) {
    this.paymentMethod = paymentMethod;
  }

  @JsonProperty("phuongThuc")
  public String getPhuongThuc() {
    return paymentMethod;
  }

  @JsonProperty("phuongThuc")
  public void setPhuongThuc(String phuongThuc) {
    this.paymentMethod = phuongThuc;
  }
}
