using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCNhanVien : UserControl
	{
		private readonly NhanVienBUS _bus = new NhanVienBUS();
		private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();
		private List<dynamic> _data;

		public UCNhanVien()
		{
			InitializeComponent();
			this.Load += UCNhanVien_Load;
			this.dgv.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has;
			btnSua.Enabled = has;
			btnXoa.Enabled = has;
			
			// ✅ Chỉ enable nút "Cấp tài khoản" nếu nhân viên CHƯA có tài khoản
			if (has && dgv.SelectedRows[0].Cells["TaiKhoan"].Value != null)
			{
				string taiKhoan = dgv.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();
				btnGanTaiKhoan.Enabled = (taiKhoan == "Chưa gắn" || taiKhoan == "Chưa cấp");
			}
			else
			{
				btnGanTaiKhoan.Enabled = false;
			}
		}

		private void UCNhanVien_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();
				LoadData();
				InitCbb();
				btnXem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGanTaiKhoan.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgv.Columns.Clear();
			dgv.Columns.Add("MaNV", "Mã NV");
			dgv.Columns.Add("TenNV", "Tên nhân viên");
			dgv.Columns.Add("NamSinh", "Năm sinh");
			dgv.Columns.Add("DiaChi", "Địa chỉ");
			dgv.Columns.Add("SDT", "Số điện thoại");
			dgv.Columns.Add("GioiTinh", "Giới tính");
			dgv.Columns.Add("TrangThai", "Trạng thái");
			dgv.Columns.Add("TaiKhoan", "Tài khoản");
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã NV");
			cbbLoai.Items.Add("Tên nhân viên");
			cbbLoai.Items.Add("Số điện thoại");
			cbbLoai.Items.Add("Địa chỉ");
			cbbLoai.Items.Add("Trạng thái");
			cbbLoai.Items.Add("Tài khoản");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;
		}

		private void LoadData()
		{
			try
			{
				var list = _bus.GetAll() ?? new List<NhanVien>();
				_data = list
					.Select(nv => {
						string taiKhoan = "Chưa cấp"; // ✅ Đổi từ "Chưa gắn" thành "Chưa cấp"
						if (nv.MaTK.HasValue && nv.TaiKhoan != null)
						{
							taiKhoan = nv.TaiKhoan.TenDangNhap;
						}

						return new
						{
							MaNV = nv.MaNV,
							TenNV = nv.TenNV,
							NamSinh = nv.NamSinh.ToString("dd/MM/yyyy"),
							DiaChi = nv.DiaChi,
							SDT = nv.SDT,
							GioiTinh = nv.GioiTinh == true ? "Nam" : "Nữ",
							TrangThai = nv.TrangThai == true ? "Hoạt động" : "Nghỉ việc",
							TaiKhoan = taiKhoan,
							NamSinhDate = nv.NamSinh,
							GioiTinhValue = nv.GioiTinh,
							TrangThaiValue = nv.TrangThai,
							MaTK = nv.MaTK
						};
					})
					.ToList<dynamic>();

				DisplayData(_data);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DisplayData(List<dynamic> list)
		{
			dgv.Rows.Clear();

			foreach (var item in list)
			{
				dgv.Rows.Add(
					item.MaNV,
					item.TenNV,
					item.NamSinh,
					item.DiaChi,
					item.SDT,
					item.GioiTinh,
					item.TrangThai,
					item.TaiKhoan
				);
			}

			dgv.ClearSelection();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			DisplayData(_data);
			txtSearch.Clear();
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new NhanVienDialog())
			{
				// Bind tài khoản chưa gắn
				var availableTK = _taiKhoanBUS.GetAvailableAccounts()
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();
				dialog.BindTaiKhoan(availableTK);

				dialog.SetMode(NhanVienDialogMode.Add);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var nv = new NhanVien
					{
						TenNV = dialog.TenNV,
						NamSinh = dialog.NamSinhValue,
						DiaChi = dialog.DiaChi,
						SDT = dialog.SDT,
						GioiTinh = dialog.GioiTinhValue,
						TrangThai = dialog.TrangThaiValue,
						MaTK = dialog.SelectedMaTK
					};

					string result = _bus.Add(nv);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
						LoadData();
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maNV = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maNV);
			if (existing == null) return;

			using (var dialog = new NhanVienDialog())
			{
				var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(existing.MaTK)
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();
				dialog.BindTaiKhoan(availableTK, existing.MaTK);

				dialog.SetMode(NhanVienDialogMode.View);
				dialog.MaNV = existing.MaNV;
				dialog.TenNV = existing.TenNV;
				dialog.NamSinhValue = existing.NamSinh;
				dialog.DiaChi = existing.DiaChi;
				dialog.SDT = existing.SDT;
				dialog.GioiTinhValue = existing.GioiTinh;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.SelectedMaTK = existing.MaTK;
				dialog.TxtMaNV.Text = existing.MaNV.ToString();
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maNV = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maNV);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new NhanVienDialog())
			{
				// Bind tài khoản: tài khoản hiện tại + tài khoản chưa gắn
				var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(existing.MaTK)
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();
				dialog.BindTaiKhoan(availableTK, existing.MaTK);

				dialog.SetMode(NhanVienDialogMode.Edit);
				dialog.MaNV = existing.MaNV;
				dialog.TenNV = existing.TenNV;
				dialog.NamSinhValue = existing.NamSinh;
				dialog.DiaChi = existing.DiaChi;
				dialog.SDT = existing.SDT;
				dialog.GioiTinhValue = existing.GioiTinh;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.SelectedMaTK = existing.MaTK;
				dialog.TxtMaNV.Text = existing.MaNV.ToString();

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					existing.TenNV = dialog.TenNV;
					existing.NamSinh = dialog.NamSinhValue;
					existing.DiaChi = dialog.DiaChi;
					existing.SDT = dialog.SDT;
					existing.GioiTinh = dialog.GioiTinhValue;
					existing.TrangThai = dialog.TrangThaiValue;
					existing.MaTK = dialog.SelectedMaTK;

					string result = _bus.Update(existing);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
						LoadData();
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maNV = int.Parse(r.Cells[0].Value.ToString());

			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string tenNV = r.Cells[1].Value?.ToString() ?? "N/A";
			string sdt = r.Cells[4].Value?.ToString() ?? "N/A";
			string taiKhoan = r.Cells[7].Value?.ToString() ?? "Chưa gắn";

			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa nhân viên?\n\n" +
				$"Tên: {tenNV}\n" +
				$"Số điện thoại: {sdt}\n" +
				$"Tài khoản: {taiKhoan}",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maNV);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("thành công"))
				{
					LoadData();
				}
			}
		}

		private void btnGanTaiKhoan_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maNV = int.Parse(r.Cells[0].Value.ToString());
			var nv = _bus.GetById(maNV);
			if (nv == null)
			{
				MessageBox.Show("Không tìm thấy nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// ✅ Kiểm tra đã có tài khoản chưa
			if (nv.MaTK.HasValue && nv.MaTK.Value > 0)
			{
				MessageBox.Show(
					$"Nhân viên '{nv.TenNV}' đã có tài khoản.\n\n" +
					$"Tên đăng nhập: {nv.TaiKhoan?.TenDangNhap}\n\n" +
					"Không thể cấp tài khoản mới.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
				return;
			}

			// ✅ Mở form CẤP TÀI KHOẢN mới
			using (var dialog = new CapTaiKhoanDialog(nv))
			{
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					// Reload data
					LoadData();

					MessageBox.Show(
						$"✅ Đã cấp tài khoản thành công cho nhân viên '{nv.TenNV}'!",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
			}
		}

		private void Filter()
		{
			if (_data == null) return;

			string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0)
			{
				DisplayData(_data);
				return;
			}

			int selected = cbbLoai.SelectedIndex;
			List<dynamic> filtered = _data;

			switch (selected)
			{
				case 1: // Mã NV
					filtered = _data.Where(x => x.MaNV.ToString().ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 2: // Tên nhân viên
					filtered = _data.Where(x => (x.TenNV ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 3: // Số điện thoại
					filtered = _data.Where(x => (x.SDT ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 4: // Địa chỉ
					filtered = _data.Where(x => (x.DiaChi ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 5: // Trạng thái
					filtered = _data.Where(x => (x.TrangThai ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 6: // Tài khoản
					filtered = _data.Where(x => (x.TaiKhoan ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				default: // Tất cả
					filtered = _data.Where(x => new[]
					{
						x.MaNV.ToString(),
						x.TenNV,
						x.SDT,
						x.DiaChi,
						x.TrangThai,
						x.TaiKhoan
					}
					.Any(v => (v ?? string.Empty).ToLowerInvariant().Contains(keyword))).ToList();
					break;
			}

			DisplayData(filtered);
		}

		private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				EnableBtn(false);
				return;
			}

			if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
			{
				EnableBtn(false);
				return;
			}

			dgv.Rows[e.RowIndex].Selected = true;

			if (dgv.Rows[e.RowIndex].Selected && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void EnableBtn(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
			btnGanTaiKhoan.Enabled = enabled;
		}
	}
}
