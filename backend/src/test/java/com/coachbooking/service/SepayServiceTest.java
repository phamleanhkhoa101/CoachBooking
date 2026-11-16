package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.config.SepayProperties;
import com.coachbooking.domain.*;
import com.coachbooking.dto.payment.*;
import com.coachbooking.repo.*;
import java.math.BigDecimal;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;
import org.springframework.http.HttpStatus;

class SepayServiceTest {
  private static final Instant NOW = Instant.parse("2030-01-01T00:00:00Z");
  private final SepaySessionRepository sessions = mock(SepaySessionRepository.class);
  private final SepayNotificationRepository notifications = mock(SepayNotificationRepository.class);
  private final TripRepository trips = mock(TripRepository.class);
  private final InvoiceRepository invoices = mock(InvoiceRepository.class);
  private final TicketRepository tickets = mock(TicketRepository.class);
  private final PaymentRepository payments = mock(PaymentRepository.class);
  private final SeatStatusService seatStatus = mock(SeatStatusService.class);
  private SepayProperties config;
  private SepayService service;
  private SepaySession session;
  private Account owner;
  private Trip trip;
  private Invoice invoice;

  @BeforeEach
  void setup() {
    config = new SepayProperties();
    config.setEnabled(true);
    config.setMerchantId("TEST_MERCHANT");
    config.setSecretKey("test-merchant-secret");
    config.setIpnSecret("test-ipn-secret");
    config.setFrontendUrl("https://tickets.example.com/");
    service =
        new SepayService(
            sessions,
            notifications,
            trips,
            invoices,
            tickets,
            payments,
            seatStatus,
            config,
            Clock.fixed(NOW, ZoneOffset.UTC));
    owner = new Account();
    owner.setAccountId(7);
    trip = new Trip();
    trip.setTripId(9);
    trip.setDepartureTime(
        LocalDateTime.ofInstant(NOW.plusSeconds(3600), ZoneId.of("Asia/Ho_Chi_Minh")));
    invoice = new Invoice();
    invoice.setInvoiceId(12);
    invoice.setTotalAmount(new BigDecimal("300000.00"));
    session = new SepaySession();
    session.setSessionId(UUID.randomUUID());
    session.setInvoiceId(12);
    session.setTripId(9);
    session.setAccountId(7);
    session.setHoldToken(UUID.randomUUID());
    session.setSeatNumbers("1,2");
    session.setTicketCount(2);
    session.setAmount(invoice.getTotalAmount());
    session.setReference("CBK_" + session.getSessionId());
    session.setStatus("PENDING");
    session.setExpiresAt(NOW.plusSeconds(600));
    when(sessions.findTripId(session.getSessionId())).thenReturn(Optional.of(9));
    when(trips.findByIdForUpdate(9)).thenReturn(Optional.of(trip));
    when(sessions.findByIdForUpdate(session.getSessionId())).thenReturn(Optional.of(session));
    when(sessions.findSessionIdByReference(session.getReference()))
        .thenReturn(Optional.of(session.getSessionId()));
    when(sessions.findSessionIdByInvoiceId(12)).thenReturn(Optional.of(session.getSessionId()));
    when(invoices.findById(12)).thenReturn(Optional.of(invoice));
    when(tickets.findByInvoiceId(12)).thenReturn(List.of(ticket(1), ticket(2)));
  }

  private Ticket ticket(int seat) {
    Ticket ticket = new Ticket();
    ticket.setTripId(9);
    ticket.setSeatNumber(seat);
    ticket.setPrice(new BigDecimal("150000"));
    return ticket;
  }

  private SepayIpn ipn(String type, String transaction, BigDecimal amount) {
    return new SepayIpn(
        type,
        new SepayIpn.Order(session.getReference(), "CAPTURED", amount, "VND"),
        new SepayIpn.Transaction(
            transaction, "APPROVED", "PAYMENT", amount, "VND", "BANK_TRANSFER"));
  }

  private SepayIpn success() {
    return ipn("ORDER_PAID", "txn-1", new BigDecimal("300000"));
  }

  @Test
  void createsUnpaidSessionWithServerAmountAndDeadline() {
    var result = service.create(invoice, owner, trip, session.getHoldToken(), List.of(2, 1));
    assertFalse(result.isPaid());
    assertEquals("PENDING", result.paymentStatus());
    assertEquals(NOW.plusSeconds(600), result.expiresAt());
    assertEquals(invoice.getTotalAmount(), result.totalAmount());
    verify(sessions)
        .save(
            argThat(s -> s.getSeatNumbers().equals("1,2") && s.getReference().startsWith("CBK_")));
    verifyNoInteractions(payments);
  }

