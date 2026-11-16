package com.coachbooking.service;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.repo.*;
import com.coachbooking.security.JwtService;
import io.jsonwebtoken.Claims;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.time.*;
import java.util.HexFormat;
import java.util.UUID;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class AuthSessionService {
  private static final Logger LOG = LoggerFactory.getLogger(AuthSessionService.class);
  private static final SecureRandom RANDOM = new SecureRandom();
  private final AuthSessionRepository sessions;
  private final AuthRefreshTokenRepository tokens;
  private final AccountRepository accounts;
  private final RoleRepository roles;
  private final JwtService jwt;
  private final Clock clock;

  public AuthSessionService(
      AuthSessionRepository sessions,
      AuthRefreshTokenRepository tokens,
      AccountRepository accounts,
      RoleRepository roles,
      JwtService jwt,
      Clock clock) {
    this.sessions = sessions;
    this.tokens = tokens;
    this.accounts = accounts;
    this.roles = roles;
    this.jwt = jwt;
    this.clock = clock;
  }

  public record Issued(
      Account account,
      String role,
      String accessToken,
      String refreshToken,
      boolean rememberMe,
      Instant refreshExpiresAt,
      long expiresIn) {}

  @Transactional
  public Issued create(Account account, String role, boolean rememberMe) {
    // Serialize session creation with password/role changes for this account.
    Account current =
        accounts.lockById(account.getAccountId()).orElseThrow(AuthSessionService::unauthorized);
    if (!Boolean.TRUE.equals(current.getIsActive())
        || !java.util.Objects.equals(current.getPassword(), account.getPassword())
        || !current.getRoleId().equals(account.getRoleId())) throw unauthorized();
    Instant now = clock.instant();
    AuthSession session = new AuthSession();
    session.setSessionId(UUID.randomUUID());
    session.setAccountId(account.getAccountId());
    session.setRoleId(account.getRoleId());
    session.setRememberMe(rememberMe);
    session.setCreatedAt(now);
    session.setLastActiveAt(now);
    session.setExpiresAt(
        now.plus(account.getRoleId() == 3 ? Duration.ofDays(30) : Duration.ofHours(12)));
    sessions.save(session);
    return issue(session, current, role, now);
  }

  @Transactional(noRollbackFor = ApiException.class)
  public Issued refresh(String rawToken) {
    if (rawToken == null || !rawToken.matches("[0-9a-f]{64}")) throw unauthorized();
    UUID sessionId =
        tokens.findSessionId(hash(rawToken)).orElseThrow(AuthSessionService::unauthorized);
    AuthSession session =
        sessions.lockById(sessionId).orElseThrow(AuthSessionService::unauthorized);
    // Reload AFTER acquiring the session lock: another transaction may have consumed the token.
    AuthRefreshToken token =
        tokens.findById(hash(rawToken)).orElseThrow(AuthSessionService::unauthorized);
    Instant now = clock.instant();
    if (token.getUsedAt() != null) {
      session.setRevokedAt(now);
      sessions.save(session);
      LOG.warn("Refresh token reuse detected; authentication session revoked");
      throw unauthorized();
    }
    Account account =
        accounts.findById(session.getAccountId()).orElseThrow(AuthSessionService::unauthorized);
    if (!active(session, account, now)) throw unauthorized();
    String role =
        roles
            .findById(account.getRoleId())
            .map(Role::getRoleName)
            .orElseThrow(AuthSessionService::unauthorized);
    token.setUsedAt(now);
    tokens.save(token);
    session.setLastActiveAt(now);
    sessions.save(session);
    return issue(session, account, role, now);
  }

  private Issued issue(AuthSession session, Account account, String role, Instant now) {
    byte[] bytes = new byte[32];
    RANDOM.nextBytes(bytes);
    String raw = HexFormat.of().formatHex(bytes);
    AuthRefreshToken token = new AuthRefreshToken();
    token.setTokenHash(hash(raw));
    token.setSessionId(session.getSessionId());
    tokens.save(token);
    Instant refreshExpiry = idleExpiry(session);
    Instant accessExpiry = now.plusSeconds(900);
    if (accessExpiry.isAfter(refreshExpiry)) accessExpiry = refreshExpiry;
    return new Issued(
        account,
        role,
        jwt.generate(account, role, session.getSessionId(), now, accessExpiry),
        raw,
        session.isRememberMe(),
        refreshExpiry,
        Duration.between(now, accessExpiry).toSeconds());
  }

  @Transactional(readOnly = true)
  public Account authenticate(Claims claims) {
    UUID sid;
    Integer accountId;
    try {
      if (claims.get("sid") == null || claims.get("accountId") == null) throw unauthorized();
      sid = UUID.fromString(claims.get("sid", String.class));
      accountId = claims.get("accountId", Integer.class);
    } catch (IllegalArgumentException | io.jsonwebtoken.JwtException ex) {
      throw unauthorized();
    }
    AuthSession session = sessions.findById(sid).orElseThrow(AuthSessionService::unauthorized);
    Account account =
        accounts.findById(session.getAccountId()).orElseThrow(AuthSessionService::unauthorized);
    if (!account.getAccountId().equals(accountId)
        || !account.getUsername().equals(claims.getSubject())
        || !active(session, account, clock.instant())) throw unauthorized();
    return account;
  }

  private boolean active(AuthSession session, Account account, Instant now) {
    return session.getRevokedAt() == null
        && Boolean.TRUE.equals(account.getIsActive())
        && session.getRoleId().equals(account.getRoleId())
        && now.isBefore(idleExpiry(session));
  }

  private Instant idleExpiry(AuthSession session) {
    Instant idle = session.getLastActiveAt().plus(Duration.ofDays(7));
    return session.getRoleId() == 3 && idle.isBefore(session.getExpiresAt())
        ? idle
        : session.getExpiresAt();
  }

  @Transactional
  public void logout(String rawToken) {
    AuthRefreshToken token = findToken(rawToken);
    if (token == null) return;
    sessions
        .lockById(token.getSessionId())
        .ifPresent(
            session -> {
              session.setRevokedAt(clock.instant());
              sessions.save(session);
            });
  }

  @Transactional
  public void revokeAll(Integer accountId) {
    sessions.revokeAll(accountId, clock.instant());
  }

  @Scheduled(fixedDelayString = "${app.auth.cleanup-interval-ms:3600000}")
  @Transactional
  public void cleanup() {
    sessions.deleteExpired(clock.instant());
  }

  private AuthRefreshToken findToken(String raw) {
    return raw == null || !raw.matches("[0-9a-f]{64}")
        ? null
        : tokens.findById(hash(raw)).orElse(null);
  }

  public static String hash(String raw) {
    try {
      return HexFormat.of()
          .formatHex(
              MessageDigest.getInstance("SHA-256").digest(raw.getBytes(StandardCharsets.UTF_8)));
    } catch (NoSuchAlgorithmException ex) {
      throw new IllegalStateException(ex);
    }
  }

  private static ApiException unauthorized() {
    return new ApiException(
        HttpStatus.UNAUTHORIZED, "Phiên đăng nhập không hợp lệ hoặc đã hết hạn.");
  }
}
