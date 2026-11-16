package com.coachbooking.repo;

import com.coachbooking.domain.SepayNotification;
import org.springframework.data.jpa.repository.JpaRepository;

public interface SepayNotificationRepository extends JpaRepository<SepayNotification, String> {}
