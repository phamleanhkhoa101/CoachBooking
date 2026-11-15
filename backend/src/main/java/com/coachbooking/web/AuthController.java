package com.coachbooking.web;

import com.coachbooking.dto.auth.AuthResponse;
import com.coachbooking.dto.auth.ChangePasswordRequest;
import com.coachbooking.dto.auth.ForgotPasswordRequest;
import com.coachbooking.dto.auth.LoginRequest;
import com.coachbooking.dto.auth.RegisterRequest;
import com.coachbooking.dto.auth.ResetPasswordRequest;
import com.coachbooking.dto.auth.UserResponse;
import com.coachbooking.dto.auth.VerifyResetOtpRequest;
import com.coachbooking.service.AuthService;
import com.coachbooking.service.CurrentUserService;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import java.util.Map;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth")
@Tag(name = "Xác thực", description = "Đăng nhập, đăng ký, đăng xuất và quên mật khẩu qua OTP")
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
  public Map<String, String> changePassword(@Valid @RequestBody ChangePasswordRequest request) {
    return authService.changePassword(currentUserService.requireAccount(), request);
  }
}
