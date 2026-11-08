package com.coachbooking.repo;

import com.coachbooking.domain.Route;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface RouteRepository extends JpaRepository<Route, Integer> {
  Optional<Route> findByDepartureAndDestination(String departure, String destination);

  boolean existsByDepartureAndDestination(String departure, String destination);
}
