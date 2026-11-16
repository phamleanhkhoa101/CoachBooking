package com.coachbooking.config;

import io.swagger.v3.oas.models.Components;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.media.StringSchema;
import io.swagger.v3.oas.models.parameters.Parameter;
import io.swagger.v3.oas.models.security.SecurityRequirement;
import io.swagger.v3.oas.models.security.SecurityScheme;
import java.util.List;
import org.springdoc.core.customizers.OpenApiCustomizer;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class OpenApiConfig {

  @Bean
  public OpenApiCustomizer authRequestHeader() {
    return api ->
        List.of(
                "/api/auth/login",
                "/api/auth/user/login",
                "/api/auth/admin/login",
                "/api/auth/refresh",
                "/api/auth/logout")
            .forEach(
                path -> {
                  var item = api.getPaths().get(path);
                  if (item != null && item.getPost() != null)
                    item.getPost()
                        .addParametersItem(
                            new Parameter()
                                .name("X-Auth-Request")
                                .in("header")
                                .required(true)
                                .schema(new StringSchema()._default("1"))
                                .example("1"));
                });
  }

  @Bean
  public OpenAPI coachBookingOpenApi() {
    final String schemeName = "bearerAuth";
    return new OpenAPI()
        .info(
            new Info()
                .title("CoachBooking API")
                .version("1.0.0")
                .description("Hệ thống đặt vé và quản lý Bến xe Miền Đông"))
        .addSecurityItem(new SecurityRequirement().addList(schemeName))
        .components(
            new Components()
                .addSecuritySchemes(
                    schemeName,
                    new SecurityScheme()
                        .name(schemeName)
                        .type(SecurityScheme.Type.HTTP)
                        .scheme("bearer")
                        .bearerFormat("JWT")));
  }
}
