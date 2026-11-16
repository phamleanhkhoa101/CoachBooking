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
  private final javax.crypto.SecretKey signingKey;

  public HttpCookieOAuth2AuthorizationRequestRepository(
      @org.springframework.beans.factory.annotation.Value("${app.jwt.secret}") String secret) {
    this.signingKey =
        new javax.crypto.spec.SecretKeySpec(
            secret.getBytes(java.nio.charset.StandardCharsets.UTF_8), "HmacSHA256");
  }

  private String sign(String value) {
    try {
      var mac = javax.crypto.Mac.getInstance("HmacSHA256");
      mac.init(signingKey);
      return Base64.getUrlEncoder()
          .withoutPadding()
          .encodeToString(mac.doFinal(value.getBytes(java.nio.charset.StandardCharsets.UTF_8)));
    } catch (java.security.GeneralSecurityException ex) {
      throw new IllegalStateException(ex);
    }
  }

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
        ResponseCookie.from(
                OAUTH2_AUTH_REQUEST_COOKIE_NAME,
                serialize(
                    OAuth2AuthorizationRequest.from(authorizationRequest)
                        .attributes(
                            attrs -> {
                              attrs.put(
                                  "rememberMe", "true".equals(request.getParameter("rememberMe")));
                              attrs.put("issuedAtEpoch", java.time.Instant.now().getEpochSecond());
                            })
                        .build()))
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
      String payload = Base64.getUrlEncoder().encodeToString(baos.toByteArray());
      return payload + "." + sign(payload);
    } catch (IOException ex) {
      throw new IllegalStateException("Không thể serialize OAuth2AuthorizationRequest.", ex);
    }
  }

  private OAuth2AuthorizationRequest deserialize(String encoded) {
    if (encoded == null || encoded.length() > 16000) return null;
    String[] parts = encoded.split("\\.", -1);
    if (parts.length != 2
        || !java.security.MessageDigest.isEqual(
            sign(parts[0]).getBytes(java.nio.charset.StandardCharsets.UTF_8),
            parts[1].getBytes(java.nio.charset.StandardCharsets.UTF_8))) return null;
    byte[] bytes;
    try {
      bytes = Base64.getUrlDecoder().decode(parts[0]);
    } catch (IllegalArgumentException ex) {
      return null;
    }
    try (ByteArrayInputStream bais = new ByteArrayInputStream(bytes);
        ObjectInputStream ois = new ObjectInputStream(bais)) {
      ois.setObjectInputFilter(
          ObjectInputFilter.Config.createFilter(
              "org.springframework.security.oauth2.core.**;java.util.*;java.lang.*;!*"));
      Object value = ois.readObject();
      if (!(value instanceof OAuth2AuthorizationRequest result)) return null;
      Object issued = result.getAttribute("issuedAtEpoch");
      long now = java.time.Instant.now().getEpochSecond();
      if (!(issued instanceof Long epoch) || epoch > now || now - epoch > COOKIE_EXPIRE_SECONDS)
        return null;
      return result;
    } catch (IOException | ClassNotFoundException ex) {
      return null;
    }
  }
}
