package vn.qlbxmd.repo;

import java.util.Collection;
import java.util.List;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.ThanhToan;

public interface ThanhToanRepository extends JpaRepository<ThanhToan, Integer> {

    Optional<ThanhToan> findByMaHd(Integer maHd);

    List<ThanhToan> findByMaHdIn(Collection<Integer> maHds);
}