  @Test
  void deadlineNeverExceedsDepartureAndPastDepartureRejected() {
    trip.setDepartureTime(
        LocalDateTime.ofInstant(NOW.plusSeconds(120), ZoneId.of("Asia/Ho_Chi_Minh")));
    assertEquals(
        NOW.plusSeconds(120),
        service.create(invoice, owner, trip, session.getHoldToken(), List.of(1)).expiresAt());
    trip.setDepartureTime(LocalDateTime.ofInstant(NOW, ZoneId.of("Asia/Ho_Chi_Minh")));
    assertEquals(
        HttpStatus.CONFLICT,
        assertThrows(
                ApiException.class,
                () -> service.create(invoice, owner, trip, session.getHoldToken(), List.of(1)))
            .getStatus());
  }

  @Test
  void configurationFailsClosed() {
    config.setEnabled(false);
    assertEquals(
        HttpStatus.SERVICE_UNAVAILABLE,
        assertThrows(ApiException.class, service::requireAvailable).getStatus());
    config.setEnabled(true);
    config.setEnvironment("production");
    assertThrows(ApiException.class, service::requireAvailable);
    config.setEnvironment("sandbox");
    config.setMerchantId("");
    assertThrows(ApiException.class, service::requireAvailable);
    config.setMerchantId("merchant");
    config.setSecretKey("");
    assertThrows(ApiException.class, service::requireAvailable);
    config.setSecretKey("secret");
    config.setIpnSecret("");
    assertThrows(ApiException.class, service::requireAvailable);
  }

  @ParameterizedTest
  @ValueSource(
      strings = {
        "",
        "http://localhost:3000",
        "https://localhost",
        "https://127.0.0.1",
        "https://user:pass@example.com",
        "https://example.com?a=b",
        "https://example.com/#x",
        "::invalid"
      })
  void invalidCallbackRejected(String url) {
    config.setFrontendUrl(url);
    assertThrows(ApiException.class, service::requireAvailable);
  }

  @Test
  void signsCheckoutInDocumentedOrderAndDoesNotExtendDeadline() {
    var checkout = service.checkout(session.getSessionId(), owner);
    assertEquals("https://pay-sandbox.sepay.vn/v1/checkout/init", checkout.checkoutUrl());
    var fields = checkout.fields();
    assertEquals(
        List.of(
            "order_amount",
            "merchant",
            "currency",
            "operation",
            "order_description",
            "order_invoice_number",
            "customer_id",
            "payment_method",
            "success_url",
            "error_url",
            "cancel_url",
            "lang",
            "signature"),
        fields.stream().map(SepayCheckout.Field::name).toList());
    String expectedSigned =
        "order_amount=300000,merchant=TEST_MERCHANT,currency=VND,operation=PURCHASE,"
            + "order_description=Thanh toán vé CoachBooking #12,order_invoice_number="
            + session.getReference()
            + ",customer_id=CBK_7,payment_method=BANK_TRANSFER,success_url=https://tickets.example.com/thanh-toan/sepay/"
            + session.getSessionId()
            + ",error_url=https://tickets.example.com/thanh-toan/sepay/"
            + session.getSessionId()
            + ",cancel_url=https://tickets.example.com/thanh-toan/sepay/"
            + session.getSessionId();
    // Independent JDK HMAC calculation checks all fields, separators, encoding and Base64.
    try {
      var mac = javax.crypto.Mac.getInstance("HmacSHA256");
      mac.init(
          new javax.crypto.spec.SecretKeySpec(
              "test-merchant-secret".getBytes(java.nio.charset.StandardCharsets.UTF_8),
              "HmacSHA256"));
      assertEquals(
          Base64.getEncoder()
              .encodeToString(
                  mac.doFinal(expectedSigned.getBytes(java.nio.charset.StandardCharsets.UTF_8))),
          fields.get(12).value());
    } catch (java.security.GeneralSecurityException ex) {
      fail(ex);
    }
    assertEquals(NOW.plusSeconds(600), session.getExpiresAt());
    assertEquals(checkout, service.checkout(session.getSessionId(), owner));
  }

