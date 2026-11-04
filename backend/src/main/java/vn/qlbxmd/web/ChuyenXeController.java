package vn.qlbxmd.web;

import jakarta.validation.Valid;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import vn.qlbxmd.domain.ChuyenXe;
import vn.qlbxmd.repo.ChuyenXeRepository;
import vn.qlbxmd.service.TripService;

@RestController
@RequestMapping("/api/chuyen-xe")
public class ChuyenXeController {

    private final ChuyenXeRepository chuyenXeRepository;
    private final TripService tripService;

    public ChuyenXeController(ChuyenXeRepository chuyenXeRepository, TripService tripService) {
        this.chuyenXeRepository = chuyenXeRepository;
        this.tripService = tripService;
    }

    @GetMapping
    @PreAuthorize("hasAnyRole('Admin','NhanVien')")
    public List<ChuyenXeRepository.TripRow> list() {
        return tripService.listAll();
    }

    @GetMapping("/tim")
    @PreAuthorize("isAuthenticated()")
    public List<ChuyenXeRepository.TripRow> search(
            @RequestParam(required = false) String diemDi,
            @RequestParam(required = false) String diemDen,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate ngay,
            @RequestParam(required = false) Integer tuGio,
            @RequestParam(required = false) Integer denGio) {
        return tripService.search(diemDi, diemDen, ngay, tuGio, denGio);
    }

    @GetMapping("/{id}/ghe")
    @PreAuthorize("isAuthenticated()")
    public TripService.SeatMapResponse seatMap(@PathVariable Integer id) {
        return tripService.seatMap(id);
    }

    @GetMapping("/{id}")
    @PreAuthorize("hasAnyRole('Admin','NhanVien')")
    public ChuyenXe get(@PathVariable Integer id) {
        return chuyenXeRepository.findById(id)
                .orElseThrow(() -> vn.qlbxmd.common.ApiException.notFound("Chuyến xe không tồn tại."));
    }

    @PostMapping
    @PreAuthorize("hasRole('Admin')")
    public ChuyenXe create(@Valid @RequestBody TripService.TripRequest request) {
        return tripService.create(request);
    }

    @PutMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public ChuyenXe update(@PathVariable Integer id, @Valid @RequestBody TripService.TripRequest request) {
        return tripService.update(id, request);
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('Admin')")
    public Map<String, String> delete(@PathVariable Integer id) {
        return tripService.delete(id);
    }
}
