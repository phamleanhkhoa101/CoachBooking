package com.coachbooking.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.scheduling.concurrent.ThreadPoolTaskScheduler;

@Configuration
public class SepayPollingConfig {
  @Bean
  public ThreadPoolTaskScheduler sepayPollingScheduler() {
    ThreadPoolTaskScheduler scheduler = new ThreadPoolTaskScheduler();
    scheduler.setPoolSize(1);
    scheduler.setThreadNamePrefix("sepay-poll-");
    scheduler.setRemoveOnCancelPolicy(true);
    return scheduler;
  }

  // Explicitly retain a separate scheduler for existing seat/payment expiry jobs.
  @Bean(name = "taskScheduler")
  public ThreadPoolTaskScheduler taskScheduler() {
    ThreadPoolTaskScheduler scheduler = new ThreadPoolTaskScheduler();
    scheduler.setPoolSize(1);
    scheduler.setThreadNamePrefix("booking-scheduler-");
    scheduler.setRemoveOnCancelPolicy(true);
    return scheduler;
  }
}
