package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;

import com.coachbooking.domain.Account;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class JwtServiceTest {

  private static final String SECRET =
      "this_is_a_very_secret_key_used_only_for_testing_purposes_123456789";
  private JwtService jwtService;

  @BeforeEach
  void setUp() {
    jwtService = new JwtService(SECRET, 60);
  }

  @Test
  void generateExtractAndValidate() {
    Account account = new Account();
    account.setAccountId(1);
    account.setUsername("testuser");

    String token = jwtService.generate(account, "KhachHang");
    assertNotNull(token);
    assertTrue(jwtService.isValid(token));
    assertEquals("testuser", jwtService.extractUsername(token));

    assertFalse(jwtService.isValid("invalid.jwt.token"));
  }
}
