package com.coachbooking.web;

import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.InvoiceService;
import com.fasterxml.jackson.annotation.JsonAlias;
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
@RequestMapping({"/api/invoices", "/api/hoa-don"})
public class InvoiceController {

  private final InvoiceService invoiceService;
  private final CurrentUserService currentUserService;

  public InvoiceController(InvoiceService invoiceService, CurrentUserService currentUserService) {
    this.invoiceService = invoiceService;
    this.currentUserService = currentUserService;
  }

  @GetMapping
  @PreAuthorize("hasAnyRole('Admin','NhanVien')")
  public List<InvoiceService.InvoiceRow> list(
      @RequestParam(value = "tu", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate tu,
      @RequestParam(value = "den", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate den,
      @RequestParam(value = "fromDate", required = false)
          @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate fromDate,
      @RequestParam(value = "toDate", required = false)
          @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate toDate,
      @RequestParam(value = "isPaid", required = false) Boolean isPaid,
      @RequestParam(value = "trangThai", required = false) Boolean trangThai) {
    LocalDate actualFrom = fromDate != null ? fromDate : tu;
    LocalDate actualTo = toDate != null ? toDate : den;
    Boolean actualPaid = isPaid != null ? isPaid : trangThai;
    return invoiceService.list(actualFrom, actualTo, actualPaid);
  }

  @GetMapping({"/my", "/cua-toi"})
  @PreAuthorize("isAuthenticated()")
  public List<InvoiceService.InvoiceRow> myInvoices() {
    return invoiceService.myInvoices(currentUserService.requireAccount());
  }

  @GetMapping("/{id}")
  @PreAuthorize("isAuthenticated()")
  public InvoiceService.InvoiceDetail detail(@PathVariable Integer id) {
    return invoiceService.detail(id, currentUserService.requireAccount());
  }

  @PostMapping({"/{id}/pay", "/{id}/thanh-toan"})
  @PreAuthorize("isAuthenticated()")
  public Map<String, String> pay(@PathVariable Integer id, @Valid @RequestBody PayRequest request) {
    return invoiceService.pay(id, request.paymentMethod(), currentUserService.requireAccount());
  }

  @PostMapping({"/{id}/cancel", "/{id}/huy"})
  @PreAuthorize("isAuthenticated()")
  public Map<String, String> cancel(@PathVariable Integer id) {
    return invoiceService.cancel(id, currentUserService.requireAccount());
  }

  @PostMapping({"/lookup", "/tra-cuu"})
  public InvoiceService.InvoiceDetail lookup(@Valid @RequestBody LookupRequest request) {
    Integer id = request.invoiceId() != null ? request.invoiceId() : request.maHoaDon();
    String ph = request.phone() != null ? request.phone() : request.soDienThoai();
    return invoiceService.lookup(id, ph);
  }

  public record LookupRequest(
      @JsonAlias({"maHoaDon", "invoiceId"}) Integer invoiceId,
      @JsonAlias({"maHoaDon", "invoiceId"}) Integer maHoaDon,
      @JsonAlias({"sdt", "phone", "soDienThoai"}) String phone,
      @JsonAlias({"sdt", "phone", "soDienThoai"}) String soDienThoai) {}

  public record PayRequest(
      @NotBlank(message = "Vui lòng chọn phương thức thanh toán.")
          @JsonAlias({"phuongThuc", "paymentMethod"})
          String paymentMethod) {}
}
