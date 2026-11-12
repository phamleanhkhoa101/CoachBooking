package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
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
}
