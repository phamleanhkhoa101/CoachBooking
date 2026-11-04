package com.coachbooking.common;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;

class ValidatorsTest {

  @Test
  void blankDetectionHandlesNullAndWhitespace() {
    assertTrue(Validators.isBlank(null));
    assertTrue(Validators.isBlank("   "));
    assertFalse(Validators.isBlank("abc"));
  }

  @Test
  void requireTextRejectsBlankValue() {
    ApiException error =
        assertThrows(ApiException.class, () -> Validators.requireText(" ", "Thiếu dữ liệu."));
    assertEquals("Thiếu dữ liệu.", error.getMessage());
    assertEquals(HttpStatus.BAD_REQUEST, error.getStatus());
  }

  @Test
  void requireEmailAcceptsValidAndRejectsInvalid() {
    assertDoesNotThrow(() -> Validators.requireEmail("khach@coachbooking.com"));
    assertThrows(ApiException.class, () -> Validators.requireEmail("khach.coachbooking.com"));
  }

  @Test
  void requirePhoneAcceptsTenOrElevenDigits() {
    assertDoesNotThrow(() -> Validators.requirePhone("0909123456"));
    assertThrows(ApiException.class, () -> Validators.requirePhone("0909abc"));
  }

  @Test
  void requirePasswordEnforcesComplexity() {
    assertDoesNotThrow(() -> Validators.requirePassword("Abcdef1!"));
    assertThrows(ApiException.class, () -> Validators.requirePassword("abcdefgh"));
  }
}
