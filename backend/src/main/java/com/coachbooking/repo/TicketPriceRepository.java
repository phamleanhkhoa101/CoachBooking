package com.coachbooking.repo;

import com.coachbooking.domain.TicketPrice;
import java.util.List;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface TicketPriceRepository extends JpaRepository<TicketPrice, Integer> {
  Optional<TicketPrice> findByCompanyIdAndTypeIdAndRouteId(
      Integer companyId, Integer typeId, Integer routeId);

  boolean existsByCompanyIdAndTypeIdAndRouteId(Integer companyId, Integer typeId, Integer routeId);

  boolean existsByCompanyId(Integer companyId);

  boolean existsByTypeId(Integer typeId);

  boolean existsByRouteId(Integer routeId);

  List<TicketPrice> findByRouteId(Integer routeId);
}
