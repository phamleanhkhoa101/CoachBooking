package com.coachbooking.web;

import com.coachbooking.common.ApiException;
import com.coachbooking.dto.auth.AuthResponse;
import com.coachbooking.dto.auth.ChangePasswordRequest;
import com.coachbooking.dto.auth.ForgotPasswordRequest;
import com.coachbooking.dto.auth.LoginRequest;
import com.coachbooking.dto.auth.RegisterRequest;
import com.coachbooking.dto.auth.ResetPasswordRequest;
import com.coachbooking.dto.auth.UserResponse;
import com.coachbooking.dto.auth.VerifyResetOtpRequest;
import com.coachbooking.security.RefreshCookieService;
import com.coachbooking.service.AuthService;
import com.coachbooking.service.AuthSessionService;
import com.coachbooking.service.CurrentUserService;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.validation.Valid;
import java.util.Map;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth")
@Tag(name = "Xác thực", description = "Đăng nhập, đăng ký, đăng xuất và quên mật khẩu qua OTP")
public class AuthController {

  private final AuthService authService;
  private final CurrentUserService currentUserService;
  private final RefreshCookieService cookies;
  private final AuthSessionService sessions;

  public AuthController(
      AuthService authService,
      CurrentUserService currentUserService,
      RefreshCookieService cookies,
      AuthSessionService sessions) {
    this.authService = authService;
    this.currentUserService = currentUserService;
    this.cookies = cookies;
    this.sessions = sessions;
  }

  @PostMapping("/login")
  public AuthResponse login(
      @Valid @RequestBody LoginRequest request, HttpServletResponse response) {
    return withCookie(authService.login(request), response);
  }

  @PostMapping("/user/login")
  public AuthResponse loginUser(
      @Valid @RequestBody LoginRequest request, HttpServletResponse response) {
    return withCookie(authService.loginUser(request), response);
  }

  @PostMapping("/admin/login")
  public AuthResponse loginAdmin(
      @Valid @RequestBody LoginRequest request, HttpServletResponse response) {
    return withCookie(authService.loginAdmin(request), response);
  }

  @PostMapping("/refresh")
  public AuthResponse refresh(HttpServletRequest request, HttpServletResponse response) {
    try {
      return withCookie(authService.refresh(cookies.read(request)), response);
    } catch (ApiException ex) {
      cookies.clear(response);
      throw ex;
    }
  }

  @PostMapping("/logout")
  @ResponseStatus(HttpStatus.NO_CONTENT)
  public void logout(HttpServletRequest request, HttpServletResponse response) {
    sessions.logout(cookies.read(request));
    cookies.clear(response);
  }

  private AuthResponse withCookie(AuthResponse auth, HttpServletResponse response) {
    cookies.write(response, auth);
    return auth;
  }

  @PostMapping("/register")
  public Map<String, String> register(@Valid @RequestBody RegisterRequest request) {
    return authService.register(request);
  }

  @PostMapping("/forgot-password")
  public Map<String, String> forgotPassword(@Valid @RequestBody ForgotPasswordRequest request) {
    return authService.forgotPassword(request);
  }

  @PostMapping("/verify-reset-otp")
  public Map<String, String> verifyResetOtp(@Valid @RequestBody VerifyResetOtpRequest request) {
    return authService.verifyResetOtp(request);
  }

  @PostMapping("/reset-password")
  public Map<String, String> resetPassword(@Valid @RequestBody ResetPasswordRequest request) {
    return authService.resetPassword(request);
  }

  @GetMapping("/me")
  public UserResponse me() {
    return authService.me(currentUserService.requireAccount());
  }

  @PostMapping("/change-password")
  public Map<String, String> changePassword(
      @Valid @RequestBody ChangePasswordRequest request, HttpServletResponse response) {
    var result = authService.changePassword(currentUserService.requireAccount(), request);
    cookies.clear(response);
    return result;
  }
}
