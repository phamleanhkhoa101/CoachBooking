package com.coachbooking.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
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
}
