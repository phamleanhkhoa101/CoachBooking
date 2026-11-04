package vn.qlbxmd.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.TaiXe;

public interface TaiXeRepository extends JpaRepository<TaiXe, Integer> {

    boolean existsByCccd(String cccd);

    boolean existsBySdt(String sdt);
}
