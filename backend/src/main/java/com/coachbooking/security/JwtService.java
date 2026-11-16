package com.coachbooking.security;

import com.coachbooking.domain.Account;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.JwtException;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import java.nio.charset.StandardCharsets;
import java.time.Instant;
import java.util.Date;
import java.util.UUID;
import javax.crypto.SecretKey;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class JwtService {

  private final SecretKey key;

  public JwtService(@Value("${app.jwt.secret}") String secret) {
    if (secret == null
        || secret.isBlank()
        || secret.contains("change-me")
        || secret.contains("CHANGE_ME")) {
      throw new IllegalArgumentException("Configure a unique APP_JWT_SECRET (at least 32 bytes)");
    }
    this.key = Keys.hmacShaKeyFor(secret.getBytes(StandardCharsets.UTF_8));
  }

  public String generate(
      Account account, String roleName, UUID sessionId, Instant now, Instant expiration) {
    return Jwts.builder()
        .subject(account.getUsername())
        .claim("sid", sessionId.toString())
        .claim("accountId", account.getAccountId())
        .claim("role", roleName)
        .claim("maTk", account.getAccountId()) // giữ backwards compatibility cho client cũ nếu có
        .claim("quyen", roleName)
        .issuedAt(Date.from(now))
        .expiration(Date.from(expiration))
        .signWith(key)
        .compact();
  }

  public String extractUsername(String token) {
    return parse(token).getSubject();
  }

  public boolean isValid(String token) {
    try {
      parse(token);
      return true;
    } catch (JwtException | IllegalArgumentException ex) {
      return false;
    }
  }

  public Claims parse(String token) {
    return Jwts.parser().verifyWith(key).build().parseSignedClaims(token).getPayload();
  }
}
