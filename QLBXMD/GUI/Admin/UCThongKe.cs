using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCThongKe : UserControl
	{
		public UCThongKe()
		{
			InitializeComponent();
			SetupDateTimePickers();
			SetupCharts();
		}

		private void SetupDateTimePickers()
		{
			// Mặc định: từ đầu tháng đến ngày hiện tại
			dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			dtpDenNgay.Value = DateTime.Now;
		}

		private void SetupCharts()
		{
			// Cấu hình chart doanh thu
			chartDoanhThu.Titles.Clear();
			chartDoanhThu.Titles.Add("Biểu Đồ Doanh Thu Theo Ngày");
			chartDoanhThu.Titles[0].Font = new Font("Segoe UI", 14F, FontStyle.Bold);
			chartDoanhThu.Titles[0].ForeColor = Color.FromArgb(33, 150, 243);

			chartDoanhThu.ChartAreas[0].AxisX.Title = "Ngày";
			chartDoanhThu.ChartAreas[0].AxisX.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
			chartDoanhThu.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
			chartDoanhThu.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
			chartDoanhThu.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
			chartDoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";

			// Cấu hình chart tuyến đường
			chartTuyenDuong.Titles.Clear();
			chartTuyenDuong.Titles.Add("Top 5 Tuyến Đường Phổ Biến");
			chartTuyenDuong.Titles[0].Font = new Font("Segoe UI", 14F, FontStyle.Bold);
			chartTuyenDuong.Titles[0].ForeColor = Color.FromArgb(76, 175, 80);
			
			chartTuyenDuong.Series[0].IsValueShownAsLabel = true;
			chartTuyenDuong.Series[0].LabelFormat = "#,##0";
			chartTuyenDuong.Series[0]["PieLabelStyle"] = "Outside";
			chartTuyenDuong.Legends[0].Docking = Docking.Bottom;

			// Cấu hình chart nhà xe
			chartNhaXe.Titles.Clear();
			chartNhaXe.Titles.Add("Doanh thu theo Nhà xe");
			chartNhaXe.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			chartNhaXe.Titles[0].ForeColor = Color.FromArgb(33, 150, 243);
			chartNhaXe.Series[0].IsValueShownAsLabel = true;
			chartNhaXe.Series[0].LabelFormat = "#,##0";
			chartNhaXe.Series[0]["PieLabelStyle"] = "Outside";
			chartNhaXe.Legends[0].Docking = Docking.Bottom;
		}

		private void UCThongKe_Load(object sender, EventArgs e)
		{
			LoadStatistics();
		}

		private void btnThongKe_Click(object sender, EventArgs e)
		{
			if (dtpTuNgay.Value > dtpDenNgay.Value)
			{
				MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Thông báo", 
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			LoadStatistics();
		}

		private void LoadStatistics()
		{
			try
			{
				DateTime tuNgay = dtpTuNgay.Value.Date;
				DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

				using (var db = new BXMDEntities())
				{
					// 1. Tổng doanh thu
					decimal tongDoanhThu = db.HoaDons
						.Where(h => h.TrangThai == true && h.NgayLap >= tuNgay && h.NgayLap <= denNgay)
						.Sum(h => (decimal?)h.TongTien) ?? 0;
					lblDoanhThuValue.Text = tongDoanhThu.ToString("#,##0") + " VNĐ";

					// 2. Số chuyến xe
					int soChuyenXe = db.ChuyenXes
						.Where(cx => cx.NgayGioKhoiHanh >= tuNgay && cx.NgayGioKhoiHanh <= denNgay)
						.Count();
					lblSoChuyenXeValue.Text = soChuyenXe.ToString();

					// 3. Số khách hàng mua vé
					int soKhachHang = db.HoaDons
						.Where(h => h.TrangThai == true && h.NgayLap >= tuNgay && h.NgayLap <= denNgay && h.MaKH != null)
						.Select(h => h.MaKH)
						.Distinct()
						.Count();
					lblSoKhachHangValue.Text = soKhachHang.ToString();

					// 4. Số vé đã bán
					int soVe = db.HoaDons
						.Where(h => h.TrangThai == true && h.NgayLap >= tuNgay && h.NgayLap <= denNgay)
						.SelectMany(h => h.VeXes)
						.Count();
					lblSoVeValue.Text = soVe.ToString();

					// 5. Biểu đồ doanh thu theo ngày
					LoadDoanhThuChart(db, tuNgay, denNgay);

					// 6. Biểu đồ tuyến đường
					LoadTuyenDuongChart(db, tuNgay, denNgay);

					// 7. Biểu đồ nhà xe
					LoadNhaXeChart(db, tuNgay, denNgay);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi tải thống kê:\n{ex.Message}", "Lỗi", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadDoanhThuChart(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
		{
			var doanhThuTheoNgay = db.HoaDons
				.Where(h => h.TrangThai == true && h.NgayLap >= tuNgay && h.NgayLap <= denNgay)
				.GroupBy(h => System.Data.Entity.DbFunctions.TruncateTime(h.NgayLap))
				.Select(g => new
				{
					Ngay = g.Key,
					TongTien = g.Sum(h => h.TongTien)
				})
				.OrderBy(x => x.Ngay)
				.ToList();

			chartDoanhThu.Series.Clear();
			var series = new Series
			{
				Name = "Doanh thu",
				ChartType = SeriesChartType.Column,
				Color = Color.FromArgb(33, 150, 243),
				BorderWidth = 2,
				IsValueShownAsLabel = true,
				LabelFormat = "#,##0"
			};

			if (doanhThuTheoNgay.Any())
			{
				foreach (var item in doanhThuTheoNgay)
				{
					series.Points.AddXY(
						item.Ngay.HasValue ? item.Ngay.Value.ToString("dd/MM") : "",
						(double)(item.TongTien ?? 0)
					);
				}
			}
			else
			{
				// Nếu không có dữ liệu, hiển thị placeholder
				series.Points.AddXY("Không có dữ liệu", 0);
			}

			chartDoanhThu.Series.Add(series);
			
			// Thêm hiệu ứng
			chartDoanhThu.ChartAreas[0].Area3DStyle.Enable3D = false;
			chartDoanhThu.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
			chartDoanhThu.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
		}

		private void LoadTuyenDuongChart(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
		{
			// Lấy top 5 tuyến đường có nhiều vé bán nhất
			var topTuyenDuong = db.VeXes
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

			chartTuyenDuong.Series.Clear();
			var series = new Series
			{
				Name = "Số vé",
				ChartType = SeriesChartType.Pie,
				IsValueShownAsLabel = true
			};

			if (topTuyenDuong.Any())
			{
				// Màu sắc đẹp cho pie chart
				Color[] colors = new Color[]
				{
					Color.FromArgb(33, 150, 243),   // Blue
					Color.FromArgb(76, 175, 80),    // Green
					Color.FromArgb(255, 152, 0),    // Orange
					Color.FromArgb(156, 39, 176),   // Purple
					Color.FromArgb(244, 67, 54)     // Red
				};

				int colorIndex = 0;
				foreach (var item in topTuyenDuong)
				{
					string tenHienThi = $"{item.DiemDi} - {item.DiemDen}";
					var point = series.Points.Add(item.SoVe);
					point.Label = $"{tenHienThi}\n{item.SoVe} vé";
					point.LegendText = tenHienThi;
					point.Color = colors[colorIndex % colors.Length];
					colorIndex++;
				}

				series["PieLabelStyle"] = "Outside";
				series["PieLineColor"] = "Black";
			}
			else
			{
				// Nếu không có dữ liệu
				var point = series.Points.Add(1);
				point.Label = "Không có dữ liệu";
				point.LegendText = "Không có dữ liệu";
				point.Color = Color.LightGray;
			}

			chartTuyenDuong.Series.Add(series);
			
			// Cấu hình thêm
			chartTuyenDuong.ChartAreas[0].Area3DStyle.Enable3D = false;
		}

		private void LoadNhaXeChart(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
		{
			// Tính tổng doanh thu theo Nhà xe (NhaXe.TenNX) trong khoảng
			var q = db.VeXes
				.Where(v => v.HoaDon.TrangThai == true && v.HoaDon.NgayLap >= tuNgay && v.HoaDon.NgayLap <= denNgay)
				.GroupBy(v => v.ChuyenXe.Xe.NhaXe.MaNX)
				.Select(g => new
				{
					MaNX = g.Key,
					TenNX = g.FirstOrDefault().ChuyenXe.Xe.NhaXe.TenNX,
					TongTien = g.Sum(v => (decimal?)v.GiaVe * 1) // sum of ticket prices
				})
				.OrderByDescending(x => x.TongTien)
				.ToList();

			chartNhaXe.Series.Clear();
			var series = new Series
			{
				Name = "DoanhThuNX",
				ChartType = SeriesChartType.Pie,
				IsValueShownAsLabel = true
			};

			if (q.Any())
			{
				Color[] colors = new Color[]
				{
					Color.FromArgb(33, 150, 243),
					Color.FromArgb(76, 175, 80),
					Color.FromArgb(255, 152, 0),
					Color.FromArgb(156, 39, 176),
					Color.FromArgb(244, 67, 54)
				};

				int idx = 0;
				foreach (var item in q)
				{
					var p = series.Points.Add((double)(item.TongTien ?? 0));
					p.Label = $"{item.TenNX}\n{(item.TongTien ?? 0):#,##0} VNĐ";
					p.LegendText = item.TenNX;
					p.Color = colors[idx % colors.Length];
					idx++;
				}
			}
			else
			{
				var p = series.Points.Add(1);
				p.Label = "Không có dữ liệu";
				p.LegendText = "Không có dữ liệu";
				p.Color = Color.LightGray;
			}

			chartNhaXe.Series.Add(series);
			chartNhaXe.ChartAreas[0].Area3DStyle.Enable3D = false;
		}
	}
}
