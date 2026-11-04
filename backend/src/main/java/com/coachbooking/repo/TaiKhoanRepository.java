package com.coachbooking.repo;

import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import com.coachbooking.domain.TaiKhoan;

public interface TaiKhoanRepository extends JpaRepository<TaiKhoan, Integer> {

    Optional<TaiKhoan> findByTenDangNhap(String tenDangNhap);

    Optional<TaiKhoan> findByTenDangNhapAndEmail(String tenDangNhap, String email);

    boolean existsByTenDangNhap(String tenDangNhap);

    boolean existsByEmail(String email);

    boolean existsByMaQuyen(Integer maQuyen);
}
