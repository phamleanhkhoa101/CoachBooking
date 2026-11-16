package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Account;
import com.coachbooking.service.AuthSessionService;
import io.jsonwebtoken.*;
import jakarta.servlet.FilterChain;
import org.junit.jupiter.api.*;
import org.springframework.mock.web.*;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.User;

class JwtAuthenticationFilterTest {
  final JwtService jwt = mock(JwtService.class);
  final AppUserDetailsService users = mock(AppUserDetailsService.class);
  final AuthSessionService sessions = mock(AuthSessionService.class);
  final JwtAuthenticationFilter filter = new JwtAuthenticationFilter(jwt, users, sessions);
  final FilterChain chain = mock(FilterChain.class);

  @AfterEach
  void cleanup() {
    SecurityContextHolder.clearContext();
  }

  MockHttpServletRequest request(String header) {
    var request = new MockHttpServletRequest();
    if (header != null) request.addHeader("Authorization", header);
    return request;
  }

  @Test
  void authenticatesSessionWithCurrentAuthoritiesAndParsesOnce() throws Exception {
    Claims claims = Jwts.claims().build();
    Account account = new Account();
    account.setUsername("customer");
    when(jwt.parse("valid")).thenReturn(claims);
    when(sessions.authenticate(claims)).thenReturn(account);
    when(users.fromAccount(account))
        .thenReturn(
            User.withUsername("customer").password("").authorities("ROLE_KhachHang").build());
    filter.doFilter(request("Bearer valid"), new MockHttpServletResponse(), chain);
    assertEquals("customer", SecurityContextHolder.getContext().getAuthentication().getName());
    verify(jwt, times(1)).parse("valid");
    verify(users, never()).loadUserByUsername(anyString());
    verify(chain).doFilter(any(), any());
  }

  @Test
  void rejectsBadTokensRevokedSessionsAndDisabledUsers() throws Exception {
    for (String header : new String[] {null, "Basic credentials"}) {
      filter.doFilter(request(header), new MockHttpServletResponse(), chain);
      assertNull(SecurityContextHolder.getContext().getAuthentication());
    }
    when(jwt.parse("bad")).thenThrow(new MalformedJwtException("bad"));
    filter.doFilter(request("Bearer bad"), new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());
    Claims claims = Jwts.claims().build();
    when(jwt.parse("revoked")).thenReturn(claims);
    when(sessions.authenticate(claims)).thenThrow(ApiException.forbidden("revoked"));
    filter.doFilter(request("Bearer revoked"), new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());
    reset(sessions);
    Account account = new Account();
    account.setUsername("locked");
    when(sessions.authenticate(claims)).thenReturn(account);
    when(users.fromAccount(account))
        .thenReturn(
            User.withUsername("locked")
                .password("")
                .authorities("ROLE_KhachHang")
                .disabled(true)
                .build());
    filter.doFilter(request("Bearer revoked"), new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());
  }

  @Test
  void infrastructureFailureIsNotMaskedAsInvalidToken() {
    when(jwt.parse("token"))
        .thenThrow(
            new org.springframework.dao.DataAccessResourceFailureException("database unavailable"));
    assertThrows(
        org.springframework.dao.DataAccessResourceFailureException.class,
        () -> filter.doFilter(request("Bearer token"), new MockHttpServletResponse(), chain));
  }
}
