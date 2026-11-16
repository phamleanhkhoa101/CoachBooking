package com.coachbooking.service;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.common.ApiException;
import com.coachbooking.domain.*;
import com.coachbooking.repo.*;
import com.coachbooking.security.JwtService;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import java.time.*;
import java.util.*;
import org.junit.jupiter.api.*;

class AuthSessionServiceTest {
  final AuthSessionRepository sessions = mock(AuthSessionRepository.class);
  final AuthRefreshTokenRepository tokens = mock(AuthRefreshTokenRepository.class);
  final AccountRepository accounts = mock(AccountRepository.class);
  final RoleRepository roles = mock(RoleRepository.class);
  final JwtService jwt = mock(JwtService.class);
  final Instant now = Instant.parse("2026-11-16T00:00:00Z");
  final Clock clock = Clock.fixed(now, ZoneOffset.UTC);
  final AuthSessionService service =
      new AuthSessionService(sessions, tokens, accounts, roles, jwt, clock);
  Account account;
  AuthSession session;
  AuthRefreshToken refresh;
  String raw = "a".repeat(64);

  @BeforeEach
  void setup() {
    account = new Account();
    account.setAccountId(1);
    account.setUsername("customer");
    account.setRoleId(3);
    account.setPassword("hash");
    account.setIsActive(true);
    session = new AuthSession();
    session.setSessionId(UUID.randomUUID());
    session.setAccountId(1);
    session.setRoleId(3);
    session.setCreatedAt(now.minusSeconds(60));
    session.setLastActiveAt(now.minusSeconds(60));
    session.setExpiresAt(now.plus(Duration.ofDays(30)));
    refresh = new AuthRefreshToken();
    refresh.setSessionId(session.getSessionId());
    refresh.setTokenHash(AuthSessionService.hash(raw));
    when(accounts.lockById(1)).thenReturn(Optional.of(account));
    when(accounts.findById(1)).thenReturn(Optional.of(account));
    when(sessions.findById(session.getSessionId())).thenReturn(Optional.of(session));
    when(sessions.lockById(session.getSessionId())).thenReturn(Optional.of(session));
    when(tokens.findById(refresh.getTokenHash())).thenReturn(Optional.of(refresh));
    when(tokens.findSessionId(refresh.getTokenHash()))
        .thenReturn(Optional.of(session.getSessionId()));
    Role role = new Role();
    role.setRoleId(3);
    role.setRoleName("KhachHang");
    when(roles.findById(3)).thenReturn(Optional.of(role));
    when(jwt.generate(any(), anyString(), any(), any(), any())).thenReturn("access");
  }

  Claims claims(Integer id, String username) {
    return Jwts.claims()
        .subject(username)
        .add("accountId", id)
        .add("sid", session.getSessionId().toString())
        .build();
  }

  @Test
  void createsCustomerAndStaffSessionsWithPolicyAndOnlyHashPersisted() {
    var issued = service.create(account, "KhachHang", true);
    var captor = org.mockito.ArgumentCaptor.forClass(AuthSession.class);
    verify(sessions).save(captor.capture());
    assertEquals(now.plus(Duration.ofDays(30)), captor.getValue().getExpiresAt());
    assertTrue(captor.getValue().isRememberMe());
    assertTrue(issued.rememberMe());
    assertEquals(900, issued.expiresIn());
    assertEquals(now.plus(Duration.ofDays(7)), issued.refreshExpiresAt());
    assertEquals(64, issued.refreshToken().length());
    var tokenCaptor = org.mockito.ArgumentCaptor.forClass(AuthRefreshToken.class);
    verify(tokens).save(tokenCaptor.capture());
    assertEquals(
        AuthSessionService.hash(issued.refreshToken()), tokenCaptor.getValue().getTokenHash());
    assertNotEquals(issued.refreshToken(), tokenCaptor.getValue().getTokenHash());
    reset(sessions);
    account.setRoleId(1);
    var staff = service.create(account, "Admin", false);
    verify(sessions).save(captor.capture());
    assertEquals(now.plus(Duration.ofHours(12)), captor.getValue().getExpiresAt());
    assertEquals(now.plus(Duration.ofHours(12)), staff.refreshExpiresAt());
    assertFalse(staff.rememberMe());
  }

