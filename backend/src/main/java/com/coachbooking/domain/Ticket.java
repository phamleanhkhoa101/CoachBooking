package com.coachbooking.domain;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;

@Entity
@Table(name = "tickets")
public class Ticket {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "ticket_id")
  private Integer ticketId;

  @Column(name = "trip_id", nullable = false)
  private Integer tripId;

  @Column(name = "invoice_id", nullable = false)
  private Integer invoiceId;

  @Column(name = "seat_number", nullable = false)
  private Integer seatNumber;

  @Column(name = "price", nullable = false)
  private BigDecimal price = BigDecimal.ZERO;

  public Integer getTicketId() {
    return ticketId;
  }

  public void setTicketId(Integer ticketId) {
    this.ticketId = ticketId;
  }

  @JsonProperty("maVe")
  public Integer getMaVe() {
    return ticketId;
  }

  @JsonProperty("maVe")
  public void setMaVe(Integer maVe) {
    this.ticketId = maVe;
  }

  public Integer getTripId() {
    return tripId;
  }

  public void setTripId(Integer tripId) {
    this.tripId = tripId;
  }

  @JsonProperty("maChuyen")
  public Integer getMaChuyen() {
    return tripId;
  }

  @JsonProperty("maChuyen")
  public void setMaChuyen(Integer maChuyen) {
    this.tripId = maChuyen;
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

  public Integer getSeatNumber() {
    return seatNumber;
  }

  public void setSeatNumber(Integer seatNumber) {
    this.seatNumber = seatNumber;
  }

  @JsonProperty("choNgoi")
  public Integer getChoNgoi() {
    return seatNumber;
  }

  @JsonProperty("choNgoi")
  public void setChoNgoi(Integer choNgoi) {
    this.seatNumber = choNgoi;
  }

  public BigDecimal getPrice() {
    return price;
  }

  public void setPrice(BigDecimal price) {
    this.price = price;
  }

  @JsonProperty("giaVe")
  public BigDecimal getGiaVe() {
    return price;
  }

  @JsonProperty("giaVe")
  public void setGiaVe(BigDecimal giaVe) {
    this.price = giaVe;
  }
}
