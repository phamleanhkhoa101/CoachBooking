package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;

import jakarta.servlet.http.Cookie;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpHeaders;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.security.authentication.AuthenticationServiceException;
import org.springframework.security.oauth2.core.OAuth2AuthenticationException;
import org.springframework.security.oauth2.core.OAuth2Error;

class OAuth2AuthenticationFailureHandlerTest {

  @Test
  void onAuthenticationFailureRedirectsToLoginAndRemovesCookieForOAuth2Exception()
      throws Exception {
    HttpCookieOAuth2AuthorizationRequestRepository repo =
        new HttpCookieOAuth2AuthorizationRequestRepository();
    OAuth2AuthenticationFailureHandler handler =
        new OAuth2AuthenticationFailureHandler(repo, "http://localhost:3000/oauth2/callback");

    MockHttpServletRequest request = new MockHttpServletRequest();
    request.setCookies(
        new Cookie(
            HttpCookieOAuth2AuthorizationRequestRepository.OAUTH2_AUTH_REQUEST_COOKIE_NAME, "val"));
    MockHttpServletResponse response = new MockHttpServletResponse();

    handler.onAuthenticationFailure(
        request, response, new OAuth2AuthenticationException(new OAuth2Error("access_denied")));

    String redirected = response.getRedirectedUrl();
    assertNotNull(redirected);
    assertTrue(redirected.startsWith("http://localhost:3000/login?error="));
    String expiredCookie = response.getHeader(HttpHeaders.SET_COOKIE);
    assertNotNull(expiredCookie);
    assertTrue(expiredCookie.contains("Max-Age=0"));
  }

  @Test
  void onAuthenticationFailureHandlesGenericAuthenticationException() throws Exception {
    HttpCookieOAuth2AuthorizationRequestRepository repo =
        new HttpCookieOAuth2AuthorizationRequestRepository();
    OAuth2AuthenticationFailureHandler handler =
        new OAuth2AuthenticationFailureHandler(repo, "http://localhost:3000/oauth2/callback");

    MockHttpServletRequest request = new MockHttpServletRequest();
    MockHttpServletResponse response = new MockHttpServletResponse();

    handler.onAuthenticationFailure(
        request, response, new AuthenticationServiceException("provider error"));

    String redirected = response.getRedirectedUrl();
    assertNotNull(redirected);
    assertTrue(redirected.startsWith("http://localhost:3000/login?error="));
  }
}
