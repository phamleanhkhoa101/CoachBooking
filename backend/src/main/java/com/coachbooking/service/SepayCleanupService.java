package com.coachbooking.service;

import com.coachbooking.repo.SepaySessionRepository;
import java.time.Clock;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

@Service
public class SepayCleanupService {
  private final SepaySessionRepository sessions;
  private final SepayService sepay;
  private final Clock clock;

  public SepayCleanupService(SepaySessionRepository sessions, SepayService sepay, Clock clock) {
    this.sessions = sessions;
    this.sepay = sepay;
    this.clock = clock;
  }

  @Scheduled(fixedDelay = 60_000)
  public void cleanup() {
    for (var session : sessions.findByStatusAndExpiresAtLessThanEqual("PENDING", clock.instant())) {
      sepay.expireSession(session.getSessionId());
    }
  }
}
