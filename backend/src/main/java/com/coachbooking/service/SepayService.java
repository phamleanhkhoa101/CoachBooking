package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.config.SepayProperties;
import com.coachbooking.domain.*;
import com.coachbooking.dto.booking.BookingResponse;
import com.coachbooking.dto.payment.*;
import com.coachbooking.repo.*;
import java.math.BigDecimal;
import java.net.URI;
import java.nio.charset.StandardCharsets;
import java.security.*;
import java.time.*;
import java.util.*;
import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SepayService {
  private static final ZoneId BUSINESS_ZONE = ZoneId.of("Asia/Ho_Chi_Minh");
  private final SepaySessionRepository sessions;
  private final SepayNotificationRepository notifications;
  private final TripRepository trips;
  private final InvoiceRepository invoices;
  private final TicketRepository tickets;
  private final PaymentRepository payments;
  private final SeatStatusService seatStatus;
  private final SepayProperties config;
  private final Clock clock;

  public SepayService(
      SepaySessionRepository sessions,
      SepayNotificationRepository notifications,
      TripRepository trips,
      InvoiceRepository invoices,
      TicketRepository tickets,
      PaymentRepository payments,
      SeatStatusService seatStatus,
      SepayProperties config,
      Clock clock) {
    this.sessions = sessions;
    this.notifications = notifications;
    this.trips = trips;
    this.invoices = invoices;
    this.tickets = tickets;
    this.payments = payments;
    this.seatStatus = seatStatus;
    this.config = config;
    this.clock = clock;
  }

  public void requireAvailable() {
    if (!config.isEnabled()
        || !"sandbox".equals(config.getEnvironment())
        || config.getMerchantId().isBlank()
        || config.getSecretKey().isBlank()
        || (!"IPN".equals(config.getConfirmationMode()) && !config.isPollingEnabled())
        || ("IPN".equals(config.getConfirmationMode()) && config.getIpnSecret().isBlank())
        || !validFrontendUrl()) {
      throw new ApiException(
          HttpStatus.SERVICE_UNAVAILABLE,
          "Thanh toán SePay sandbox chưa được cấu hình. Vui lòng thử lại sau.");
    }
  }

  private boolean validFrontendUrl() {
    try {
      URI uri = URI.create(config.getFrontendUrl());
      return "https".equals(uri.getScheme())
          && uri.getHost() != null
          && !Set.of("localhost", "127.0.0.1", "::1").contains(uri.getHost())
          && uri.getUserInfo() == null
          && uri.getQuery() == null
          && uri.getFragment() == null;
    } catch (IllegalArgumentException ex) {
      return false;
    }
  }

  // Caller already holds the trip lock. Replays must match the original immutable booking.
  public Optional<BookingResponse> replay(
      Account actor, UUID holdToken, Integer tripId, List<Integer> seats, String paymentMethod) {
    if (holdToken == null) return Optional.empty();
    return sessions
        .findByHoldToken(holdToken)
        .map(
            session -> {
              requireOwner(session, actor);
              if (!session.getTripId().equals(tripId)
                  || !session.getSeatNumbers().equals(seatKey(seats))
                  || !"Chuyển khoản".equals(paymentMethod)) {
                throw ApiException.conflict("Yêu cầu không khớp với lượt đặt vé ban đầu.");
              }
              expire(session);
              return bookingResponse(session);
            });
  }

  public BookingResponse create(
      Invoice invoice, Account actor, Trip trip, UUID holdToken, List<Integer> seats) {
    requireAvailable();
    SepaySession session = new SepaySession();
    session.setSessionId(UUID.randomUUID());
    session.setInvoiceId(invoice.getInvoiceId());
    session.setAccountId(actor.getAccountId());
    session.setTripId(trip.getTripId());
    session.setHoldToken(holdToken);
    session.setSeatNumbers(seatKey(seats));
    session.setTicketCount(seats.size());
    session.setAmount(invoice.getTotalAmount());
    session.setReference("CBK_" + session.getSessionId());
    session.setStatus("PENDING");
    session.setConfirmationMode(config.getConfirmationMode());
    session.setCreatedAt(clock.instant());
    if (config.isPollingEnabled()) session.setNextCheckAt(clock.instant());
    Instant departure = trip.getDepartureTime().atZone(BUSINESS_ZONE).toInstant();
    Instant deadline = clock.instant().plus(Duration.ofMinutes(10));
    session.setExpiresAt(departure.isBefore(deadline) ? departure : deadline);
    if (!session.getExpiresAt().isAfter(clock.instant())) {
      throw ApiException.conflict("Chuyến xe đã khởi hành.");
    }
    sessions.save(session);
    return bookingResponse(session);
  }

  private String seatKey(List<Integer> seats) {
    return seats.stream()
        .sorted()
        .map(String::valueOf)
        .collect(java.util.stream.Collectors.joining(","));
  }

  private BookingResponse bookingResponse(SepaySession session) {
    return new BookingResponse(
        session.getInvoiceId(),
        session.getAmount(),
        session.getTicketCount(),
        "Chuyển khoản",
        "PAID".equals(session.getStatus()),
        session.getSessionId(),
        session.getStatus(),
        session.getExpiresAt());
  }

  @Transactional
  public SepayStatus status(UUID id, Account actor) {
    SepaySession session = lock(id);
    requireOwner(session, actor);
    expire(session);
    return SepayStatus.from(session);
  }

  // Returning 409 must not roll back an expiry and seat release.
  @Transactional(noRollbackFor = ApiException.class)
  public SepayCheckout checkout(UUID id, Account actor) {
    requireAvailable();
    SepaySession session = lock(id);
    requireOwner(session, actor);
    expire(session);
    if (!"PENDING".equals(session.getStatus())) {
      throw ApiException.conflict("Phiên thanh toán không còn hiệu lực.");
    }
    String callback =
        config.getFrontendUrl().replaceAll("/+$", "")
            + "/thanh-toan/sepay/"
            + session.getSessionId();
    if ("POLLING".equals(session.getConfirmationMode())) {
      if (!config.isPollingEnabled())
        throw ApiException.conflict("Phiên này cần cấu hình polling local.");
      callback =
          config.getFrontendUrl().replaceAll("/+$", "")
              + "/sepay-local-return.html?sessionId="
              + session.getSessionId()
              + "&port="
              + URI.create(config.getLocalReturnUrl()).getPort();
    }
    // SePay signs only these fields, in this exact order, using unescaped values.
    LinkedHashMap<String, String> fields = new LinkedHashMap<>();
    fields.put("order_amount", session.getAmount().stripTrailingZeros().toPlainString());
    fields.put("merchant", config.getMerchantId());
    fields.put("currency", "VND");
    fields.put("operation", "PURCHASE");
    fields.put("order_description", "Thanh toán vé CoachBooking #" + session.getInvoiceId());
    fields.put("order_invoice_number", session.getReference());
    fields.put("customer_id", "CBK_" + session.getAccountId());
    fields.put("payment_method", "BANK_TRANSFER");
    fields.put("success_url", callback);
    fields.put("error_url", callback);
    fields.put("cancel_url", callback);
    String signed =
        fields.entrySet().stream()
            .map(e -> e.getKey() + "=" + e.getValue())
            .collect(java.util.stream.Collectors.joining(","));
    fields.put("lang", "vi");
    fields.put("signature", sign(signed));
    return new SepayCheckout(
        "https://pay-sandbox.sepay.vn/v1/checkout/init",
        fields.entrySet().stream()
            .map(e -> new SepayCheckout.Field(e.getKey(), e.getValue()))
            .toList());
  }

  private String sign(String value) {
    try {
      Mac mac = Mac.getInstance("HmacSHA256");
      mac.init(
          new SecretKeySpec(config.getSecretKey().getBytes(StandardCharsets.UTF_8), "HmacSHA256"));
      return Base64.getEncoder()
          .encodeToString(mac.doFinal(value.getBytes(StandardCharsets.UTF_8)));
    } catch (GeneralSecurityException ex) {
      throw new IllegalStateException("Không thể ký yêu cầu thanh toán.", ex);
    }
  }

  public void authenticateIpn(String secret) {
    if (config.getIpnSecret().isBlank()
        || secret == null
        || !MessageDigest.isEqual(
            config.getIpnSecret().getBytes(StandardCharsets.UTF_8),
            secret.getBytes(StandardCharsets.UTF_8))) {
      throw new ApiException(HttpStatus.UNAUTHORIZED, "IPN không được phép.");
    }
  }

  @Transactional
  public void receive(SepayIpn ipn) {
    UUID id = sessions.findSessionIdByReference(ipn.order().orderInvoiceNumber()).orElse(null);
    SepaySession session = id == null ? null : lock(id);
    recordNotification(session, ipn, "IPN");
  }

  private void recordNotification(SepaySession session, SepayIpn ipn, String source) {
    String eventKey = eventKey(source, ipn);
    if (notifications.existsById(eventKey)) return;
    String outcome = "UNKNOWN_ORDER";
    if (session != null) {
      expire(session);
      outcome = applyPayment(session, ipn);
    }
    SepayNotification notification = new SepayNotification();
    notification.setEventKey(eventKey);
    notification.setSessionId(session == null ? null : session.getSessionId());
    notification.setSource(source);
    notification.setReference(ipn.order().orderInvoiceNumber());
    notification.setProviderTransactionId(ipn.transaction().id());
    notification.setNotificationType(ipn.notificationType());
    notification.setAmount(ipn.transaction().transactionAmount());
    notification.setCurrency(ipn.transaction().transactionCurrency());
    notification.setOutcome(outcome);
    notification.setReceivedAt(clock.instant());
    notifications.save(notification);
  }

  private String applyPayment(SepaySession session, SepayIpn ipn) {
    if (!"ORDER_PAID".equals(ipn.notificationType())) {
      if ("TRANSACTION_VOID".equals(ipn.notificationType()) && "PAID".equals(session.getStatus())) {
        session.setStatus("REVIEW_REQUIRED");
      }
      return "IGNORED_NOTIFICATION";
    }
    boolean valid =
        "CAPTURED".equals(ipn.order().orderStatus())
            && "APPROVED".equals(ipn.transaction().transactionStatus())
            && "PAYMENT".equals(ipn.transaction().transactionType())
            && "BANK_TRANSFER".equals(ipn.transaction().paymentMethod())
            && "VND".equals(ipn.order().orderCurrency())
            && "VND".equals(ipn.transaction().transactionCurrency())
            && sameAmount(session.getAmount(), ipn.order().orderAmount())
            && sameAmount(session.getAmount(), ipn.transaction().transactionAmount());
    if (!valid) {
      review(session);
      return "INVALID_PAYMENT";
    }
    if (ipn.transaction().id().equals(session.getProviderTransactionId()))
      return "DUPLICATE_PAYMENT";
    if (!"PENDING".equals(session.getStatus())
        || sessions.existsByProviderTransactionId(ipn.transaction().id())) {
      review(session);
      return "REVIEW_REQUIRED";
    }
    Invoice invoice = invoices.findById(session.getInvoiceId()).orElseThrow();
    List<Ticket> reserved = tickets.findByInvoiceId(session.getInvoiceId());
    if (Boolean.TRUE.equals(invoice.getIsPaid())
        || reserved.size() != session.getTicketCount()
        || !reserved.stream().allMatch(t -> t.getTripId().equals(session.getTripId()))
        || !seatKey(reserved.stream().map(Ticket::getSeatNumber).toList())
            .equals(session.getSeatNumbers())
        || !sameAmount(
            session.getAmount(),
            reserved.stream().map(Ticket::getPrice).reduce(BigDecimal.ZERO, BigDecimal::add))) {
      review(session);
      return "INVOICE_CONFLICT";
    }
    Payment payment = new Payment();
    payment.setInvoiceId(session.getInvoiceId());
    payment.setAmount(session.getAmount());
    payment.setPaymentDate(LocalDateTime.ofInstant(clock.instant(), BUSINESS_ZONE));
    payment.setPaymentMethod("Chuyển khoản");
    payments.save(payment);
    invoice.setIsPaid(true);
    invoices.save(invoice);
    session.setProviderTransactionId(ipn.transaction().id());
    session.setStatus("PAID");
    return "PAID";
  }

  private boolean sameAmount(BigDecimal expected, BigDecimal actual) {
    return actual != null && expected.compareTo(actual) == 0;
  }

  private void review(SepaySession session) {
    // Review is terminal for seat allocation. Preserve paid tickets, release unpaid ones.
    if ("PENDING".equals(session.getStatus())) release(session);
    session.setStatus("REVIEW_REQUIRED");
  }

  private String eventKey(String source, SepayIpn ipn) {
    // Include status and amount so altered or subsequent notifications are still audited.
    try {
      String value = source + ":" + ipn;
      return HexFormat.of()
          .formatHex(
              MessageDigest.getInstance("SHA-256").digest(value.getBytes(StandardCharsets.UTF_8)));
    } catch (NoSuchAlgorithmException ex) {
      throw new IllegalStateException(ex);
    }
  }

  @Transactional
  public Optional<SepaySyncClaim> claimSync(UUID id, Account actor) {
    requireAvailable();
    if (!config.isPollingEnabled())
      throw ApiException.conflict("Đồng bộ SePay chỉ dùng cho polling local.");
    SepaySession session = lock(id);
    if (actor != null) requireOwner(session, actor);
    if (!"POLLING".equals(session.getConfirmationMode()))
      throw ApiException.conflict("Phiên này được xác nhận bằng IPN.");
    expire(session);
    Instant now = clock.instant();
    if ("PAID".equals(session.getStatus())
        || !session.getCreatedAt().plus(Duration.ofHours(24)).isAfter(now)
        || (session.getNextCheckAt() != null && session.getNextCheckAt().isAfter(now))) {
      return Optional.empty();
    }
    UUID lease = UUID.randomUUID();
    session.setSyncLeaseToken(lease);
    session.setNextCheckAt(now.plusSeconds(15));
    return Optional.of(new SepaySyncClaim(id, lease, session.getReference()));
  }

  @Transactional
  public void completeSync(SepaySyncClaim claim, SepayLookupResult result) {
    SepaySession session = lock(claim.sessionId());
    if (!claim.leaseToken().equals(session.getSyncLeaseToken())) return;
    expire(session);
    session.setSyncLeaseToken(null);
    session.setLastCheckedAt(clock.instant());
    session.setLastCheckError(result.error());
    if (result.error() == null) {
      session.setSyncFailures(0);
      if (result.order() != null) applyOrderSnapshot(session, result.order());
    }
    if (session.getLastCheckError() != null)
      session.setSyncFailures(Math.min(10, session.getSyncFailures() + 1));
    long delay = "PENDING".equals(session.getStatus()) ? 15 : 60;
    if (session.getLastCheckError() != null)
      delay = Math.min(60, 15L << Math.min(2, session.getSyncFailures() - 1));
    if (result.retryAfterSeconds() != null) delay = Math.max(delay, result.retryAfterSeconds());
    session.setNextCheckAt(
        "PAID".equals(session.getStatus()) ? null : clock.instant().plusSeconds(delay));
  }

  private void applyOrderSnapshot(SepaySession session, SepayOrderResponse.Order order) {
    if (!session.getReference().equals(order.orderInvoiceNumber())) {
      session.setLastCheckError("Mã đơn trả về từ SePay không khớp. Vui lòng kiểm tra cấu hình.");
      return;
    }
    if ("CAPTURED".equals(order.orderStatus())) {
      List<SepayIpn.Transaction> approved =
          order.transactions() == null
              ? List.of()
              : order.transactions().stream()
                  .filter(
                      t ->
                          "APPROVED".equals(t.transactionStatus())
                              && "PAYMENT".equals(t.transactionType()))
                  .toList();
      if (approved.isEmpty()) {
        session.setLastCheckError(
            "SePay báo đã thanh toán nhưng chưa có giao dịch hợp lệ. Hệ thống sẽ kiểm tra lại.");
        return;
      }
      for (var transaction : approved) {
        recordNotification(
            session,
            new SepayIpn(
                "ORDER_PAID",
                new SepayIpn.Order(
                    order.orderInvoiceNumber(),
                    order.orderStatus(),
                    order.orderAmount(),
                    order.orderCurrency()),
                transaction),
            "POLLING");
      }
    } else if ("CANCELLED".equals(order.orderStatus())
        && "PENDING".equals(session.getStatus())
        && "VND".equals(order.orderCurrency())
        && sameAmount(session.getAmount(), order.orderAmount())) {
      release(session);
      session.setStatus("CANCELLED");
    }
  }

  @Transactional
  public void expireSession(UUID id) {
    expire(lock(id));
  }

  @Transactional
  public boolean cancelInvoice(Integer invoiceId) {
    UUID id = sessions.findSessionIdByInvoiceId(invoiceId).orElse(null);
    if (id == null) return false;
    SepaySession session = lock(id);
    expire(session);
    if ("PAID".equals(session.getStatus()) || session.getProviderTransactionId() != null) {
      throw ApiException.conflict("Không thể hủy hóa đơn đã thanh toán.");
    }
    if ("PENDING".equals(session.getStatus())) {
      release(session);
      session.setStatus("CANCELLED");
    }
    return true;
  }

  private SepaySession lock(UUID id) {
    // Global order: trip -> session -> invoice. Query scalar IDs before taking locks
    // to avoid reading stale managed session state while waiting for another transaction.
    Integer tripId =
        sessions
            .findTripId(id)
            .orElseThrow(() -> ApiException.notFound("Phiên thanh toán không tồn tại."));
    trips
        .findByIdForUpdate(tripId)
        .orElseThrow(() -> ApiException.notFound("Chuyến xe không tồn tại."));
    return sessions
        .findByIdForUpdate(id)
        .orElseThrow(() -> ApiException.notFound("Phiên thanh toán không tồn tại."));
  }

  private void requireOwner(SepaySession session, Account actor) {
    if (!session.getAccountId().equals(actor.getAccountId())) {
      throw ApiException.forbidden("Bạn không có quyền xem phiên thanh toán này.");
    }
  }

  private void expire(SepaySession session) {
    if ("PENDING".equals(session.getStatus()) && !session.getExpiresAt().isAfter(clock.instant())) {
      release(session);
      session.setStatus("EXPIRED");
    }
  }

  private void release(SepaySession session) {
    tickets.deleteByInvoiceId(session.getInvoiceId());
    tickets.flush();
    seatStatus.publishAfterCommit(session.getTripId());
  }
}
