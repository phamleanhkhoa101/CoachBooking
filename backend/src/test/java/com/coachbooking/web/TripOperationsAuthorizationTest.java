package com.coachbooking.web;

import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

import com.coachbooking.service.TripScheduleService;
import java.util.List;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.context.TestConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.http.HttpStatus;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.HttpStatusEntryPoint;
import org.springframework.test.context.ContextConfiguration;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

@WebMvcTest
@ContextConfiguration(
    classes = {TripScheduleController.class, TripOperationsAuthorizationTest.Security.class})
class TripOperationsAuthorizationTest {
  @TestConfiguration
  @EnableMethodSecurity
  static class Security {
    @Bean
    SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
      return http.csrf(c -> c.disable())
          .authorizeHttpRequests(a -> a.anyRequest().authenticated())
          .exceptionHandling(
              e -> e.authenticationEntryPoint(new HttpStatusEntryPoint(HttpStatus.UNAUTHORIZED)))
          .build();
    }
  }

  @Autowired MockMvc mvc;
  @MockitoBean TripScheduleService schedules;

  @Test
  void schedulesRequireLogin() throws Exception {
    mvc.perform(get("/api/trip-schedules")).andExpect(status().isUnauthorized());
  }

  @Test
  @WithMockUser(roles = "KhachHang")
  void customerCannotManageSchedules() throws Exception {
    mvc.perform(get("/api/trip-schedules")).andExpect(status().isForbidden());
  }

  @Test
  @WithMockUser(roles = "NhanVien")
  void employeeCannotRunSchedules() throws Exception {
    mvc.perform(
            post("/api/trip-schedules/1/generate")
                .contentType("application/json")
                .content("{\"from\":\"2027-01-01\",\"to\":\"2027-01-02\"}"))
        .andExpect(status().isForbidden());
    verifyNoInteractions(schedules);
  }

  @Test
  @WithMockUser(roles = "Admin")
  void adminCanManageAndValidationRejectsMalformedInputs() throws Exception {
    when(schedules.list()).thenReturn(List.of());
    mvc.perform(get("/api/trip-schedules")).andExpect(status().isOk());
    mvc.perform(
            post("/api/trip-schedules/1/generate").contentType("application/json").content("{}"))
        .andExpect(status().isBadRequest());
  }
}