  @Test
  void authenticatesSeparateIpnSecret() {
    assertDoesNotThrow(() -> service.authenticateIpn("test-ipn-secret"));
    assertEquals(
        HttpStatus.UNAUTHORIZED,
        assertThrows(ApiException.class, () -> service.authenticateIpn("test-merchant-secret"))
            .getStatus());
    assertThrows(ApiException.class, () -> service.authenticateIpn(null));
    config.setIpnSecret("");
    assertThrows(ApiException.class, () -> service.authenticateIpn(""));
  }

  @Test
  void statusRequiresOwnershipAndExistingSession() {
    assertEquals("PENDING", service.status(session.getSessionId(), owner).paymentStatus());
    owner.setAccountId(8);
    assertEquals(
        HttpStatus.FORBIDDEN,
        assertThrows(ApiException.class, () -> service.status(session.getSessionId(), owner))
            .getStatus());
    assertEquals(
        HttpStatus.NOT_FOUND,
        assertThrows(ApiException.class, () -> service.status(UUID.randomUUID(), owner))
            .getStatus());
    when(trips.findByIdForUpdate(9)).thenReturn(Optional.empty());
    assertEquals(
        HttpStatus.NOT_FOUND,
        assertThrows(ApiException.class, () -> service.status(session.getSessionId(), owner))
            .getStatus());
  }

  @Test
  void paidIpnUpdatesInvoiceAndPaymentExactlyOnce() {
    service.receive(success());
    assertEquals("PAID", session.getStatus());
    assertTrue(invoice.getIsPaid());
    assertEquals("txn-1", session.getProviderTransactionId());
    verify(payments)
        .save(
            argThat(
                p ->
                    p.getInvoiceId().equals(12)
                        && p.getAmount().compareTo(new BigDecimal("300000")) == 0));
    service.receive(success());
    verify(payments, times(1)).save(any());
    verify(notifications, times(2)).save(any());
  }

  @Test
  void storedDuplicateIsAcknowledgedWithoutWrites() {
    when(notifications.existsById(anyString())).thenReturn(true);
    service.receive(success());
    verifyNoInteractions(payments);
    verify(notifications, never()).save(any());
    assertEquals("PENDING", session.getStatus());
  }

  @Test
  void unknownOrderIsAudited() {
    when(sessions.findSessionIdByReference(anyString())).thenReturn(Optional.empty());
    service.receive(success());
    verify(notifications)
        .save(argThat(n -> n.getSessionId() == null && n.getOutcome().equals("UNKNOWN_ORDER")));
    verifyNoInteractions(payments);
  }

  @Test
  void mismatchedAmountReleasesUnpaidSeatsAndRequiresReview() {
    service.receive(ipn("ORDER_PAID", "wrong-amount", BigDecimal.ONE));
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    assertFalse(invoice.getIsPaid());
    verify(tickets).deleteByInvoiceId(12);
    verifyNoInteractions(payments);
    verify(notifications).save(argThat(n -> n.getOutcome().equals("INVALID_PAYMENT")));
  }

  @Test
  void wrongCurrencyStatusAndMethodCannotConfirmPayment() {
    for (SepayIpn bad :
        List.of(
            new SepayIpn(
                "ORDER_PAID",
                new SepayIpn.Order(session.getReference(), "CREATED", session.getAmount(), "VND"),
                success().transaction()),
            new SepayIpn(
                "ORDER_PAID",
                success().order(),
                new SepayIpn.Transaction(
                    "txn-bad", "DECLINED", "PAYMENT", session.getAmount(), "VND", "BANK_TRANSFER")),
            new SepayIpn(
                "ORDER_PAID",
                success().order(),
                new SepayIpn.Transaction(
                    "txn-bad", "APPROVED", "PAYMENT", session.getAmount(), "USD", "BANK_TRANSFER")),
            new SepayIpn(
                "ORDER_PAID",
                success().order(),
                new SepayIpn.Transaction(
                    "txn-bad", "APPROVED", "PAYMENT", session.getAmount(), "VND", "CARD")))) {
      session.setStatus("PENDING");
      service.receive(bad);
      assertEquals("REVIEW_REQUIRED", session.getStatus());
    }
    verifyNoInteractions(payments);
  }

