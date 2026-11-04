package vn.qlbxmd.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.NhaXe;

public interface NhaXeRepository extends JpaRepository<NhaXe, Integer> {

    boolean existsBySdt(String sdt);

    boolean existsByEmail(String email);
}
