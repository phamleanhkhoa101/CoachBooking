package com.coachbooking.security;

import com.coachbooking.common.ApiException;
import com.coachbooking.dto.auth.AuthResponse;
import com.coachbooking.service.AuthService;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import java.io.IOException;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.Authentication;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.security.web.authentication.SimpleUrlAuthenticationSuccessHandler;
import org.springframework.stereotype.Component;
import org.springframework.web.util.UriComponentsBuilder;

@Component
public class OAuth2AuthenticationSuccessHandler extends SimpleUrlAuthenticationSuccessHandler {

  private final AuthService authService;
  private final HttpCookieOAuth2AuthorizationRequestRepository authorizationRequestRepository;
  private final String redirectUri;
  private final RefreshCookieService cookies;

  public OAuth2AuthenticationSuccessHandler(
      AuthService authService,
      HttpCookieOAuth2AuthorizationRequestRepository authorizationRequestRepository,
      RefreshCookieService cookies,
      @Value("${app.oauth2.redirect-uri:http://localhost:5173/oauth2/callback}")
          String redirectUri) {
    this.authService = authService;
    this.authorizationRequestRepository = authorizationRequestRepository;
    this.redirectUri = redirectUri;
    this.cookies = cookies;
  }

  @Override
  public void onAuthenticationSuccess(
      HttpServletRequest request, HttpServletResponse response, Authentication authentication)
      throws IOException {
    var authRequest = authorizationRequestRepository.loadAuthorizationRequest(request);
    boolean rememberMe =
        authRequest != null && Boolean.TRUE.equals(authRequest.getAttribute("rememberMe"));
    authorizationRequestRepository.removeAuthorizationRequestCookies(request, response);

    OAuth2User oAuth2User = (OAuth2User) authentication.getPrincipal();
    String sub = oAuth2User.getAttribute("sub");
    String email = oAuth2User.getAttribute("email");
    Object emailVerifiedAttr = oAuth2User.getAttribute("email_verified");
    boolean emailVerified =
        Boolean.TRUE.equals(emailVerifiedAttr)
            || "true".equalsIgnoreCase(String.valueOf(emailVerifiedAttr));

    try {
      AuthResponse authResponse =
          authService.loginWithGoogle(sub, email, emailVerified, rememberMe);
      cookies.write(response, authResponse);
      String targetUrl =
          UriComponentsBuilder.fromUriString(redirectUri).fragment(null).build().toUriString();
      getRedirectStrategy().sendRedirect(request, response, targetUrl);
    } catch (ApiException ex) {
      String loginErrorUrl =
          UriComponentsBuilder.fromUriString(redirectUri)
              .replacePath("/login")
              .replaceQuery(null)
              .fragment(null)
              .queryParam("error", ex.getMessage())
              .build()
              .encode()
              .toUriString();
      getRedirectStrategy().sendRedirect(request, response, loginErrorUrl);
    }
  }
}
