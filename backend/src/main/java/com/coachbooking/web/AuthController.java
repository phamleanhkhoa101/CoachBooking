package com.coachbooking.web;

import com.coachbooking.dto.auth.AuthResponse;
import com.coachbooking.dto.auth.ChangePasswordRequest;
import com.coachbooking.dto.auth.ForgotPasswordRequest;
import com.coachbooking.dto.auth.LoginRequest;
import com.coachbooking.dto.auth.RegisterRequest;
import com.coachbooking.dto.auth.UserResponse;
import com.coachbooking.service.AuthService;
import com.coachbooking.service.CurrentUserService;
import jakarta.validation.Valid;
import java.util.Map;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

  private final AuthService authService;
  private final CurrentUserService currentUserService;

  public AuthController(AuthService authService, CurrentUserService currentUserService) {
    this.authService = authService;
    this.currentUserService = currentUserService;
  }

  @PostMapping("/login")
  public AuthResponse login(@Valid @RequestBody LoginRequest request) {
    return authService.login(request);
  }

  @PostMapping("/user/login")
  public AuthResponse loginUser(@Valid @RequestBody LoginRequest request) {
    return authService.loginUser(request);
  }

  @PostMapping("/admin/login")
  public AuthResponse loginAdmin(@Valid @RequestBody LoginRequest request) {
    return authService.loginAdmin(request);
  }

  @PostMapping("/register")
  public Map<String, String> register(@Valid @RequestBody RegisterRequest request) {
    return authService.register(request);
  }

  @PostMapping({"/forgot-password", "/quen-mat-khau"})
  public Map<String, String> forgotPassword(@Valid @RequestBody ForgotPasswordRequest request) {
    return authService.forgotPassword(request);
  }

  @GetMapping("/me")
  public UserResponse me() {
    return authService.me(currentUserService.requireAccount());
  }

  @PostMapping({"/change-password", "/doi-mat-khau"})
  public Map<String, String> changePassword(@Valid @RequestBody ChangePasswordRequest request) {
    return authService.changePassword(currentUserService.requireAccount(), request);
  }
}
