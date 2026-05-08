using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCHoaDon : UserControl
	{
		private readonly HoaDonBUS _bus = new HoaDonBUS();
		private List<dynamic> _data;

		public UCHoaDon()
		{
			InitializeComponent();
			this.Load += UCHoaDon_Load;
			this.dgvVe.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgvVe.SelectedRows.Count > 0 && dgvVe.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has; 
			btnSua.Enabled = has; 
			btnXoa.Enabled = has;
			
			// Load vé khi ch?n hóa ðõn
			if (has)
			{
				int maHD = int.Parse(dgvVe.SelectedRows[0].Cells[0].Value.ToString());
				LoadVeXeByHoaDon(maHD);
			}
			else
			{
				ClearVeXeGrid();
			}
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
						TrangThai = hd.TrangThai == true ? "Ð? thanh toán" : "Chýa thanh toán",
						MaKH = hd.MaKH,
						MaNV = hd.MaNV,
						NgayLapDate = hd.NgayLap,
						TongTienValue = hd.TongTien,
						TrangThaiValue = hd.TrangThai
					})
					.ToList<dynamic>();

				LoadData(_data);
				InitCbb();
				btnXem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
				ClearVeXeGrid();
			}
			catch (Exception ex)
			{
				MessageBox.Show("T?i d? li?u th?t b?i: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgvVe.Columns.Clear();
			dgvVe.Columns.Add("MaHD", "M? hóa ðõn");
			dgvVe.Columns.Add("KhachHang", "Khách hàng");
			dgvVe.Columns.Add("NhanVien", "Nhân viên");
			dgvVe.Columns.Add("NgayLap", "Ngày l?p");
			dgvVe.Columns.Add("TongTien", "T?ng ti?n (þ)");
			dgvVe.Columns.Add("TrangThai", "Tr?ng thái");
		}

		private void SetupVeXeGridHeaders()
		{
			dgvVeXe.Columns.Clear();
			dgvVeXe.Columns.Add("MaVe", "M? vé");
			dgvVeXe.Columns.Add("TuyenDuong", "Tuy?n ðý?ng");
			dgvVeXe.Columns.Add("NgayKhoiHanh", "Ngày kh?i hành");
			dgvVeXe.Columns.Add("GioKhoiHanh", "Gi? kh?i hành");
			dgvVeXe.Columns.Add("ChoNgoi", "Ch? ng?i");
			dgvVeXe.Columns.Add("GiaVe", "Giá vé (þ)");
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
				MessageBox.Show("T?i danh sách vé th?t b?i: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ClearVeXeGrid()
		{
			dgvVeXe.Rows.Clear();
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("T?t c?");
			cbbLoai.Items.Add("M? hóa ðõn");
			cbbLoai.Items.Add("Khách hàng");
			cbbLoai.Items.Add("Nhân viên");
			cbbLoai.Items.Add("Ngày l?p");
			cbbLoai.Items.Add("Tr?ng thái");
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
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadData(_data);
			txtSearch.Clear();
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
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCHoaDon_Load(null, null);
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgvVe.SelectedRows.Count == 0) return;
			var r = dgvVe.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			
			int maHD = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maHD);
			if (existing == null) return;
			
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
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgvVe.SelectedRows.Count == 0) return;
			var r = dgvVe.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			
			int maHD = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maHD);
			if (existing == null)
			{
				MessageBox.Show("Không t?m th?y hóa ðõn.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCHoaDon_Load(null, null);
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgvVe.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui l?ng ch?n hóa ðõn c?n xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			var r = dgvVe.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui l?ng ch?n hóa ðõn c?n xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			int maHD = int.Parse(r.Cells[0].Value.ToString());
			
			// L?y thông tin ð? hi?n th? trong câu h?i xác nh?n
			string khachHang = r.Cells[1].Value?.ToString() ?? "N/A";
			string nhanVien = r.Cells[2].Value?.ToString() ?? "N/A";
			string ngayLap = r.Cells[3].Value?.ToString() ?? "N/A";
			string tongTien = r.Cells[4].Value?.ToString() ?? "N/A";
			
			// Hi?n th? dialog xác nh?n xóa
			var confirmResult = MessageBox.Show(
				$"B?n có ch?c ch?n mu?n xóa hóa ðõn?\n\n" +
				$"Khách hàng: {khachHang}\n" +
				$"Nhân viên: {nhanVien}\n" +
				$"Ngày l?p: {ngayLap}\n" +
				$"T?ng ti?n: {tongTien} þ",
				"Xác nh?n xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);
			
			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maHD);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
				
				if (result.Contains("thành công")) 
				{
					UCHoaDon_Load(null, null);
				}
			}
		}

		private void Filter()
		{
			if (_data == null) return;
			
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
				case 1: // M? hóa ðõn
					filtered = _data.Where(x => x.MaHD.ToString().ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 2: // Khách hàng
					filtered = _data.Where(x => (x.KhachHang ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 3: // Nhân viên
					filtered = _data.Where(x => (x.NhanVien ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 4: // Ngày l?p
					filtered = _data.Where(x => (x.NgayLap ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 5: // Tr?ng thái
					filtered = _data.Where(x => (x.TrangThai ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				default: // T?t c?
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
			if (e.RowIndex < 0)
			{
				EnableBtn(false);
				return;
			}
			
			if (dgvVe.Rows[e.RowIndex].Cells[0].Value == null)
			{
				EnableBtn(false);
				return;
			}
			
			dgvVe.Rows[e.RowIndex].Selected = true;
			
			if (dgvVe.Rows[e.RowIndex].Selected && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void EnableBtn(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
		}
	}
}