  @Test
  void expiredOrCancelledPaymentNeverReissuesSeats() {
    session.setExpiresAt(NOW);
    service.receive(success());
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    verify(tickets, times(1)).deleteByInvoiceId(12);
    verifyNoInteractions(payments);
    session.setStatus("CANCELLED");
    service.receive(ipn("ORDER_PAID", "txn-2", session.getAmount()));
    verify(tickets, times(1)).deleteByInvoiceId(12);
    verify(tickets, never()).saveAllAndFlush(any());
  }

  @Test
  void differentSecondPaymentRequiresReviewWithoutDeletingPaidTickets() {
    service.receive(success());
    service.receive(ipn("ORDER_PAID", "txn-2", session.getAmount()));
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    assertTrue(invoice.getIsPaid());
    verify(payments, times(1)).save(any());
    verify(tickets, never()).deleteByInvoiceId(any());
  }

  @Test
  void transactionAlreadyUsedElsewhereRequiresReview() {
    when(sessions.existsByProviderTransactionId("txn-1")).thenReturn(true);
    service.receive(success());
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    verifyNoInteractions(payments);
  }

  @Test
  void invoiceAndSeatConflictPreventsPayment() {
    when(tickets.findByInvoiceId(12)).thenReturn(List.of(ticket(1)));
    service.receive(success());
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    verify(notifications).save(argThat(n -> n.getOutcome().equals("INVOICE_CONFLICT")));
    verifyNoInteractions(payments);
  }

  @Test
  void voidRequiresReviewForPaidInvoiceAndOtherNotificationsAreIgnored() {
    service.receive(ipn("OTHER_EVENT", "txn-1", session.getAmount()));
    assertEquals("PENDING", session.getStatus());
    service.receive(success());
    service.receive(ipn("TRANSACTION_VOID", "txn-1", session.getAmount()));
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    assertTrue(invoice.getIsPaid());
    verify(tickets, never()).deleteByInvoiceId(any());
  }

  @Test
  void expiryIsIdempotentAndCheckoutRejectsExpiredSession() {
    session.setExpiresAt(NOW);
    assertEquals("EXPIRED", service.status(session.getSessionId(), owner).paymentStatus());
    service.expireSession(session.getSessionId());
    assertThrows(ApiException.class, () -> service.checkout(session.getSessionId(), owner));
    verify(tickets, times(1)).deleteByInvoiceId(12);
    verify(seatStatus).publishAfterCommit(9);
    verify(invoices, never()).delete(any());
  }

  @Test
  void cancelRetainsInvoiceAndRejectsPaidSession() {
    assertTrue(service.cancelInvoice(12));
    assertEquals("CANCELLED", session.getStatus());
    assertTrue(service.cancelInvoice(12));
    verify(tickets, times(1)).deleteByInvoiceId(12);
    assertFalse(service.cancelInvoice(99));
    session.setStatus("PAID");
    assertThrows(ApiException.class, () -> service.cancelInvoice(12));
    session.setStatus("REVIEW_REQUIRED");
    session.setProviderTransactionId("paid-txn");
    assertThrows(ApiException.class, () -> service.cancelInvoice(12));
    verify(invoices, never()).delete(any());
  }

  @Test
  void replayMatchesOriginalOwnerTripAndSeatsWithoutCreatingAnotherSession() {
    when(sessions.findByHoldToken(session.getHoldToken())).thenReturn(Optional.of(session));
    assertTrue(service.replay(owner, null, 9, List.of(1, 2), "Chuyển khoản").isEmpty());
    var response =
        service
            .replay(owner, session.getHoldToken(), 9, List.of(2, 1), "Chuyển khoản")
            .orElseThrow();
    assertEquals(session.getSessionId(), response.paymentSessionId());
    assertThrows(
        ApiException.class,
        () -> service.replay(owner, session.getHoldToken(), 9, List.of(1), "Chuyển khoản"));
    assertThrows(
        ApiException.class,
        () -> service.replay(owner, session.getHoldToken(), 8, List.of(1, 2), "Chuyển khoản"));
    assertThrows(
        ApiException.class,
        () -> service.replay(owner, session.getHoldToken(), 9, List.of(1, 2), "Tiền mặt"));
    verify(sessions, never()).save(any());
  }

  @Test
  void cleanupDelegatesEachExpiredSessionThroughTransactionalService() {
    when(sessions.findByStatusAndExpiresAtLessThanEqual("PENDING", NOW))
        .thenReturn(List.of(session));
    SepayService worker = mock(SepayService.class);
    new SepayCleanupService(sessions, worker, Clock.fixed(NOW, ZoneOffset.UTC)).cleanup();
    verify(worker).expireSession(session.getSessionId());
  }

