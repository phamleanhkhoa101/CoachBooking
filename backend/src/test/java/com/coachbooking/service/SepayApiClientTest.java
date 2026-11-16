package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.config.SepayProperties;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.validation.Validation;
import jakarta.validation.ValidatorFactory;
import java.net.http.*;
import java.nio.charset.StandardCharsets;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.*;
import org.mockito.ArgumentCaptor;

class SepayApiClientTest {
  static ValidatorFactory factory;
  HttpClient http;
  HttpResponse<String> response;
  SepayApiClient client;
  static final Instant NOW = Instant.parse("2030-01-01T00:00:00Z");
  static final String ORDER =
      """
    {"data":{"order_invoice_number":"CBK_TEST","order_status":"CAPTURED","order_amount":"150000.00","order_currency":"VND",
    "transactions":[{"id":"tx-1","payment_method":"BANK_TRANSFER","transaction_type":"PAYMENT","transaction_amount":"150000","transaction_currency":"VND","transaction_status":"APPROVED","card_number":"ignored"}],"customer_id":"ignored"}}
    """;

  @BeforeAll
  static void beforeAll() {
    factory = Validation.buildDefaultValidatorFactory();
  }

  @AfterAll
  static void afterAll() {
    factory.close();
  }

  @BeforeEach
  @SuppressWarnings("unchecked")
  void setup() throws Exception {
    http = mock(HttpClient.class);
    response = mock(HttpResponse.class);
    when(response.statusCode()).thenReturn(200);
    when(response.body()).thenReturn(ORDER);
    when(response.headers()).thenReturn(HttpHeaders.of(Map.of(), (a, b) -> true));
    when(http.send(any(HttpRequest.class), any(HttpResponse.BodyHandler.class)))
        .thenReturn(response);
    SepayProperties config = new SepayProperties();
    config.setMerchantId("test-merchant");
    config.setSecretKey("test-secret");
    client =
        new SepayApiClient(
            config,
            new ObjectMapper(),
            factory.getValidator(),
            Clock.fixed(NOW, ZoneOffset.UTC),
            http);
  }

  @Test
  void usesServerOnlyBasicAuthFixedHostTimeoutAndInvoiceReference() throws Exception {
    var result = client.lookup("CBK_TEST");
    assertNull(result.error());
    assertEquals("CAPTURED", result.order().orderStatus());
    assertEquals("tx-1", result.order().transactions().get(0).id());
    var request = ArgumentCaptor.forClass(HttpRequest.class);
    verify(http).send(request.capture(), any());
    assertEquals(
        "https://pgapi-sandbox.sepay.vn/v1/order/detail/CBK_TEST",
        request.getValue().uri().toString());
    assertEquals("GET", request.getValue().method());
    assertEquals(Duration.ofSeconds(5), request.getValue().timeout().orElseThrow());
    assertEquals(
        "Basic "
            + Base64.getEncoder()
                .encodeToString("test-merchant:test-secret".getBytes(StandardCharsets.UTF_8)),
        request.getValue().headers().firstValue("Authorization").orElseThrow());
  }

  @Test
  void pendingAndNotYetCreatedAreNotPaymentFailures() {
    when(response.body()).thenReturn(ORDER.replace("CAPTURED", "AUTHENTICATION_NOT_NEEDED"));
    assertNull(client.lookup("CBK_TEST").error());
    when(response.statusCode()).thenReturn(404);
    var result = client.lookup("CBK_TEST");
    assertNull(result.error());
    assertNull(result.order());
  }

  @Test
  void authenticationAndServerErrorsNeverExposeBodyOrCredentials() {
    when(response.body()).thenReturn("secret-credential-with-private-customer-data");
    for (int status : List.of(401, 403, 500, 502, 302)) {
      when(response.statusCode()).thenReturn(status);
      var result = client.lookup("CBK_TEST");
      assertNotNull(result.error());
      assertFalse(result.error().contains("secret"));
      assertNull(result.order());
    }
  }

  @Test
  void rateLimitSupportsSecondsDateAndInvalidHeader() {
    when(response.statusCode()).thenReturn(429);
    when(response.headers())
        .thenReturn(HttpHeaders.of(Map.of("Retry-After", List.of("120")), (a, b) -> true));
    assertEquals(120L, client.lookup("CBK_TEST").retryAfterSeconds());
    when(response.headers())
        .thenReturn(
            HttpHeaders.of(
                Map.of("Retry-After", List.of("Tue, 1 Jan 2030 00:02:00 GMT")), (a, b) -> true));
    assertEquals(120L, client.lookup("CBK_TEST").retryAfterSeconds());
    when(response.headers())
        .thenReturn(HttpHeaders.of(Map.of("Retry-After", List.of("invalid")), (a, b) -> true));
    assertEquals(60L, client.lookup("CBK_TEST").retryAfterSeconds());
  }

  @Test
  void malformedOrIncompleteResponseIsNotConfirmation() {
    for (String body :
        List.of(
            "{broken",
            "null",
            "{}",
            "{\"data\":null}",
            ORDER.replace("150000.00", "not-money"),
            ORDER.replace("CBK_TEST", ""),
            ORDER.replace("tx-1", ""))) {
      when(response.body()).thenReturn(body);
      assertNotNull(client.lookup("CBK_TEST").error());
    }
  }

  @Test
  void timeoutAndInterruptedCallsAreSafe() throws Exception {
    doThrow(new HttpTimeoutException("private endpoint"))
        .when(http)
        .send(any(HttpRequest.class), any(HttpResponse.BodyHandler.class));
    assertNotNull(client.lookup("CBK_TEST").error());
    doThrow(new InterruptedException())
        .when(http)
        .send(any(HttpRequest.class), any(HttpResponse.BodyHandler.class));
    try {
      assertNotNull(client.lookup("CBK_TEST").error());
      assertTrue(Thread.currentThread().isInterrupted());
    } finally {
      Thread.interrupted();
    }
  }
}
