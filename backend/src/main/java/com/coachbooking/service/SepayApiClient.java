package com.coachbooking.service;

import com.coachbooking.config.SepayProperties;
import com.coachbooking.dto.payment.*;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.validation.Validator;
import java.io.IOException;
import java.net.*;
import java.net.http.*;
import java.nio.charset.StandardCharsets;
import java.time.*;
import java.time.format.DateTimeFormatter;
import java.util.Base64;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class SepayApiClient {
  private final SepayProperties config;
  private final ObjectMapper json;
  private final Validator validator;
  private final HttpClient http;
  private final Clock clock;

  @Autowired
  public SepayApiClient(
      SepayProperties config, ObjectMapper json, Validator validator, Clock clock) {
    this(
        config,
        json,
        validator,
        clock,
        HttpClient.newBuilder()
            .connectTimeout(Duration.ofSeconds(2))
            .followRedirects(HttpClient.Redirect.NEVER)
            .build());
  }

  SepayApiClient(
      SepayProperties config,
      ObjectMapper json,
      Validator validator,
      Clock clock,
      HttpClient http) {
    this.config = config;
    this.json = json;
    this.validator = validator;
    this.clock = clock;
    this.http = http;
  }

  public SepayLookupResult lookup(String reference) {
    String credential =
        Base64.getEncoder()
            .encodeToString(
                (config.getMerchantId() + ":" + config.getSecretKey())
                    .getBytes(StandardCharsets.UTF_8));
    HttpRequest request =
        HttpRequest.newBuilder(
                URI.create(
                    "https://pgapi-sandbox.sepay.vn/v1/order/detail/"
                        + URLEncoder.encode(reference, StandardCharsets.UTF_8)))
            .timeout(Duration.ofSeconds(5))
            .header("Authorization", "Basic " + credential)
            .header("Accept", "application/json")
            .GET()
            .build();
    try {
      HttpResponse<String> response =
          http.send(request, HttpResponse.BodyHandlers.ofString(StandardCharsets.UTF_8));
      if (response.statusCode() == 404) return new SepayLookupResult(null, null, null);
      if (response.statusCode() == 401 || response.statusCode() == 403) {
        return new SepayLookupResult(
            null, "Không thể xác thực API SePay. Vui lòng kiểm tra merchant credentials.", 60L);
      }
      if (response.statusCode() == 429) {
        return new SepayLookupResult(
            null,
            "SePay đang giới hạn lượt kiểm tra. Hệ thống sẽ tự thử lại.",
            retryAfter(response));
      }
      if (response.statusCode() != 200)
        return new SepayLookupResult(
            null, "Chưa kiểm tra được trạng thái từ SePay. Hệ thống sẽ tự thử lại.", null);
      SepayOrderResponse body = json.readValue(response.body(), SepayOrderResponse.class);
      if (body == null || !validator.validate(body).isEmpty()) {
        return new SepayLookupResult(
            null, "Dữ liệu tra cứu SePay chưa hợp lệ. Hệ thống sẽ kiểm tra lại.", null);
      }
      return new SepayLookupResult(body.data(), null, null);
    } catch (InterruptedException ex) {
      Thread.currentThread().interrupt();
      return new SepayLookupResult(
          null, "Lượt kiểm tra SePay bị gián đoạn. Hệ thống sẽ thử lại.", null);
    } catch (IOException | IllegalArgumentException ex) {
      return new SepayLookupResult(
          null, "Không kết nối được hoặc dữ liệu SePay chưa hợp lệ. Hệ thống sẽ thử lại.", null);
    }
  }

  private Long retryAfter(HttpResponse<?> response) {
    String value = response.headers().firstValue("Retry-After").orElse("");
    try {
      return Math.max(1, Math.min(86_400, Long.parseLong(value)));
    } catch (NumberFormatException ex) {
      try {
        return Math.max(
            1,
            Math.min(
                86_400,
                Duration.between(
                        clock.instant(),
                        ZonedDateTime.parse(value, DateTimeFormatter.RFC_1123_DATE_TIME)
                            .toInstant())
                    .getSeconds()));
      } catch (java.time.format.DateTimeParseException ignored) {
        return 60L;
      }
    }
  }
}
