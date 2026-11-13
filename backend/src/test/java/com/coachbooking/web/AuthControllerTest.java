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
    controller = new AuthController(authService, currentUserService);
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

    assertEquals("tok", controller.login(req).token());
    assertEquals("tok", controller.loginUser(req).token());
    assertEquals("tok", controller.loginAdmin(req).token());
  }

  @Test
  void otherEndpointsDelegateToService() {
    RegisterRequest reg = new RegisterRequest("user12", "u@test.com", "Pass@123", "Pass@123");
    ForgotPasswordRequest forgot = new ForgotPasswordRequest("user12", "u@test.com");
    ChangePasswordRequest change = new ChangePasswordRequest("old", "NewPass@123");
    Account acc = new Account();

    when(authService.register(reg)).thenReturn(Map.of("message", "registered"));
    when(authService.forgotPassword(forgot)).thenReturn(Map.of("message", "reset"));
    when(currentUserService.requireAccount()).thenReturn(acc);
    when(authService.changePassword(acc, change)).thenReturn(Map.of("message", "changed"));
    when(authService.me(acc))
        .thenReturn(
            new UserResponse(1, "user", "u@test.com", "Admin", 1, null, null, "User", true));

    assertEquals("registered", controller.register(reg).get("message"));
    assertEquals("reset", controller.forgotPassword(forgot).get("message"));
    assertEquals("changed", controller.changePassword(change).get("message"));
    assertEquals(1, controller.me().accountId());
  }
}
