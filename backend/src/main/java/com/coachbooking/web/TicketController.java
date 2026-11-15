package com.coachbooking.web;

import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.InvoiceService;
import io.swagger.v3.oas.annotations.tags.Tag;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/tickets", "/api/ve"})
@Tag(name = "Vé xe", description = "Tra cứu chi tiết vé và hủy vé đủ điều kiện")
public class TicketController {

  private final InvoiceService invoiceService;
  private final CurrentUserService currentUserService;

  public TicketController(InvoiceService invoiceService, CurrentUserService currentUserService) {
    this.invoiceService = invoiceService;
    this.currentUserService = currentUserService;
  }

  @PostMapping({"/{id}/cancel", "/{id}/huy"})
  @PreAuthorize("isAuthenticated()")
  public Map<String, String> cancelTicket(@PathVariable Integer id) {
    return invoiceService.cancelTicket(id, currentUserService.requireAccount());
  }
}
