package vn.qlbxmd.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.Xe;

public interface XeRepository extends JpaRepository<Xe, Integer> {

    boolean existsByMaNx(Integer maNx);

    boolean existsByMaLx(Integer maLx);

    boolean existsByBienSoXe(String bienSoXe);
}