  private void enablePolling() {
    var environment = new org.springframework.mock.env.MockEnvironment();
    environment.setActiveProfiles("local");
    config.setRuntimeEnvironment(environment);
    config.setConfirmationMode("POLLING");
    config.setLocalReturnUrl("http://localhost:3000");
    session.setConfirmationMode("POLLING");
    session.setCreatedAt(NOW);
  }

  private SepayLookupResult captured(SepayIpn.Transaction... transactions) {
    return new SepayLookupResult(
        new SepayOrderResponse.Order(
            session.getReference(), "CAPTURED", session.getAmount(), "VND", List.of(transactions)),
        null,
        null);
  }

  @Test
  void localPollingDoesNotRequireIpnSecretAndSignsPublicBridge() {
    enablePolling();
    config.setIpnSecret("");
    assertDoesNotThrow(service::requireAvailable);
    config.validateConfirmationMode();
    var fields = service.checkout(session.getSessionId(), owner).fields();
    assertEquals(
        "https://tickets.example.com/sepay-local-return.html?sessionId="
            + session.getSessionId()
            + "&port=3000",
        fields.stream()
            .filter(f -> f.name().equals("success_url"))
            .findFirst()
            .orElseThrow()
            .value());
    config.setLocalReturnUrl("http://localhost:5173");
    assertTrue(
        service.checkout(session.getSessionId(), owner).fields().stream()
            .anyMatch(f -> f.name().equals("cancel_url") && f.value().endsWith("&port=5173")));
    var created = service.create(invoice, owner, trip, session.getHoldToken(), List.of(1, 2));
    assertFalse(created.isPaid());
    verify(sessions)
        .save(
            argThat(
                s ->
                    s.getConfirmationMode().equals("POLLING")
                        && s.getCreatedAt().equals(NOW)
                        && s.getNextCheckAt().equals(NOW)));
  }

  @Test
  void pollingConfigurationFailsOutsideLocalAndForArbitraryReturnUrls() {
    config.setConfirmationMode("POLLING");
    assertThrows(IllegalStateException.class, config::validateConfirmationMode);
    assertThrows(ApiException.class, service::requireAvailable);
    enablePolling();
    for (String target :
        List.of(
            "https://evil.example",
            "http://localhost.evil:3000",
            "http://localhost:3000/path",
            "http://127.0.0.1:3000",
            "http://localhost:8080",
            "http://localhost:3000?url=https://evil.example")) {
      config.setLocalReturnUrl(target);
      assertThrows(IllegalStateException.class, config::validateConfirmationMode);
    }
    config.setLocalReturnUrl("http://localhost:3000");
    config.setEnvironment("production");
    assertThrows(IllegalStateException.class, config::validateConfirmationMode);
    config.setConfirmationMode("BAD");
    assertThrows(IllegalStateException.class, config::validateConfirmationMode);
    config.setConfirmationMode("IPN");
    assertDoesNotThrow(config::validateConfirmationMode);
  }

  @Test
  void syncRequiresModeOwnershipAndRespectsLeaseAndReconciliationDeadline() {
    assertThrows(ApiException.class, () -> service.claimSync(session.getSessionId(), owner));
    enablePolling();
    session.setConfirmationMode("IPN");
    assertThrows(ApiException.class, () -> service.claimSync(session.getSessionId(), owner));
    session.setConfirmationMode("POLLING");
    owner.setAccountId(99);
    assertEquals(
        HttpStatus.FORBIDDEN,
        assertThrows(ApiException.class, () -> service.claimSync(session.getSessionId(), owner))
            .getStatus());
    owner.setAccountId(7);
    var claim = service.claimSync(session.getSessionId(), owner).orElseThrow();
    assertEquals(session.getReference(), claim.reference());
    assertTrue(service.claimSync(session.getSessionId(), owner).isEmpty());
    assertEquals(NOW.plusSeconds(15), session.getNextCheckAt());
    session.setNextCheckAt(null);
    session.setStatus("PAID");
    assertTrue(service.claimSync(session.getSessionId(), owner).isEmpty());
    session.setStatus("PENDING");
    session.setCreatedAt(NOW.minus(Duration.ofHours(24)));
    assertTrue(service.claimSync(session.getSessionId(), owner).isEmpty());
  }

