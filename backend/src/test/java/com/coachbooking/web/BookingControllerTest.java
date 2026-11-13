package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Account;
import com.coachbooking.dto.booking.BookingRequest;
import com.coachbooking.dto.booking.BookingResponse;
import com.coachbooking.service.BookingService;
import com.coachbooking.service.CurrentUserService;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;
import org.junit.jupiter.api.Test;

class BookingControllerTest {

  @Test
  void bookTicketsDelegatesToService() {
    BookingService bookingService = mock(BookingService.class);
    CurrentUserService currentUserService = mock(CurrentUserService.class);
    BookingController controller = new BookingController(bookingService, currentUserService);

    Account account = new Account();
    account.setAccountId(1);
    when(currentUserService.requireAccount()).thenReturn(account);

    BookingRequest req =
        new BookingRequest(
            10, List.of(1), "Nguyen", "0901234567", LocalDate.now(), true, "Cash", null);
    BookingResponse resp = new BookingResponse(100, BigDecimal.valueOf(100_000), 1, "Cash", true);
    when(bookingService.bookTickets(account, req)).thenReturn(resp);

    BookingResponse actual = controller.bookTickets(req);
    assertEquals(100, actual.invoiceId());
  }
}
