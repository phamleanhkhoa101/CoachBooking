package com.coachbooking.dto.booking;

import jakarta.validation.constraints.NotNull;
import java.time.LocalDate;
import java.util.List;

public class BookingRequest {
  @NotNull(message = "Vui lòng chọn chuyến xe.")
  private Integer tripId;

  private List<Integer> seatNumbers;
  private String customerName;
  private String phone;
  private LocalDate dateOfBirth;
  private Boolean gender;
  private String paymentMethod;

  public BookingRequest() {}

  public BookingRequest(
      Integer tripId,
      List<Integer> seatNumbers,
      String customerName,
      String phone,
      LocalDate dateOfBirth,
      Boolean gender,
      String paymentMethod) {
    this.tripId = tripId;
    this.seatNumbers = seatNumbers;
    this.customerName = customerName;
    this.phone = phone;
    this.dateOfBirth = dateOfBirth;
    this.gender = gender;
    this.paymentMethod = paymentMethod;
  }

  public Integer tripId() {
    return tripId;
  }

  public List<Integer> seatNumbers() {
    return seatNumbers;
  }

  public String customerName() {
    return customerName;
  }

  public String phone() {
    return phone;
  }

  public LocalDate dateOfBirth() {
    return dateOfBirth;
  }

  public Boolean gender() {
    return gender;
  }

  public String paymentMethod() {
    return paymentMethod;
  }

  public Integer getTripId() {
    return tripId;
  }

  public void setTripId(Integer tripId) {
    if (this.tripId == null) {
      this.tripId = tripId;
    }
  }

  public void setMaChuyen(Integer maChuyen) {
    if (this.tripId == null) {
      this.tripId = maChuyen;
    }
  }

  public List<Integer> getSeatNumbers() {
    return seatNumbers;
  }

  public void setSeatNumbers(List<Integer> seatNumbers) {
    if (this.seatNumbers == null || !seatNumbers.isEmpty()) {
      this.seatNumbers = seatNumbers;
    }
  }

  public void setChoNgois(List<Integer> choNgois) {
    if (this.seatNumbers == null || this.seatNumbers.isEmpty()) {
      this.seatNumbers = choNgois;
    }
  }

  public String getCustomerName() {
    return customerName;
  }

  public void setCustomerName(String customerName) {
    if (this.customerName == null || !customerName.isBlank()) {
      this.customerName = customerName;
    }
  }

  public void setTenKhachHang(String tenKhachHang) {
    if (this.customerName == null || this.customerName.isBlank()) {
      this.customerName = tenKhachHang;
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

  public LocalDate getDateOfBirth() {
    return dateOfBirth;
  }

  public void setDateOfBirth(LocalDate dateOfBirth) {
    if (this.dateOfBirth == null) {
      this.dateOfBirth = dateOfBirth;
    }
  }

  public void setNamSinh(LocalDate namSinh) {
    if (this.dateOfBirth == null) {
      this.dateOfBirth = namSinh;
    }
  }

  public Boolean getGender() {
    return gender;
  }

  public void setGender(Boolean gender) {
    if (this.gender == null) {
      this.gender = gender;
    }
  }

  public void setGioiTinh(Boolean gioiTinh) {
    if (this.gender == null) {
      this.gender = gioiTinh;
    }
  }

  public String getPaymentMethod() {
    return paymentMethod;
  }

  public void setPaymentMethod(String paymentMethod) {
    if (this.paymentMethod == null || !paymentMethod.isBlank()) {
      this.paymentMethod = paymentMethod;
    }
  }

  public void setPhuongThuc(String phuongThuc) {
    if (this.paymentMethod == null || this.paymentMethod.isBlank()) {
      this.paymentMethod = phuongThuc;
    }
  }
}
