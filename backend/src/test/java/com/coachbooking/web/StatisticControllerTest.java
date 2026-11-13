package com.coachbooking.web;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import com.coachbooking.dto.statistic.StatisticResponse;
import com.coachbooking.service.StatisticService;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;
import org.junit.jupiter.api.Test;

class StatisticControllerTest {

  @Test
  void getStatisticsDelegatesToService() {
    StatisticService statisticService = mock(StatisticService.class);
    StatisticController controller = new StatisticController(statisticService);

    LocalDate now = LocalDate.now();
    StatisticResponse resp =
        new StatisticResponse(10, 5, BigDecimal.valueOf(500_000), 20, List.of(), List.of());
    when(statisticService.getStatistics(now, now)).thenReturn(resp);

    StatisticResponse res = controller.getStatistics(null, null, now, now);
    assertEquals(10, res.customerCount());
  }
}
