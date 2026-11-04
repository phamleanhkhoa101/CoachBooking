package vn.qlbxmd.web;

import jakarta.validation.Valid;
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
import vn.qlbxmd.domain.Xe;
import vn.qlbxmd.repo.ChuyenXeRepository;
import vn.qlbxmd.repo.LoaiXeRepository;
import vn.qlbxmd.repo.NhaXeRepository;
import vn.qlbxmd.repo.XeRepository;

@RestController
@RequestMapping("/api/xe")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class XeController {

    private final XeRepository xeRepository;
    private final NhaXeRepository nhaXeRepository;
    private final LoaiXeRepository loaiXeRepository;
    private final ChuyenXeRepository chuyenXeRepository;

    public XeController(XeRepository xeRepository,
                        NhaXeRepository nhaXeRepository,
                        LoaiXeRepository loaiXeRepository,
                        ChuyenXeRepository chuyenXeRepository) {
        this.xeRepository = xeRepository;
        this.nhaXeRepository = nhaXeRepository;
        this.loaiXeRepository = loaiXeRepository;
        this.chuyenXeRepository = chuyenXeRepository;
    }

    @GetMapping
    public List<Xe> list() {
        return xeRepository.findAll();
    }

    @GetMapping("/{id}")
    public Xe get(@PathVariable Integer id) {
        return xeRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Xe không tồn tại."));
    }

    @PostMapping
    @PreAuthorize("hasRole('Admin')")
    public Xe create(@Valid @RequestBody Request request) {
        Xe xe = new Xe();
        apply(xe, request);
        return xeRepository.save(xe);
    }

    @PutMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Xe update(@PathVariable Integer id, @Valid @RequestBody Request request) {
        Xe xe = get(id);
        apply(xe, request);
        return xeRepository.save(xe);
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Map<String, String> delete(@PathVariable Integer id) {
        Xe xe = get(id);
        if (chuyenXeRepository.existsByMaXe(id)) {
            throw ApiException.conflict("Không thể xóa xe đã có chuyến.");
        }
        xeRepository.delete(xe);
        return Map.of("message", "Xóa xe thành công.");
    }

    private void apply(Xe xe, Request request) {
        if (!nhaXeRepository.existsById(request.maNx())) {
            throw ApiException.notFound("Nhà xe không tồn tại.");
        }
        if (!loaiXeRepository.existsById(request.maLx())) {
            throw ApiException.notFound("Loại xe không tồn tại.");
        }
        xe.setMaNx(request.maNx());
        xe.setMaLx(request.maLx());
        xe.setBienSoXe(request.bienSoXe().trim());
        if (request.trangThai() != null) {
            xe.setTrangThai(request.trangThai());
        }
    }

    public record Request(
            @NotNull(message = "Vui lòng chọn nhà xe.") Integer maNx,
            @NotNull(message = "Vui lòng chọn loại xe.") Integer maLx,
            @NotBlank(message = "Vui lòng nhập biển số xe.") String bienSoXe,
            Boolean trangThai) {
    }
}
