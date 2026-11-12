package com.coachbooking.dto;

import static org.junit.jupiter.api.Assertions.*;

import com.coachbooking.dto.auth.ChangePasswordRequest;
import com.coachbooking.dto.auth.ForgotPasswordRequest;
import com.coachbooking.dto.auth.LoginRequest;
import com.coachbooking.dto.auth.RegisterRequest;
import com.coachbooking.dto.booking.BookingRequest;
import com.coachbooking.dto.invoice.LookupRequest;
import com.coachbooking.dto.profile.UpdateProfileRequest;
import java.time.LocalDate;
import java.util.List;
import java.util.UUID;
import org.junit.jupiter.api.Test;

class DtoTest {

  @Test
  void testAuthDtos() {
    LoginRequest login = new LoginRequest("user", "pass");
    assertEquals("user", login.username());
    assertEquals("pass", login.password());

    RegisterRequest reg = new RegisterRequest("user", "user@test.com", "pass", "pass");
    assertEquals("user", reg.username());
    assertEquals("user@test.com", reg.email());
    assertEquals("pass", reg.password());
    assertEquals("pass", reg.confirmPassword());

    ChangePasswordRequest cp = new ChangePasswordRequest("old", "new");
    assertEquals("old", cp.oldPassword());
    assertEquals("new", cp.newPassword());

    ForgotPasswordRequest fp = new ForgotPasswordRequest("user", "user@test.com");
    assertEquals("user", fp.username());
    assertEquals("user@test.com", fp.email());
  }

  @Test
  void testBookingAndProfileAndInvoiceDtos() {
    UUID token = UUID.randomUUID();
    BookingRequest br =
        new BookingRequest(
            1, List.of(1, 2), "Name", "0901234567", LocalDate.now(), true, "Cash", token);
    assertEquals(1, br.tripId());
    assertEquals(List.of(1, 2), br.seatNumbers());
    assertEquals("Name", br.customerName());
    assertEquals("0901234567", br.phone());
    assertEquals(true, br.gender());
    assertEquals("Cash", br.paymentMethod());
    assertEquals(token, br.holdToken());

    LookupRequest lr = new LookupRequest(10, "0901234567");
    assertEquals(10, lr.invoiceId());
    assertEquals("0901234567", lr.phone());

    UpdateProfileRequest up =
        new UpdateProfileRequest("Name", "0901234567", LocalDate.now(), false, "Address");
    assertEquals("Name", up.fullName());
    assertEquals("0901234567", up.phone());
    assertEquals(false, up.gender());
    assertEquals("Address", up.address());
  }
}
