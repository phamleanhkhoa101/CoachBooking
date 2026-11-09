package com.coachbooking.dto.invoice;

public class LookupRequest {
  private Integer invoiceId;
  private String phone;

  public LookupRequest() {}

  public LookupRequest(Integer invoiceId, String phone) {
    this.invoiceId = invoiceId;
    this.phone = phone;
  }

  public Integer invoiceId() {
    return invoiceId;
  }

  public String phone() {
    return phone;
  }

  public Integer getInvoiceId() {
    return invoiceId;
  }

  public void setInvoiceId(Integer invoiceId) {
    if (this.invoiceId == null) {
      this.invoiceId = invoiceId;
    }
  }

  public void setMaHoaDon(Integer maHoaDon) {
    if (this.invoiceId == null) {
      this.invoiceId = maHoaDon;
    }
  }

  public String getPhone() {
    return phone;
  }

  public void setPhone(String phone) {
    if (this.phone == null || !phone.isBlank()) {
      this.phone = phone;
    }
  }

  public void setSdt(String sdt) {
    if (this.phone == null || this.phone.isBlank()) {
      this.phone = sdt;
    }
  }

  public void setSoDienThoai(String soDienThoai) {
    if (this.phone == null || this.phone.isBlank()) {
      this.phone = soDienThoai;
    }
  }
}
