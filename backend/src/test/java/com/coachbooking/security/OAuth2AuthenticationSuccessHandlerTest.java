package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.dto.auth.AuthResponse;
import com.coachbooking.dto.auth.UserResponse;
import com.coachbooking.service.AuthService;
import jakarta.servlet.http.Cookie;
import java.util.Map;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpHeaders;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.security.core.Authentication;
import org.springframework.security.oauth2.core.user.DefaultOAuth2User;
import org.springframework.security.oauth2.core.user.OAuth2User;

class OAuth2AuthenticationSuccessHandlerTest {

  @Test
  void onAuthenticationSuccessRedirectsWithoutTokenAndClearsCookie() throws Exception {
    AuthService authService = mock(AuthService.class);
    HttpCookieOAuth2AuthorizationRequestRepository repo =
        new HttpCookieOAuth2AuthorizationRequestRepository(
            "test-only-auth-signing-key-0123456789abcdef0123456789");
    OAuth2AuthenticationSuccessHandler handler =
        new OAuth2AuthenticationSuccessHandler(
            authService,
            repo,
            mock(RefreshCookieService.class),
            "http://localhost:5173/oauth2/callback");

    OAuth2User principal =
        new DefaultOAuth2User(
            java.util.List.of(),
            Map.of("sub", "sub-1", "email", "user@gmail.com", "email_verified", true),
            "sub");
    Authentication authentication = mock(Authentication.class);
    when(authentication.getPrincipal()).thenReturn(principal);

    UserResponse user =
        new UserResponse(1, "user", "user@gmail.com", "KhachHang", 3, null, null, "user", true);
    when(authService.loginWithGoogle("sub-1", "user@gmail.com", true, false))
        .thenReturn(new AuthResponse("jwt-token-abc", user));

    MockHttpServletRequest request = new MockHttpServletRequest();
    request.setCookies(
        new Cookie(
            HttpCookieOAuth2AuthorizationRequestRepository.OAUTH2_AUTH_REQUEST_COOKIE_NAME, "val"));
    MockHttpServletResponse response = new MockHttpServletResponse();

    handler.onAuthenticationSuccess(request, response, authentication);

    assertEquals("http://localhost:5173/oauth2/callback", response.getRedirectedUrl());
    String expiredCookie = response.getHeader(HttpHeaders.SET_COOKIE);
    assertNotNull(expiredCookie);
    assertTrue(expiredCookie.contains("Max-Age=0"));
  }

  @Test
  void onAuthenticationSuccessRedirectsToLoginWithErrorWhenAuthServiceThrowsApiException()
      throws Exception {
    AuthService authService = mock(AuthService.class);
    HttpCookieOAuth2AuthorizationRequestRepository repo =
        new HttpCookieOAuth2AuthorizationRequestRepository(
            "test-only-auth-signing-key-0123456789abcdef0123456789");
    OAuth2AuthenticationSuccessHandler handler =
        new OAuth2AuthenticationSuccessHandler(
            authService,
            repo,
            mock(RefreshCookieService.class),
            "http://localhost:5173/oauth2/callback");

    OAuth2User principal =
        new DefaultOAuth2User(
            java.util.List.of(),
            Map.of("sub", "sub-admin", "email", "admin@gmail.com", "email_verified", "true"),
            "sub");
    Authentication authentication = mock(Authentication.class);
    when(authentication.getPrincipal()).thenReturn(principal);
    when(authService.loginWithGoogle("sub-admin", "admin@gmail.com", true, false))
        .thenThrow(ApiException.forbidden("Không có quyền."));

    MockHttpServletRequest request = new MockHttpServletRequest();
    MockHttpServletResponse response = new MockHttpServletResponse();

    handler.onAuthenticationSuccess(request, response, authentication);

    String redirected = response.getRedirectedUrl();
    assertNotNull(redirected);
    assertTrue(redirected.startsWith("http://localhost:5173/login?error="));
  }
}
