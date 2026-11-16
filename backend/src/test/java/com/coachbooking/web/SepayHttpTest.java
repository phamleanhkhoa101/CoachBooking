package com.coachbooking.web;

import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;
import static org.springframework.security.test.web.servlet.request.SecurityMockMvcRequestPostProcessors.user;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

import com.coachbooking.config.DataInitializer;
import com.coachbooking.domain.Account;
import com.coachbooking.dto.payment.SepayCheckout;
import com.coachbooking.dto.payment.SepayStatus;
import com.coachbooking.service.CurrentUserService;
import com.coachbooking.service.SepayPollingService;
import com.coachbooking.service.SepayService;
import java.math.BigDecimal;
import java.time.Instant;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

/** Uses the application SecurityFilterChain, not a permissive replacement. */
@SpringBootTest(
    properties = {
      "spring.datasource.url=jdbc:h2:mem:sepayhttp;MODE=PostgreSQL;DB_CLOSE_DELAY=-1",
      "spring.datasource.driver-class-name=org.h2.Driver",
      "spring.datasource.username=sa",
      "spring.datasource.password=",
      "spring.jpa.hibernate.ddl-auto=create-drop",
      "spring.flyway.enabled=false",
      "app.jwt.secret=test-only-auth-signing-key-0123456789abcdef0123456789"
    })
@AutoConfigureMockMvc
class SepayHttpTest {
  @Autowired MockMvc mvc;
  @MockitoBean DataInitializer initializer;
  @MockitoBean SepayService sepay;
  @MockitoBean SepayPollingService polling;
  @MockitoBean CurrentUserService currentUser;
  static final String ID = "1d5ca891-63ce-4409-9bd3-ac5846233469";
  static final String PAYLOAD =
      """
    {"notification_type":"ORDER_PAID","order":{"order_invoice_number":"CBK_TEST","order_status":"CAPTURED","order_amount":"150000.00","order_currency":"VND"},
     "transaction":{"id":"transaction-1","transaction_status":"APPROVED","transaction_type":"PAYMENT","transaction_amount":"150000","transaction_currency":"VND","payment_method":"BANK_TRANSFER"}}
    """;

  @Test
  void ipnIsPublicButUsesIndependentSecretAndParsesSnakeCase() throws Exception {
    mvc.perform(
            post("/api/payments/sepay/ipn")
                .header("X-Secret-Key", "test-key")
                .contentType("application/json")
                .content(PAYLOAD))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.success").value(true));
    verify(sepay).authenticateIpn("test-key");
    verify(sepay)
        .receive(
            argThat(
                ipn ->
                    ipn.order().orderInvoiceNumber().equals("CBK_TEST")
                        && ipn.transaction().transactionAmount().compareTo(new BigDecimal("150000"))
                            == 0));
    verifyNoInteractions(currentUser);
  }

  @Test
  void wrongOrMissingSecretIsRejected() throws Exception {
    doThrow(
            new com.coachbooking.common.ApiException(
                org.springframework.http.HttpStatus.UNAUTHORIZED, "IPN không được phép."))
        .when(sepay)
        .authenticateIpn(nullable(String.class));
    mvc.perform(post("/api/payments/sepay/ipn").contentType("application/json").content(PAYLOAD))
        .andExpect(status().isUnauthorized());
    mvc.perform(
            post("/api/payments/sepay/ipn")
                .header("X-Secret-Key", "wrong")
                .contentType("application/json")
                .content(PAYLOAD))
        .andExpect(status().isUnauthorized());
    verify(sepay, never()).receive(any());
  }

  @Test
  void malformedIpnAndGetCannotUsePublicPostException() throws Exception {
    mvc.perform(post("/api/payments/sepay/ipn").contentType("application/json").content("{}"))
        .andExpect(status().isBadRequest());
    mvc.perform(
            post("/api/payments/sepay/ipn")
                .contentType("application/json")
                .content(PAYLOAD.replace("150000.00", "not-money")))
        .andExpect(status().isBadRequest());
    mvc.perform(get("/api/payments/sepay/ipn")).andExpect(status().isUnauthorized());
    verify(sepay, never()).receive(any());
  }

  @Test
  void checkoutAndStatusRequireCustomerRole() throws Exception {
    mvc.perform(post("/api/payments/sepay/" + ID + "/sync")).andExpect(status().isUnauthorized());
    mvc.perform(get("/api/payments/sepay/" + ID)).andExpect(status().isUnauthorized());
    mvc.perform(post("/api/payments/sepay/" + ID + "/checkout"))
        .andExpect(status().isUnauthorized());
    for (String role : List.of("Admin", "NhanVien")) {
      mvc.perform(post("/api/payments/sepay/" + ID + "/sync").with(user("staff").roles(role)))
          .andExpect(status().isForbidden());
      mvc.perform(get("/api/payments/sepay/" + ID).with(user("staff").roles(role)))
          .andExpect(status().isForbidden());
      mvc.perform(post("/api/payments/sepay/" + ID + "/checkout").with(user("staff").roles(role)))
          .andExpect(status().isForbidden());
    }
    verifyNoInteractions(sepay);
    verifyNoInteractions(polling);
  }

  @Test
  void authenticatedCustomerCanReadStatusAndGetForm() throws Exception {
    Account owner = new Account();
    owner.setAccountId(7);
    when(currentUser.requireAccount()).thenReturn(owner);
    when(polling.sync(UUID.fromString(ID), owner))
        .thenReturn(
            new SepayStatus(
                UUID.fromString(ID),
                12,
                "PENDING",
                BigDecimal.TEN,
                Instant.parse("2030-01-01T00:10:00Z"),
                "POLLING",
                null,
                null));
    mvc.perform(
            post("/api/payments/sepay/" + ID + "/sync").with(user("customer").roles("KhachHang")))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.confirmationMode").value("POLLING"));
    when(sepay.status(UUID.fromString(ID), owner))
        .thenReturn(
            new SepayStatus(
                UUID.fromString(ID),
                12,
                "PENDING",
                BigDecimal.TEN,
                Instant.parse("2030-01-01T00:10:00Z")));
    when(sepay.checkout(UUID.fromString(ID), owner))
        .thenReturn(
            new SepayCheckout(
                "https://pay-sandbox.sepay.vn/v1/checkout/init",
                List.of(new SepayCheckout.Field("signature", "signed"))));
    mvc.perform(get("/api/payments/sepay/" + ID).with(user("customer").roles("KhachHang")))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.paymentStatus").value("PENDING"));
    mvc.perform(
            post("/api/payments/sepay/" + ID + "/checkout")
                .with(user("customer").roles("KhachHang")))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.fields[0].name").value("signature"));
  }
}
