package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;

import com.coachbooking.dto.auth.AuthResponse;
import jakarta.servlet.http.Cookie;
import java.time.*;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.*;

class RefreshCookieServiceTest {
  final Instant now = Instant.parse("2026-11-16T00:00:00Z");
  final RefreshCookieService cookies =
      new RefreshCookieService(true, Clock.fixed(now, ZoneOffset.UTC));

  @Test
  void persistentAndSessionCookieAttributesAndReadAndClear() {
    var response = new MockHttpServletResponse();
    cookies.write(
        response, new AuthResponse("access", null, 900, "secret", true, now.plusSeconds(604800)));
    String header = response.getHeader("Set-Cookie");
    assertTrue(header.contains("Max-Age=604800"));
    assertTrue(header.contains("HttpOnly"));
    assertTrue(header.contains("Secure"));
    assertTrue(header.contains("SameSite=Lax"));
    assertTrue(header.contains("Path=/api/auth"));
    assertFalse(header.contains("Domain="));
    assertEquals("no-store", response.getHeader("Cache-Control"));
    response = new MockHttpServletResponse();
    cookies.write(
        response, new AuthResponse("access", null, 900, "secret", false, now.plusSeconds(604800)));
    assertFalse(response.getHeader("Set-Cookie").contains("Max-Age"));
    var request = new MockHttpServletRequest();
    assertNull(cookies.read(request));
    request.setCookies(new Cookie("other", "value"));
    assertNull(cookies.read(request));
    request.setCookies(new Cookie(RefreshCookieService.NAME, "secret"));
    assertEquals("secret", cookies.read(request));
    cookies.clear(response);
    assertTrue(response.getHeaders("Set-Cookie").stream().anyMatch(h -> h.contains("Max-Age=0")));
  }

  @Test
  void localCookieAndExpiredLifetime() {
    var local = new RefreshCookieService(false, Clock.fixed(now, ZoneOffset.UTC));
    var response = new MockHttpServletResponse();
    local.write(
        response, new AuthResponse("access", null, 900, "secret", true, now.minusSeconds(1)));
    assertTrue(response.getHeader("Set-Cookie").contains("Max-Age=0"));
    assertFalse(response.getHeader("Set-Cookie").contains("Secure"));
  }
}
