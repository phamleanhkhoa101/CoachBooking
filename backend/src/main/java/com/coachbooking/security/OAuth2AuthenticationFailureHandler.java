package com.coachbooking.security;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import java.io.IOException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.oauth2.core.OAuth2AuthenticationException;
import org.springframework.security.web.authentication.SimpleUrlAuthenticationFailureHandler;
import org.springframework.stereotype.Component;
import org.springframework.web.util.UriComponentsBuilder;

@Component
public class OAuth2AuthenticationFailureHandler extends SimpleUrlAuthenticationFailureHandler {

  private static final Logger log =
      LoggerFactory.getLogger(OAuth2AuthenticationFailureHandler.class);

  private final HttpCookieOAuth2AuthorizationRequestRepository authorizationRequestRepository;
  private final String redirectUri;

  public OAuth2AuthenticationFailureHandler(
      HttpCookieOAuth2AuthorizationRequestRepository authorizationRequestRepository,
      @Value("${app.oauth2.redirect-uri:http://localhost:5173/oauth2/callback}")
          String redirectUri) {
    this.authorizationRequestRepository = authorizationRequestRepository;
    this.redirectUri = redirectUri;
  }

  @Override
  public void onAuthenticationFailure(
      HttpServletRequest request, HttpServletResponse response, AuthenticationException exception)
      throws IOException {
    String errorCode =
        exception instanceof OAuth2AuthenticationException oauthEx
            ? oauthEx.getError().getErrorCode()
            : exception.getClass().getSimpleName();
    log.warn("Google OAuth2 login failed [{}]: {}", errorCode, exception.getMessage(), exception);
    authorizationRequestRepository.removeAuthorizationRequestCookies(request, response);
    String message = "Đăng nhập Google thất bại hoặc đã bị hủy. Vui lòng thử lại.";
    String targetUrl =
        UriComponentsBuilder.fromUriString(redirectUri)
            .replacePath("/login")
            .replaceQuery(null)
            .fragment(null)
            .queryParam("error", message)
            .build()
            .encode()
            .toUriString();
    getRedirectStrategy().sendRedirect(request, response, targetUrl);
  }
}