  @Test
  void rotatesRefreshAndEnforcesAbsoluteLimit() {
    session.setRememberMe(true);
    session.setExpiresAt(now.plusSeconds(40));
    var issued = service.refresh(raw);
    assertNotEquals(raw, issued.refreshToken());
    assertEquals(now, refresh.getUsedAt());
    assertEquals(now, session.getLastActiveAt());
    assertEquals(40, issued.expiresIn());
    assertEquals(session.getExpiresAt(), issued.refreshExpiresAt());
  }

  @Test
  void replayRevokesFamilyAndLogoutIsIdempotent() {
    refresh.setUsedAt(now.minusSeconds(1));
    assertThrows(ApiException.class, () -> service.refresh(raw));
    assertEquals(now, session.getRevokedAt());
    verify(sessions).save(session);
    assertThrows(ApiException.class, () -> service.authenticate(claims(1, "customer")));
    service.logout(raw);
    service.logout(raw);
    service.logout(null);
    service.logout("bad");
    assertEquals(now, session.getRevokedAt());
    service.revokeAll(1);
    verify(sessions).revokeAll(1, now);
    service.cleanup();
    verify(sessions).deleteExpired(now);
  }

  @Test
  void rejectsIdleAbsoluteRevokedDisabledMissingAndRoleChangedSessions() {
    session.setLastActiveAt(now.minus(Duration.ofDays(7)));
    assertThrows(ApiException.class, () -> service.refresh(raw));
    session.setLastActiveAt(now);
    session.setExpiresAt(now);
    assertThrows(ApiException.class, () -> service.refresh(raw));
    session.setExpiresAt(now.plusSeconds(900));
    session.setRevokedAt(now);
    assertThrows(ApiException.class, () -> service.refresh(raw));
    session.setRevokedAt(null);
    account.setIsActive(false);
    assertThrows(ApiException.class, () -> service.refresh(raw));
    account.setIsActive(true);
    account.setRoleId(2);
    assertThrows(ApiException.class, () -> service.refresh(raw));
    account.setRoleId(3);
    when(roles.findById(3)).thenReturn(Optional.empty());
    assertThrows(ApiException.class, () -> service.refresh(raw));
    when(accounts.findById(1)).thenReturn(Optional.empty());
    assertThrows(ApiException.class, () -> service.refresh(raw));
    assertThrows(ApiException.class, () -> service.refresh(null));
    assertThrows(ApiException.class, () -> service.refresh("invalid"));
    assertThrows(ApiException.class, () -> service.refresh("b".repeat(64)));
    verify(tokens, never()).save(any());
  }

  @Test
  void authenticatesIdentityAndRejectsLegacyOrRecreatedAccount() {
    assertSame(account, service.authenticate(claims(1, "customer")));
    assertThrows(ApiException.class, () -> service.authenticate(claims(2, "customer")));
    assertThrows(ApiException.class, () -> service.authenticate(claims(1, "different")));
    assertThrows(
        ApiException.class, () -> service.authenticate(Jwts.claims().subject("customer").build()));
    assertThrows(
        ApiException.class,
        () ->
            service.authenticate(Jwts.claims().add("sid", "invalid").add("accountId", 1).build()));
    assertThrows(
        ApiException.class,
        () ->
            service.authenticate(
                Jwts.claims()
                    .add("sid", session.getSessionId().toString())
                    .add("accountId", "string")
                    .build()));
    when(sessions.findById(session.getSessionId())).thenReturn(Optional.empty());
    assertThrows(ApiException.class, () -> service.authenticate(claims(1, "customer")));
  }

  @Test
  void deniesNewSessionsForMissingLockedOrChangedAccounts() {
    account.setIsActive(false);
    assertThrows(ApiException.class, () -> service.create(account, "KhachHang", false));
    account.setIsActive(true);
    Account changed = new Account();
    changed.setAccountId(1);
    changed.setRoleId(3);
    changed.setPassword("old");
    assertThrows(ApiException.class, () -> service.create(changed, "KhachHang", false));
    changed.setPassword("hash");
    changed.setRoleId(2);
    assertThrows(ApiException.class, () -> service.create(changed, "NhanVien", false));
    when(accounts.lockById(1)).thenReturn(Optional.empty());
    assertThrows(ApiException.class, () -> service.create(account, "KhachHang", false));
    verify(sessions, never()).save(any());
  }
}
