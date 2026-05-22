using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;
using QRCoder;

namespace QLBXMD.GUI.Utils
{
    public class TicketSlipData
    {
        public int MaVe { get; set; }
        public string ChoNgoi { get; set; }
        public decimal GiaVe { get; set; }
        public DateTime? NgayKhoiHanh { get; set; }
        public string DiemDi { get; set; }
        public string DiemDen { get; set; }
        public string BienSoXe { get; set; }
        public string TenNX { get; set; }
        public string SdtNX { get; set; }
    }

    public static class HoaDonRenderer
    {
        public static void ExportToPdf(int maHD)
        {
            try
            {
                using (BXMDEntities db = new BXMDEntities())
                {
                    var hd = db.HoaDons
                        .Include("KhachHang")
                        .Include("NhanVien")
                        .FirstOrDefault(h => h.MaHD == maHD);

                    if (hd == null)
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu hóa đơn mang mã số: " + maHD, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var queryResult = db.VeXes
                        .Where(v => v.MaHD == maHD)
                        .Select(v => new
                        {
                            v.MaVe,
                            v.ChoNgoi,
                            v.GiaVe,
                            NgayKhoiHanh = v.ChuyenXe != null ? (DateTime?)v.ChuyenXe.NgayGioKhoiHanh : null,
                            DiemDi = v.ChuyenXe != null && v.ChuyenXe.TuyenDuong != null ? v.ChuyenXe.TuyenDuong.DiemDi : "N/A",
                            DiemDen = v.ChuyenXe != null && v.ChuyenXe.TuyenDuong != null ? v.ChuyenXe.TuyenDuong.DiemDen : "N/A",
                            BienSoXe = v.ChuyenXe != null && v.ChuyenXe.Xe != null ? v.ChuyenXe.Xe.BienSoXe : "N/A",
                            TenNX = v.ChuyenXe != null && v.ChuyenXe.Xe != null && v.ChuyenXe.Xe.NhaXe != null ? v.ChuyenXe.Xe.NhaXe.TenNX : "N/A",
                            SdtNX = v.ChuyenXe != null && v.ChuyenXe.Xe != null && v.ChuyenXe.Xe.NhaXe != null ? v.ChuyenXe.Xe.NhaXe.SDT : "N/A"
                        })
                        .ToList();

                    var veXes = queryResult.Select(v => new TicketSlipData
                    {
                        MaVe = v.MaVe,
                        ChoNgoi = v.ChoNgoi.ToString(),
                        GiaVe = v.GiaVe.HasValue ? v.GiaVe.Value : 0m,
                        NgayKhoiHanh = v.NgayKhoiHanh,
                        DiemDi = v.DiemDi,
                        DiemDen = v.DiemDen,
                        BienSoXe = v.BienSoXe,
                        TenNX = v.TenNX,
                        SdtNX = v.SdtNX
                    }).ToList();

                    if (veXes.Count == 0)
                    {
                        MessageBox.Show("Hóa đơn này chưa có vé xe nào để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "PDF Files (*.pdf)|*.pdf";
                        sfd.FileName = $"HoaDon_{maHD}.pdf";
                        sfd.Title = "Chọn nơi lưu hóa đơn PDF";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                            pd.PrinterSettings.PrintToFile = true;
                            pd.PrinterSettings.PrintFileName = sfd.FileName;
                            pd.PrintController = new StandardPrintController();

                            PaperSize a4Size = null;
                            foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
                            {
                                if (ps.Kind == PaperKind.A4)
                                {
                                    a4Size = ps;
                                    break;
                                }
                            }
                            if (a4Size != null)
                            {
                                pd.DefaultPageSettings.PaperSize = a4Size;
                            }

                            pd.DefaultPageSettings.Landscape = false;
                            pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

                            int currentTicketIndex = 0;
                            pd.PrintPage += (sender, e) =>
                            {
                                if (currentTicketIndex < veXes.Count)
                                {
                                    var ve = veXes[currentTicketIndex];
                                    DrawTicketSlip(e.Graphics, hd, ve, e.PageBounds);

                                    currentTicketIndex++;
                                    e.HasMorePages = (currentTicketIndex < veXes.Count);
                                }
                                else
                                {
                                    e.HasMorePages = false;
                                }
                            };

                            pd.Print();

                            MessageBox.Show(
                                "Xuất hóa đơn PDF thành công!\n\nĐường dẫn: " + sfd.FileName,
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xuất hóa đơn PDF: " + ex.Message + "\n\nLưu ý: Hãy chắc chắn máy tính của bạn đã bật tính năng 'Microsoft Print to PDF' trong Windows Features.",
                    "Lỗi xuất PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void DrawTicketSlip(Graphics g, HoaDon hd, TicketSlipData ve, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Nền trắng cho hóa đơn
            g.Clear(Color.White);

            // Fonts
            Font fontTicketNo = new Font("Consolas", 10, FontStyle.Regular);
            Font fontNX = new Font("Segoe UI", 12, FontStyle.Bold);
            Font fontNXSub = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            Font fontTitle = new Font("Segoe UI", 14, FontStyle.Bold);
            Font fontDate = new Font("Segoe UI", 9.5f, FontStyle.Italic);
            Font fontLabel = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontRouteVal = new Font("Segoe UI", 13, FontStyle.Bold);
            Font fontNormal = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontBold = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            Font fontPrice = new Font("Segoe UI", 13.5f, FontStyle.Bold);
            Font fontNote = new Font("Segoe UI", 8.5f, FontStyle.Italic);

            Brush brushMain = new SolidBrush(Color.Black);
            Pen penDashed = new Pen(Color.FromArgb(148, 163, 184), 1f) { DashStyle = DashStyle.Dash };
            Pen penBorder = new Pen(Color.FromArgb(203, 213, 225), 1.5f);

            // Căn giữa vé đứng trên trang giấy A4 (rộng: 827px, cao: 1169px)
            int cardWidth = 500;
            int cardHeight = 750;
            int left = (bounds.Width - cardWidth) / 2;
            int top = 80;

            Rectangle cardRect = new Rectangle(left, top, cardWidth, cardHeight);
            
            // Vẽ viền bao quanh vé
            g.DrawRectangle(penBorder, cardRect);

            StringFormat rightAlign = new StringFormat { Alignment = StringAlignment.Far };
            StringFormat centerAlign = new StringFormat { Alignment = StringAlignment.Center };

            // 1. Số vé ở góc phải
            string ticketNoText = "Số: " + ((int)ve.MaVe).ToString("D7");
            g.DrawString(ticketNoText, fontTicketNo, brushMain, new Rectangle(left + 20, top + 15, cardWidth - 40, 20), rightAlign);

            // 2. Tên Nhà xe / HTX
            string rawNX = ve.TenNX ?? "Huy Hoàng";
            string displayNX = rawNX.ToUpper();
            if (!displayNX.Contains("HTX") && !displayNX.Contains("CÔNG TY") && !displayNX.Contains("HỢP TÁC XÃ"))
            {
                displayNX = "HTX CƠ KHÍ VÀ VẬN TẢI Ô TÔ " + displayNX;
            }
            g.DrawString(displayNX, fontNX, brushMain, new Rectangle(left + 20, top + 45, cardWidth - 40, 50), centerAlign);

            // 3. Địa chỉ, Mã số thuế, Số điện thoại động
            string address = GetAddressForNhaXe(rawNX);
            string mst = GetMstForNhaXe(rawNX);
            string phone = string.IsNullOrEmpty(ve.SdtNX) || ve.SdtNX == "N/A" ? "0839904777" : ve.SdtNX;

            g.DrawString("Địa chỉ: " + address, fontNXSub, brushMain, new Rectangle(left + 20, top + 95, cardWidth - 40, 35), centerAlign);
            g.DrawString("Mã số thuế: " + mst, fontNXSub, brushMain, new Rectangle(left + 20, top + 130, cardWidth - 40, 18), centerAlign);
            g.DrawString("Số điện thoại: " + phone, fontNXSub, brushMain, new Rectangle(left + 20, top + 148, cardWidth - 40, 18), centerAlign);

            // Đường nét đứt ngang ngăn cách
            g.DrawLine(penDashed, left + 30, top + 178, left + cardWidth - 30, top + 178);

            // 4. Tiêu đề "BẢN THỂ HIỆN VÉ XE KHÁCH"
            g.DrawString("BẢN THỂ HIỆN VÉ XE KHÁCH", fontTitle, brushMain, new Rectangle(left + 20, top + 195, cardWidth - 40, 30), centerAlign);
            
            DateTime departureDate = ve.NgayKhoiHanh ?? DateTime.Now;
            string dateVietnamese = $"(Ngày {departureDate.Day} tháng {departureDate.Month} năm {departureDate.Year})";
            g.DrawString(dateVietnamese, fontDate, brushMain, new Rectangle(left + 20, top + 225, cardWidth - 40, 20), centerAlign);

            // 5. Tuyến đường
            g.DrawString("Tuyến đường:", fontLabel, brushMain, new Rectangle(left + 20, top + 265, cardWidth - 40, 20), centerAlign);
            
            string routeText = (ve.DiemDi ?? "Đồng Hới") + " - " + (ve.DiemDen ?? "Trung Tâm Đà Nẵng");
            g.DrawString(routeText, fontRouteVal, brushMain, new Rectangle(left + 20, top + 285, cardWidth - 40, 30), centerAlign);

            // 6. Ghế và Số xe
            int col1X = left + 75;
            int col2X = left + 275;
            int infoY = top + 335;

            g.DrawString("Tên chỗ: ", fontNormal, brushMain, col1X, infoY);
            g.DrawString(ve.ChoNgoi ?? "KSD", fontBold, brushMain, col1X + 65, infoY);

            g.DrawString("Số xe: ", fontNormal, brushMain, col2X, infoY);
            g.DrawString(ve.BienSoXe ?? "73B-00696", fontBold, brushMain, col2X + 50, infoY);

            // 7. Thời gian khởi hành
            int timeY = top + 370;
            string timeString = departureDate.ToString("HH:mm, dd/MM/yyyy");
            g.DrawString("Thời gian khởi hành: ", fontNormal, brushMain, col1X, timeY);
            g.DrawString(timeString, fontBold, brushMain, col1X + 135, timeY);

            // Đường nét đứt ngang ngăn cách
            g.DrawLine(penDashed, left + 30, top + 415, left + cardWidth - 30, top + 415);

            // 8. Giá tiền
            int priceY = top + 435;
            string totalText = "TỔNG TIỀN: " + ((decimal)ve.GiaVe).ToString("#,##0") + " VNĐ";
            g.DrawString(totalText, fontPrice, brushMain, new Rectangle(left + 20, priceY, cardWidth - 40, 30), centerAlign);
            g.DrawString("(Đã bao gồm thuế GTGT và BHHK)", fontNote, brushMain, new Rectangle(left + 20, priceY + 30, cardWidth - 40, 20), centerAlign);

            // 9. Mã QR Code định danh
            int qrSize = 180;
            int qrX = left + (cardWidth - qrSize) / 2;
            int qrY = top + 510;

            string qrPayload = $"VE:{ve.MaVe}|GHE:{ve.ChoNgoi}|XE:{ve.BienSoXe}|GIA:{ve.GiaVe}|TG:{timeString}";
            Bitmap qrBmp = GenerateQRCode(qrPayload);
            if (qrBmp != null)
            {
                g.DrawImage(qrBmp, qrX, qrY, qrSize, qrSize);
            }
        }

        private static Bitmap GenerateQRCode(string payload)
        {
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        return qrCode.GetGraphic(20);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private static string GetAddressForNhaXe(string tenNX)
        {
            if (string.IsNullOrEmpty(tenNX) || tenNX == "N/A")
                return "292 Đinh Bộ Lĩnh, Phường 26, Quận Bình Thạnh, TP. Hồ Chí Minh";
            if (tenNX.Contains("Huy Hoàng") || tenNX.Contains("Huy Hoang"))
                return "Số 08 Lý Thái Tổ - Phường Bắc Nghĩa - Thành phố Đồng Hới - Quảng Bình";
            if (tenNX.Contains("Phượng Hoàng"))
                return "Huyện Quỳnh Phụ, Tỉnh Thái Bình";
            return "Khu liên hợp Bến Xe Miền Đông, TP. Hồ Chí Minh";
        }

        private static string GetMstForNhaXe(string tenNX)
        {
            if (string.IsNullOrEmpty(tenNX) || tenNX == "N/A")
                return "0312548796";
            if (tenNX.Contains("Huy Hoàng") || tenNX.Contains("Huy Hoang"))
                return "3100194308";
            int hash = Math.Abs(tenNX.GetHashCode() % 1000000);
            return "031" + hash.ToString("D7");
        }
    }
}
