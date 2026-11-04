package com.coachbooking.web;

import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.InvoiceService;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api")
public class HoaDonController {

  private final InvoiceService invoiceService;
  private final CurrentUserService currentUserService;

  public HoaDonController(InvoiceService invoiceService, CurrentUserService currentUserService) {
    this.invoiceService = invoiceService;
    this.currentUserService = currentUserService;
  }

  @GetMapping("/hoa-don")
  @PreAuthorize("hasAnyRole('Admin','NhanVien')")
  public List<InvoiceService.HoaDonRow> list(
      @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate tu,
      @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate den,
      @RequestParam(required = false) Boolean trangThai) {
    return invoiceService.list(tu, den, trangThai);
  }

  @GetMapping("/hoa-don/cua-toi")
  @PreAuthorize("isAuthenticated()")
  public List<InvoiceService.HoaDonRow> cuaToi() {
    return invoiceService.myInvoices(currentUserService.requireTaiKhoan());
  }

  @GetMapping("/hoa-don/{id}")
  @PreAuthorize("isAuthenticated()")
  public InvoiceService.HoaDonDetail detail(@PathVariable Integer id) {
    return invoiceService.detail(id, currentUserService.requireTaiKhoan());
  }

  @PostMapping("/hoa-don/{id}/thanh-toan")
  @PreAuthorize("isAuthenticated()")
  public Map<String, String> thanhToan(
      @PathVariable Integer id, @Valid @RequestBody PayRequest request) {
    return invoiceService.pay(id, request.phuongThuc(), currentUserService.requireTaiKhoan());
  }

  @PostMapping("/hoa-don/{id}/huy")
  @PreAuthorize("isAuthenticated()")
  public Map<String, String> huy(@PathVariable Integer id) {
    return invoiceService.cancel(id, currentUserService.requireTaiKhoan());
  }

  @PostMapping("/ve/{maVe}/huy")
  @PreAuthorize("isAuthenticated()")
  public Map<String, String> huyVe(@PathVariable Integer maVe) {
    return invoiceService.cancelTicket(maVe, currentUserService.requireTaiKhoan());
  }

  public record PayRequest(
      @NotBlank(message = "Vui lòng chọn phương thức thanh toán.") String phuongThuc) {}
}
