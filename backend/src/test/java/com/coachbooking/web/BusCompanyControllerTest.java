package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.BusCompany;
import com.coachbooking.repo.BusCompanyRepository;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.VehicleRepository;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class BusCompanyControllerTest {

  private BusCompanyRepository busCompanyRepository;
  private VehicleRepository vehicleRepository;
  private TicketPriceRepository ticketPriceRepository;
  private BusCompanyController controller;

  @BeforeEach
  void setUp() {
    busCompanyRepository = mock(BusCompanyRepository.class);
    vehicleRepository = mock(VehicleRepository.class);
    ticketPriceRepository = mock(TicketPriceRepository.class);
    controller =
        new BusCompanyController(busCompanyRepository, vehicleRepository, ticketPriceRepository);
  }

  @Test
  void listAndGetSuccess() {
    BusCompany bc = new BusCompany();
    bc.setCompanyId(1);
    bc.setCompanyName("Phuong Trang");
    when(busCompanyRepository.findAll()).thenReturn(List.of(bc));
    when(busCompanyRepository.findById(1)).thenReturn(Optional.of(bc));

    assertEquals(1, controller.list().size());
    assertEquals("Phuong Trang", controller.get(1).getCompanyName());

    when(busCompanyRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createUpdateAndDeleteSuccess() {
    BusCompanyController.Request req =
        new BusCompanyController.Request("Thanh Buoi", "0901234567", "buoi@test.com", true);
    when(busCompanyRepository.save(any(BusCompany.class))).thenAnswer(i -> i.getArgument(0));

    BusCompany created = controller.create(req);
    assertNotNull(created);
    assertEquals("Thanh Buoi", created.getCompanyName());

    BusCompany bc = new BusCompany();
    bc.setCompanyId(1);
    when(busCompanyRepository.findById(1)).thenReturn(Optional.of(bc));
    BusCompany updated = controller.update(1, req);
    assertEquals("Thanh Buoi", updated.getCompanyName());

    // delete success
    when(vehicleRepository.existsByCompanyId(1)).thenReturn(false);
    when(ticketPriceRepository.existsByCompanyId(1)).thenReturn(false);
    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa nhà xe thành công.", res.get("message"));

    // delete conflict if has vehicles
    when(vehicleRepository.existsByCompanyId(1)).thenReturn(true);
    ApiException vehicleConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, vehicleConflict.getStatus());
  }
}
