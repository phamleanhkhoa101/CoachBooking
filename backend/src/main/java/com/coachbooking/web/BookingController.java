package com.coachbooking.web;

import com.coachbooking.service.BookingService;
import com.coachbooking.service.CurrentUserService;
import jakarta.validation.Valid;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/dat-ve")
public class BookingController {

  private final BookingService bookingService;
  private final CurrentUserService currentUserService;

  public BookingController(BookingService bookingService, CurrentUserService currentUserService) {
    this.bookingService = bookingService;
    this.currentUserService = currentUserService;
  }

  @PostMapping
  @PreAuthorize("isAuthenticated()")
  public BookingService.DatVeResponse datVe(
      @Valid @RequestBody BookingService.DatVeRequest request) {
    return bookingService.datVe(currentUserService.requireTaiKhoan(), request);
  }
}
