package com.coachbooking.repo;

import com.coachbooking.domain.Trip;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface TripRepository extends JpaRepository<Trip, Integer> {

  List<Trip> findAllByOrderByDepartureTimeAsc();

  long countByDepartureTimeGreaterThanEqualAndDepartureTimeLessThan(
      LocalDateTime from, LocalDateTime to);

  boolean existsByVehicleId(Integer vehicleId);

  boolean existsByRouteId(Integer routeId);

  boolean existsByDriverId(Integer driverId);

  boolean existsByVehicleIdAndDepartureTimeBetween(
      Integer vehicleId, LocalDateTime from, LocalDateTime to);

  boolean existsByVehicleIdAndDepartureTimeBetweenAndTripIdNot(
      Integer vehicleId, LocalDateTime from, LocalDateTime to, Integer tripId);

  @Query(
      """
            select t.tripId as tripId,
                   t.vehicleId as vehicleId,
                   v.licensePlate as licensePlate,
                   bc.companyId as companyId,
                   bc.companyName as companyName,
                   vt.typeId as typeId,
                   vt.typeName as typeName,
                   vt.seatCapacity as seatCapacity,
                   r.routeId as routeId,
                   r.departure as departure,
                   r.destination as destination,
                   d.driverId as driverId,
                   d.fullName as driverName,
                   t.departureTime as departureTime,
                   t.estimatedArrivalTime as estimatedArrivalTime,
                   t.actualPrice as actualPrice,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as bookedSeats
            from Trip t
            join Vehicle v on v.vehicleId = t.vehicleId
            join BusCompany bc on bc.companyId = v.companyId
            join VehicleType vt on vt.typeId = v.typeId
            join Route r on r.routeId = t.routeId
            join Driver d on d.driverId = t.driverId
            order by t.departureTime
            """)
  List<TripRow> findAllRows();

  @Query(
      """
            select t.tripId as tripId,
                   t.vehicleId as vehicleId,
                   v.licensePlate as licensePlate,
                   bc.companyId as companyId,
                   bc.companyName as companyName,
                   vt.typeId as typeId,
                   vt.typeName as typeName,
                   vt.seatCapacity as seatCapacity,
                   r.routeId as routeId,
                   r.departure as departure,
                   r.destination as destination,
                   d.driverId as driverId,
                   d.fullName as driverName,
                   t.departureTime as departureTime,
                   t.estimatedArrivalTime as estimatedArrivalTime,
                   t.actualPrice as actualPrice,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as bookedSeats
            from Trip t
            join Vehicle v on v.vehicleId = t.vehicleId
            join BusCompany bc on bc.companyId = v.companyId
            join VehicleType vt on vt.typeId = v.typeId
            join Route r on r.routeId = t.routeId
            join Driver d on d.driverId = t.driverId
            where (:departure is null or r.departure = :departure)
              and (:destination is null or r.destination = :destination)
              and t.departureTime >= :from
              and t.departureTime < :to
            order by t.departureTime
            """)
  List<TripRow> search(
      @Param("departure") String departure,
      @Param("destination") String destination,
      @Param("from") LocalDateTime from,
      @Param("to") LocalDateTime to);

  interface TripRow {
    Integer getTripId();

    Integer getVehicleId();

    String getLicensePlate();

    Integer getCompanyId();

    String getCompanyName();

    Integer getTypeId();

    String getTypeName();

    Integer getSeatCapacity();

    Integer getRouteId();

    String getDeparture();

    String getDestination();

    Integer getDriverId();

    String getDriverName();

    LocalDateTime getDepartureTime();

    LocalDateTime getEstimatedArrivalTime();

    BigDecimal getActualPrice();

    Long getBookedSeats();
  }
}
