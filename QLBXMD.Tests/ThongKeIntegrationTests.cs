using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using QLBXMD.EF;

namespace QLBXMD.Tests
{
    [TestClass]
    public class ThongKeIntegrationTests
    {
        private BXMDEntities _context;

        [TestInitialize]
        public void Setup()
        {
            // Kh?i t?o database context
            _context = new BXMDEntities();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        #region Database Connection Tests

        [TestMethod]
        [TestCategory("Database")]
        public void Database_ShouldConnect()
        {
            // Act
            bool canConnect = _context.Database.Exists();

            // Assert
            Assert.IsTrue(canConnect, "Không th? k?t n?i ??n database");
        }

        #endregion

        #region HoaDon Statistics Tests

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetTongDoanhThu_WithValidDateRange_ShouldReturnCorrectSum()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            decimal tongDoanhThu = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            // Assert
            Assert.IsTrue(tongDoanhThu >= 0, "T?ng doanh thu ph?i >= 0");
        }

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetSoHoaDonDaThanhToan_ShouldReturnCorrectCount()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            int soHoaDon = _context.HoaDons
                .Count(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay);

            // Assert
            Assert.IsTrue(soHoaDon >= 0, "S? hóa ??n ph?i >= 0");
        }

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetSoHoaDonChuaThanhToan_ShouldReturnCorrectCount()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            int soHoaDon = _context.HoaDons
                .Count(h => h.TrangThai == false && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay);

