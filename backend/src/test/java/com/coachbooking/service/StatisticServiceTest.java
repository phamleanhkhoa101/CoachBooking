package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Route;
import com.coachbooking.domain.Ticket;
import com.coachbooking.domain.Trip;
import com.coachbooking.dto.statistic.StatisticResponse;
import com.coachbooking.repo.InvoiceRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class StatisticServiceTest {

  private InvoiceRepository invoiceRepository;
  private TicketRepository ticketRepository;
  private TripRepository tripRepository;
  private RouteRepository routeRepository;
  private StatisticService service;

  @BeforeEach
  void setUp() {
    invoiceRepository = mock(InvoiceRepository.class);
    ticketRepository = mock(TicketRepository.class);
    tripRepository = mock(TripRepository.class);
    routeRepository = mock(RouteRepository.class);

    service =
        new StatisticService(invoiceRepository, ticketRepository, tripRepository, routeRepository);
  }

  @Test
  void getStatisticsCalculatesExpectedAggregates() {
    LocalDate from = LocalDate.of(2026, 5, 1);
    LocalDate to = LocalDate.of(2026, 5, 2);

    Invoice inv1 = new Invoice();
    inv1.setInvoiceId(1);
    inv1.setCustomerId(10);
    inv1.setTotalAmount(BigDecimal.valueOf(200_000));
    inv1.setCreatedAt(LocalDateTime.of(2026, 5, 1, 10, 0));

    Invoice inv2 = new Invoice();
    inv2.setInvoiceId(2);
    inv2.setCustomerId(11);
    inv2.setTotalAmount(BigDecimal.valueOf(150_000));
    inv2.setCreatedAt(LocalDateTime.of(2026, 5, 2, 11, 0));

    when(invoiceRepository.findByIsPaidTrueAndCreatedAtGreaterThanEqualAndCreatedAtLessThan(
            any(), any()))
        .thenReturn(List.of(inv1, inv2));
    when(tripRepository.countByDepartureTimeGreaterThanEqualAndDepartureTimeLessThan(any(), any()))
        .thenReturn(5L);

    Ticket t1 = new Ticket();
    t1.setTicketId(101);
    t1.setInvoiceId(1);
    t1.setTripId(50);

    Ticket t2 = new Ticket();
    t2.setTicketId(102);
    t2.setInvoiceId(2);
    t2.setTripId(50);

    when(ticketRepository.findByInvoiceIdIn(List.of(1, 2))).thenReturn(List.of(t1, t2));

    Trip trip = new Trip();
    trip.setTripId(50);
    trip.setRouteId(90);
    when(tripRepository.findAllById(any())).thenReturn(List.of(trip));

    Route route = new Route();
    route.setRouteId(90);
    route.setDeparture("Sài Gòn");
    route.setDestination("Vũng Tàu");
    when(routeRepository.findAllById(any())).thenReturn(List.of(route));

    StatisticResponse res = service.getStatistics(from, to);
    assertEquals(2, res.customerCount());
    assertEquals(5, res.tripCount());
    assertEquals(BigDecimal.valueOf(350_000), res.totalRevenue());
    assertEquals(2, res.ticketCount());
    assertEquals(2, res.dailyRevenue().size());
    assertEquals(1, res.popularRoutes().size());
    assertEquals("Sài Gòn → Vũng Tàu", res.popularRoutes().get(0).routeName());
    assertEquals(2, res.popularRoutes().get(0).ticketCount());
  }

  @Test
  void getStatisticsRejectsInvalidDateRange() {
    LocalDate from = LocalDate.of(2026, 5, 10);
    LocalDate to = LocalDate.of(2026, 5, 1);

    ApiException ex = assertThrows(ApiException.class, () -> service.getStatistics(from, to));
    assertEquals(HttpStatus.BAD_REQUEST, ex.getStatus());
  }

  @Test
  void getStatisticsDefaultsDatesWhenNull() {
    when(invoiceRepository.findByIsPaidTrueAndCreatedAtGreaterThanEqualAndCreatedAtLessThan(
            any(), any()))
        .thenReturn(List.of());
    when(tripRepository.countByDepartureTimeGreaterThanEqualAndDepartureTimeLessThan(any(), any()))
        .thenReturn(0L);

    StatisticResponse res = service.getStatistics(null, null);
    assertEquals(0, res.customerCount());
    assertEquals(30, res.dailyRevenue().size());
    assertTrue(res.popularRoutes().isEmpty());
  }
}
