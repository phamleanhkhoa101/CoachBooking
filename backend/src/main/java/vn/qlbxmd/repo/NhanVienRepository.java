package vn.qlbxmd.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.NhanVien;

public interface NhanVienRepository extends JpaRepository<NhanVien, Integer> {

    Optional<NhanVien> findByMaTk(Integer maTk);

    Optional<NhanVien> findBySdt(String sdt);

    boolean existsByMaTk(Integer maTk);

    boolean existsBySdt(String sdt);
}
