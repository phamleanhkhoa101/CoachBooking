package com.coachbooking.web;

import com.coachbooking.dto.hold.SeatHoldRequest;
import com.coachbooking.dto.hold.SeatHoldResponse;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.SeatHoldService;
import jakarta.validation.Valid;
import java.util.UUID;
import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/seat-holds")
@PreAuthorize("hasRole('KhachHang')")
public class SeatHoldController {

  private final SeatHoldService seatHoldService;
  private final CurrentUserService currentUserService;

  public SeatHoldController(
      SeatHoldService seatHoldService, CurrentUserService currentUserService) {
    this.seatHoldService = seatHoldService;
    this.currentUserService = currentUserService;
  }

  @PostMapping
  @ResponseStatus(HttpStatus.CREATED)
  public SeatHoldResponse create(@Valid @RequestBody SeatHoldRequest request) {
    return seatHoldService.create(currentUserService.requireAccount(), request);
  }

  @GetMapping("/{token}")
  public SeatHoldResponse get(@PathVariable UUID token) {
    return seatHoldService.get(currentUserService.requireAccount(), token);
  }

  @DeleteMapping("/{token}")
  @ResponseStatus(HttpStatus.NO_CONTENT)
  public void release(@PathVariable UUID token) {
    seatHoldService.release(currentUserService.requireAccount(), token);
  }
}
