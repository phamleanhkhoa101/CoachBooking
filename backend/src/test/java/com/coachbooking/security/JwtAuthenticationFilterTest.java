package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import jakarta.servlet.FilterChain;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UsernameNotFoundException;

class JwtAuthenticationFilterTest {

  @AfterEach
  void cleanUp() {
    SecurityContextHolder.clearContext();
  }

  @Test
  void setsAuthenticationForValidTokenAndEnabledUser() throws Exception {
    JwtService jwtService = mock(JwtService.class);
    AppUserDetailsService userDetailsService = mock(AppUserDetailsService.class);
    JwtAuthenticationFilter filter = new JwtAuthenticationFilter(jwtService, userDetailsService);

    when(jwtService.isValid("good-token")).thenReturn(true);
    when(jwtService.extractUsername("good-token")).thenReturn("kh1");
    UserDetails user = User.withUsername("kh1").password("").authorities("ROLE_KhachHang").build();
    when(userDetailsService.loadUserByUsername("kh1")).thenReturn(user);

    MockHttpServletRequest request = new MockHttpServletRequest();
    request.addHeader("Authorization", "Bearer good-token");
    MockHttpServletResponse response = new MockHttpServletResponse();
    FilterChain chain = mock(FilterChain.class);

    filter.doFilter(request, response, chain);

    assertNotNull(SecurityContextHolder.getContext().getAuthentication());
    assertEquals("kh1", SecurityContextHolder.getContext().getAuthentication().getName());
    verify(chain).doFilter(request, response);
  }

  @Test
  void skipsOrClearsAuthenticationForMissingInvalidDisabledOrUnknownUser() throws Exception {
    JwtService jwtService = mock(JwtService.class);
    AppUserDetailsService userDetailsService = mock(AppUserDetailsService.class);
    JwtAuthenticationFilter filter = new JwtAuthenticationFilter(jwtService, userDetailsService);
    FilterChain chain = mock(FilterChain.class);

    // Missing header
    MockHttpServletRequest noHeaderReq = new MockHttpServletRequest();
    filter.doFilter(noHeaderReq, new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());

    // Invalid token
    when(jwtService.isValid("bad-token")).thenReturn(false);
    MockHttpServletRequest badReq = new MockHttpServletRequest();
    badReq.addHeader("Authorization", "Bearer bad-token");
    filter.doFilter(badReq, new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());

    // Disabled user
    when(jwtService.isValid("disabled-token")).thenReturn(true);
    when(jwtService.extractUsername("disabled-token")).thenReturn("locked");
    UserDetails disabledUser =
        User.withUsername("locked")
            .password("")
            .authorities("ROLE_KhachHang")
            .disabled(true)
            .build();
    when(userDetailsService.loadUserByUsername("locked")).thenReturn(disabledUser);
    MockHttpServletRequest disabledReq = new MockHttpServletRequest();
    disabledReq.addHeader("Authorization", "Bearer disabled-token");
    filter.doFilter(disabledReq, new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());

    // Unknown user throws RuntimeException
    when(jwtService.isValid("ghost-token")).thenReturn(true);
    when(jwtService.extractUsername("ghost-token")).thenReturn("ghost");
    when(userDetailsService.loadUserByUsername("ghost"))
        .thenThrow(new UsernameNotFoundException("missing"));
    MockHttpServletRequest ghostReq = new MockHttpServletRequest();
    ghostReq.addHeader("Authorization", "Bearer ghost-token");
    filter.doFilter(ghostReq, new MockHttpServletResponse(), chain);
    assertNull(SecurityContextHolder.getContext().getAuthentication());
  }
}
