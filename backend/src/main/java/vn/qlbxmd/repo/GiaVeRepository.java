package vn.qlbxmd.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.GiaVe;

public interface GiaVeRepository extends JpaRepository<GiaVe, Integer> {

    Optional<GiaVe> findByMaNxAndMaLxAndMaTuyen(Integer maNx, Integer maLx, Integer maTuyen);

    boolean existsByMaNxAndMaLxAndMaTuyen(Integer maNx, Integer maLx, Integer maTuyen);

    boolean existsByMaNx(Integer maNx);

    boolean existsByMaLx(Integer maLx);

    boolean existsByMaTuyen(Integer maTuyen);
}
