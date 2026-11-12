package com.coachbooking.dto.hold;

import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;
import java.util.List;

public record SeatHoldRequest(
    @NotNull(message = "Vui lòng chọn chuyến xe.") Integer tripId,
    @NotEmpty(message = "Vui lòng chọn ít nhất một ghế.") List<Integer> seatNumbers) {}
