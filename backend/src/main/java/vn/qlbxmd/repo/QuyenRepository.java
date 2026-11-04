package vn.qlbxmd.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import vn.qlbxmd.domain.Quyen;

public interface QuyenRepository extends JpaRepository<Quyen, Integer> {

    Optional<Quyen> findByTenQuyen(String tenQuyen);
}
