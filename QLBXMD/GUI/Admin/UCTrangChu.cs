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
			ApplyDashboardTheme();
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

		private void btnRefresh_Click(object sender, EventArgs e)
		{
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

		private void ApplyDashboardTheme()
		{
			BackColor = Color.FromArgb(248, 250, 252);
			pnlHero.BackColor = Color.FromArgb(240, 253, 250);
			lblWelcome.ForeColor = Color.FromArgb(15, 23, 42);
			lblSubtitle.ForeColor = Color.FromArgb(71, 85, 105);
			lblHeroBadge.BackColor = Color.FromArgb(204, 251, 241);
			lblHeroBadge.ForeColor = Color.FromArgb(15, 118, 110);

			StyleCard(pnlCard1, Color.FromArgb(255, 247, 237));
			StyleCard(pnlCard2, Color.FromArgb(239, 246, 255));
			StyleCard(pnlCard3, Color.FromArgb(236, 253, 245));
			StyleCard(pnlCard4, Color.FromArgb(238, 242, 255));
			StyleSurface(pnlHero);
			StyleSurface(pnlFilter);
			StyleSurface(pnlTableWrap);

			pnlTableHeader.BackColor = Color.FromArgb(248, 250, 252);
			btnRefresh.BackColor = Color.FromArgb(20, 184, 166);
			btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(13, 148, 136);
			btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(15, 118, 110);

			dgvVeSold.EnableHeadersVisualStyles = false;
			dgvVeSold.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(204, 251, 241);
			dgvVeSold.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(17, 94, 89);
			dgvVeSold.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			dgvVeSold.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgvVeSold.ColumnHeadersHeight = 42;
			dgvVeSold.DefaultCellStyle.BackColor = Color.White;
			dgvVeSold.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
			dgvVeSold.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 251, 241);
			dgvVeSold.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
			dgvVeSold.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
			dgvVeSold.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
			dgvVeSold.GridColor = Color.FromArgb(226, 232, 240);
			dgvVeSold.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			dgvVeSold.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
		}

		private void StyleCard(Panel panel, Color accent)
		{
			panel.Padding = new Padding(0);
			panel.Margin = new Padding(panel.Margin.Left, 0, panel.Margin.Right, 0);
			panel.Paint += (sender, e) =>
			{
				using (var topBrush = new SolidBrush(accent))
				using (var borderPen = new Pen(Color.FromArgb(226, 232, 240)))
				{
					e.Graphics.FillRectangle(topBrush, 0, 0, panel.Width, 6);
					e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
				}
			};
		}

		private void StyleSurface(Panel panel)
		{
			panel.Paint += (sender, e) =>
			{
				using (var borderPen = new Pen(Color.FromArgb(226, 232, 240)))
				{
					e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
				}
			};
		}
	}
}
