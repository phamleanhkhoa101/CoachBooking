package com.coachbooking.web;

import java.util.Map;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import jakarta.validation.Valid;
import com.coachbooking.service.AuthService;
import com.coachbooking.service.CurrentUserService;

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
    public AuthService.AuthResponse login(@Valid @RequestBody AuthService.LoginRequest request) {
        return authService.login(request);
    }

    @PostMapping("/register")
    public Map<String, String> register(@Valid @RequestBody AuthService.RegisterRequest request) {
        return authService.register(request);
    }

    @PostMapping("/quen-mat-khau")
    public Map<String, String> forgotPassword(@Valid @RequestBody AuthService.ForgotPasswordRequest request) {
        return authService.forgotPassword(request);
    }

    @GetMapping("/me")
    public AuthService.NguoiDung me() {
        return authService.me(currentUserService.requireTaiKhoan());
    }

    @PostMapping("/doi-mat-khau")
    public Map<String, String> changePassword(@Valid @RequestBody AuthService.ChangePasswordRequest request) {
        return authService.changePassword(currentUserService.requireTaiKhoan(), request);
    }
}
