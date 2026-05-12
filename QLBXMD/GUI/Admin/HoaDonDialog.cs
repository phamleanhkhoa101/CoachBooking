using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public enum HoaDonDialogMode
	{
		Add,
		View,
		Edit
	}

	public partial class HoaDonDialog : Form
	{
		private bool _runtimeInitialized;
		private HoaDonDialogMode _mode;

		public HoaDonDialogMode Mode
		{
			get { return _mode; }
			private set { _mode = value; }
		}

		public string MaHD
		{
			get { return txtMaHD.Text; }
			set { txtMaHD.Text = value; }
		}

		public DateTime NgayLapValue
		{
			get { return dtpNgayLap.Value; }
			set { dtpNgayLap.Value = value; }
		}

		public decimal TongTienValue
		{
			get { return numTongTien.Value; }
			set { numTongTien.Value = value; }
		}

		public int? SelectedMaKH
		{
			get
			{
				if (cbbKhachHang.SelectedValue is int)
				{
					return (int)cbbKhachHang.SelectedValue;
				}

				return cbbKhachHang.SelectedValue as int?;
			}
			set
			{
				if (value.HasValue)
				{
					cbbKhachHang.SelectedValue = value.Value;
				}
				else
				{
					cbbKhachHang.SelectedIndex = -1;
				}
			}
		}

		public int? SelectedMANV
		{
			get
			{
				if (cbbNhanVien.SelectedValue is int)
				{
					return (int)cbbNhanVien.SelectedValue;
				}

				return cbbNhanVien.SelectedValue as int?;
			}
			set
			{
				if (value.HasValue)
				{
					cbbNhanVien.SelectedValue = value.Value;
				}
				else
				{
					cbbNhanVien.SelectedIndex = -1;
				}
			}
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0)
				{
					return true;
				}

				if (cbbTrangThai.SelectedIndex == 1)
				{
					return false;
				}

				return null;
			}
			set
			{
				if (value == true)
				{
					cbbTrangThai.SelectedIndex = 0;
				}
				else if (value == false)
				{
					cbbTrangThai.SelectedIndex = 1;
				}
				else
				{
					cbbTrangThai.SelectedIndex = -1;
				}
			}
		}

		public HoaDonDialog()
		{
			InitializeComponent();
			_mode = HoaDonDialogMode.Add;
		}

		public void BindKhachHang(IEnumerable<KeyValuePair<int, string>> items)
		{
			List<KeyValuePair<int, string>> list = items != null
				? items.ToList()
				: new List<KeyValuePair<int, string>>();

			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không chọn --"));
			cbbKhachHang.DataSource = list;
			cbbKhachHang.DisplayMember = "Value";
			cbbKhachHang.ValueMember = "Key";
		}

		public void BindNhanVien(IEnumerable<KeyValuePair<int, string>> items)
		{
			List<KeyValuePair<int, string>> list = items != null
				? items.ToList()
				: new List<KeyValuePair<int, string>>();

			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không chọn --"));
			cbbNhanVien.DataSource = list;
			cbbNhanVien.DisplayMember = "Value";
			cbbNhanVien.ValueMember = "Key";
		}

		public void SetMode(HoaDonDialogMode mode)
		{
			EnsureRuntimeInitialized();
			Mode = mode;

			switch (mode)
			{
				case HoaDonDialogMode.Add:
					Text = "Thêm hóa đơn";
					lblHeader.Text = "THÊM HÓA ĐƠN MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.Visible = true;
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					btnCancel.Text = "Hủy";
					btnCancel.BackColor = Color.FromArgb(244, 67, 54);
					btnPrimary.Location = new Point(350, 440);
					btnCancel.Location = new Point(460, 440);
					ClientSize = new Size(630, 515);
					txtMaHD.Enabled = false;
					SetReadOnly(false);
					ToggleTicketGrid(false);
					break;

				case HoaDonDialogMode.Edit:
					Text = "Sửa hóa đơn";
					lblHeader.Text = "CẬP NHẬT HÓA ĐƠN";
					btnPrimary.Text = "Lưu";
					btnPrimary.Visible = true;
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					btnCancel.Text = "Hủy";
					btnCancel.BackColor = Color.FromArgb(244, 67, 54);
					btnPrimary.Location = new Point(680, 590);
					btnCancel.Location = new Point(780, 590);
					ClientSize = new Size(900, 660);
					txtMaHD.Enabled = false;
					SetReadOnly(false);
					ToggleTicketGrid(true);
					break;

				case HoaDonDialogMode.View:
					Text = "Xem hóa đơn";
					lblHeader.Text = "THÔNG TIN HÓA ĐƠN";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(780, 590);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					ClientSize = new Size(900, 660);
					SetReadOnly(true);
					ToggleTicketGrid(true);
					break;
			}
		}

		public void LoadVeXeByHoaDon(int maHD)
		{
			dgvDanhSachVe.Rows.Clear();
			if (maHD <= 0)
			{
				return;
			}

			try
			{
				using (BXMDEntities db = new BXMDEntities())
				{
					var veXeList = db.VeXes
						.Where(v => v.MaHD == maHD)
						.Select(v => new
						{
							MaVe = v.MaVe,
							TuyenDuong = v.ChuyenXe != null && v.ChuyenXe.TuyenDuong != null
								? v.ChuyenXe.TuyenDuong.DiemDi + " - " + v.ChuyenXe.TuyenDuong.DiemDen
								: "N/A",
							NgayKhoiHanh = v.ChuyenXe != null
								? (DateTime?)v.ChuyenXe.NgayGioKhoiHanh
								: null,
							ChoNgoi = v.ChoNgoi,
							GiaVe = v.GiaVe ?? 0,
							NhaXe = v.ChuyenXe != null && v.ChuyenXe.Xe != null && v.ChuyenXe.Xe.NhaXe != null
								? v.ChuyenXe.Xe.NhaXe.TenNX
								: "N/A"
						})
						.ToList();

					foreach (var ve in veXeList)
					{
						dgvDanhSachVe.Rows.Add(
							ve.MaVe,
							ve.TuyenDuong,
							ve.NgayKhoiHanh.HasValue ? ve.NgayKhoiHanh.Value.ToString("dd/MM/yyyy HH:mm") : "N/A",
							ve.ChoNgoi,
							ve.GiaVe.ToString("#,##0"),
							ve.NhaXe
						);
					}
				}

				dgvDanhSachVe.ClearSelection();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					"Không thể tải danh sách vé của hóa đơn: " + ex.Message,
					"Lỗi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private void SetupButtonEffects()
		{
			btnPrimary.MouseEnter += delegate
			{
				if (Mode == HoaDonDialogMode.Edit)
				{
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				}
				else
				{
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
				}
			};

			btnPrimary.MouseLeave += delegate
			{
				if (Mode == HoaDonDialogMode.Edit)
				{
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				}
				else
				{
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
				}
			};

			btnPrimary.MouseDown += delegate
			{
				if (Mode == HoaDonDialogMode.Edit)
				{
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				}
				else
				{
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
				}
			};

			btnPrimary.MouseUp += delegate
			{
				if (Mode == HoaDonDialogMode.Edit)
				{
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				}
				else
				{
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
				}
			};

			btnCancel.MouseEnter += delegate
			{
				btnCancel.BackColor = Color.FromArgb(255, 87, 74);
			};

			btnCancel.MouseLeave += delegate
			{
				btnCancel.BackColor = Color.FromArgb(244, 67, 54);
			};

			btnCancel.MouseDown += delegate
			{
				btnCancel.BackColor = Color.FromArgb(200, 50, 40);
			};

			btnCancel.MouseUp += delegate
			{
				btnCancel.BackColor = Color.FromArgb(255, 87, 74);
			};
		}

		private void SetupNumericUpDownValidation()
		{
			numTongTien.Minimum = 0;
			numTongTien.Maximum = 1000000000;
			numTongTien.DecimalPlaces = 0;
			numTongTien.Increment = 10000;

			numTongTien.ValueChanged += delegate
			{
				if (numTongTien.Value < 0)
				{
					numTongTien.Value = 0;
				}
			};

			numTongTien.KeyPress += delegate(object sender, KeyPressEventArgs e)
			{
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			numTongTien.Leave += delegate
			{
				if (numTongTien.Value < 0)
				{
					numTongTien.Value = 0;
					MessageBox.Show(
						"Tổng tiền không thể là số âm. Giá trị đã được đặt lại về 0.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
				}
			};
		}

		private void SetReadOnly(bool readOnly)
		{
			txtMaHD.Enabled = !readOnly;
			cbbKhachHang.Enabled = !readOnly;
			cbbNhanVien.Enabled = !readOnly;
			dtpNgayLap.Enabled = !readOnly;
			numTongTien.Enabled = !readOnly;
			cbbTrangThai.Enabled = !readOnly;

			cbbKhachHang.TabStop = !readOnly;
			cbbNhanVien.TabStop = !readOnly;
			dtpNgayLap.TabStop = !readOnly;
			numTongTien.TabStop = !readOnly;
			cbbTrangThai.TabStop = !readOnly;

			if (readOnly)
			{
				txtMaHD.BackColor = Color.WhiteSmoke;
				cbbKhachHang.BackColor = Color.WhiteSmoke;
				cbbNhanVien.BackColor = Color.WhiteSmoke;
				dtpNgayLap.BackColor = Color.WhiteSmoke;
				numTongTien.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaHD.BackColor = Color.WhiteSmoke;
				cbbKhachHang.BackColor = Color.White;
				cbbNhanVien.BackColor = Color.White;
				dtpNgayLap.BackColor = Color.White;
				numTongTien.BackColor = Color.White;
				cbbTrangThai.BackColor = Color.White;
			}
		}

		private void btnPrimary_Click(object sender, EventArgs e)
		{
			if (ValidateInput())
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private bool ValidateInput()
		{
			if (Mode == HoaDonDialogMode.View)
			{
				return true;
			}

			int? maKH = SelectedMaKH;
			int? maNV = SelectedMANV;

			if ((!maKH.HasValue || maKH.Value == 0) && (!maNV.HasValue || maNV.Value == 0))
			{
				MessageBox.Show(
					"Vui lòng chọn ít nhất khách hàng hoặc nhân viên.\n\nHóa đơn phải liên kết với ít nhất một khách hàng hoặc nhân viên.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				cbbKhachHang.Focus();
				return false;
			}

			if (dtpNgayLap.Value > DateTime.Now)
			{
				MessageBox.Show(
					"Ngày lập không được lớn hơn ngày hiện tại.\n\nNgày hiện tại: "
						+ DateTime.Now.ToString("dd/MM/yyyy HH:mm")
						+ "\nNgày đã chọn: "
						+ dtpNgayLap.Value.ToString("dd/MM/yyyy HH:mm"),
					"Lỗi ngày lập",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				dtpNgayLap.Focus();
				return false;
			}

			if (dtpNgayLap.Value < DateTime.Now.AddYears(-1))
			{
				DialogResult result = MessageBox.Show(
					"Ngày lập đã chọn là "
						+ DateTime.Now.Subtract(dtpNgayLap.Value).Days
						+ " ngày trước.\n\nBạn có chắc chắn muốn tiếp tục?",
					"Xác nhận ngày lập",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

				if (result != DialogResult.Yes)
				{
					dtpNgayLap.Focus();
					return false;
				}
			}

			if (numTongTien.Value < 0)
			{
				MessageBox.Show(
					"Tổng tiền không thể là số âm.\n\nVui lòng nhập tổng tiền hợp lệ.",
					"Lỗi nhập liệu",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				numTongTien.Value = 0;
				numTongTien.Focus();
				return false;
			}

			if (numTongTien.Value == 0)
			{
				MessageBox.Show(
					"Tổng tiền phải lớn hơn 0.\n\nVui lòng nhập tổng tiền hợp lệ.",
					"Lỗi nhập liệu",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				numTongTien.Focus();
				return false;
			}

			if (numTongTien.Value < 10000)
			{
				DialogResult lowValueResult = MessageBox.Show(
					"Tổng tiền quá thấp (" + numTongTien.Value.ToString("#,##0") + " VNĐ).\n\n"
						+ "Tổng tiền thông thường từ 10,000 VNĐ trở lên.\n"
						+ "Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận tổng tiền",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

				if (lowValueResult != DialogResult.Yes)
				{
					numTongTien.Focus();
					return false;
				}
			}

			if (numTongTien.Value > 100000000)
			{
				DialogResult highValueResult = MessageBox.Show(
					"Tổng tiền rất cao (" + numTongTien.Value.ToString("#,##0") + " VNĐ).\n\n"
						+ "Bạn có chắc chắn tổng tiền này đúng?",
					"Xác nhận tổng tiền",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

				if (highValueResult != DialogResult.Yes)
				{
					numTongTien.Focus();
					return false;
				}
			}

			if (TrangThaiValue == null)
			{
				MessageBox.Show(
					"Vui lòng chọn trạng thái thanh toán.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				cbbTrangThai.Focus();
				return false;
			}

			return true;
		}

		private void ToggleTicketGrid(bool visible)
		{
			lblDanhSachVe.Visible = visible;
			dgvDanhSachVe.Visible = visible;
		}

		private void EnsureRuntimeInitialized()
		{
			if (_runtimeInitialized)
			{
				return;
			}

			cbbTrangThai.SelectedIndex = 1;
			SetupButtonEffects();
			SetupNumericUpDownValidation();
			AdminGridTheme.Apply(dgvDanhSachVe);
			_runtimeInitialized = true;
		}

		private void dgvDanhSachVe_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

			if (dgvDanhSachVe.Columns[e.ColumnIndex].Name == "colAction")
			{
				e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

				bool isPaid = TrangThaiValue == true;
				bool canDelete = !isPaid && dgvDanhSachVe.Rows.Count > 1;
				
				bool canEdit = !isPaid;
				string khStr = dgvDanhSachVe.Rows[e.RowIndex].Cells["colNgayKhoiHanh"].Value?.ToString();
				if (DateTime.TryParseExact(khStr, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime ngayKhoiHanh))
				{
					if (DateTime.Now <= ngayKhoiHanh.AddMinutes(-30))
					{
						canEdit = true;
					}
				}

				int buttonWidth = 60;
				int buttonHeight = 25;
				int spacing = 5;
				int startX = e.CellBounds.Left + (e.CellBounds.Width - (buttonWidth * 2 + spacing)) / 2;
				int startY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;

				// Nút Sửa (Yellow/Orange)
				Rectangle rectSua = new Rectangle(startX, startY, buttonWidth, buttonHeight);
				DrawActionButton(e.Graphics, rectSua, "Sửa", Color.FromArgb(255, 193, 7), QLBXMD.Properties.Resources.edit, canEdit);

				// Nút Xóa (Red)
				Rectangle rectXoa = new Rectangle(startX + buttonWidth + spacing, startY, buttonWidth, buttonHeight);
				DrawActionButton(e.Graphics, rectXoa, "Xóa", Color.FromArgb(244, 67, 54), QLBXMD.Properties.Resources.delete, canDelete);

				e.Handled = true;
			}
		}

		private void DrawActionButton(Graphics g, Rectangle rect, string text, Color baseColor, Image icon, bool enabled)
		{
			Color backColor = enabled ? baseColor : Color.FromArgb(200, 200, 200);
			using (SolidBrush brush = new SolidBrush(backColor))
			{
				g.FillRectangle(brush, rect);
			}

			if (icon != null)
			{
				int iconSize = 14;
				g.DrawImage(icon, rect.X + 4, rect.Y + (rect.Height - iconSize) / 2, iconSize, iconSize);
			}

			using (Font font = new Font("Segoe UI", 8, FontStyle.Bold))
			using (SolidBrush brush = new SolidBrush(Color.White))
			{
				g.DrawString(text, font, brush, rect.X + 20, rect.Y + (rect.Height - 14) / 2);
			}
		}

		private void dgvDanhSachVe_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

			if (dgvDanhSachVe.Columns[e.ColumnIndex].Name == "colAction")
			{
				int buttonWidth = 60;
				int spacing = 5;
				int cellWidth = dgvDanhSachVe.Columns[e.ColumnIndex].Width;
				int startX = (cellWidth - (buttonWidth * 2 + spacing)) / 2;

				Point clickPoint = dgvDanhSachVe.PointToClient(Cursor.Position);
				Rectangle cellRect = dgvDanhSachVe.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
				int localX = clickPoint.X - cellRect.Left;

				bool isPaid = TrangThaiValue == true;
				bool canEdit = !isPaid;
				string khStr = dgvDanhSachVe.Rows[e.RowIndex].Cells["colNgayKhoiHanh"].Value?.ToString();
				if (DateTime.TryParseExact(khStr, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime ngayKhoiHanh))
				{
					if (DateTime.Now <= ngayKhoiHanh.AddMinutes(-30))
					{
						canEdit = true;
					}
				}

				if (localX >= startX && localX <= startX + buttonWidth)
				{
					// Action Sửa
					if (!canEdit)
					{
						if (isPaid) MessageBox.Show("Chỉ được phép đổi ghế trước giờ khởi hành ít nhất 30 phút.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
					HandleTicketAction(e.RowIndex, VeDialogMode.Edit);
				}
				else if (localX >= startX + buttonWidth + spacing && localX <= startX + buttonWidth * 2 + spacing)
				{
					// Action Xóa
					if (isPaid || dgvDanhSachVe.Rows.Count <= 1) return;
					DeleteTicket(e.RowIndex);
				}
			}
		}

		private void HandleTicketAction(int rowIndex, VeDialogMode mode)
		{
			int maVe = (int)dgvDanhSachVe.Rows[rowIndex].Cells["colMaVe"].Value;
			using (var db = new BXMDEntities())
			{
				var ve = db.VeXes.Find(maVe);
				if (ve == null) return;

				using (var dialog = new VeDialog())
				{
					dialog.LoadData(ve.MaChuyen, ve.ChoNgoi, mode);

					if (dialog.ShowDialog() == DialogResult.OK)
					{
						ve.ChoNgoi = dialog.ChoNgoiValue;
						db.SaveChanges();
						LoadVeXeByHoaDon(ve.MaHD);
						RecalculateTotalAmount();
					}
				}
			}
		}

		private void DeleteTicket(int rowIndex)
		{
			if (dgvDanhSachVe.Rows.Count <= 1)
			{
				MessageBox.Show("Không thể xóa. Mỗi hóa đơn phải có ít nhất 1 vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maVe = (int)dgvDanhSachVe.Rows[rowIndex].Cells["colMaVe"].Value;
			if (MessageBox.Show("Bạn có chắc chắn muốn xóa vé này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				using (var db = new BXMDEntities())
				{
					var ve = db.VeXes.Find(maVe);
					if (ve != null)
					{
						int maHD = ve.MaHD;
						db.VeXes.Remove(ve);
						db.SaveChanges();
						LoadVeXeByHoaDon(maHD);
						RecalculateTotalAmount();
					}
				}
			}
		}

		private void RecalculateTotalAmount()
		{
			decimal total = 0;
			foreach (DataGridViewRow row in dgvDanhSachVe.Rows)
			{
				string valStr = row.Cells["colGiaVe"].Value?.ToString()?.Replace(",", "");
				if (decimal.TryParse(valStr, out decimal giaVe))
				{
					total += giaVe;
				}
			}
			numTongTien.Value = total;
		}
	}
}
