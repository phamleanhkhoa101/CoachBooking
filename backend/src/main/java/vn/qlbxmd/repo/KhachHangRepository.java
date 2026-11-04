package vn.qlbxmd.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.KhachHang;

public interface KhachHangRepository extends JpaRepository<KhachHang, Integer> {

    Optional<KhachHang> findByMaTk(Integer maTk);

    Optional<KhachHang> findBySdt(String sdt);

    boolean existsBySdt(String sdt);

    boolean existsByMaTk(Integer maTk);
}
