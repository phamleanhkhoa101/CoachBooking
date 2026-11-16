package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Account;
import com.coachbooking.dto.auth.*;
import com.coachbooking.service.AuthService;
import com.coachbooking.service.CurrentUserService;
import java.util.Map;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class AuthControllerTest {

  private AuthService authService;
  private CurrentUserService currentUserService;
  private AuthController controller;

  @BeforeEach
  void setUp() {
    authService = mock(AuthService.class);
    currentUserService = mock(CurrentUserService.class);
    controller =
        new AuthController(
            authService,
            currentUserService,
            mock(com.coachbooking.security.RefreshCookieService.class),
            mock(com.coachbooking.service.AuthSessionService.class));
  }

  @Test
  void loginEndpointsDelegateToService() {
    LoginRequest req = new LoginRequest("user", "pass");
    UserResponse u =
        new UserResponse(1, "user", "u@test.com", "Admin", 1, null, null, "User", true);
    AuthResponse resp = new AuthResponse("tok", u);

    when(authService.login(req)).thenReturn(resp);
    when(authService.loginUser(req)).thenReturn(resp);
    when(authService.loginAdmin(req)).thenReturn(resp);

    assertEquals(
        "tok",
        controller.login(req, new org.springframework.mock.web.MockHttpServletResponse()).token());
    assertEquals(
        "tok",
        controller
            .loginUser(req, new org.springframework.mock.web.MockHttpServletResponse())
            .token());
    assertEquals(
        "tok",
        controller
            .loginAdmin(req, new org.springframework.mock.web.MockHttpServletResponse())
            .token());
  }

  @Test
  void otherEndpointsDelegateToService() {
    RegisterRequest reg = new RegisterRequest("user12", "u@test.com", "Pass@123", "Pass@123");
    ForgotPasswordRequest forgot = new ForgotPasswordRequest("u@test.com");
    VerifyResetOtpRequest verifyOtp = new VerifyResetOtpRequest("u@test.com", "123456");
    ResetPasswordRequest reset = new ResetPasswordRequest("u@test.com", "tok123", "NewPass@123");
    ChangePasswordRequest change = new ChangePasswordRequest("old", "NewPass@123");
    Account acc = new Account();

    when(authService.register(reg)).thenReturn(Map.of("message", "registered"));
    when(authService.forgotPassword(forgot)).thenReturn(Map.of("message", "otp_sent"));
    when(authService.verifyResetOtp(verifyOtp))
        .thenReturn(Map.of("message", "otp_ok", "resetToken", "tok123"));
    when(authService.resetPassword(reset)).thenReturn(Map.of("message", "reset_ok"));
    when(currentUserService.requireAccount()).thenReturn(acc);
    when(authService.changePassword(acc, change)).thenReturn(Map.of("message", "changed"));
    when(authService.me(acc))
        .thenReturn(
            new UserResponse(1, "user", "u@test.com", "Admin", 1, null, null, "User", true));

    assertEquals("registered", controller.register(reg).get("message"));
    assertEquals("otp_sent", controller.forgotPassword(forgot).get("message"));
    assertEquals("tok123", controller.verifyResetOtp(verifyOtp).get("resetToken"));
    assertEquals("reset_ok", controller.resetPassword(reset).get("message"));
    assertEquals(
        "changed",
        controller
            .changePassword(change, new org.springframework.mock.web.MockHttpServletResponse())
            .get("message"));
    assertEquals(1, controller.me().accountId());
  }
}
