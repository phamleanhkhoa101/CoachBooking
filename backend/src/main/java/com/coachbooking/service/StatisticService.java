package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Invoice;
import com.coachbooking.domain.Route;
import com.coachbooking.domain.Ticket;
import com.coachbooking.domain.Trip;
import com.coachbooking.repo.InvoiceRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketRepository;
import com.coachbooking.repo.TripRepository;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;
import java.util.stream.Collectors;
import org.springframework.stereotype.Service;

@Service
public class StatisticService {

  private final InvoiceRepository invoiceRepository;
  private final TicketRepository ticketRepository;
  private final TripRepository tripRepository;
  private final RouteRepository routeRepository;

  public StatisticService(
      InvoiceRepository invoiceRepository,
      TicketRepository ticketRepository,
      TripRepository tripRepository,
      RouteRepository routeRepository) {
    this.invoiceRepository = invoiceRepository;
    this.ticketRepository = ticketRepository;
    this.tripRepository = tripRepository;
    this.routeRepository = routeRepository;
  }

  public StatisticResponse getStatistics(LocalDate fromDate, LocalDate toDate) {
    LocalDate actualToDate = toDate == null ? LocalDate.now() : toDate;
    LocalDate actualFromDate = fromDate == null ? actualToDate.minusDays(29) : fromDate;
    if (actualFromDate.isAfter(actualToDate)) {
      throw ApiException.badRequest("Ngày bắt đầu phải trước ngày kết thúc.");
    }

    List<Invoice> invoices =
        invoiceRepository.findByIsPaidTrueAndCreatedAtGreaterThanEqualAndCreatedAtLessThan(
            actualFromDate.atStartOfDay(), actualToDate.plusDays(1).atStartOfDay());

    long customerCount =
        invoices.stream()
            .map(Invoice::getCustomerId)
            .filter(java.util.Objects::nonNull)
            .distinct()
            .count();
    BigDecimal totalRevenue =
        invoices.stream().map(Invoice::getTotalAmount).reduce(BigDecimal.ZERO, BigDecimal::add);
    long tripCount =
        tripRepository.countByDepartureTimeGreaterThanEqualAndDepartureTimeLessThan(
            actualFromDate.atStartOfDay(), actualToDate.plusDays(1).atStartOfDay());

    List<Integer> invoiceIds = invoices.stream().map(Invoice::getInvoiceId).toList();
    List<Ticket> tickets =
        invoiceIds.isEmpty() ? List.of() : ticketRepository.findByInvoiceIdIn(invoiceIds);

    Map<Integer, LocalDate> dateByInvoice =
        invoices.stream()
            .collect(
                Collectors.toMap(
                    Invoice::getInvoiceId, invoice -> invoice.getCreatedAt().toLocalDate()));

    Map<LocalDate, BigDecimal> revenueByDate = new LinkedHashMap<>();
    Map<LocalDate, Long> ticketCountByDate = new LinkedHashMap<>();
    for (LocalDate date = actualFromDate; !date.isAfter(actualToDate); date = date.plusDays(1)) {
      revenueByDate.put(date, BigDecimal.ZERO);
      ticketCountByDate.put(date, 0L);
    }
    for (Invoice invoice : invoices) {
      LocalDate date = invoice.getCreatedAt().toLocalDate();
      revenueByDate.computeIfPresent(date, (key, value) -> value.add(invoice.getTotalAmount()));
    }
    for (Ticket ticket : tickets) {
      LocalDate date = dateByInvoice.get(ticket.getInvoiceId());
      if (date != null) {
        ticketCountByDate.computeIfPresent(date, (key, value) -> value + 1);
      }
    }

    List<RevenuePoint> revenueList = new ArrayList<>();
    revenueByDate.forEach(
        (date, revenue) ->
            revenueList.add(
                new RevenuePoint(
                    date.toString(), revenue, ticketCountByDate.getOrDefault(date, 0L))));

    return new StatisticResponse(
        customerCount,
        tripCount,
        totalRevenue,
        tickets.size(),
        revenueList,
        getPopularRoutes(tickets));
  }

  private List<PopularRoute> getPopularRoutes(List<Ticket> tickets) {
    if (tickets.isEmpty()) {
      return List.of();
    }
    Map<Integer, Long> ticketCountByTrip =
        tickets.stream().collect(Collectors.groupingBy(Ticket::getTripId, Collectors.counting()));
    Map<Integer, Trip> tripMap =
        tripRepository.findAllById(ticketCountByTrip.keySet()).stream()
            .collect(Collectors.toMap(Trip::getTripId, Function.identity()));
    Map<Integer, Route> routeMap =
        routeRepository
            .findAllById(tripMap.values().stream().map(Trip::getRouteId).distinct().toList())
            .stream()
            .collect(Collectors.toMap(Route::getRouteId, Function.identity()));

    Map<String, Long> ticketCountByRoute = new HashMap<>();
    ticketCountByTrip.forEach(
        (tripId, ticketCount) -> {
          Trip trip = tripMap.get(tripId);
          if (trip == null) {
            return;
          }
          Route route = routeMap.get(trip.getRouteId());
          if (route == null) {
            return;
          }
          String routeName = route.getDeparture() + " → " + route.getDestination();
          ticketCountByRoute.merge(routeName, ticketCount, Long::sum);
        });

    return ticketCountByRoute.entrySet().stream()
        .sorted(Map.Entry.<String, Long>comparingByValue().reversed())
        .limit(10)
        .map(entry -> new PopularRoute(entry.getKey(), entry.getValue()))
        .toList();
  }

  public record RevenuePoint(String date, BigDecimal revenue, Long ticketCount) {
    public String getNgay() {
      return date;
    }

    public BigDecimal getDoanhThu() {
      return revenue;
    }

    public Long getSoVe() {
      return ticketCount;
    }
  }

  public record PopularRoute(String routeName, Long ticketCount) {
    public String getTenTuyen() {
      return routeName;
    }

    public Long getSoVe() {
      return ticketCount;
    }
  }

  public record StatisticResponse(
      long customerCount,
      long tripCount,
      BigDecimal totalRevenue,
      long ticketCount,
      List<RevenuePoint> dailyRevenue,
      List<PopularRoute> popularRoutes) {
    public long getSoKhachHang() {
      return customerCount;
    }

    public long getSoChuyenXe() {
      return tripCount;
    }

    public BigDecimal getTongDoanhThu() {
      return totalRevenue;
    }

    public long getSoVe() {
      return ticketCount;
    }

    public List<RevenuePoint> getDoanhThuTheoNgay() {
      return dailyRevenue;
    }

    public List<PopularRoute> getTuyenPhoBien() {
      return popularRoutes;
    }
  }
}
