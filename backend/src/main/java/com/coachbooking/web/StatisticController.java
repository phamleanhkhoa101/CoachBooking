package com.coachbooking.web;

import com.coachbooking.dto.statistic.StatisticResponse;
import com.coachbooking.service.StatisticService;
import java.time.LocalDate;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api/statistics", "/api/thong-ke"})
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class StatisticController {

  private final StatisticService statisticService;

  public StatisticController(StatisticService statisticService) {
    this.statisticService = statisticService;
  }

  @GetMapping
  public StatisticResponse getStatistics(
      @RequestParam(value = "tu", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate tu,
      @RequestParam(value = "den", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate den,
      @RequestParam(value = "fromDate", required = false)
          @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate fromDate,
      @RequestParam(value = "toDate", required = false)
          @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
          LocalDate toDate) {
    LocalDate actualFrom = fromDate != null ? fromDate : tu;
    LocalDate actualTo = toDate != null ? toDate : den;
    return statisticService.getStatistics(actualFrom, actualTo);
  }
}
