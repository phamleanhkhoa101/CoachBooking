package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

import com.coachbooking.config.SepayProperties;
import com.coachbooking.domain.Account;
import com.coachbooking.dto.payment.*;
import com.coachbooking.repo.SepaySessionRepository;
import java.math.BigDecimal;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.mock.env.MockEnvironment;

class SepayPollingServiceTest {
  @Test
  void workerRunsWithoutBrowserAndContinuesAfterOneTransactionFails() {
    var sessions = mock(SepaySessionRepository.class);
    var sepay = mock(SepayService.class);
    var client = mock(SepayApiClient.class);
    var config = new SepayProperties();
    var environment = new MockEnvironment();
    environment.setActiveProfiles("local");
    config.setRuntimeEnvironment(environment);
    config.setConfirmationMode("POLLING");
    config.setEnabled(true);
    UUID bad = UUID.randomUUID(), good = UUID.randomUUID();
    var claim = new SepaySyncClaim(good, UUID.randomUUID(), "CBK_TEST");
    var result = new SepayLookupResult(null, null, null);
    when(sessions.findDuePollingIds(any(), any(), any())).thenReturn(List.of(bad, good));
    when(sepay.claimSync(bad, null)).thenThrow(new IllegalStateException("storage error"));
    when(sepay.claimSync(good, null)).thenReturn(Optional.of(claim));
    when(client.lookup("CBK_TEST")).thenReturn(result);
    var worker = new SepayPollingService(sepay, client, sessions, config, Clock.systemUTC());
    worker.pollDueSessions();
    verify(sepay).completeSync(claim, result);
    config.setEnabled(false);
    clearInvocations(sessions);
    worker.pollDueSessions();
    verifyNoInteractions(sessions);
    config.setEnabled(true);
    config.setConfirmationMode("IPN");
    worker.pollDueSessions();
    verifyNoInteractions(sessions);
  }

  @Test
  void manualSyncHonorsClaimThrottle() {
    var sepay = mock(SepayService.class);
    var client = mock(SepayApiClient.class);
    var owner = new Account();
    UUID id = UUID.randomUUID();
    var status = new SepayStatus(id, 12, "PENDING", BigDecimal.TEN, Instant.now());
    when(sepay.claimSync(id, owner)).thenReturn(Optional.empty());
    when(sepay.status(id, owner)).thenReturn(status);
    var worker =
        new SepayPollingService(
            sepay,
            client,
            mock(SepaySessionRepository.class),
            new SepayProperties(),
            Clock.systemUTC());
    assertEquals(status, worker.sync(id, owner));
    verifyNoInteractions(client);
  }
}
