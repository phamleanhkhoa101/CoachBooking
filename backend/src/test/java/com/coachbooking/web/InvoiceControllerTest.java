package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.domain.Account;
import com.coachbooking.dto.invoice.InvoiceDetail;
import com.coachbooking.dto.invoice.InvoiceRow;
import com.coachbooking.dto.invoice.LookupRequest;
import com.coachbooking.dto.invoice.PayRequest;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.InvoiceService;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class InvoiceControllerTest {

  private InvoiceService invoiceService;
  private CurrentUserService currentUserService;
  private InvoiceController controller;

  private Account account;

  @BeforeEach
  void setUp() {
    invoiceService = mock(InvoiceService.class);
    currentUserService = mock(CurrentUserService.class);
    controller = new InvoiceController(invoiceService, currentUserService);

    account = new Account();
    account.setAccountId(1);
    when(currentUserService.requireAccount()).thenReturn(account);
  }

  @Test
  void listAndMyInvoices() {
    InvoiceRow row =
        new InvoiceRow(
            1,
            "Nguyen A",
            "Tran B",
            LocalDateTime.now(),
            BigDecimal.valueOf(100_000),
            true,
            1L,
            "Cash");
    when(invoiceService.list(any(), any(), any())).thenReturn(List.of(row));
    when(invoiceService.myInvoices(account)).thenReturn(List.of(row));

    List<InvoiceRow> list =
        controller.list(null, null, LocalDate.now(), LocalDate.now(), true, null);
    assertEquals(1, list.size());

    List<InvoiceRow> myInvoices = controller.myInvoices();
    assertEquals(1, myInvoices.size());
  }

  @Test
  void detailPayCancelAndLookup() {
    InvoiceDetail detail =
        new InvoiceDetail(
            1,
            "Nguyen A",
            "0901234567",
            null,
            LocalDateTime.now(),
            BigDecimal.valueOf(100_000),
            false,
            List.of(),
            null);
    when(invoiceService.detail(1, account)).thenReturn(detail);
    when(invoiceService.pay(1, "Cash", account)).thenReturn(Map.of("message", "ok"));
    when(invoiceService.cancel(1, account)).thenReturn(Map.of("message", "cancelled"));
    when(invoiceService.lookup(1, "0901234567")).thenReturn(detail);

    assertEquals(1, controller.detail(1).invoiceId());
    assertEquals("ok", controller.pay(1, new PayRequest("Cash")).get("message"));
    assertEquals("cancelled", controller.cancel(1).get("message"));
    assertEquals(1, controller.lookup(new LookupRequest(1, "0901234567")).invoiceId());
  }
}
