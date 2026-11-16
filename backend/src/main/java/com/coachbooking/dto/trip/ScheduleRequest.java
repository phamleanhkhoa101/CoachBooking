package com.coachbooking.dto.trip;

import jakarta.validation.constraints.*;
import java.time.*;
import java.util.Set;

public record ScheduleRequest(
    @NotBlank @Size(max = 200) String name,
    @NotNull Integer vehicleId,
    @NotNull Integer routeId,
    @NotNull Integer driverId,
    @NotNull LocalTime departureTime,
    @NotEmpty Set<@Min(1) @Max(7) Integer> weekdays,
    @NotNull LocalDate validFrom,
    @NotNull LocalDate validTo,
    @NotNull Boolean isActive,
    @NotNull @Size(max = 366) Set<@NotNull LocalDate> excludedDates) {}
