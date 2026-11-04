package com.coachbooking.web;

import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.ProfileService;
import jakarta.validation.Valid;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/ho-so")
@PreAuthorize("isAuthenticated()")
public class ProfileController {

  private final ProfileService profileService;
  private final CurrentUserService currentUserService;

  public ProfileController(ProfileService profileService, CurrentUserService currentUserService) {
    this.profileService = profileService;
    this.currentUserService = currentUserService;
  }

  @GetMapping
  public ProfileService.ProfileResponse get() {
    return profileService.get(currentUserService.requireTaiKhoan());
  }

  @PutMapping
  public ProfileService.ProfileResponse update(
      @Valid @RequestBody ProfileService.UpdateProfileRequest request) {
    return profileService.update(currentUserService.requireTaiKhoan(), request);
  }
}
