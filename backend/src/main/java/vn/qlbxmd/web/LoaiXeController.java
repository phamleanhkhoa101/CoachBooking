package vn.qlbxmd.web;

import jakarta.validation.Valid;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
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
import vn.qlbxmd.common.ApiException;
import vn.qlbxmd.domain.LoaiXe;
import vn.qlbxmd.repo.GiaVeRepository;
import vn.qlbxmd.repo.LoaiXeRepository;
import vn.qlbxmd.repo.XeRepository;

@RestController
@RequestMapping("/api/loai-xe")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class LoaiXeController {

    private final LoaiXeRepository loaiXeRepository;
    private final XeRepository xeRepository;
    private final GiaVeRepository giaVeRepository;

    public LoaiXeController(LoaiXeRepository loaiXeRepository,
                            XeRepository xeRepository,
                            GiaVeRepository giaVeRepository) {
        this.loaiXeRepository = loaiXeRepository;
        this.xeRepository = xeRepository;
        this.giaVeRepository = giaVeRepository;
    }

    @GetMapping
    public List<LoaiXe> list() {
        return loaiXeRepository.findAll();
    }

    @GetMapping("/{id}")
    public LoaiXe get(@PathVariable Integer id) {
        return loaiXeRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Loại xe không tồn tại."));
    }

    @PostMapping
    @PreAuthorize("hasRole('Admin')")
    public LoaiXe create(@Valid @RequestBody Request request) {
        LoaiXe loaiXe = new LoaiXe();
        apply(loaiXe, request);
        return loaiXeRepository.save(loaiXe);
    }

    @PutMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public LoaiXe update(@PathVariable Integer id, @Valid @RequestBody Request request) {
        LoaiXe loaiXe = get(id);
        apply(loaiXe, request);
        return loaiXeRepository.save(loaiXe);
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Map<String, String> delete(@PathVariable Integer id) {
        LoaiXe loaiXe = get(id);
        if (xeRepository.existsByMaLx(id)) {
            throw ApiException.conflict("Không thể xóa loại xe đang được sử dụng.");
        }
        if (giaVeRepository.existsByMaLx(id)) {
            throw ApiException.conflict("Không thể xóa loại xe đang có bảng giá.");
        }
        loaiXeRepository.delete(loaiXe);
        return Map.of("message", "Xóa loại xe thành công.");
    }

    private void apply(LoaiXe loaiXe, Request request) {
        loaiXe.setTenLx(request.tenLx().trim());
        loaiXe.setSoCho(request.soCho());
        if (request.trangThai() != null) {
            loaiXe.setTrangThai(request.trangThai());
        }
    }

    public record Request(
            @NotBlank(message = "Vui lòng nhập tên loại xe.") String tenLx,
            @NotNull(message = "Vui lòng nhập số chỗ.") @Min(value = 1, message = "Số chỗ phải lớn hơn 0.") Integer soCho,
            Boolean trangThai) {
    }
}
