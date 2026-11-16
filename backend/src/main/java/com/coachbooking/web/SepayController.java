package com.coachbooking.web;

import com.coachbooking.dto.payment.*;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.SepayPollingService;
import com.coachbooking.service.SepayService;
import jakarta.validation.Valid;
import java.util.Map;
import java.util.UUID;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/payments/sepay")
public class SepayController {
  private final SepayService sepay;
  private final CurrentUserService currentUser;
  private final SepayPollingService polling;

  public SepayController(
      SepayService sepay, CurrentUserService currentUser, SepayPollingService polling) {
    this.sepay = sepay;
    this.currentUser = currentUser;
    this.polling = polling;
  }

  @PostMapping("/{sessionId}/sync")
  @PreAuthorize("hasRole('KhachHang')")
  public SepayStatus sync(@PathVariable UUID sessionId) {
    return polling.sync(sessionId, currentUser.requireAccount());
  }

  @GetMapping("/{sessionId}")
  @PreAuthorize("hasRole('KhachHang')")
  public SepayStatus status(@PathVariable UUID sessionId) {
    return sepay.status(sessionId, currentUser.requireAccount());
  }

  @PostMapping("/{sessionId}/checkout")
  @PreAuthorize("hasRole('KhachHang')")
  public SepayCheckout checkout(@PathVariable UUID sessionId) {
    return sepay.checkout(sessionId, currentUser.requireAccount());
  }

  @PostMapping("/ipn")
  public Map<String, Boolean> ipn(
      @RequestHeader(value = "X-Secret-Key", required = false) String key,
      @Valid @RequestBody SepayIpn body) {
    sepay.authenticateIpn(key);
    sepay.receive(body);
    return Map.of("success", true);
  }
}
