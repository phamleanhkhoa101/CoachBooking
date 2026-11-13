package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.TicketPrice;
import com.coachbooking.repo.BusCompanyRepository;
import com.coachbooking.repo.RouteRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import java.math.BigDecimal;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class TicketPriceControllerTest {

  private TicketPriceRepository ticketPriceRepository;
  private BusCompanyRepository busCompanyRepository;
  private VehicleTypeRepository vehicleTypeRepository;
  private RouteRepository routeRepository;
  private TicketPriceController controller;

  @BeforeEach
  void setUp() {
    ticketPriceRepository = mock(TicketPriceRepository.class);
    busCompanyRepository = mock(BusCompanyRepository.class);
    vehicleTypeRepository = mock(VehicleTypeRepository.class);
    routeRepository = mock(RouteRepository.class);

    controller =
        new TicketPriceController(
            ticketPriceRepository, busCompanyRepository, vehicleTypeRepository, routeRepository);
  }

  @Test
  void listAndGetSuccess() {
    TicketPrice tp = new TicketPrice();
    tp.setPriceId(1);
    tp.setPrice(BigDecimal.valueOf(200_000));
    when(ticketPriceRepository.findAll()).thenReturn(List.of(tp));
    when(ticketPriceRepository.findById(1)).thenReturn(Optional.of(tp));

    assertEquals(1, controller.list().size());
    assertEquals(1, controller.get(1).getPriceId());

    when(ticketPriceRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndReferenceValidation() {
    TicketPriceController.Request req =
        new TicketPriceController.Request(1, 2, 3, BigDecimal.valueOf(200_000));

    when(busCompanyRepository.existsById(1)).thenReturn(true);
    when(vehicleTypeRepository.existsById(2)).thenReturn(true);
    when(routeRepository.existsById(3)).thenReturn(true);
    when(ticketPriceRepository.existsByCompanyIdAndTypeIdAndRouteId(1, 2, 3)).thenReturn(false);
    when(ticketPriceRepository.save(any(TicketPrice.class))).thenAnswer(i -> i.getArgument(0));

    TicketPrice created = controller.create(req);
    assertNotNull(created);
    assertEquals(BigDecimal.valueOf(200_000), created.getPrice());

    // duplicate price
    when(ticketPriceRepository.existsByCompanyIdAndTypeIdAndRouteId(1, 2, 3)).thenReturn(true);
    ApiException dup = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.CONFLICT, dup.getStatus());

    // company not found
    when(busCompanyRepository.existsById(1)).thenReturn(false);
    ApiException compEx = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.NOT_FOUND, compEx.getStatus());
  }

  @Test
  void updateAndDeleteSuccess() {
    TicketPrice tp = new TicketPrice();
    tp.setPriceId(1);
    when(ticketPriceRepository.findById(1)).thenReturn(Optional.of(tp));
    when(busCompanyRepository.existsById(1)).thenReturn(true);
    when(vehicleTypeRepository.existsById(2)).thenReturn(true);
    when(routeRepository.existsById(3)).thenReturn(true);
    when(ticketPriceRepository.findByCompanyIdAndTypeIdAndRouteId(1, 2, 3))
        .thenReturn(Optional.empty());
    when(ticketPriceRepository.save(any(TicketPrice.class))).thenAnswer(i -> i.getArgument(0));

    TicketPriceController.Request req =
        new TicketPriceController.Request(1, 2, 3, BigDecimal.valueOf(250_000));
    TicketPrice updated = controller.update(1, req);
    assertEquals(BigDecimal.valueOf(250_000), updated.getPrice());

    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa giá vé thành công.", res.get("message"));
    verify(ticketPriceRepository).delete(tp);
  }
}
