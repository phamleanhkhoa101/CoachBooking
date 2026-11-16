package com.coachbooking.security;

import jakarta.servlet.*;
import jakarta.servlet.http.*;
import java.io.IOException;
import java.util.*;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

/** CSRF protection for endpoints that create or consume the refresh cookie. */
@Component
public class AuthOriginFilter extends OncePerRequestFilter {
  private static final Set<String> PATHS =
      Set.of(
          "/api/auth/login",
          "/api/auth/user/login",
          "/api/auth/admin/login",
          "/api/auth/refresh",
          "/api/auth/logout");
  private final Set<String> origins;

  public AuthOriginFilter(@Value("${app.cors.allowed-origins}") String origins) {
    this.origins = new HashSet<>(Arrays.stream(origins.split(",")).map(String::trim).toList());
  }

  @Override
  protected void doFilterInternal(
      HttpServletRequest request, HttpServletResponse response, FilterChain chain)
      throws ServletException, IOException {
    if ("POST".equals(request.getMethod())
        && PATHS.contains(
            request.getServletPath().isEmpty()
                ? request.getRequestURI()
                : request.getServletPath())) {
      response.setHeader("Cache-Control", "no-store");
      if (!"1".equals(request.getHeader("X-Auth-Request"))
          || !origins.contains(request.getHeader("Origin"))) {
        response.setStatus(403);
        response.setContentType("application/json;charset=UTF-8");
        response.getWriter().write("{\"message\":\"Yêu cầu xác thực không được phép.\"}");
        return;
      }
    }
    chain.doFilter(request, response);
  }
}
