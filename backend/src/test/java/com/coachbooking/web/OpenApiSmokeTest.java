package com.coachbooking.web;

import static org.hamcrest.Matchers.containsString;
import static org.hamcrest.Matchers.not;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.content;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

import com.coachbooking.config.DataInitializer;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

@SpringBootTest(
    properties = {
      "spring.datasource.url=jdbc:h2:mem:openapidb;MODE=PostgreSQL;DB_CLOSE_DELAY=-1",
      "spring.datasource.driver-class-name=org.h2.Driver",
      "spring.datasource.username=sa",
      "spring.datasource.password=",
      "spring.jpa.hibernate.ddl-auto=create-drop",
      "spring.flyway.enabled=false"
    })
@AutoConfigureMockMvc
class OpenApiSmokeTest {

  @Autowired private MockMvc mockMvc;

  @MockitoBean private DataInitializer dataInitializer;

  @Test
  void apiDocsAndSwaggerUiArePubliclyAccessible() throws Exception {
    mockMvc
        .perform(get("/v3/api-docs"))
        .andExpect(status().isOk())
        .andExpect(content().string(containsString("CoachBooking API")))
        .andExpect(content().string(containsString("bearerAuth")))
        .andExpect(content().string(not(containsString("\"/api/chuyen-xe\""))))
        .andExpect(content().string(not(containsString("\"/api/dat-ve\""))))
        .andExpect(content().string(not(containsString("\"/api/auth/xac-nhan-otp\""))));

    mockMvc.perform(get("/swagger-ui/index.html")).andExpect(status().isOk());
  }
}
