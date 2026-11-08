package com.coachbooking.security;

import com.coachbooking.domain.Account;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.JwtException;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import java.nio.charset.StandardCharsets;
import java.util.Date;
import javax.crypto.SecretKey;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class JwtService {

  private final SecretKey key;
  private final long expirationMinutes;

  public JwtService(
      @Value("${app.jwt.secret}") String secret,
      @Value("${app.jwt.expiration-minutes}") long expirationMinutes) {
    this.key = Keys.hmacShaKeyFor(secret.getBytes(StandardCharsets.UTF_8));
    this.expirationMinutes = expirationMinutes;
  }

  public String generate(Account account, String roleName) {
    Date now = new Date();
    Date expiration = new Date(now.getTime() + expirationMinutes * 60_000L);
    return Jwts.builder()
        .subject(account.getUsername())
        .claim("accountId", account.getAccountId())
        .claim("role", roleName)
        .claim("maTk", account.getAccountId()) // giữ backwards compatibility cho client cũ nếu có
        .claim("quyen", roleName)
        .issuedAt(now)
        .expiration(expiration)
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

  private Claims parse(String token) {
    return Jwts.parser().verifyWith(key).build().parseSignedClaims(token).getPayload();
  }
}
