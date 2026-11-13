package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Route;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.TripRepository;
import java.time.LocalTime;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class RouteControllerTest {

  private RouteRepository routeRepository;
  private TripRepository tripRepository;
  private TicketPriceRepository ticketPriceRepository;
  private RouteController controller;

  @BeforeEach
  void setUp() {
    routeRepository = mock(RouteRepository.class);
    tripRepository = mock(TripRepository.class);
    ticketPriceRepository = mock(TicketPriceRepository.class);
    controller = new RouteController(routeRepository, tripRepository, ticketPriceRepository);
  }

  @Test
  void listAndGetSuccess() {
    Route r = new Route();
    r.setRouteId(1);
    r.setDeparture("Sài Gòn");
    r.setDestination("Đà Lạt");
    when(routeRepository.findAll()).thenReturn(List.of(r));
    when(routeRepository.findById(1)).thenReturn(Optional.of(r));

    assertEquals(1, controller.list().size());
    assertEquals("Sài Gòn", controller.get(1).getDeparture());

    when(routeRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndDuplicateConflict() {
    RouteController.Request req =
        new RouteController.Request("Sài Gòn", "Đà Lạt", 300, LocalTime.of(6, 0), true);

    when(routeRepository.existsByDepartureAndDestination("Sài Gòn", "Đà Lạt")).thenReturn(false);
    when(routeRepository.save(any(Route.class))).thenAnswer(i -> i.getArgument(0));

    Route created = controller.create(req);
    assertNotNull(created);
    assertEquals("Sài Gòn", created.getDeparture());
    assertEquals("Đà Lạt", created.getDestination());

    // duplicate route
    when(routeRepository.existsByDepartureAndDestination("Sài Gòn", "Đà Lạt")).thenReturn(true);
    ApiException conflict = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.CONFLICT, conflict.getStatus());
  }

  @Test
  void updateSuccessAndValidation() {
    Route r = new Route();
    r.setRouteId(1);
    when(routeRepository.findById(1)).thenReturn(Optional.of(r));
    when(routeRepository.findByDepartureAndDestination("Sài Gòn", "Nha Trang"))
        .thenReturn(Optional.empty());
    when(routeRepository.save(any(Route.class))).thenAnswer(i -> i.getArgument(0));

    RouteController.Request req =
        new RouteController.Request("Sài Gòn", "Nha Trang", 450, LocalTime.of(9, 0), true);
    Route updated = controller.update(1, req);
    assertEquals("Nha Trang", updated.getDestination());
  }

  @Test
  void deleteSuccessAndRestrictions() {
    Route r = new Route();
    r.setRouteId(1);
    when(routeRepository.findById(1)).thenReturn(Optional.of(r));
    when(tripRepository.existsByRouteId(1)).thenReturn(false);
    when(ticketPriceRepository.existsByRouteId(1)).thenReturn(false);

    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa tuyến đường thành công.", res.get("message"));
    verify(routeRepository).delete(r);

    // cannot delete if has trips
    when(tripRepository.existsByRouteId(1)).thenReturn(true);
    ApiException tripConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, tripConflict.getStatus());

    // cannot delete if has price
    when(tripRepository.existsByRouteId(1)).thenReturn(false);
    when(ticketPriceRepository.existsByRouteId(1)).thenReturn(true);
    ApiException priceConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, priceConflict.getStatus());
  }
}
