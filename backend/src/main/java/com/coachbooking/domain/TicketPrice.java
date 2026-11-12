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
@Table(name = "ticket_prices")
public class TicketPrice {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "price_id")
  private Integer priceId;

  @Column(name = "company_id", nullable = false)
  private Integer companyId;

  @Column(name = "type_id", nullable = false)
  private Integer typeId;

  @Column(name = "route_id", nullable = false)
  private Integer routeId;

  @Column(name = "price", nullable = false)
  private BigDecimal price;
}
