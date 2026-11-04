package com.coachbooking.web;

import jakarta.validation.Valid;
import jakarta.validation.constraints.NotBlank;
import java.util.List;
import java.util.Map;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import com.coachbooking.common.ApiException;
import com.coachbooking.domain.Quyen;
import com.coachbooking.repo.QuyenRepository;
import com.coachbooking.repo.TaiKhoanRepository;

@RestController
@RequestMapping("/api/quyen")
@PreAuthorize("hasRole('Admin')")
public class QuyenController {

    private final QuyenRepository quyenRepository;
    private final TaiKhoanRepository taiKhoanRepository;

    public QuyenController(QuyenRepository quyenRepository, TaiKhoanRepository taiKhoanRepository) {
        this.quyenRepository = quyenRepository;
        this.taiKhoanRepository = taiKhoanRepository;
    }

    @GetMapping
    public List<Quyen> list() {
        return quyenRepository.findAll();
    }

    @GetMapping("/{id}")
    public Quyen get(@PathVariable Integer id) {
        return quyenRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Quyền không tồn tại."));
    }

    @PostMapping
    public Quyen create(@Valid @RequestBody Request request) {
        if (quyenRepository.findByTenQuyen(request.tenQuyen().trim()).isPresent()) {
            throw ApiException.conflict("Tên quyền đã tồn tại.");
        }
        Quyen quyen = new Quyen();
        apply(quyen, request);
        return quyenRepository.save(quyen);
    }

    @PutMapping("/{id}")
    public Quyen update(@PathVariable Integer id, @Valid @RequestBody Request request) {
        Quyen quyen = get(id);
        quyenRepository.findByTenQuyen(request.tenQuyen().trim())
                .filter(other -> !other.getMaQuyen().equals(id))
                .ifPresent(other -> {
                    throw ApiException.conflict("Tên quyền đã tồn tại.");
                });
        apply(quyen, request);
        return quyenRepository.save(quyen);
    }

    @DeleteMapping("/{id}")
    public Map<String, String> delete(@PathVariable Integer id) {
        Quyen quyen = get(id);
        if (taiKhoanRepository.existsByMaQuyen(id)) {
            throw ApiException.conflict("Không thể xóa quyền đang được gán cho tài khoản.");
        }
        quyenRepository.delete(quyen);
        return Map.of("message", "Xóa quyền thành công.");
    }

    private void apply(Quyen quyen, Request request) {
        quyen.setTenQuyen(request.tenQuyen().trim());
        quyen.setMoTa(request.moTa());
    }

    public record Request(
            @NotBlank(message = "Vui lòng nhập tên quyền.") String tenQuyen,
            String moTa) {
    }
}
