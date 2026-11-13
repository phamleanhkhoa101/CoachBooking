package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Account;
import com.coachbooking.dto.profile.ProfileResponse;
import com.coachbooking.dto.profile.UpdateProfileRequest;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.ProfileService;
import java.time.LocalDate;
import org.junit.jupiter.api.Test;

class ProfileControllerTest {

  @Test
  void getAndUpdateDelegateToService() {
    ProfileService profileService = mock(ProfileService.class);
    CurrentUserService currentUserService = mock(CurrentUserService.class);
    ProfileController controller = new ProfileController(profileService, currentUserService);

    Account account = new Account();
    when(currentUserService.requireAccount()).thenReturn(account);

    ProfileResponse resp =
        new ProfileResponse(
            "CUSTOMER",
            1,
            null,
            "Name",
            LocalDate.now(),
            "0901234567",
            true,
            null,
            "e@test.com",
            "user",
            "KhachHang");
    when(profileService.get(account)).thenReturn(resp);

    UpdateProfileRequest req =
        new UpdateProfileRequest("New Name", "0901234567", LocalDate.now(), true, null);
    when(profileService.update(account, req)).thenReturn(resp);

    assertEquals("CUSTOMER", controller.get().type());
    assertEquals("Name", controller.update(req).fullName());
  }
}
