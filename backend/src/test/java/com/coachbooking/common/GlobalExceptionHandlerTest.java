package com.coachbooking.common;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import java.util.Map;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

class GlobalExceptionHandlerTest {

  private GlobalExceptionHandler handler;

  @BeforeEach
  void setUp() {
    handler = new GlobalExceptionHandler();
  }

  @Test
  void handleIntegrity_returnsFriendlySeatMessage_whenTicketsConstraintViolated() {
    Throwable cause =
        new RuntimeException(
            "ERROR: duplicate key value violates unique constraint \"tickets_trip_id_seat_number_key\"");
    DataIntegrityViolationException ex = new DataIntegrityViolationException("Conflict", cause);

    ResponseEntity<Map<String, String>> response = handler.handleIntegrity(ex);

    assertEquals(HttpStatus.CONFLICT, response.getStatusCode());
    assertNotNull(response.getBody());
    assertEquals(
        "Ghế vừa được người khác đặt. Vui lòng chọn ghế khác.", response.getBody().get("message"));
  }

  @Test
  void handleIntegrity_returnsDefaultMessage_whenGenericConstraintViolated() {
    Throwable cause = new RuntimeException("ERROR: some_other_constraint");
    DataIntegrityViolationException ex = new DataIntegrityViolationException("Conflict", cause);

    ResponseEntity<Map<String, String>> response = handler.handleIntegrity(ex);

    assertEquals(HttpStatus.CONFLICT, response.getStatusCode());
    assertNotNull(response.getBody());
    assertEquals("Dữ liệu bị trùng hoặc vi phạm ràng buộc.", response.getBody().get("message"));
  }
}
