package vn.qlbxmd.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.TuyenDuong;

public interface TuyenDuongRepository extends JpaRepository<TuyenDuong, Integer> {

    boolean existsByDiemDiAndDiemDen(String diemDi, String diemDen);

    Optional<TuyenDuong> findByDiemDiAndDiemDen(String diemDi, String diemDen);
}
