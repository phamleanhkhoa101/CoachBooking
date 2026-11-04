package com.coachbooking.repo;

import com.coachbooking.domain.VeXe;
import java.util.Collection;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface VeXeRepository extends JpaRepository<VeXe, Integer> {

  List<VeXe> findByMaChuyen(Integer maChuyen);

  List<VeXe> findByMaHd(Integer maHd);

  List<VeXe> findByMaHdIn(Collection<Integer> maHds);

  boolean existsByMaChuyen(Integer maChuyen);

  void deleteByMaHd(Integer maHd);

  @Query("select v.choNgoi from VeXe v where v.maChuyen = :maChuyen")
  List<Integer> findChoNgoiByMaChuyen(@Param("maChuyen") Integer maChuyen);
}
