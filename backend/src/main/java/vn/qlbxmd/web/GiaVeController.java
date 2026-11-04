package vn.qlbxmd.web;

import jakarta.validation.Valid;
import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotNull;
import java.math.BigDecimal;
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
import vn.qlbxmd.domain.GiaVe;
import vn.qlbxmd.repo.GiaVeRepository;
import vn.qlbxmd.repo.LoaiXeRepository;
import vn.qlbxmd.repo.NhaXeRepository;
import vn.qlbxmd.repo.TuyenDuongRepository;

@RestController
@RequestMapping("/api/gia-ve")
@PreAuthorize("hasAnyRole('Admin','NhanVien')")
public class GiaVeController {

    private final GiaVeRepository giaVeRepository;
    private final NhaXeRepository nhaXeRepository;
    private final LoaiXeRepository loaiXeRepository;
    private final TuyenDuongRepository tuyenDuongRepository;

    public GiaVeController(GiaVeRepository giaVeRepository,
                           NhaXeRepository nhaXeRepository,
                           LoaiXeRepository loaiXeRepository,
                           TuyenDuongRepository tuyenDuongRepository) {
        this.giaVeRepository = giaVeRepository;
        this.nhaXeRepository = nhaXeRepository;
        this.loaiXeRepository = loaiXeRepository;
        this.tuyenDuongRepository = tuyenDuongRepository;
    }

    @GetMapping
    public List<GiaVe> list() {
        return giaVeRepository.findAll();
    }

    @GetMapping("/{id}")
    public GiaVe get(@PathVariable Integer id) {
        return giaVeRepository.findById(id)
                .orElseThrow(() -> ApiException.notFound("Giá vé không tồn tại."));
    }

    @PostMapping
    @PreAuthorize("hasRole('Admin')")
    public GiaVe create(@Valid @RequestBody Request request) {
        validateReferences(request);
        if (giaVeRepository.existsByMaNxAndMaLxAndMaTuyen(
                request.maNx(), request.maLx(), request.maTuyen())) {
            throw ApiException.conflict("Giá vé cho nhà xe, loại xe và tuyến đường này đã tồn tại.");
        }
        GiaVe giaVe = new GiaVe();
        apply(giaVe, request);
        return giaVeRepository.save(giaVe);
    }

    @PutMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public GiaVe update(@PathVariable Integer id, @Valid @RequestBody Request request) {
        GiaVe giaVe = get(id);
        validateReferences(request);
        giaVeRepository.findByMaNxAndMaLxAndMaTuyen(request.maNx(), request.maLx(), request.maTuyen())
                .filter(other -> !other.getMaGv().equals(id))
                .ifPresent(other -> {
                    throw ApiException.conflict("Giá vé cho nhà xe, loại xe và tuyến đường này đã tồn tại.");
                });
        apply(giaVe, request);
        return giaVeRepository.save(giaVe);
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Map<String, String> delete(@PathVariable Integer id) {
        GiaVe giaVe = get(id);
        giaVeRepository.delete(giaVe);
        return Map.of("message", "Xóa giá vé thành công.");
    }

    private void validateReferences(Request request) {
        if (!nhaXeRepository.existsById(request.maNx())) {
            throw ApiException.notFound("Nhà xe không tồn tại.");
        }
        if (!loaiXeRepository.existsById(request.maLx())) {
            throw ApiException.notFound("Loại xe không tồn tại.");
        }
        if (!tuyenDuongRepository.existsById(request.maTuyen())) {
            throw ApiException.notFound("Tuyến đường không tồn tại.");
        }
    }

    private void apply(GiaVe giaVe, Request request) {
        giaVe.setMaNx(request.maNx());
        giaVe.setMaLx(request.maLx());
        giaVe.setMaTuyen(request.maTuyen());
        giaVe.setGiaVe(request.giaVe());
    }

    public record Request(
            @NotNull(message = "Vui lòng chọn nhà xe.") Integer maNx,
            @NotNull(message = "Vui lòng chọn loại xe.") Integer maLx,
            @NotNull(message = "Vui lòng chọn tuyến đường.") Integer maTuyen,
            @NotNull(message = "Vui lòng nhập giá vé.")
            @DecimalMin(value = "1", message = "Giá vé phải lớn hơn 0.") BigDecimal giaVe) {
    }
}
