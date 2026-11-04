package vn.qlbxmd.repo;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import vn.qlbxmd.domain.ChuyenXe;

public interface ChuyenXeRepository extends JpaRepository<ChuyenXe, Integer> {

    List<ChuyenXe> findAllByOrderByNgayGioKhoiHanhAsc();

    long countByNgayGioKhoiHanhGreaterThanEqualAndNgayGioKhoiHanhLessThan(
            LocalDateTime tu, LocalDateTime den);

    boolean existsByMaXe(Integer maXe);

    boolean existsByMaTuyen(Integer maTuyen);

    boolean existsByMaTx(Integer maTx);

    boolean existsByMaXeAndNgayGioKhoiHanhBetween(Integer maXe, LocalDateTime tu, LocalDateTime den);

    boolean existsByMaXeAndNgayGioKhoiHanhBetweenAndMaChuyenNot(
            Integer maXe, LocalDateTime tu, LocalDateTime den, Integer maChuyen);

    @Query("""
            select cx.maChuyen as maChuyen,
                   cx.maXe as maXe,
                   x.bienSoXe as bienSoXe,
                   nx.maNx as maNhaXe,
                   nx.tenNx as tenNhaXe,
                   lx.maLx as maLoaiXe,
                   lx.tenLx as tenLoaiXe,
                   lx.soCho as soCho,
                   td.maTuyen as maTuyen,
                   td.diemDi as diemDi,
                   td.diemDen as diemDen,
                   tx.maTx as maTx,
                   tx.tenTx as tenTx,
                   cx.ngayGioKhoiHanh as ngayGioKhoiHanh,
                   cx.ngayGioDenDuKien as ngayGioDenDuKien,
                   cx.giaVeThucTe as giaVeThucTe,
                   (select count(v) from VeXe v where v.maChuyen = cx.maChuyen) as soVeDaBan
            from ChuyenXe cx
            join Xe x on x.maXe = cx.maXe
            join NhaXe nx on nx.maNx = x.maNx
            join LoaiXe lx on lx.maLx = x.maLx
            join TuyenDuong td on td.maTuyen = cx.maTuyen
            join TaiXe tx on tx.maTx = cx.maTx
            order by cx.ngayGioKhoiHanh
            """)
    List<TripRow> findAllRows();

    @Query("""
            select cx.maChuyen as maChuyen,
                   cx.maXe as maXe,
                   x.bienSoXe as bienSoXe,
                   nx.maNx as maNhaXe,
                   nx.tenNx as tenNhaXe,
                   lx.maLx as maLoaiXe,
                   lx.tenLx as tenLoaiXe,
                   lx.soCho as soCho,
                   td.maTuyen as maTuyen,
                   td.diemDi as diemDi,
                   td.diemDen as diemDen,
                   tx.maTx as maTx,
                   tx.tenTx as tenTx,
                   cx.ngayGioKhoiHanh as ngayGioKhoiHanh,
                   cx.ngayGioDenDuKien as ngayGioDenDuKien,
                   cx.giaVeThucTe as giaVeThucTe,
                   (select count(v) from VeXe v where v.maChuyen = cx.maChuyen) as soVeDaBan
            from ChuyenXe cx
            join Xe x on x.maXe = cx.maXe
            join NhaXe nx on nx.maNx = x.maNx
            join LoaiXe lx on lx.maLx = x.maLx
            join TuyenDuong td on td.maTuyen = cx.maTuyen
            join TaiXe tx on tx.maTx = cx.maTx
            where (:diemDi is null or td.diemDi = :diemDi)
              and (:diemDen is null or td.diemDen = :diemDen)
              and cx.ngayGioKhoiHanh >= :tu
              and cx.ngayGioKhoiHanh < :den
            order by cx.ngayGioKhoiHanh
            """)
    List<TripRow> search(@Param("diemDi") String diemDi,
                         @Param("diemDen") String diemDen,
                         @Param("tu") LocalDateTime tu,
                         @Param("den") LocalDateTime den);

    interface TripRow {

        Integer getMaChuyen();

        Integer getMaXe();

        String getBienSoXe();

        Integer getMaNhaXe();

        String getTenNhaXe();

        Integer getMaLoaiXe();

        String getTenLoaiXe();

        Integer getSoCho();

        Integer getMaTuyen();

        String getDiemDi();

        String getDiemDen();

        Integer getMaTx();

        String getTenTx();

        LocalDateTime getNgayGioKhoiHanh();

        LocalDateTime getNgayGioDenDuKien();

        BigDecimal getGiaVeThucTe();

        Long getSoVeDaBan();
    }
}
