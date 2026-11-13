package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Account;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.InvoiceService;
import java.util.Map;
import org.junit.jupiter.api.Test;

class TicketControllerTest {

  @Test
  void cancelTicketDelegatesToService() {
    InvoiceService invoiceService = mock(InvoiceService.class);
    CurrentUserService currentUserService = mock(CurrentUserService.class);
    TicketController controller = new TicketController(invoiceService, currentUserService);

    Account account = new Account();
    when(currentUserService.requireAccount()).thenReturn(account);
    when(invoiceService.cancelTicket(10, account)).thenReturn(Map.of("message", "ok"));

    Map<String, String> res = controller.cancelTicket(10);
    assertEquals("ok", res.get("message"));
  }
}
