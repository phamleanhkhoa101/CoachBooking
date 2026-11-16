package com.coachbooking.security;

import static org.junit.jupiter.api.Assertions.*;

import jakarta.servlet.http.Cookie;
import java.net.HttpCookie;
import java.util.List;
import java.util.Set;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpHeaders;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.security.oauth2.core.endpoint.OAuth2AuthorizationRequest;

class HttpCookieOAuth2AuthorizationRequestRepositoryTest {

  private final HttpCookieOAuth2AuthorizationRequestRepository repository =
      new HttpCookieOAuth2AuthorizationRequestRepository(
          "test-only-auth-signing-key-0123456789abcdef0123456789");

  @Test
  void saveLoadAndRemoveAuthorizationRequestRoundTrip() {
    OAuth2AuthorizationRequest authRequest =
        OAuth2AuthorizationRequest.authorizationCode()
            .authorizationUri("https://accounts.google.com/o/oauth2/v2/auth")
            .clientId("client-123")
            .redirectUri("http://localhost:3000/api/login/oauth2/code/google")
            .scopes(Set.of("openid", "profile", "email"))
            .state("state-abc")
            .attributes(attrs -> attrs.put("registration_id", "google"))
            .build();

    MockHttpServletRequest saveReq = new MockHttpServletRequest();
    saveReq.setParameter("rememberMe", "true");
    MockHttpServletResponse saveRes = new MockHttpServletResponse();
    repository.saveAuthorizationRequest(authRequest, saveReq, saveRes);

    String setCookieHeader = saveRes.getHeader(HttpHeaders.SET_COOKIE);
    assertNotNull(setCookieHeader);
    assertTrue(setCookieHeader.contains("SameSite=Lax"));
    assertTrue(setCookieHeader.contains("HttpOnly"));

    List<HttpCookie> parsed = HttpCookie.parse(setCookieHeader);
    String cookieValue = parsed.get(0).getValue();
    assertTrue(cookieValue.length() < 4000, "OAuth cookie must fit browser limits");

    MockHttpServletRequest callbackReq = new MockHttpServletRequest();
    callbackReq.setCookies(
        new Cookie(
            HttpCookieOAuth2AuthorizationRequestRepository.OAUTH2_AUTH_REQUEST_COOKIE_NAME,
            cookieValue));
    MockHttpServletResponse callbackRes = new MockHttpServletResponse();

    OAuth2AuthorizationRequest loaded =
        repository.removeAuthorizationRequest(callbackReq, callbackRes);
    assertNotNull(loaded);
    assertEquals("state-abc", loaded.getState());
    assertEquals("client-123", loaded.getClientId());
    assertEquals("google", loaded.getAttribute("registration_id"));
    assertEquals(Boolean.TRUE, loaded.getAttribute("rememberMe"));
    assertTrue(loaded.getScopes().contains("email"));

    String expiredHeader = callbackRes.getHeader(HttpHeaders.SET_COOKIE);
    assertNotNull(expiredHeader);
    assertTrue(expiredHeader.contains("Max-Age=0"));

    callbackReq.setCookies(
        new Cookie(
            HttpCookieOAuth2AuthorizationRequestRepository.OAUTH2_AUTH_REQUEST_COOKIE_NAME,
            cookieValue + "tampered"));
    assertNull(repository.loadAuthorizationRequest(callbackReq));
  }

  @Test
  void handlesNullAndMalformedAuthorizationRequestCookies() {
    MockHttpServletRequest emptyReq = new MockHttpServletRequest();
    assertNull(repository.loadAuthorizationRequest(emptyReq));

    MockHttpServletRequest badCookieReq = new MockHttpServletRequest();
    badCookieReq.setCookies(
        new Cookie("other_cookie", "123"),
        new Cookie(
            HttpCookieOAuth2AuthorizationRequestRepository.OAUTH2_AUTH_REQUEST_COOKIE_NAME,
            "bm90LWEtc2VyaWFsaXplZC1vYmplY3Q="));
    assertNull(repository.loadAuthorizationRequest(badCookieReq));

    MockHttpServletResponse removeRes = new MockHttpServletResponse();
    repository.saveAuthorizationRequest(null, badCookieReq, removeRes);
    String expiredHeader = removeRes.getHeader(HttpHeaders.SET_COOKIE);
    assertNotNull(expiredHeader);
    assertTrue(expiredHeader.contains("Max-Age=0"));
  }
}
