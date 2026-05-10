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
			
			// Load vé khi chọn hóa đơn
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
						TrangThai = hd.TrangThai == true ? "Đã thanh toán" : "Chưa thanh toán",
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
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgvVe.Columns.Clear();
			dgvVe.Columns.Add("MaHD", "Mã hóa đơn");
			dgvVe.Columns.Add("KhachHang", "Khách hàng");
			dgvVe.Columns.Add("NhanVien", "Nhân viên");
			dgvVe.Columns.Add("NgayLap", "Ngày lập");
			dgvVe.Columns.Add("TongTien", "Tổng tiền (₫)");
			dgvVe.Columns.Add("TrangThai", "Trạng thái");
		}

		private void SetupVeXeGridHeaders()
		{
			dgvVeXe.Columns.Clear();
			dgvVeXe.Columns.Add("MaVe", "Mã vé");
			dgvVeXe.Columns.Add("TuyenDuong", "Tuyến đường");
			dgvVeXe.Columns.Add("NgayKhoiHanh", "Ngày khởi hành");
			dgvVeXe.Columns.Add("GioKhoiHanh", "Giờ khởi hành");
			dgvVeXe.Columns.Add("ChoNgoi", "Chỗ ngồi");
			dgvVeXe.Columns.Add("GiaVe", "Giá vé (₫)");
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
				MessageBox.Show("Tải danh sách vé thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ClearVeXeGrid()
		{
			dgvVeXe.Rows.Clear();
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã hóa đơn");
			cbbLoai.Items.Add("Khách hàng");
			cbbLoai.Items.Add("Nhân viên");
			cbbLoai.Items.Add("Ngày lập");
			cbbLoai.Items.Add("Trạng thái");
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
				MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show("Vui lòng chọn hóa đơn cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			var r = dgvVe.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn hóa đơn cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			int maHD = int.Parse(r.Cells[0].Value.ToString());
			
			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string khachHang = r.Cells[1].Value?.ToString() ?? "N/A";
			string nhanVien = r.Cells[2].Value?.ToString() ?? "N/A";
			string ngayLap = r.Cells[3].Value?.ToString() ?? "N/A";
			string tongTien = r.Cells[4].Value?.ToString() ?? "N/A";
			
			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa hóa đơn?\n\n" +
				$"Khách hàng: {khachHang}\n" +
				$"Nhân viên: {nhanVien}\n" +
				$"Ngày lập: {ngayLap}\n" +
				$"Tổng tiền: {tongTien} ₫",
				"Xác nhận xóa",
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
				case 1: // Mã hóa đơn
					filtered = _data.Where(x => x.MaHD.ToString().ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 2: // Khách hàng
					filtered = _data.Where(x => (x.KhachHang ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 3: // Nhân viên
					filtered = _data.Where(x => (x.NhanVien ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 4: // Ngày lập
					filtered = _data.Where(x => (x.NgayLap ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 5: // Trạng thái
					filtered = _data.Where(x => (x.TrangThai ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				default: // Tất cả
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