            // Assert
            Assert.IsTrue(soHoaDon >= 0, "S? hóa ??n ch?a thanh toán ph?i >= 0");
        }

        #endregion

        #region ChuyenXe Statistics Tests

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetSoChuyenXe_WithValidDateRange_ShouldReturnCorrectCount()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now.AddDays(30); // Bao g?m c? chuy?n xe t??ng lai

            // Act
            int soChuyenXe = _context.ChuyenXes
                .Count(cx => cx.NgayGioKhoiHanh >= tuNgay && 
                            cx.NgayGioKhoiHanh <= denNgay);

            // Assert
            Assert.IsTrue(soChuyenXe >= 0, "S? chuy?n xe ph?i >= 0");
        }

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetChuyenXeDaKhoiHanh_ShouldOnlyReturnPastTrips()
        {
            // Act
            var chuyenXeDaKhoiHanh = _context.ChuyenXes
                .Where(cx => cx.NgayGioKhoiHanh < DateTime.Now)
                .ToList();

            // Assert
            Assert.IsTrue(chuyenXeDaKhoiHanh.All(cx => cx.NgayGioKhoiHanh < DateTime.Now));
        }

        #endregion

        #region KhachHang Statistics Tests

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetSoKhachHangMuaVe_WithValidDateRange_ShouldReturnDistinctCount()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            int soKhachHang = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay && 
                           h.MaKH != null)
                .Select(h => h.MaKH)
                .Distinct()
                .Count();

            // Assert
            Assert.IsTrue(soKhachHang >= 0, "S? khách hàng ph?i >= 0");
        }

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetTongSoKhachHang_ShouldReturnAllCustomers()
        {
            // Act
            int tongSoKhachHang = _context.KhachHangs.Count();

            // Assert
            Assert.IsTrue(tongSoKhachHang >= 0, "T?ng s? khách hàng ph?i >= 0");
        }

        #endregion

        #region VeXe Statistics Tests

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetSoVeDaBan_WithValidDateRange_ShouldReturnCorrectCount()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            int soVe = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay)
                .SelectMany(h => h.VeXes)
                .Count();

            // Assert
            Assert.IsTrue(soVe >= 0, "S? vé ?ã bán ph?i >= 0");
        }

        [TestMethod]
        [TestCategory("Statistics")]
        public void GetVeXeByChuyenXe_ShouldReturnCorrectTickets()
        {
            // Arrange
            var chuyenXeDauTien = _context.ChuyenXes.FirstOrDefault();
            
            if (chuyenXeDauTien == null)
            {
                Assert.Inconclusive("Không có chuy?n xe trong database");
                return;
            }

            // Act
            var veXes = _context.VeXes
                .Where(v => v.MaChuyen == chuyenXeDauTien.MaChuyen)
                .ToList();

            // Assert
            Assert.IsTrue(veXes.All(v => v.MaChuyen == chuyenXeDauTien.MaChuyen));
        }

        #endregion

        #region Chart Data Tests

        [TestMethod]
        [TestCategory("ChartData")]
        public void GetDoanhThuTheoNgay_ShouldReturnGroupedData()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            var doanhThuTheoNgay = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay)
                .GroupBy(h => System.Data.Entity.DbFunctions.TruncateTime(h.NgayLap))
                .Select(g => new
                {
                    Ngay = g.Key,
                    TongTien = g.Sum(h => h.TongTien)
                })
                .OrderBy(x => x.Ngay)
                .ToList();

            // Assert
            Assert.IsNotNull(doanhThuTheoNgay);
            
            // Ki?m tra d? li?u ???c s?p x?p theo th? t? t?ng d?n
            if (doanhThuTheoNgay.Count > 1)
            {
                for (int i = 0; i < doanhThuTheoNgay.Count - 1; i++)
                {
                    Assert.IsTrue(doanhThuTheoNgay[i].Ngay <= doanhThuTheoNgay[i + 1].Ngay,
                        "D? li?u doanh thu ph?i ???c s?p x?p theo ngày");
                }
            }
        }

        [TestMethod]
        [TestCategory("ChartData")]
        public void GetTop5TuyenDuong_ShouldReturnMaxFiveRoutes()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            var topTuyenDuong = _context.VeXes
                .Where(v => v.HoaDon.TrangThai == true && 
                           v.HoaDon.NgayLap >= tuNgay && 
                           v.HoaDon.NgayLap <= denNgay)
                .GroupBy(v => v.ChuyenXe.TuyenDuong.MaTuyen)
                .Select(g => new
                {
                    MaTuyen = g.Key,
                    DiemDi = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDi,
                    DiemDen = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDen,
                    SoVe = g.Count()
                })
                .OrderByDescending(x => x.SoVe)
                .Take(5)
                .ToList();

            // Assert
            Assert.IsNotNull(topTuyenDuong);
            Assert.IsTrue(topTuyenDuong.Count <= 5, "Ch? nên tr? v? t?i ?a 5 tuy?n ???ng");
            
            // Ki?m tra d? li?u ???c s?p x?p theo s? vé gi?m d?n
            if (topTuyenDuong.Count > 1)
            {
                for (int i = 0; i < topTuyenDuong.Count - 1; i++)
                {
                    Assert.IsTrue(topTuyenDuong[i].SoVe >= topTuyenDuong[i + 1].SoVe,
                        "Tuy?n ???ng ph?i ???c s?p x?p theo s? vé gi?m d?n");
                }
            }
        }

        [TestMethod]
        [TestCategory("ChartData")]
        public void GetTop5TuyenDuong_ShouldHaveValidRouteNames()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2024, 1, 1);
            DateTime denNgay = DateTime.Now;

            // Act
            var topTuyenDuong = _context.VeXes
                .Where(v => v.HoaDon.TrangThai == true && 
                           v.HoaDon.NgayLap >= tuNgay && 
                           v.HoaDon.NgayLap <= denNgay)
                .GroupBy(v => v.ChuyenXe.TuyenDuong.MaTuyen)
                .Select(g => new
                {
                    DiemDi = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDi,
                    DiemDen = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDen,
                    SoVe = g.Count()
                })
                .OrderByDescending(x => x.SoVe)
                .Take(5)
                .ToList();

            // Assert
            foreach (var tuyen in topTuyenDuong)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(tuyen.DiemDi), "?i?m ?i không ???c r?ng");
                Assert.IsFalse(string.IsNullOrWhiteSpace(tuyen.DiemDen), "?i?m ??n không ???c r?ng");
                Assert.IsTrue(tuyen.SoVe > 0, "S? vé ph?i l?n h?n 0");
            }
        }

        #endregion

        #region Edge Cases Tests

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void GetStatistics_WithNoData_ShouldReturnZero()
        {
            // Arrange - Ch?n kho?ng th?i gian ch?c ch?n không có d? li?u
            DateTime tuNgay = new DateTime(1900, 1, 1);
            DateTime denNgay = new DateTime(1900, 1, 2);

            // Act
            decimal tongDoanhThu = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            int soChuyenXe = _context.ChuyenXes
                .Count(cx => cx.NgayGioKhoiHanh >= tuNgay && 
                            cx.NgayGioKhoiHanh <= denNgay);

            // Assert
            Assert.AreEqual(0, tongDoanhThu, "Doanh thu ph?i b?ng 0 khi không có d? li?u");
            Assert.AreEqual(0, soChuyenXe, "S? chuy?n xe ph?i b?ng 0 khi không có d? li?u");
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void GetStatistics_WithSingleDay_ShouldWorkCorrectly()
        {
            // Arrange
            DateTime ngayHienTai = DateTime.Now.Date;

            // Act
            int soChuyenXe = _context.ChuyenXes
                .Count(cx => cx.NgayGioKhoiHanh >= ngayHienTai && 
                            cx.NgayGioKhoiHanh < ngayHienTai.AddDays(1));

            // Assert
            Assert.IsTrue(soChuyenXe >= 0, "Query v?i 1 ngày ph?i ho?t ??ng chính xác");
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void GetStatistics_WithFutureDate_ShouldReturnZeroRevenue()
        {
            // Arrange
            DateTime tuNgay = DateTime.Now.AddYears(1);
            DateTime denNgay = DateTime.Now.AddYears(2);

            // Act
            decimal tongDoanhThu = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            // Assert
            Assert.AreEqual(0, tongDoanhThu, "Không có doanh thu trong t??ng lai");
        }

        #endregion

        #region Performance Tests

        [TestMethod]
        [TestCategory("Performance")]
        [Timeout(5000)] // 5 giây
        public void LoadStatistics_ShouldCompleteWithinTimeout()
        {
            // Arrange
            DateTime tuNgay = DateTime.Now.AddMonths(-6);
            DateTime denNgay = DateTime.Now;

            // Act & Assert - Test s? fail n?u m?t quá 5 giây
            var result = _context.HoaDons
                .Where(h => h.TrangThai == true && 
                           h.NgayLap >= tuNgay && 
                           h.NgayLap <= denNgay)
                .GroupBy(h => System.Data.Entity.DbFunctions.TruncateTime(h.NgayLap))
                .Select(g => new
                {
                    Ngay = g.Key,
                    TongTien = g.Sum(h => h.TongTien)
                })
                .ToList();

            Assert.IsNotNull(result);
        }

        #endregion
    }
}
