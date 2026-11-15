package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import jakarta.mail.Session;
import jakarta.mail.internet.MimeMessage;
import java.util.Optional;
import java.util.Properties;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.mail.javamail.JavaMailSender;

class EmailServiceTest {

  @Test
  void throwsWhenMailDisabled() {
    EmailService service =
        new EmailService(Optional.empty(), false, "no-reply@test.com", "CoachBooking");
    ApiException ex =
        assertThrows(
            ApiException.class,
            () -> service.sendPasswordResetOtp("user@test.com", "user12", "123456", 15));
    assertEquals(HttpStatus.SERVICE_UNAVAILABLE, ex.getStatus());
  }

  @Test
  void sendsHtmlEmailWhenEnabled() {
    JavaMailSender sender = mock(JavaMailSender.class);
    MimeMessage mime = new MimeMessage(Session.getInstance(new Properties()));
    when(sender.createMimeMessage()).thenReturn(mime);

    EmailService service =
        new EmailService(Optional.of(sender), true, "no-reply@test.com", "CoachBooking");
    service.sendPasswordResetOtp("user@test.com", "user12", "654321", 15);

    verify(sender).send(mime);
  }
}