  @Test
  void pollingAndIpnUseSamePaymentRulesAndDedupe() {
    enablePolling();
    var claim = service.claimSync(session.getSessionId(), owner).orElseThrow();
    service.completeSync(claim, captured(success().transaction()));
    assertEquals("PAID", session.getStatus());
    assertTrue(invoice.getIsPaid());
    assertEquals(NOW, session.getLastCheckedAt());
    assertNull(session.getNextCheckAt());
    verify(notifications)
        .save(argThat(n -> n.getSource().equals("POLLING") && n.getOutcome().equals("PAID")));
    service.receive(success());
    verify(payments, times(1)).save(any());
    verify(notifications)
        .save(
            argThat(
                n -> n.getSource().equals("IPN") && n.getOutcome().equals("DUPLICATE_PAYMENT")));
  }

  @Test
  void staleLeaseCannotOverwriteNewerSync() {
    enablePolling();
    var claim = service.claimSync(session.getSessionId(), owner).orElseThrow();
    session.setSyncLeaseToken(UUID.randomUUID());
    service.completeSync(claim, captured(success().transaction()));
    assertEquals("PENDING", session.getStatus());
    verifyNoInteractions(payments);
    assertNull(session.getLastCheckedAt());
  }

  @Test
  void syncErrorBackoffPreservesSeatsAndHonorsRetryAfter() {
    enablePolling();
    for (int failure = 1; failure <= 4; failure++) {
      session.setNextCheckAt(null);
      var claim = service.claimSync(session.getSessionId(), owner).orElseThrow();
      service.completeSync(claim, new SepayLookupResult(null, "Không kết nối được SePay", null));
      assertEquals(NOW.plusSeconds(Math.min(60, 15L << (failure - 1))), session.getNextCheckAt());
      assertEquals(failure, session.getSyncFailures());
    }
    session.setNextCheckAt(null);
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(),
        new SepayLookupResult(null, "Rate limited", 120L));
    assertEquals(NOW.plusSeconds(120), session.getNextCheckAt());
    assertEquals("PENDING", session.getStatus());
    verify(tickets, never()).deleteByInvoiceId(any());
    session.setNextCheckAt(null);
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(),
        new SepayLookupResult(null, null, null));
    assertEquals(0, session.getSyncFailures());
    assertNull(session.getLastCheckError());
  }

  @Test
  void snapshotWithoutApprovedTransactionsOrWrongReferenceNeverPays() {
    enablePolling();
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(), captured());
    assertNotNull(session.getLastCheckError());
    assertEquals("PENDING", session.getStatus());
    session.setNextCheckAt(null);
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(),
        new SepayLookupResult(
            new SepayOrderResponse.Order(
                "wrong-order",
                "CAPTURED",
                session.getAmount(),
                "VND",
                List.of(success().transaction())),
            null,
            null));
    assertTrue(session.getLastCheckError().contains("không khớp"));
    verifyNoInteractions(payments);
  }

  @Test
  void providerCancellationReleasesSeatsButCallbackDoesNot() {
    enablePolling();
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(),
        new SepayLookupResult(
            new SepayOrderResponse.Order(
                session.getReference(), "CANCELLED", session.getAmount(), "VND", null),
            null,
            null));
    assertEquals("CANCELLED", session.getStatus());
    verify(tickets).deleteByInvoiceId(12);
    assertEquals(NOW.plusSeconds(60), session.getNextCheckAt());
  }

  @Test
  void pollingLateOrAdditionalPaymentNeedsReconciliation() {
    enablePolling();
    session.setStatus("CANCELLED");
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(),
        captured(success().transaction()));
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    verifyNoInteractions(payments);
    session.setStatus("PENDING");
    session.setNextCheckAt(null);
    service.completeSync(
        service.claimSync(session.getSessionId(), owner).orElseThrow(),
        captured(
            new SepayIpn.Transaction(
                "txn-a", "APPROVED", "PAYMENT", session.getAmount(), "VND", "BANK_TRANSFER"),
            new SepayIpn.Transaction(
                "txn-b", "APPROVED", "PAYMENT", session.getAmount(), "VND", "BANK_TRANSFER")));
    assertEquals("REVIEW_REQUIRED", session.getStatus());
    verify(payments, times(1)).save(any());
  }
}
