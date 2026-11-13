package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Driver;
import com.coachbooking.repo.DriverRepository;
import com.coachbooking.repo.TripRepository;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class DriverControllerTest {

  private DriverRepository driverRepository;
  private TripRepository tripRepository;
  private DriverController controller;

  @BeforeEach
  void setUp() {
    driverRepository = mock(DriverRepository.class);
    tripRepository = mock(TripRepository.class);
    controller = new DriverController(driverRepository, tripRepository);
  }

  @Test
  void listAndGetSuccess() {
    Driver d = new Driver();
    d.setDriverId(1);
    d.setFullName("Le Van Tai");
    when(driverRepository.findAll()).thenReturn(List.of(d));
    when(driverRepository.findById(1)).thenReturn(Optional.of(d));

    assertEquals(1, controller.list().size());
    assertEquals("Le Van Tai", controller.get(1).getFullName());

    when(driverRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndAgeValidation() {
    DriverController.Request req =
        new DriverController.Request(
            "Le Van Tai",
            LocalDate.now().minusYears(30),
            "079123456789",
            true,
            "0901234567",
            "123 Nguyen Trai",
            "B2-123456",
            LocalDate.of(2020, 1, 1),
            true);

    when(driverRepository.save(any(Driver.class))).thenAnswer(i -> i.getArgument(0));
    Driver created = controller.create(req);
    assertNotNull(created);
    assertEquals("Le Van Tai", created.getFullName());

    // under 18
    DriverController.Request youngReq =
        new DriverController.Request(
            "Le Van Tai",
            LocalDate.now().minusYears(16),
            "079123456789",
            true,
            "0901234567",
            "123 Nguyen Trai",
            "B2-123456",
            LocalDate.of(2020, 1, 1),
            true);
    ApiException ageEx = assertThrows(ApiException.class, () -> controller.create(youngReq));
    assertEquals(HttpStatus.BAD_REQUEST, ageEx.getStatus());
  }

  @Test
  void updateAnddeleteSuccess() {
    Driver d = new Driver();
    d.setDriverId(1);
    when(driverRepository.findById(1)).thenReturn(Optional.of(d));
    when(driverRepository.save(any(Driver.class))).thenAnswer(i -> i.getArgument(0));

    DriverController.Request req =
        new DriverController.Request(
            "Le Van Tai New",
            LocalDate.now().minusYears(32),
            "079123456789",
            true,
            "0909999999",
            "456 Le Loi",
            "B2-123456",
            LocalDate.of(2020, 1, 1),
            true);
    Driver updated = controller.update(1, req);
    assertEquals("Le Van Tai New", updated.getFullName());

    // delete success
    when(tripRepository.existsByDriverId(1)).thenReturn(false);
    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa tài xế thành công.", res.get("message"));

    // delete conflict if has trips
    when(tripRepository.existsByDriverId(1)).thenReturn(true);
    ApiException tripConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, tripConflict.getStatus());
  }
}
