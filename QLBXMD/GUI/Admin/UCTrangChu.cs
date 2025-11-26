using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCTrangChu : UserControl
	{
		public UCTrangChu()
		{
			InitializeComponent();
			// Wire events for date pickers to auto-refresh
			this.dtpFrom.ValueChanged += DateRange_ValueChanged;
			this.dtpTo.ValueChanged += DateRange_ValueChanged;
		}

		private void UCTrangChu_Load(object sender, EventArgs e)
		{
			// Set default dates
			dtpTo.Value = DateTime.Now.Date;
			dtpFrom.Value = DateTime.Now.Date.AddDays(-30);
			var from = dtpFrom.Value.Date;
			var to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);
			LoadStatistics(from, to);
			LoadTicketsInRange(from, to);
		}

		private void DateRange_ValueChanged(object sender, EventArgs e)
		{
			if (dtpFrom.Value.Date > dtpTo.Value.Date)
			{
				// simple validation: keep dtpTo at least dtpFrom
				dtpTo.Value = dtpFrom.Value.Date;
			}

			var from = dtpFrom.Value.Date;
			var to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);
			LoadStatistics(from, to);
			LoadTicketsInRange(from, to);
		}

		private void LoadStatistics(DateTime fromDate, DateTime toDate)
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// 1. Số khách hàng mua vé trong khoảng (distinct MaKH where HoaDon.TrangThai == true)
					int soKhachHang = db.HoaDons
						.Where(h => h.TrangThai == true && h.NgayLap >= fromDate && h.NgayLap <= toDate && h.MaKH != null)
						.Select(h => h.MaKH)
						.Distinct()
						.Count();
					lblKhachHangValue.Text = soKhachHang.ToString();

					// 2. Số chuyến xe có khởi hành trong khoảng (theo NgayGioKhoiHanh)
					int soChuyenXe = db.ChuyenXes
						.Where(cx => cx.NgayGioKhoiHanh >= fromDate && cx.NgayGioKhoiHanh <= toDate)
						.Count();
					lblSoChuyenXeValue.Text = soChuyenXe.ToString();

					// 3. Tổng doanh thu: sum ThanhToans (or HoaDons.TongTien) for paid invoices in range
					decimal tongDoanhThu = db.HoaDons
						.Where(h => h.TrangThai == true && h.NgayLap >= fromDate && h.NgayLap <= toDate)
						.Sum(h => (decimal?)h.TongTien) ?? 0;
					lblTongDoanhThuValue.Text = tongDoanhThu.ToString("#,##0") + " VNĐ";

					// 4. Số vé đã bán trong khoảng
					int soVe = db.HoaDons
						.Where(h => h.TrangThai == true && h.NgayLap >= fromDate && h.NgayLap <= toDate)
						.SelectMany(h => h.VeXes)
						.Count();
					lblSoVeValue.Text = soVe.ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadTicketsInRange(DateTime fromDate, DateTime toDate)
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// Query tickets (VeXes) that belong to paid invoices (HoaDons) within range
					var q = from v in db.VeXes
						join h in db.HoaDons on v.MaHD equals h.MaHD
						join cx in db.ChuyenXes on v.MaChuyen equals cx.MaChuyen
						join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
						where h.NgayLap >= fromDate && h.NgayLap <= toDate && h.TrangThai == true
						select new
						{
							MaHD = h.MaHD,
							NguoiMua = h.MaKH != null ? h.KhachHang.TenKH : "Khách lẻ",
							ChoNgoi = v.ChoNgoi,
							GiaVe = v.GiaVe,
							NgayLap = h.NgayLap,
							Tuyen = td.DiemDi + " → " + td.DiemDen,
							NhaXe = cx.Xe == null ? "" : cx.Xe.NhaXe == null ? "" : cx.Xe.NhaXe.TenNX
						};

					var list = q.ToList();

					// Setup DataGridView columns
					dgvVeSold.Columns.Clear();
					dgvVeSold.AutoGenerateColumns = false;

					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaHD", HeaderText = "Mã HD", Name = "MaHD" });
					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NguoiMua", HeaderText = "Người mua", Name = "NguoiMua" });
					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ChoNgoi", HeaderText = "Chỗ ngồi", Name = "ChoNgoi" });
					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GiaVe", HeaderText = "Giá vé", Name = "GiaVe", DefaultCellStyle = new DataGridViewCellStyle { Format = "#,##0" } });
					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgayLap", HeaderText = "Ngày mua", Name = "NgayLap", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tuyen", HeaderText = "Tuyến", Name = "Tuyen" });
					dgvVeSold.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NhaXe", HeaderText = "Nhà xe", Name = "NhaXe" });

					dgvVeSold.DataSource = list;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải vé: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
