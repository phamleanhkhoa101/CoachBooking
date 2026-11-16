package com.coachbooking.web;

import static org.hamcrest.Matchers.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

import com.coachbooking.config.DataInitializer;
import com.coachbooking.domain.*;
import com.coachbooking.repo.*;
import com.coachbooking.security.RefreshCookieService;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.servlet.http.Cookie;
import org.junit.jupiter.api.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.*;

@SpringBootTest(
    properties = {
      "spring.datasource.url=jdbc:h2:mem:authhttp;MODE=PostgreSQL;DB_CLOSE_DELAY=-1",
      "spring.datasource.driver-class-name=org.h2.Driver",
      "spring.datasource.username=sa",
      "spring.datasource.password=",
      "spring.jpa.hibernate.ddl-auto=create-drop",
      "spring.flyway.enabled=false",
      "app.jwt.secret=test-only-auth-signing-key-0123456789abcdef0123456789",
      "app.auth.cookie-secure=false"
    })
@AutoConfigureMockMvc
class AuthSessionHttpTest {
  @Autowired MockMvc mvc;
  @Autowired AccountRepository accounts;
  @Autowired RoleRepository roles;
  @Autowired PasswordEncoder encoder;
  @Autowired ObjectMapper json;
  @Autowired org.springframework.jdbc.core.JdbcTemplate jdbc;
  @MockitoBean DataInitializer initializer;
  static final String ORIGIN = "http://localhost:5173";

  @BeforeEach
  void setup() {
    if (!roles.existsById(3))
      jdbc.update("insert into roles(role_id, role_name) values(3, 'KhachHang')");
    if (!accounts.existsByUsername("http_customer")) {
      Account account = new Account();
      account.setUsername("http_customer");
      account.setEmail("http_customer@test.local");
      account.setRoleId(3);
      account.setPassword(encoder.encode("Password@123"));
      accounts.save(account);
    }
  }

  MvcResult login(boolean remember) throws Exception {
    return mvc.perform(
            post("/api/auth/user/login")
                .header("Origin", ORIGIN)
                .header("X-Auth-Request", "1")
                .contentType("application/json")
                .content(
                    "{\"username\":\"http_customer\",\"password\":\"Password@123\",\"rememberMe\":"
                        + remember
                        + "}"))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.expiresIn").value(900))
        .andExpect(jsonPath("$.refreshToken").doesNotExist())
        .andExpect(header().string("Cache-Control", "no-store"))
        .andReturn();
  }

  String access(MvcResult result) throws Exception {
    return json.readTree(result.getResponse().getContentAsString()).get("token").asText();
  }

  Cookie refresh(MvcResult result) {
    return result.getResponse().getCookie(RefreshCookieService.NAME);
  }

  @Test
  void loginRefreshRotationReplayAndImmediateAccessRevocation() throws Exception {
    MvcResult login = login(true);
    Cookie old = refresh(login);
    assertNotNull(old);
    assertTrue(old.getMaxAge() > 0);
    mvc.perform(get("/api/auth/me").header("Authorization", "Bearer " + access(login)))
        .andExpect(status().isOk());
    MvcResult rotated =
        mvc.perform(
                post("/api/auth/refresh")
                    .header("Origin", ORIGIN)
                    .header("X-Auth-Request", "1")
                    .cookie(old))
            .andExpect(status().isOk())
            .andReturn();
    assertNotEquals(old.getValue(), refresh(rotated).getValue());
    mvc.perform(
            post("/api/auth/refresh")
                .header("Origin", ORIGIN)
                .header("X-Auth-Request", "1")
                .cookie(old))
        .andExpect(status().isUnauthorized());
    mvc.perform(get("/api/auth/me").header("Authorization", "Bearer " + access(rotated)))
        .andExpect(status().isUnauthorized())
        .andExpect(header().string("X-Auth-Expired", "1"));
    mvc.perform(
            post("/api/auth/refresh")
                .header("Origin", ORIGIN)
                .header("X-Auth-Request", "1")
                .cookie(refresh(rotated)))
        .andExpect(status().isUnauthorized());
  }

  @Test
  void logoutIdempotenceSessionCookieAndCustomerForbiddenFromAdmin() throws Exception {
    var login = login(false);
    assertEquals(-1, refresh(login).getMaxAge());
    mvc.perform(get("/api/accounts").header("Authorization", "Bearer " + access(login)))
        .andExpect(status().isForbidden());
    for (int n = 0; n < 2; n++)
      mvc.perform(
              post("/api/auth/logout")
                  .header("Origin", ORIGIN)
                  .header("X-Auth-Request", "1")
                  .cookie(refresh(login)))
          .andExpect(status().isNoContent());
    mvc.perform(get("/api/auth/me").header("Authorization", "Bearer " + access(login)))
        .andExpect(status().isUnauthorized());
  }

  @Test
  void rejectsCsrfMissingTokenAndLegacyJwt() throws Exception {
    for (String endpoint :
        new String[] {"/login", "/user/login", "/admin/login", "/refresh", "/logout"}) {
      mvc.perform(
              post("/api/auth" + endpoint)
                  .header("Origin", "https://attacker.invalid")
                  .header("X-Auth-Request", "1"))
          .andExpect(status().isForbidden());
      mvc.perform(post("/api/auth" + endpoint).header("Origin", ORIGIN))
          .andExpect(status().isForbidden());
      mvc.perform(post("/api/auth" + endpoint).header("X-Auth-Request", "1"))
          .andExpect(status().isForbidden());
    }
    mvc.perform(post("/api/auth/refresh").header("Origin", ORIGIN).header("X-Auth-Request", "1"))
        .andExpect(status().isUnauthorized());
    mvc.perform(get("/api/auth/me")).andExpect(status().isUnauthorized());
  }

  @Test
  void changingPasswordRevokesAllSessions() throws Exception {
    var first = login(false);
    var second = login(true);
    mvc.perform(
            post("/api/auth/change-password")
                .header("Authorization", "Bearer " + access(first))
                .contentType("application/json")
                .content("{\"oldPassword\":\"Password@123\",\"newPassword\":\"Changed@123\"}"))
        .andExpect(status().isOk());
    mvc.perform(get("/api/auth/me").header("Authorization", "Bearer " + access(second)))
        .andExpect(status().isUnauthorized());
    mvc.perform(
            post("/api/auth/refresh")
                .header("Origin", ORIGIN)
                .header("X-Auth-Request", "1")
                .cookie(refresh(second)))
        .andExpect(status().isUnauthorized());
    Account account = accounts.findByUsername("http_customer").orElseThrow();
    account.setPassword(encoder.encode("Password@123"));
    accounts.save(account);
  }
}
