package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.VehicleType;
import com.coachbooking.repo.TicketPriceRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class VehicleTypeControllerTest {

  private VehicleTypeRepository vehicleTypeRepository;
  private VehicleRepository vehicleRepository;
  private TicketPriceRepository ticketPriceRepository;
  private VehicleTypeController controller;

  @BeforeEach
  void setUp() {
    vehicleTypeRepository = mock(VehicleTypeRepository.class);
    vehicleRepository = mock(VehicleRepository.class);
    ticketPriceRepository = mock(TicketPriceRepository.class);
    controller =
        new VehicleTypeController(vehicleTypeRepository, vehicleRepository, ticketPriceRepository);
  }

  @Test
  void listAndGetSuccess() {
    VehicleType vt = new VehicleType();
    vt.setTypeId(1);
    vt.setTypeName("Ghe ngoi 16 cho");
    when(vehicleTypeRepository.findAll()).thenReturn(List.of(vt));
    when(vehicleTypeRepository.findById(1)).thenReturn(Optional.of(vt));

    assertEquals(1, controller.list().size());
    assertEquals("Ghe ngoi 16 cho", controller.get(1).getTypeName());

    when(vehicleTypeRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createUpdateAndDeleteSuccess() {
    VehicleTypeController.Request req =
        new VehicleTypeController.Request("Giuong nam 36 cho", 36, true);
    when(vehicleTypeRepository.save(any(VehicleType.class))).thenAnswer(i -> i.getArgument(0));

    VehicleType created = controller.create(req);
    assertNotNull(created);
    assertEquals("Giuong nam 36 cho", created.getTypeName());
    assertEquals(36, created.getSeatCapacity());

    VehicleType vt = new VehicleType();
    vt.setTypeId(1);
    when(vehicleTypeRepository.findById(1)).thenReturn(Optional.of(vt));
    VehicleType updated = controller.update(1, req);
    assertEquals(36, updated.getSeatCapacity());

    // delete success
    when(vehicleRepository.existsByTypeId(1)).thenReturn(false);
    when(ticketPriceRepository.existsByTypeId(1)).thenReturn(false);
    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa loại xe thành công.", res.get("message"));

    // delete conflict if in use
    when(vehicleRepository.existsByTypeId(1)).thenReturn(true);
    ApiException usedConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, usedConflict.getStatus());
  }
}
