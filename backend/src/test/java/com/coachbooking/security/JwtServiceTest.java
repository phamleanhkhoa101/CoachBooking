package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;

import com.coachbooking.domain.Account;
import io.jsonwebtoken.*;
import io.jsonwebtoken.security.Keys;
import java.nio.charset.StandardCharsets;
import java.time.Instant;
import java.util.*;
import org.junit.jupiter.api.Test;

class JwtServiceTest {
  static final String SECRET = "test-only-auth-signing-key-0123456789abcdef0123456789";
  private final JwtService jwt = new JwtService(SECRET);

  @Test
  void signedClaimsRoundTripAndExpiry() {
    Account account = new Account();
    account.setAccountId(7);
    account.setUsername("customer");
    UUID sid = UUID.randomUUID();
    Instant now = Instant.now();
    String token = jwt.generate(account, "KhachHang", sid, now, now.plusSeconds(900));
    Claims claims = jwt.parse(token);
    assertEquals(sid.toString(), claims.get("sid"));
    assertEquals(7, claims.get("accountId", Integer.class));
    assertEquals("KhachHang", claims.get("role"));
    assertEquals(900, (claims.getExpiration().getTime() - claims.getIssuedAt().getTime()) / 1000);
    assertEquals("customer", jwt.extractUsername(token));
    assertTrue(jwt.isValid(token));
    assertFalse(
        jwt.isValid(
            jwt.generate(account, "KhachHang", sid, now.minusSeconds(100), now.minusSeconds(1))));
  }

  @Test
  void rejectsMalformedTamperedUnsignedAndWrongKey() {
    assertFalse(jwt.isValid(null));
    assertFalse(jwt.isValid(""));
    assertFalse(jwt.isValid("invalid.jwt.token"));
    String foreign =
        Jwts.builder()
            .subject("admin")
            .signWith(Keys.hmacShaKeyFor((SECRET + "foreign").getBytes(StandardCharsets.UTF_8)))
            .compact();
    assertFalse(jwt.isValid(foreign));
    assertFalse(jwt.isValid(Jwts.builder().subject("admin").compact()));
    Account account = new Account();
    account.setAccountId(1);
    account.setUsername("customer");
    String token =
        jwt.generate(
            account, "KhachHang", UUID.randomUUID(), Instant.now(), Instant.now().plusSeconds(900));
    String[] parts = token.split("\\.");
    String payload =
        Base64.getUrlEncoder()
            .withoutPadding()
            .encodeToString("{\"sub\":\"admin\"}".getBytes(StandardCharsets.UTF_8));
    assertFalse(jwt.isValid(parts[0] + "." + payload + "." + parts[2]));
  }

  @Test
  void rejectsMissingSampleAndWeakKeys() {
    assertThrows(IllegalArgumentException.class, () -> new JwtService(null));
    assertThrows(IllegalArgumentException.class, () -> new JwtService(" "));
    assertThrows(
        IllegalArgumentException.class,
        () -> new JwtService("coachbooking-dev-secret-key-change-me-in-production-2026"));
    assertThrows(io.jsonwebtoken.security.WeakKeyException.class, () -> new JwtService("short"));
  }
}
