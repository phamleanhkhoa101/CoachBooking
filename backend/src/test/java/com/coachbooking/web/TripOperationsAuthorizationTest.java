package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

import com.coachbooking.domain.TripSchedule;
import com.coachbooking.dto.trip.ScheduleRequest;
import com.coachbooking.service.TripScheduleService;
import java.time.LocalDate;
import java.util.List;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
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

  @Test
  @WithMockUser(roles = "Admin")
  void adminCreatesUpdatesDisablesAndRunsSchedules() throws Exception {
    String request =
        """
        {"name":"Sai Gon - Da Lat","vehicleId":1,"routeId":2,"driverId":3,
         "departureTime":"08:30:00","weekdays":[1,3,7],"validFrom":"2030-01-01",
         "validTo":"2030-01-31","isActive":true,"excludedDates":[]}
        """;
    TripSchedule saved = new TripSchedule();
    saved.setScheduleId(7);
    when(schedules.save(isNull(), any())).thenReturn(saved);
    when(schedules.save(eq(7), any())).thenReturn(saved);
    mvc.perform(post("/api/trip-schedules").contentType("application/json").content(request))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.scheduleId").value(7));
    var captured = ArgumentCaptor.forClass(ScheduleRequest.class);
    verify(schedules).save(isNull(), captured.capture());
    assertEquals(2, captured.getValue().routeId());
    mvc.perform(put("/api/trip-schedules/7").contentType("application/json").content(request))
        .andExpect(status().isOk());
    verify(schedules).save(eq(7), eq(captured.getValue()));
    for (String operation : List.of("preview", "generate")) {
      mvc.perform(
              post("/api/trip-schedules/7/" + operation)
                  .contentType("application/json")
                  .content("{\"from\":\"2030-01-01\",\"to\":\"2030-01-02\"}"))
          .andExpect(status().isOk());
      verify(schedules)
          .run(7, LocalDate.of(2030, 1, 1), LocalDate.of(2030, 1, 2), operation.equals("preview"));
    }
    mvc.perform(delete("/api/trip-schedules/7"))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.message").isNotEmpty());
    verify(schedules).disable(7);
  }
}
