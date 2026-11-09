package com.coachbooking.dto.trip;

import com.fasterxml.jackson.annotation.JsonAlias;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDateTime;

public record TripRequest(
    @NotNull(message = "Vui lòng chọn xe.") @JsonAlias({"vehicleId", "maXe"}) Integer vehicleId,
    @NotNull(message = "Vui lòng chọn tuyến đường.") @JsonAlias({"routeId", "maTuyen"})
        Integer routeId,
    @NotNull(message = "Vui lòng chọn tài xế.") @JsonAlias({"driverId", "maTx"}) Integer driverId,
    @NotNull(message = "Vui lòng chọn ngày giờ khởi hành.")
        @JsonAlias({"departureTime", "ngayGioKhoiHanh"})
        LocalDateTime departureTime) {}
