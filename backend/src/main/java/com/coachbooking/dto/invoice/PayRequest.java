package com.coachbooking.dto.invoice;

import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.constraints.NotBlank;

public record PayRequest(
    @NotBlank(message = "Vui lòng chọn phương thức thanh toán.")
        @JsonAlias({"phuongThuc", "paymentMethod"})
        String paymentMethod) {}
