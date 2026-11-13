package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Vehicle;
import com.coachbooking.repo.BusCompanyRepository;
import com.coachbooking.repo.TripRepository;
import com.coachbooking.repo.VehicleRepository;
import com.coachbooking.repo.VehicleTypeRepository;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class VehicleControllerTest {

  private VehicleRepository vehicleRepository;
  private BusCompanyRepository busCompanyRepository;
  private VehicleTypeRepository vehicleTypeRepository;
  private TripRepository tripRepository;
  private VehicleController controller;

  @BeforeEach
  void setUp() {
    vehicleRepository = mock(VehicleRepository.class);
    busCompanyRepository = mock(BusCompanyRepository.class);
    vehicleTypeRepository = mock(VehicleTypeRepository.class);
    tripRepository = mock(TripRepository.class);

    controller =
        new VehicleController(
            vehicleRepository, busCompanyRepository, vehicleTypeRepository, tripRepository);
  }

  @Test
  void listAndGetSuccess() {
    Vehicle v = new Vehicle();
    v.setVehicleId(1);
    v.setLicensePlate("51B-12345");
    when(vehicleRepository.findAll()).thenReturn(List.of(v));
    when(vehicleRepository.findById(1)).thenReturn(Optional.of(v));

    assertEquals(1, controller.list().size());
    assertEquals("51B-12345", controller.get(1).getLicensePlate());

    when(vehicleRepository.findById(99)).thenReturn(Optional.empty());
    ApiException ex = assertThrows(ApiException.class, () -> controller.get(99));
    assertEquals(HttpStatus.NOT_FOUND, ex.getStatus());
  }

  @Test
  void createSuccessAndValidation() {
    VehicleController.Request req = new VehicleController.Request(1, 2, "51B-99999", true);

    when(busCompanyRepository.existsById(1)).thenReturn(true);
    when(vehicleTypeRepository.existsById(2)).thenReturn(true);
    when(vehicleRepository.save(any(Vehicle.class))).thenAnswer(i -> i.getArgument(0));

    Vehicle created = controller.create(req);
    assertNotNull(created);
    assertEquals("51B-99999", created.getLicensePlate());

    // company not found
    when(busCompanyRepository.existsById(1)).thenReturn(false);
    ApiException compEx = assertThrows(ApiException.class, () -> controller.create(req));
    assertEquals(HttpStatus.NOT_FOUND, compEx.getStatus());
  }

  @Test
  void updateAndDeleteSuccess() {
    Vehicle v = new Vehicle();
    v.setVehicleId(1);
    when(vehicleRepository.findById(1)).thenReturn(Optional.of(v));
    when(busCompanyRepository.existsById(1)).thenReturn(true);
    when(vehicleTypeRepository.existsById(2)).thenReturn(true);
    when(vehicleRepository.save(any(Vehicle.class))).thenAnswer(i -> i.getArgument(0));

    VehicleController.Request req = new VehicleController.Request(1, 2, "51B-88888", true);
    Vehicle updated = controller.update(1, req);
    assertEquals("51B-88888", updated.getLicensePlate());

    // delete success
    when(tripRepository.existsByVehicleId(1)).thenReturn(false);
    Map<String, String> res = controller.delete(1);
    assertEquals("Xóa xe thành công.", res.get("message"));

    // delete conflict if has trips
    when(tripRepository.existsByVehicleId(1)).thenReturn(true);
    ApiException tripConflict = assertThrows(ApiException.class, () -> controller.delete(1));
    assertEquals(HttpStatus.CONFLICT, tripConflict.getStatus());
  }
}
