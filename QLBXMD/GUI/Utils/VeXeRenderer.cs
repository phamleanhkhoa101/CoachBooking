using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using QLBXMD.EF;
using QRCoder;

namespace QLBXMD.GUI.Utils
{
    public static class VeXeRenderer
    {
        public static Bitmap GenerateTicket(VeXe ve, ChuyenXe chuyen, KhachHang khach, NhaXe nhaXe, TuyenDuong tuyen)
        {
            // Kích thước vé: Rộng 400px, Dài tự động (khoảng 700px)
            int width = 450;
            int height = 800;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                // Nền trắng
                g.Clear(Color.White);

                int y = 20; // Vị trí Y hiện tại
                int padding = 20;

                // Fonts
                Font fontTitle = new Font("Arial", 14, FontStyle.Bold);
                Font fontBold = new Font("Arial", 12, FontStyle.Bold);
                Font fontNormal = new Font("Arial", 11, FontStyle.Regular);
                Font fontSmall = new Font("Arial", 10, FontStyle.Regular);
                Font fontItalic = new Font("Arial", 10, FontStyle.Italic);
                
                StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };
                StringFormat leftFormat = new StringFormat { Alignment = StringAlignment.Near };
                StringFormat rightFormat = new StringFormat { Alignment = StringAlignment.Far };

                // 1. HEADER (Thông tin nhà xe)
                string tenNhaXe = nhaXe?.TenNX?.ToUpper() ?? "HTX CƠ KHÍ VÀ VẬN TẢI Ô TÔ HUY HOÀNG";
                DrawWrappedText(g, tenNhaXe, fontTitle, new Rectangle(padding, y, width - padding * 2, 60), leftFormat);
                y += (int)g.MeasureString(tenNhaXe, fontTitle, width - padding * 2).Height + 5;

                //g.DrawString($"Địa chỉ: {nhaXe?.DiaChi ?? "Số 08 Lý Thái Tổ - Đồng Hới - Quảng Bình"}", fontSmall, Brushes.Black, new Rectangle(padding, y, width - padding * 2, 40));
                //y += 40;

                g.DrawString($"Số điện thoại: {nhaXe?.SDT ?? "0839904777"}", fontSmall, Brushes.Black, padding, y);
                y += 30;

                // Dấu gạch ngang
                g.DrawLine(new Pen(Color.Black, 1) { DashStyle = DashStyle.Dash }, padding, y, width - padding, y);
                y += 20;

                // 2. TITLE
                g.DrawString("BẢN THỂ HIỆN VÉ XE KHÁCH", fontBold, Brushes.Black, new Rectangle(0, y, width, 30), centerFormat);
                y += 25;
                g.DrawString($"(Ngày {DateTime.Now.Day:00} tháng {DateTime.Now.Month:00} năm {DateTime.Now.Year})", fontItalic, Brushes.Black, new Rectangle(0, y, width, 30), centerFormat);
                y += 40;

                // 3. THÔNG TIN CHUYẾN
                g.DrawString("Tuyến đường:", fontBold, Brushes.Black, new Rectangle(0, y, width, 30), centerFormat);
                y += 25;
                string diemDiDen = tuyen != null ? $"{tuyen.DiemDi} - {tuyen.DiemDen}" : "N/A";
                g.DrawString(diemDiDen, fontBold, Brushes.Black, new Rectangle(0, y, width, 30), centerFormat);
                y += 40;

                // Ghế & Số xe
                string ghe = $"Tên chỗ: {ve?.ChoNgoi.ToString() ?? "N/A"}";
                string bsx = $"Số xe: {chuyen?.Xe?.BienSoXe ?? "N/A"}";
                g.DrawString(ghe, fontNormal, Brushes.Black, padding, y);
                g.DrawString(bsx, fontNormal, Brushes.Black, new Rectangle(0, y, width - padding, 30), rightFormat);
                y += 35;

                // Thời gian khởi hành
                string timeStr = chuyen?.NgayGioKhoiHanh != null ? chuyen.NgayGioKhoiHanh.ToString("HH:mm, dd/MM/yyyy") : "N/A";
                g.DrawString("Thời gian khởi hành: " + timeStr, fontNormal, Brushes.Black, padding, y);
                y += 40;

                // 4. TIỀN TỆ
                string tienStr = $"TỔNG TIỀN: {(ve?.GiaVe ?? 0).ToString("#,##0")} VNĐ";
                g.DrawString(tienStr, fontBold, Brushes.Black, new Rectangle(0, y, width, 30), centerFormat);
                y += 25;
                g.DrawString("(Đã bao gồm thuế GTGT và BHHK)", fontSmall, Brushes.Black, new Rectangle(0, y, width, 30), centerFormat);
                y += 40;

                // 5. QR CODE
                string qrData = $"VE:{ve?.MaVe}|HD:{ve?.MaHD}|SDT:{khach?.SDT}|GHE:{ve?.ChoNgoi}";
                Bitmap qrCodeBmp = GenerateQRCode(qrData);
                if (qrCodeBmp != null)
                {
                    int qrSize = 250;
                    int qrX = (width - qrSize) / 2;
                    g.DrawImage(qrCodeBmp, qrX, y, qrSize, qrSize);
                    y += qrSize + 20;
                }

                // Cắt ảnh cho vừa vặn
                Bitmap finalBmp = new Bitmap(width, y);
                using (Graphics fg = Graphics.FromImage(finalBmp))
                {
                    fg.DrawImage(bmp, 0, 0, new Rectangle(0, 0, width, y), GraphicsUnit.Pixel);
                }

                return finalBmp;
            }
        }

        private static void DrawWrappedText(Graphics g, string text, Font font, Rectangle rect, StringFormat format)
        {
            g.DrawString(text, font, Brushes.Black, rect, format);
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
                // Fallback nếu thiếu thư viện QRCoder
                return null;
            }
        }
    }
}
