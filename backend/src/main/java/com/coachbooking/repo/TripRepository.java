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
                   t.tripId as maChuyen,
                   t.vehicleId as vehicleId,
                   t.vehicleId as maXe,
                   v.licensePlate as licensePlate,
                   v.licensePlate as bienSoXe,
                   bc.companyId as companyId,
                   bc.companyId as maNhaXe,
                   bc.companyName as companyName,
                   bc.companyName as tenNhaXe,
                   vt.typeId as typeId,
                   vt.typeId as maLoaiXe,
                   vt.typeName as typeName,
                   vt.typeName as tenLoaiXe,
                   vt.seatCapacity as seatCapacity,
                   vt.seatCapacity as soCho,
                   r.routeId as routeId,
                   r.routeId as maTuyen,
                   r.departure as departure,
                   r.departure as diemDi,
                   r.destination as destination,
                   r.destination as diemDen,
                   d.driverId as driverId,
                   d.driverId as maTx,
                   d.fullName as driverName,
                   d.fullName as tenTx,
                   t.departureTime as departureTime,
                   t.departureTime as ngayGioKhoiHanh,
                   t.estimatedArrivalTime as estimatedArrivalTime,
                   t.estimatedArrivalTime as ngayGioDenDuKien,
                   t.actualPrice as actualPrice,
                   t.actualPrice as giaVeThucTe,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as bookedSeats,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as soVeDaBan,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as soGheDaDat
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
                   t.tripId as maChuyen,
                   t.vehicleId as vehicleId,
                   t.vehicleId as maXe,
                   v.licensePlate as licensePlate,
                   v.licensePlate as bienSoXe,
                   bc.companyId as companyId,
                   bc.companyId as maNhaXe,
                   bc.companyName as companyName,
                   bc.companyName as tenNhaXe,
                   vt.typeId as typeId,
                   vt.typeId as maLoaiXe,
                   vt.typeName as typeName,
                   vt.typeName as tenLoaiXe,
                   vt.seatCapacity as seatCapacity,
                   vt.seatCapacity as soCho,
                   r.routeId as routeId,
                   r.routeId as maTuyen,
                   r.departure as departure,
                   r.departure as diemDi,
                   r.destination as destination,
                   r.destination as diemDen,
                   d.driverId as driverId,
                   d.driverId as maTx,
                   d.fullName as driverName,
                   d.fullName as tenTx,
                   t.departureTime as departureTime,
                   t.departureTime as ngayGioKhoiHanh,
                   t.estimatedArrivalTime as estimatedArrivalTime,
                   t.estimatedArrivalTime as ngayGioDenDuKien,
                   t.actualPrice as actualPrice,
                   t.actualPrice as giaVeThucTe,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as bookedSeats,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as soVeDaBan,
                   (select count(tk) from Ticket tk where tk.tripId = t.tripId) as soGheDaDat
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

    Integer getMaChuyen();

    Integer getVehicleId();

    Integer getMaXe();

    String getLicensePlate();

    String getBienSoXe();

    Integer getCompanyId();

    Integer getMaNhaXe();

    String getCompanyName();

    String getTenNhaXe();

    Integer getTypeId();

    Integer getMaLoaiXe();

    String getTypeName();

    String getTenLoaiXe();

    Integer getSeatCapacity();

    Integer getSoCho();

    Integer getRouteId();

    Integer getMaTuyen();

    String getDeparture();

    String getDiemDi();

    String getDestination();

    String getDiemDen();

    Integer getDriverId();

    Integer getMaTx();

    String getDriverName();

    String getTenTx();

    LocalDateTime getDepartureTime();

    LocalDateTime getNgayGioKhoiHanh();

    LocalDateTime getEstimatedArrivalTime();

    LocalDateTime getNgayGioDenDuKien();

    BigDecimal getActualPrice();

    BigDecimal getGiaVeThucTe();

    Long getBookedSeats();

    Long getSoVeDaBan();

    Long getSoGheDaDat();
  }
}
