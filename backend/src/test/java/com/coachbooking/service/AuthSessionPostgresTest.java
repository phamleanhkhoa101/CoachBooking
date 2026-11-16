package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.repo.*;
import com.coachbooking.security.JwtService;
import java.time.*;
import java.util.concurrent.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.EnabledIfEnvironmentVariable;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.TestConfiguration;
import org.springframework.context.annotation.*;
import org.springframework.test.context.*;
import org.springframework.test.context.bean.override.mockito.MockitoSpyBean;
import org.springframework.transaction.annotation.*;

/** Only an explicitly supplied disposable auth-test database is accepted. */
@EnabledIfEnvironmentVariable(
    named = "AUTH_TEST_DB_URL",
    matches = "jdbc:postgresql://127\\.0\\.0\\.1:55439/coachbooking_auth_test")
@DataJpaTest(
    showSql = false,
    properties = {
      "spring.flyway.enabled=true",
      "spring.jpa.hibernate.ddl-auto=none",
      "app.jwt.secret=test-only-auth-signing-key-0123456789abcdef0123456789"
    })
@AutoConfigureTestDatabase(replace = AutoConfigureTestDatabase.Replace.NONE)
@Import({AuthSessionService.class, JwtService.class, AuthSessionPostgresTest.TimeConfig.class})
@Transactional(propagation = Propagation.NOT_SUPPORTED)
class AuthSessionPostgresTest {
  @DynamicPropertySource
  static void database(DynamicPropertyRegistry properties) {
    properties.add("spring.datasource.url", () -> System.getenv("AUTH_TEST_DB_URL"));
    properties.add("spring.datasource.username", () -> "auth_test");
    properties.add("spring.datasource.password", () -> "auth_test_local_only");
  }

  @TestConfiguration
  static class TimeConfig {
    @Bean
    Clock clock() {
      return Clock.systemUTC();
    }
  }

  @Autowired AuthSessionService service;
  @Autowired AccountRepository accounts;
  @Autowired AuthSessionRepository sessions;
  @Autowired JwtService jwt;
  @MockitoSpyBean AuthRefreshTokenRepository tokens;
  Account account;

  @BeforeEach
  void fixture() {
    reset(tokens);
    account = new Account();
    account.setUsername("auth_test_" + java.util.UUID.randomUUID());
    account.setEmail(account.getUsername() + "@test.local");
    account.setRoleId(3);
    account.setPassword("test_hash");
    accounts.save(account);
  }

  @Test
  void concurrentUseHasOneWinnerAndReplayRevocationCommits() throws Exception {
    var issued = service.create(account, "KhachHang", true);
    var start = new CountDownLatch(1);
    var pool = Executors.newFixedThreadPool(2);
    Callable<Boolean> request =
        () -> {
          start.await();
          try {
            service.refresh(issued.refreshToken());
            return true;
          } catch (ApiException ex) {
            return false;
          }
        };
    try {
      var one = pool.submit(request);
      var two = pool.submit(request);
      start.countDown();
      assertNotEquals(one.get(15, TimeUnit.SECONDS), two.get(15, TimeUnit.SECONDS));
      assertNotNull(
          sessions
              .findById(
                  java.util.UUID.fromString(
                      jwt.parse(issued.accessToken()).get("sid", String.class)))
              .orElseThrow()
              .getRevokedAt());
      assertThrows(ApiException.class, () -> service.authenticate(jwt.parse(issued.accessToken())));
    } finally {
      pool.shutdownNow();
    }
  }

  @Test
  void storageFailureRollsBackConsumedTokenAndSessionActivity() {
    var issued = service.create(account, "KhachHang", false);
    String hash = AuthSessionService.hash(issued.refreshToken());
    var sid = java.util.UUID.fromString(jwt.parse(issued.accessToken()).get("sid", String.class));
    Instant before = sessions.findById(sid).orElseThrow().getLastActiveAt();
    doThrow(new org.springframework.dao.DataAccessResourceFailureException("simulated failure"))
        .when(tokens)
        .save(argThat(token -> token != null && !hash.equals(token.getTokenHash())));
    assertThrows(
        org.springframework.dao.DataAccessResourceFailureException.class,
        () -> service.refresh(issued.refreshToken()));
    assertNull(tokens.findById(hash).orElseThrow().getUsedAt());
    assertEquals(before, sessions.findById(sid).orElseThrow().getLastActiveAt());
    reset(tokens);
    assertNotNull(service.refresh(issued.refreshToken()).accessToken());
  }

  @Test
  void logoutAndAccountRecreationCannotReviveOldAccessToken() {
    var issued = service.create(account, "KhachHang", true);
    service.logout(issued.refreshToken());
    assertThrows(ApiException.class, () -> service.authenticate(jwt.parse(issued.accessToken())));
    accounts.deleteById(account.getAccountId());
    Account replacement = new Account();
    replacement.setUsername(account.getUsername());
    replacement.setEmail(account.getEmail());
    replacement.setRoleId(3);
    replacement.setPassword("new_hash");
    accounts.save(replacement);
    assertNotEquals(account.getAccountId(), replacement.getAccountId());
    assertThrows(ApiException.class, () -> service.authenticate(jwt.parse(issued.accessToken())));
    assertTrue(tokens.findById(AuthSessionService.hash(issued.refreshToken())).isEmpty());
  }
}
