package com.coachbooking.service;

import com.coachbooking.config.SepayProperties;
import com.coachbooking.domain.Account;
import com.coachbooking.dto.payment.SepayStatus;
import com.coachbooking.repo.SepaySessionRepository;
import java.time.*;
import java.util.UUID;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.PageRequest;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

/** Network calls run outside database transactions and outside the seat-expiry scheduler. */
@Service
public class SepayPollingService {
  private static final Logger LOG = LoggerFactory.getLogger(SepayPollingService.class);
  private final SepayService sepay;
  private final SepayApiClient client;
  private final SepaySessionRepository sessions;
  private final SepayProperties config;
  private final Clock clock;

  public SepayPollingService(
      SepayService sepay,
      SepayApiClient client,
      SepaySessionRepository sessions,
      SepayProperties config,
      Clock clock) {
    this.sepay = sepay;
    this.client = client;
    this.sessions = sessions;
    this.config = config;
    this.clock = clock;
  }

  public SepayStatus sync(UUID id, Account actor) {
    check(id, actor);
    return sepay.status(id, actor);
  }

  private void check(UUID id, Account actor) {
    sepay
        .claimSync(id, actor)
        .ifPresent(claim -> sepay.completeSync(claim, client.lookup(claim.reference())));
  }

  @Scheduled(fixedDelay = 1000, scheduler = "sepayPollingScheduler")
  public void pollDueSessions() {
    if (!config.isEnabled() || !config.isPollingEnabled()) return;
    for (UUID id :
        sessions.findDuePollingIds(
            clock.instant(), clock.instant().minus(Duration.ofHours(24)), PageRequest.of(0, 20))) {
      if (Thread.currentThread().isInterrupted()) break;
      try {
        check(id, null);
      } catch (RuntimeException ex) {
        // Do not log external response bodies, credentials or personal data.
        LOG.warn(
            "SePay polling could not process session {} ({})", id, ex.getClass().getSimpleName());
      }
    }
  }
}
