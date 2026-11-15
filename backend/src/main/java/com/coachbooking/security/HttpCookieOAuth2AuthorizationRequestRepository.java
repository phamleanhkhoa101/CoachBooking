package com.coachbooking.security;

import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputFilter;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.Arrays;
import java.util.Base64;
import java.util.Optional;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseCookie;
import org.springframework.security.oauth2.client.web.AuthorizationRequestRepository;
import org.springframework.security.oauth2.core.endpoint.OAuth2AuthorizationRequest;
import org.springframework.stereotype.Component;

/**
 * Lưu trữ OAuth2AuthorizationRequest (chứa state/PKCE) trong cookie HttpOnly ngắn hạn để giữ nguyên
 * kiến trúc SessionCreationPolicy.STATELESS.
 */
@Component
public class HttpCookieOAuth2AuthorizationRequestRepository
    implements AuthorizationRequestRepository<OAuth2AuthorizationRequest> {

  public static final String OAUTH2_AUTH_REQUEST_COOKIE_NAME = "coachbooking_oauth2_auth_request";
  private static final int COOKIE_EXPIRE_SECONDS = 180;

  @Override
  public OAuth2AuthorizationRequest loadAuthorizationRequest(HttpServletRequest request) {
    return getCookie(request, OAUTH2_AUTH_REQUEST_COOKIE_NAME)
        .map(Cookie::getValue)
        .map(this::deserialize)
        .orElse(null);
  }

  @Override
  public void saveAuthorizationRequest(
      OAuth2AuthorizationRequest authorizationRequest,
      HttpServletRequest request,
      HttpServletResponse response) {
    if (authorizationRequest == null) {
      removeAuthorizationRequestCookies(request, response);
      return;
    }
    ResponseCookie cookie =
        ResponseCookie.from(OAUTH2_AUTH_REQUEST_COOKIE_NAME, serialize(authorizationRequest))
            .path("/")
            .httpOnly(true)
            .maxAge(COOKIE_EXPIRE_SECONDS)
            .secure(request.isSecure())
            .sameSite("Lax")
            .build();
    response.addHeader(HttpHeaders.SET_COOKIE, cookie.toString());
  }

  @Override
  public OAuth2AuthorizationRequest removeAuthorizationRequest(
      HttpServletRequest request, HttpServletResponse response) {
    OAuth2AuthorizationRequest authorizationRequest = loadAuthorizationRequest(request);
    removeAuthorizationRequestCookies(request, response);
    return authorizationRequest;
  }

  public void removeAuthorizationRequestCookies(
      HttpServletRequest request, HttpServletResponse response) {
    if (request.getCookies() == null) {
      return;
    }
    for (Cookie cookie : request.getCookies()) {
      if (OAUTH2_AUTH_REQUEST_COOKIE_NAME.equals(cookie.getName())) {
        ResponseCookie expired =
            ResponseCookie.from(OAUTH2_AUTH_REQUEST_COOKIE_NAME, "")
                .path("/")
                .httpOnly(true)
                .maxAge(0)
                .secure(request.isSecure())
                .sameSite("Lax")
                .build();
        response.addHeader(HttpHeaders.SET_COOKIE, expired.toString());
      }
    }
  }

  private Optional<Cookie> getCookie(HttpServletRequest request, String name) {
    if (request.getCookies() == null) {
      return Optional.empty();
    }
    return Arrays.stream(request.getCookies())
        .filter(cookie -> name.equals(cookie.getName()))
        .findFirst();
  }

  private String serialize(OAuth2AuthorizationRequest authorizationRequest) {
    try (ByteArrayOutputStream baos = new ByteArrayOutputStream();
        ObjectOutputStream oos = new ObjectOutputStream(baos)) {
      oos.writeObject(authorizationRequest);
      oos.flush();
      return Base64.getUrlEncoder().encodeToString(baos.toByteArray());
    } catch (IOException ex) {
      throw new IllegalStateException("Không thể serialize OAuth2AuthorizationRequest.", ex);
    }
  }

  private OAuth2AuthorizationRequest deserialize(String encoded) {
    byte[] bytes = Base64.getUrlDecoder().decode(encoded);
    try (ByteArrayInputStream bais = new ByteArrayInputStream(bytes);
        ObjectInputStream ois = new ObjectInputStream(bais)) {
      ois.setObjectInputFilter(
          ObjectInputFilter.Config.createFilter(
              "org.springframework.security.oauth2.core.**;java.util.*;java.lang.*;!*"));
      return (OAuth2AuthorizationRequest) ois.readObject();
    } catch (IOException | ClassNotFoundException ex) {
      return null;
    }
  }
}
