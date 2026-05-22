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
			SetupComboBox();
		}

		private void SetupDateTimePickers()
		{
			// Mặc định: từ đầu tháng đến ngày hiện tại
			dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			dtpDenNgay.Value = DateTime.Now;
		}

		private void SetupComboBox()
		{
			cboLoaiBaoCao.Items.Clear();
			cboLoaiBaoCao.Items.Add("Báo cáo doanh thu theo ngày");
			cboLoaiBaoCao.Items.Add("Hiệu suất hoạt động của Nhà xe");
			cboLoaiBaoCao.Items.Add("Hiệu suất hoạt động của Tuyến đường");
			cboLoaiBaoCao.SelectedIndex = 0;
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

					// 5. Doanh thu trung bình trên mỗi vé (KPI mới)
					decimal trungBinhVe = soVe > 0 ? (tongDoanhThu / soVe) : 0;
					lblTrungBinhVeValue.Text = trungBinhVe.ToString("#,##0") + " VNĐ";

					// 6. Biểu đồ doanh thu theo ngày
					LoadDoanhThuChart(db, tuNgay, denNgay);

					// 7. Biểu đồ tuyến đường
					LoadTuyenDuongChart(db, tuNgay, denNgay);

					// 8. Biểu đồ nhà xe
					LoadNhaXeChart(db, tuNgay, denNgay);
					
					// 9. Báo cáo chi tiết dạng bảng
					LoadDetailedReport();
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

		// ================= BÁO CÁO CHI TIẾT DẠNG BẢNG =================

		private void cboLoaiBaoCao_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadDetailedReport();
		}

		private void LoadDetailedReport()
		{
			try
			{
				DateTime tuNgay = dtpTuNgay.Value.Date;
				DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

				using (var db = new BXMDEntities())
				{
					int selectedIndex = cboLoaiBaoCao.SelectedIndex;
					switch (selectedIndex)
					{
						case 0:
							LoadDailyReport(db, tuNgay, denNgay);
							break;
						case 1:
							LoadOperatorReport(db, tuNgay, denNgay);
							break;
						case 2:
							LoadRouteReport(db, tuNgay, denNgay);
							break;
					}
					AdminGridTheme.Apply(dgvChiTiet);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi tải báo cáo chi tiết:\n{ex.Message}", "Lỗi", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadDailyReport(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
		{
			var dailyData = db.HoaDons
				.Where(h => h.TrangThai == true && h.NgayLap >= tuNgay && h.NgayLap <= denNgay)
				.GroupBy(h => System.Data.Entity.DbFunctions.TruncateTime(h.NgayLap))
				.Select(g => new
				{
					Ngay = g.Key,
					SoHoaDon = g.Count(),
					SoVe = g.SelectMany(h => h.VeXes).Count(),
					DoanhThu = g.Sum(h => h.TongTien) ?? 0
				})
				.OrderBy(x => x.Ngay)
				.ToList()
				.Select(x => new
				{
					Ngay = x.Ngay.HasValue ? x.Ngay.Value.ToString("dd/MM/yyyy") : "",
					SoHoaDon = x.SoHoaDon,
					SoVe = x.SoVe,
					DoanhThu = x.DoanhThu,
					DoanhThuTB = x.SoVe > 0 ? (x.DoanhThu / x.SoVe) : 0
				})
				.ToList();

			dgvChiTiet.DataSource = dailyData;
			
			if (dgvChiTiet.Columns.Count > 0)
			{
				dgvChiTiet.Columns["Ngay"].HeaderText = "Ngày";
				dgvChiTiet.Columns["SoHoaDon"].HeaderText = "Số hóa đơn";
				dgvChiTiet.Columns["SoVe"].HeaderText = "Số vé bán";
				dgvChiTiet.Columns["DoanhThu"].HeaderText = "Doanh thu";
				dgvChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "#,##0 VNĐ";
				dgvChiTiet.Columns["DoanhThuTB"].HeaderText = "Doanh thu TB/Vé";
				dgvChiTiet.Columns["DoanhThuTB"].DefaultCellStyle.Format = "#,##0 VNĐ";
			}
		}

		private void LoadOperatorReport(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
		{
			// 1. Lấy tất cả chuyến đi và tổng ghế trong khoảng thời gian theo nhà xe
			var operatorTrips = db.ChuyenXes
				.Where(cx => cx.NgayGioKhoiHanh >= tuNgay && cx.NgayGioKhoiHanh <= denNgay)
				.GroupBy(cx => cx.Xe.NhaXe.MaNX)
				.Select(g => new
				{
					MaNX = g.Key,
					SoChuyen = g.Count(),
					TongGhe = g.Sum(cx => (int?)cx.Xe.LoaiXe.SoCho) ?? 0
				})
				.ToList();

			// 2. Lấy số lượng vé bán và doanh thu theo nhà xe
			var operatorSales = db.VeXes
				.Where(v => v.HoaDon.TrangThai == true && v.HoaDon.NgayLap >= tuNgay && v.HoaDon.NgayLap <= denNgay)
				.GroupBy(v => v.ChuyenXe.Xe.NhaXe.MaNX)
				.Select(g => new
				{
					MaNX = g.Key,
					TenNX = g.FirstOrDefault().ChuyenXe.Xe.NhaXe.TenNX,
					SoVe = g.Count(),
					DoanhThu = g.Sum(v => (decimal?)v.GiaVe) ?? 0
				})
				.ToList();

			// 3. Kết hợp 2 nguồn dữ liệu trong bộ nhớ để tính Tỷ lệ lấp đầy ghế %
			var operatorData = (from s in operatorSales
								join t in operatorTrips on s.MaNX equals t.MaNX into joined
								from t in joined.DefaultIfEmpty()
								select new
								{
									TenNX = s.TenNX,
									SoChuyen = t != null ? t.SoChuyen : 0,
									SoVe = s.SoVe,
									DoanhThu = s.DoanhThu,
									TyLeLapDay = (t != null && t.TongGhe > 0) 
										? ((double)s.SoVe / t.TongGhe * 100) 
										: 0
								})
								.OrderByDescending(x => x.DoanhThu)
								.ToList()
								.Select(x => new
								{
									TenNX = x.TenNX,
									SoChuyen = x.SoChuyen,
									SoVe = x.SoVe,
									DoanhThu = x.DoanhThu,
									TyLeLapDay = x.TyLeLapDay.ToString("0.0") + "%"
								})
								.ToList();

			dgvChiTiet.DataSource = operatorData;

			if (dgvChiTiet.Columns.Count > 0)
			{
				dgvChiTiet.Columns["TenNX"].HeaderText = "Tên nhà xe";
				dgvChiTiet.Columns["SoChuyen"].HeaderText = "Số chuyến xe";
				dgvChiTiet.Columns["SoVe"].HeaderText = "Số vé bán";
				dgvChiTiet.Columns["DoanhThu"].HeaderText = "Doanh thu";
				dgvChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "#,##0 VNĐ";
				dgvChiTiet.Columns["TyLeLapDay"].HeaderText = "Tỷ lệ lấp đầy ghế";
			}
		}

		private void LoadRouteReport(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
		{
			// 1. Lấy số chuyến xe theo tuyến đường trong khoảng thời gian
			var routeTrips = db.ChuyenXes
				.Where(cx => cx.NgayGioKhoiHanh >= tuNgay && cx.NgayGioKhoiHanh <= denNgay)
				.GroupBy(cx => cx.MaTuyen)
				.Select(g => new
				{
					MaTuyen = g.Key,
					SoChuyen = g.Count()
				})
				.ToList();

			// 2. Lấy số lượng vé bán và doanh thu theo tuyến đường
			var routeSales = db.VeXes
				.Where(v => v.HoaDon.TrangThai == true && v.HoaDon.NgayLap >= tuNgay && v.HoaDon.NgayLap <= denNgay)
				.GroupBy(v => v.ChuyenXe.MaTuyen)
				.Select(g => new
				{
					MaTuyen = g.Key,
					DiemDi = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDi,
					DiemDen = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDen,
					SoVe = g.Count(),
					DoanhThu = g.Sum(v => (decimal?)v.GiaVe) ?? 0
				})
				.ToList();

			// 3. Kết hợp trong bộ nhớ
			var routeData = (from s in routeSales
							 join t in routeTrips on s.MaTuyen equals t.MaTuyen into joined
							 from t in joined.DefaultIfEmpty()
							 select new
							 {
								 TuyenDuong = $"{s.DiemDi} - {s.DiemDen}",
								 SoChuyen = t != null ? t.SoChuyen : 0,
								 SoVe = s.SoVe,
								 DoanhThu = s.DoanhThu
							 })
							 .OrderByDescending(x => x.DoanhThu)
							 .ToList();

			dgvChiTiet.DataSource = routeData;

			if (dgvChiTiet.Columns.Count > 0)
			{
				dgvChiTiet.Columns["TuyenDuong"].HeaderText = "Tuyến đường";
				dgvChiTiet.Columns["SoChuyen"].HeaderText = "Số chuyến khai thác";
				dgvChiTiet.Columns["SoVe"].HeaderText = "Số vé bán";
				dgvChiTiet.Columns["DoanhThu"].HeaderText = "Doanh thu";
				dgvChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "#,##0 VNĐ";
			}
		}

		private void btnExportCSV_Click(object sender, EventArgs e)
		{
			string prefix = "BaoCaoThongKe";
			int selectedIndex = cboLoaiBaoCao.SelectedIndex;
			switch (selectedIndex)
			{
				case 0:
					prefix = "DoanhThuTheoNgay";
					break;
				case 1:
					prefix = "HieuSuatNhaXe";
					break;
				case 2:
					prefix = "HieuSuatTuyenDuong";
					break;
			}
			ExportDataGridViewToCSV(dgvChiTiet, prefix);
		}

		private void ExportDataGridViewToCSV(DataGridView dgv, string fileNamePrefix)
		{
			if (dgv.Rows.Count == 0)
			{
				MessageBox.Show("Không có dữ liệu để xuất báo cáo chi tiết.", "Thông báo", 
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{
				SaveFileDialog saveDialog = new SaveFileDialog
				{
					Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
					FileName = $"{fileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
					Title = "Xuất báo cáo chi tiết thống kê"
				};

				if (saveDialog.ShowDialog() == DialogResult.OK)
				{
					using (var writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
					{
						// Viết BOM UTF-8 để Excel mở hiển thị đúng tiếng Việt có dấu
						writer.Write('\uFEFF');

						// Header
						var headers = new List<string>();
						foreach (DataGridViewColumn col in dgv.Columns)
						{
							headers.Add(EscapeCSV(col.HeaderText));
						}
						writer.WriteLine(string.Join(",", headers));

						// Data rows
						foreach (DataGridViewRow row in dgv.Rows)
						{
							if (row.IsNewRow) continue;
							var cells = new List<string>();
							foreach (DataGridViewCell cell in row.Cells)
							{
								cells.Add(EscapeCSV(cell.Value?.ToString() ?? ""));
							}
							writer.WriteLine(string.Join(",", cells));
						}
					}

					MessageBox.Show($"Xuất báo cáo thành công!\nĐường dẫn: {saveDialog.FileName}", "Thành công", 
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi xuất báo cáo:\n{ex.Message}", "Lỗi", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private string EscapeCSV(string text)
		{
			if (text.Contains(",") || text.Contains("\"") || text.Contains("\r") || text.Contains("\n"))
			{
				return $"\"{text.Replace("\"", "\"\"")}\"";
			}
			return text;
		}
	}
}
