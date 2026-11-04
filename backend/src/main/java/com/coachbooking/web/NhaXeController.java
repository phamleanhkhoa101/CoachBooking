package com.coachbooking.web;

import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
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
import com.coachbooking.domain.NhaXe;
import com.coachbooking.repo.GiaVeRepository;
import com.coachbooking.repo.NhaXeRepository;
import com.coachbooking.repo.XeRepository;

@RestController
@RequestMapping("/api/nha-xe")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class NhaXeController {

    private final NhaXeRepository nhaXeRepository;
    private final XeRepository xeRepository;
    private final GiaVeRepository giaVeRepository;

    public NhaXeController(NhaXeRepository nhaXeRepository,
                           XeRepository xeRepository,
                           GiaVeRepository giaVeRepository) {
        this.nhaXeRepository = nhaXeRepository;
        this.xeRepository = xeRepository;
        this.giaVeRepository = giaVeRepository;
    }

    @GetMapping
    public List<NhaXe> list() {
        return nhaXeRepository.findAll();
    }

    @GetMapping("/{id}")
    public NhaXe get(@PathVariable Integer id) {
        return nhaXeRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Nhà xe không tồn tại."));
    }

    @PostMapping
    @PreAuthorize("hasRole('Admin')")
    public NhaXe create(@Valid @RequestBody Request request) {
        NhaXe nhaXe = new NhaXe();
        apply(nhaXe, request);
        return nhaXeRepository.save(nhaXe);
    }

    @PutMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public NhaXe update(@PathVariable Integer id, @Valid @RequestBody Request request) {
        NhaXe nhaXe = get(id);
        apply(nhaXe, request);
        return nhaXeRepository.save(nhaXe);
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Map<String, String> delete(@PathVariable Integer id) {
        NhaXe nhaXe = get(id);
        if (xeRepository.existsByMaNx(id)) {
            throw ApiException.conflict("Không thể xóa nhà xe đang có xe.");
        }
        if (giaVeRepository.existsByMaNx(id)) {
            throw ApiException.conflict("Không thể xóa nhà xe đang có bảng giá.");
        }
        nhaXeRepository.delete(nhaXe);
        return Map.of("message", "Xóa nhà xe thành công.");
    }

    private void apply(NhaXe nhaXe, Request request) {
        nhaXe.setTenNx(request.tenNx().trim());
        nhaXe.setSdt(request.sdt().trim());
        nhaXe.setEmail(request.email().trim());
        if (request.trangThai() != null) {
            nhaXe.setTrangThai(request.trangThai());
        }
    }

    public record Request(
            @NotBlank(message = "Vui lòng nhập tên nhà xe.") String tenNx,
            @NotBlank(message = "Vui lòng nhập số điện thoại.") String sdt,
            @NotBlank(message = "Vui lòng nhập email.") @Email(message = "Email không đúng định dạng.") String email,
            Boolean trangThai) {
    }
}
