using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCHoaDon : UserControl
	{
		private sealed class HoaDonActionState
		{
			public bool Exists { get; set; }
			public bool CanEdit { get; set; }
			public bool CanCancel { get; set; }
			public string CancelBlockedReason { get; set; }
		}

		private readonly HoaDonBUS _bus = new HoaDonBUS();
		private List<dynamic> _data;

		public UCHoaDon()
		{
			InitializeComponent();
			btnXem.Text = "Xem chi ti\u1ebft";
			btnXoa.Text = "H\u1ee7y h\u00f3a \u0111\u01a1n";
			this.Load += UCHoaDon_Load;
			this.dgvVe.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			int? maHD = GetSelectedMaHD();

			if (maHD.HasValue)
			{
				LoadVeXeByHoaDon(maHD.Value);
			}
			else
			{
				ClearVeXeGrid();
			}

			UpdateActionButtons();
		}

		private void UCHoaDon_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();
				SetupVeXeGridHeaders();

				var list = _bus.GetAll() ?? new List<HoaDon>();
				_data = list
					.Select(hd => new
					{
						MaHD = hd.MaHD,
						KhachHang = hd.KhachHang != null ? hd.KhachHang.TenKH : "N/A",
						NhanVien = hd.NhanVien != null ? hd.NhanVien.TenNV : "N/A",
						NgayLap = hd.NgayLap.ToString("dd/MM/yyyy HH:mm"),
						TongTien = hd.TongTien?.ToString("N0") ?? "0",
						TrangThai = hd.TrangThai == true ? "\u0110\u00e3 thanh to\u00e1n" : "Ch\u01b0a thanh to\u00e1n",
						MaKH = hd.MaKH,
						MaNV = hd.MaNV,
						NgayLapDate = hd.NgayLap,
						TongTienValue = hd.TongTien,
						TrangThaiValue = hd.TrangThai
					})
					.ToList<dynamic>();

				LoadData(_data);
				InitCbb();
				ClearVeXeGrid();
				UpdateActionButtons();
			}
			catch (Exception ex)
			{
				MessageBox.Show("T\u1ea3i d\u1eef li\u1ec7u th\u1ea5t b\u1ea1i: " + ex.Message, "L\u1ed7i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgvVe.Columns.Clear();
			dgvVe.Columns.Add("MaHD", "M\u00e3 h\u00f3a \u0111\u01a1n");
			dgvVe.Columns.Add("KhachHang", "Kh\u00e1ch h\u00e0ng");
			dgvVe.Columns.Add("NhanVien", "Nh\u00e2n vi\u00ean");
			dgvVe.Columns.Add("NgayLap", "Ng\u00e0y l\u1eadp");
			dgvVe.Columns.Add("TongTien", "T\u1ed5ng ti\u1ec1n (VN\u0110)");
			dgvVe.Columns.Add("TrangThai", "Tr\u1ea1ng th\u00e1i");
		}

		private void SetupVeXeGridHeaders()
		{
			dgvVeXe.Columns.Clear();
			dgvVeXe.Columns.Add("MaVe", "M\u00e3 v\u00e9");
			dgvVeXe.Columns.Add("TuyenDuong", "Tuy\u1ebfn \u0111\u01b0\u1eddng");
			dgvVeXe.Columns.Add("NgayKhoiHanh", "Ng\u00e0y kh\u1edfi h\u00e0nh");
			dgvVeXe.Columns.Add("GioKhoiHanh", "Gi\u1edd kh\u1edfi h\u00e0nh");
			dgvVeXe.Columns.Add("ChoNgoi", "Ch\u1ed7 ng\u1ed3i");
			dgvVeXe.Columns.Add("GiaVe", "Gi\u00e1 v\u00e9 (VN\u0110)");
		}

		private void LoadVeXeByHoaDon(int maHD)
		{
			try
			{
				dgvVeXe.Rows.Clear();

				using (var db = new BXMDEntities())
				{
					var veXeList = db.VeXes
						.Where(v => v.MaHD == maHD)
						.Select(v => new
						{
							MaVe = v.MaVe,
							TuyenDuong = v.ChuyenXe != null && v.ChuyenXe.TuyenDuong != null
								? v.ChuyenXe.TuyenDuong.DiemDi + " - " + v.ChuyenXe.TuyenDuong.DiemDen
								: "N/A",
							NgayGioKhoiHanh = v.ChuyenXe != null
								? v.ChuyenXe.NgayGioKhoiHanh
								: (DateTime?)null,
							ChoNgoi = v.ChoNgoi,
							GiaVe = v.GiaVe ?? 0
						})
						.ToList();

					foreach (var ve in veXeList)
					{
						dgvVeXe.Rows.Add(
							ve.MaVe,
							ve.TuyenDuong,
							ve.NgayGioKhoiHanh.HasValue ? ve.NgayGioKhoiHanh.Value.ToString("dd/MM/yyyy") : "N/A",
							ve.NgayGioKhoiHanh.HasValue ? ve.NgayGioKhoiHanh.Value.ToString("HH:mm") : "N/A",
							ve.ChoNgoi,
							ve.GiaVe.ToString("N0")
						);
					}
				}

				dgvVeXe.ClearSelection();
			}
			catch (Exception ex)
			{
				MessageBox.Show("T\u1ea3i danh s\u00e1ch v\u00e9 th\u1ea5t b\u1ea1i: " + ex.Message, "L\u1ed7i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ClearVeXeGrid()
		{
			dgvVeXe.Rows.Clear();
		}

		private void InitCbb()
		{
			cbbLoai.SelectedIndexChanged -= cbbLoai_SelectedIndexChanged;
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("T\u1ea5t c\u1ea3");
			cbbLoai.Items.Add("M\u00e3 h\u00f3a \u0111\u01a1n");
			cbbLoai.Items.Add("Kh\u00e1ch h\u00e0ng");
			cbbLoai.Items.Add("Nh\u00e2n vi\u00ean");
			cbbLoai.Items.Add("Ng\u00e0y l\u1eadp");
			cbbLoai.Items.Add("Tr\u1ea1ng th\u00e1i");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;
		}

		private void LoadData(List<dynamic> list)
		{
			dgvVe.Rows.Clear();

			foreach (var item in list)
			{
				dgvVe.Rows.Add(
					item.MaHD,
					item.KhachHang,
					item.NhanVien,
					item.NgayLap,
					item.TongTien,
					item.TrangThai
				);
			}

			dgvVe.ClearSelection();
			UpdateActionButtons();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadData(_data);
			txtSearch.Clear();
			UpdateActionButtons();
		}

		private IEnumerable<KeyValuePair<int, string>> GetKhachHangItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.KhachHangs.AsNoTracking()
								 .Select(kh => new { kh.MaKH, kh.TenKH })
								 .ToList();
				return data.Select(kh => new KeyValuePair<int, string>(kh.MaKH, kh.TenKH))
						   .ToList();
			}
		}

		private IEnumerable<KeyValuePair<int, string>> GetNhanVienItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.NhanViens.AsNoTracking()
					 .Select(nv => new { nv.MaNV, nv.TenNV })
					 .ToList();
				return data.Select(nv => new KeyValuePair<int, string>(nv.MaNV, nv.TenNV))
				   .ToList();
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new HoaDonDialog())
			{
				dialog.BindKhachHang(GetKhachHangItems());
				dialog.BindNhanVien(GetNhanVienItems());
				dialog.SetMode(HoaDonDialogMode.Add);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var hd = new HoaDon
					{
						MaKH = dialog.SelectedMaKH,
						MaNV = dialog.SelectedMANV,
						NgayLap = dialog.NgayLapValue,
						TongTien = dialog.TongTienValue,
						TrangThai = dialog.TrangThaiValue
					};

					string result = _bus.Add(hd);
					MessageBox.Show(result, "Th\u00f4ng b\u00e1o", MessageBoxButtons.OK,
						result.Contains("th\u00e0nh c\u00f4ng") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("th\u00e0nh c\u00f4ng"))
					{
						UCHoaDon_Load(null, null);
					}
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			int? maHD = GetSelectedMaHD();
			if (!maHD.HasValue)
			{
				return;
			}

			var existing = _bus.GetById(maHD.Value);
			if (existing == null)
			{
				return;
			}

			using (var dialog = new HoaDonDialog())
			{
				dialog.BindKhachHang(GetKhachHangItems());
				dialog.BindNhanVien(GetNhanVienItems());
				dialog.SetMode(HoaDonDialogMode.View);
				dialog.MaHD = existing.MaHD.ToString();
				dialog.SelectedMaKH = existing.MaKH;
				dialog.SelectedMANV = existing.MaNV;
				dialog.NgayLapValue = existing.NgayLap;
				dialog.TongTienValue = existing.TongTien ?? 0;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.LoadVeXeByHoaDon(existing.MaHD);
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			int? maHD = GetSelectedMaHD();
			if (!maHD.HasValue)
			{
				return;
			}

			HoaDonActionState actionState = GetHoaDonActionState(maHD.Value);
			if (!actionState.Exists)
			{
				MessageBox.Show("Kh\u00f4ng t\u00ecm th\u1ea5y h\u00f3a \u0111\u01a1n.", "L\u1ed7i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!actionState.CanEdit)
			{
				MessageBox.Show(
					"Ch\u1ec9 c\u00f3 th\u1ec3 s\u1eeda h\u00f3a \u0111\u01a1n \u1edf tr\u1ea1ng th\u00e1i ch\u01b0a thanh to\u00e1n.",
					"Kh\u00f4ng th\u1ec3 ch\u1ec9nh s\u1eeda",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				return;
			}

			var existing = _bus.GetById(maHD.Value);
			if (existing == null)
			{
				MessageBox.Show("Kh\u00f4ng t\u00ecm th\u1ea5y h\u00f3a \u0111\u01a1n.", "L\u1ed7i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new HoaDonDialog())
			{
				dialog.BindKhachHang(GetKhachHangItems());
				dialog.BindNhanVien(GetNhanVienItems());
				dialog.SetMode(HoaDonDialogMode.Edit);
				dialog.MaHD = existing.MaHD.ToString();
				dialog.SelectedMaKH = existing.MaKH;
				dialog.SelectedMANV = existing.MaNV;
				dialog.NgayLapValue = existing.NgayLap;
				dialog.TongTienValue = existing.TongTien ?? 0;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.LoadVeXeByHoaDon(existing.MaHD);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var update = new HoaDon
					{
						MaHD = existing.MaHD,
						MaKH = dialog.SelectedMaKH,
						MaNV = dialog.SelectedMANV,
						NgayLap = dialog.NgayLapValue,
						TongTien = dialog.TongTienValue,
						TrangThai = dialog.TrangThaiValue
					};

					string result = _bus.Update(update);
					MessageBox.Show(result, "Th\u00f4ng b\u00e1o", MessageBoxButtons.OK,
						result.Contains("th\u00e0nh c\u00f4ng") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("th\u00e0nh c\u00f4ng"))
					{
						UCHoaDon_Load(null, null);
					}
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgvVe.SelectedRows.Count == 0 || dgvVe.SelectedRows[0].Cells[0].Value == null)
			{
				MessageBox.Show("Vui l\u00f2ng ch\u1ecdn h\u00f3a \u0111\u01a1n c\u1ea7n h\u1ee7y.", "Th\u00f4ng b\u00e1o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maHD = int.Parse(dgvVe.SelectedRows[0].Cells[0].Value.ToString());
			HoaDonActionState actionState = GetHoaDonActionState(maHD);
			if (!actionState.Exists)
			{
				MessageBox.Show("Kh\u00f4ng t\u00ecm th\u1ea5y h\u00f3a \u0111\u01a1n.", "L\u1ed7i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!actionState.CanCancel)
			{
				MessageBox.Show(
					actionState.CancelBlockedReason ?? "H\u00f3a \u0111\u01a1n n\u00e0y kh\u00f4ng th\u1ec3 h\u1ee7y.",
					"Kh\u00f4ng th\u1ec3 h\u1ee7y h\u00f3a \u0111\u01a1n",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				return;
			}

			string lyDoHuy = PromptCancelReason(maHD);
			if (string.IsNullOrWhiteSpace(lyDoHuy))
			{
				return;
			}

			var r = dgvVe.SelectedRows[0];
			string khachHang = r.Cells[1].Value != null ? r.Cells[1].Value.ToString() : "N/A";
			string nhanVien = r.Cells[2].Value != null ? r.Cells[2].Value.ToString() : "N/A";
			string ngayLap = r.Cells[3].Value != null ? r.Cells[3].Value.ToString() : "N/A";
			string tongTien = r.Cells[4].Value != null ? r.Cells[4].Value.ToString() : "N/A";

			var confirmResult = MessageBox.Show(
				"B\u1ea1n c\u00f3 ch\u1eafc ch\u1eafn mu\u1ed1n h\u1ee7y h\u00f3a \u0111\u01a1n?\n\n" +
				"Kh\u00e1ch h\u00e0ng: " + khachHang + "\n" +
				"Nh\u00e2n vi\u00ean: " + nhanVien + "\n" +
				"Ng\u00e0y l\u1eadp: " + ngayLap + "\n" +
				"T\u1ed5ng ti\u1ec1n: " + tongTien + " VN\u0110\n" +
				"L\u00fd do h\u1ee7y: " + lyDoHuy,
				"X\u00e1c nh\u1eadn h\u1ee7y h\u00f3a \u0111\u01a1n",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = CancelHoaDon(maHD, lyDoHuy);
				MessageBox.Show(result, "Th\u00f4ng b\u00e1o", MessageBoxButtons.OK,
					result.Contains("th\u00e0nh c\u00f4ng") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("th\u00e0nh c\u00f4ng"))
				{
					UCHoaDon_Load(null, null);
				}
			}
		}

		private void Filter()
		{
			if (_data == null)
			{
				return;
			}

			string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0)
			{
				LoadData(_data);
				return;
			}

			int selected = cbbLoai.SelectedIndex;
			List<dynamic> filtered = _data;

			switch (selected)
			{
				case 1:
					filtered = _data.Where(x => x.MaHD.ToString().ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 2:
					filtered = _data.Where(x => (x.KhachHang ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 3:
					filtered = _data.Where(x => (x.NhanVien ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 4:
					filtered = _data.Where(x => (x.NgayLap ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 5:
					filtered = _data.Where(x => (x.TrangThai ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				default:
					filtered = _data.Where(x => new[]
					{
						x.MaHD.ToString(),
						x.KhachHang,
						x.NhanVien,
						x.NgayLap,
						x.TrangThai
					}
					.Any(v => (v ?? string.Empty).ToLowerInvariant().Contains(keyword))).ToList();
					break;
			}

			LoadData(filtered);
		}

		private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || dgvVe.Rows[e.RowIndex].Cells[0].Value == null)
			{
				UpdateActionButtons();
				return;
			}

			dgvVe.Rows[e.RowIndex].Selected = true;
			UpdateActionButtons();
		}

		private void EnableBtn(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
		}

		private int? GetSelectedMaHD()
		{
			if (dgvVe.SelectedRows.Count == 0 || dgvVe.SelectedRows[0].Cells[0].Value == null)
			{
				return null;
			}

			int maHD;
			return int.TryParse(dgvVe.SelectedRows[0].Cells[0].Value.ToString(), out maHD)
				? maHD
				: (int?)null;
		}

		private HoaDonActionState GetHoaDonActionState(int maHD)
		{
			using (var db = new BXMDEntities())
			{
				var hoaDon = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
				if (hoaDon == null)
				{
					return new HoaDonActionState
					{
						Exists = false,
						CanEdit = false,
						CanCancel = false,
						CancelBlockedReason = "Kh\u00f4ng t\u00ecm th\u1ea5y h\u00f3a \u0111\u01a1n."
					};
				}

				bool isPaid = hoaDon.TrangThai == true;
				bool hasStartedTrip = db.VeXes
					.Where(v => v.MaHD == maHD)
					.Any(v => v.ChuyenXe != null && v.ChuyenXe.NgayGioKhoiHanh <= DateTime.Now);

				string cancelBlockedReason = null;
				if (isPaid)
				{
					cancelBlockedReason = "H\u00f3a \u0111\u01a1n \u0111\u00e3 thanh to\u00e1n ch\u1ec9 \u0111\u01b0\u1ee3c xem chi ti\u1ebft. Kh\u00f4ng th\u1ec3 h\u1ee7y ho\u1eb7c ch\u1ec9nh s\u1eeda.";
				}
				else if (hasStartedTrip)
				{
					cancelBlockedReason = "Kh\u00f4ng th\u1ec3 h\u1ee7y h\u00f3a \u0111\u01a1n khi chuy\u1ebfn xe \u0111\u00e3 ch\u1ea1y ho\u1eb7c \u0111ang di\u1ec5n ra.";
				}

				return new HoaDonActionState
				{
					Exists = true,
					CanEdit = !isPaid,
					CanCancel = !isPaid && !hasStartedTrip,
					CancelBlockedReason = cancelBlockedReason
				};
			}
		}

		private void UpdateActionButtons()
		{
			int? maHD = GetSelectedMaHD();
			if (!maHD.HasValue)
			{
				EnableBtn(false);
				return;
			}

			HoaDonActionState actionState = GetHoaDonActionState(maHD.Value);
			btnXem.Enabled = actionState.Exists;
			btnSua.Enabled = actionState.Exists && actionState.CanEdit;
			btnXoa.Enabled = actionState.Exists && actionState.CanEdit;
		}

		private string PromptCancelReason(int maHD)
		{
			using (var form = new Form())
			using (var lblTitle = new Label())
			using (var txtReason = new TextBox())
			using (var btnOk = new Button())
			using (var btnCancel = new Button())
			{
				form.Text = "L\u00fd do h\u1ee7y h\u00f3a \u0111\u01a1n";
				form.FormBorderStyle = FormBorderStyle.FixedDialog;
				form.StartPosition = FormStartPosition.CenterParent;
				form.MinimizeBox = false;
				form.MaximizeBox = false;
				form.ShowInTaskbar = false;
				form.ClientSize = new Size(500, 230);

				lblTitle.AutoSize = false;
				lblTitle.Location = new Point(16, 16);
				lblTitle.Size = new Size(468, 50);
				lblTitle.Text = "Nh\u1eadp l\u00fd do h\u1ee7y cho h\u00f3a \u0111\u01a1n #" + maHD + ".\r\nTh\u00f4ng tin n\u00e0y l\u00e0 b\u1eaft bu\u1ed9c tr\u01b0\u1edbc khi thao t\u00e1c.";

				txtReason.Location = new Point(20, 72);
				txtReason.Multiline = true;
				txtReason.ScrollBars = ScrollBars.Vertical;
				txtReason.Size = new Size(460, 100);

				btnOk.Text = "X\u00e1c nh\u1eadn";
				btnOk.Location = new Point(286, 184);
				btnOk.Size = new Size(90, 30);
				btnOk.DialogResult = DialogResult.None;
				btnOk.Click += (sender, e) =>
				{
					if (string.IsNullOrWhiteSpace(txtReason.Text))
					{
						MessageBox.Show(
							"Vui l\u00f2ng nh\u1eadp l\u00fd do h\u1ee7y h\u00f3a \u0111\u01a1n.",
							"Thi\u1ebfu th\u00f4ng tin",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning);
						txtReason.Focus();
						return;
					}

					form.DialogResult = DialogResult.OK;
					form.Close();
				};

				btnCancel.Text = "\u0110\u00f3ng";
				btnCancel.Location = new Point(390, 184);
				btnCancel.Size = new Size(90, 30);
				btnCancel.DialogResult = DialogResult.Cancel;

				form.Controls.Add(lblTitle);
				form.Controls.Add(txtReason);
				form.Controls.Add(btnOk);
				form.Controls.Add(btnCancel);
				form.AcceptButton = btnOk;
				form.CancelButton = btnCancel;

				return form.ShowDialog(this) == DialogResult.OK
					? txtReason.Text.Trim()
					: null;
			}
		}

		private string CancelHoaDon(int maHD, string lyDoHuy)
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					var hoaDon = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
					if (hoaDon == null)
					{
						return "Kh\u00f4ng t\u00ecm th\u1ea5y h\u00f3a \u0111\u01a1n.";
					}

					if (hoaDon.TrangThai == true)
					{
						return "H\u00f3a \u0111\u01a1n \u0111\u00e3 thanh to\u00e1n ch\u1ec9 \u0111\u01b0\u1ee3c xem chi ti\u1ebft. Kh\u00f4ng th\u1ec3 h\u1ee7y.";
					}

					bool hasStartedTrip = db.VeXes
						.Where(v => v.MaHD == maHD)
						.Any(v => v.ChuyenXe != null && v.ChuyenXe.NgayGioKhoiHanh <= DateTime.Now);

					if (hasStartedTrip)
					{
						return "Kh\u00f4ng th\u1ec3 h\u1ee7y h\u00f3a \u0111\u01a1n khi chuy\u1ebfn xe \u0111\u00e3 ch\u1ea1y ho\u1eb7c \u0111ang di\u1ec5n ra.";
					}

					var thanhToans = db.ThanhToans.Where(tt => tt.MaHD == maHD).ToList();
					if (thanhToans.Count > 0)
					{
						db.ThanhToans.RemoveRange(thanhToans);
					}

					var veXes = db.VeXes.Where(v => v.MaHD == maHD).ToList();
					if (veXes.Count > 0)
					{
						db.VeXes.RemoveRange(veXes);
					}

					db.HoaDons.Remove(hoaDon);
					db.SaveChanges();
				}

				System.Diagnostics.Debug.WriteLine(
					string.Format("Huy hoa don #{0}. Ly do: {1}", maHD, lyDoHuy));

				return "H\u1ee7y h\u00f3a \u0111\u01a1n th\u00e0nh c\u00f4ng.";
			}
			catch (Exception ex)
			{
				return "H\u1ee7y h\u00f3a \u0111\u01a1n th\u1ea5t b\u1ea1i: " + ex.Message;
			}
		}
	}
}
