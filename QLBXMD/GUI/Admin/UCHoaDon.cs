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
			btnExportPdf.Enabled = enabled;
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
			btnExportPdf.Enabled = actionState.Exists;
		}

		private void btnExportPdf_Click(object sender, EventArgs e)
		{
			int? maHD = GetSelectedMaHD();
			if (!maHD.HasValue)
			{
				MessageBox.Show("Vui lòng chọn một hóa đơn để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Kiểm tra trạng thái thanh toán từ Database
			using (var db = new BXMDEntities())
			{
				var hd = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD.Value);
				if (hd == null)
				{
					MessageBox.Show("Không tìm thấy dữ liệu hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (hd.TrangThai != true)
				{
					MessageBox.Show("Chỉ hóa đơn đã thanh toán mới có thể xuất hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
			}

			QLBXMD.GUI.Utils.HoaDonRenderer.ExportToPdf(maHD.Value);
		}
	}
}
