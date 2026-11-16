package com.coachbooking.security;

import com.coachbooking.dto.auth.AuthResponse;
import jakarta.servlet.http.*;
import java.time.Clock;
import java.time.Duration;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.*;
import org.springframework.stereotype.Component;

@Component
public class RefreshCookieService {
  public static final String NAME = "coachbooking_refresh";
  private final boolean secure;
  private final Clock clock;

  public RefreshCookieService(
      @Value("${app.auth.cookie-secure:true}") boolean secure, Clock clock) {
    this.secure = secure;
    this.clock = clock;
  }

  public String read(HttpServletRequest request) {
    if (request.getCookies() != null)
      for (Cookie cookie : request.getCookies()) {
        if (NAME.equals(cookie.getName())) return cookie.getValue();
      }
    return null;
  }

  public void write(HttpServletResponse response, AuthResponse auth) {
    ResponseCookie.ResponseCookieBuilder builder = cookie(auth.refreshToken());
    if (auth.rememberMe()) {
      Duration remaining = Duration.between(clock.instant(), auth.refreshExpiresAt());
      builder.maxAge(remaining.isNegative() ? Duration.ZERO : remaining);
    }
    response.addHeader(HttpHeaders.SET_COOKIE, builder.build().toString());
    response.setHeader(HttpHeaders.CACHE_CONTROL, "no-store");
  }

  public void clear(HttpServletResponse response) {
    response.addHeader(HttpHeaders.SET_COOKIE, cookie("").maxAge(0).build().toString());
    response.setHeader(HttpHeaders.CACHE_CONTROL, "no-store");
  }

  private ResponseCookie.ResponseCookieBuilder cookie(String value) {
    return ResponseCookie.from(NAME, value)
        .httpOnly(true)
        .secure(secure)
        .sameSite("Lax")
        .path("/api/auth");
  }
}
