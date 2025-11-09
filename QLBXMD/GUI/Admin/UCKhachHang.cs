using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCKhachHang : UserControl
	{
		private readonly KhachHangBUS _bus = new KhachHangBUS();
		private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();
		private List<dynamic> _data;

		public UCKhachHang()
		{
			InitializeComponent();
			this.Load += UCKhachHang_Load;
			this.dgv.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].Cells[0].Value != null;
			btnThem.Enabled = has;
			btnSua.Enabled = has;
			btnXoa.Enabled = has;
			btnGanTaiKhoan.Enabled = has;
		}

		private void UCKhachHang_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();
				LoadData();
				InitCbb();
				btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGanTaiKhoan.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgv.Columns.Clear();
			dgv.Columns.Add("MaKH", "Mã KH");
			dgv.Columns.Add("TenKH", "Tên khách hàng");
			dgv.Columns.Add("NamSinh", "Năm sinh");
			dgv.Columns.Add("SDT", "Số điện thoại");
			dgv.Columns.Add("GioiTinh", "Giới tính");
			dgv.Columns.Add("TaiKhoan", "Tài khoản");
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã KH");
			cbbLoai.Items.Add("Tên khách hàng");
			cbbLoai.Items.Add("Số điện thoại");
			cbbLoai.Items.Add("Tài khoản");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;
		}

		private void LoadData()
		{
			try
			{
				var list = _bus.GetAll() ?? new List<KhachHang>();
				_data = list
					.Select(kh => {
						string taiKhoan = "Chưa gắn";
						if (kh.MaTK.HasValue && kh.TaiKhoan != null)
						{
							taiKhoan = kh.TaiKhoan.TenDangNhap;
						}

						return new
						{
							MaKH = kh.MaKH,
							TenKH = kh.TenKH,
							NamSinh = kh.NamSinh.ToString("dd/MM/yyyy"),
							SDT = kh.SDT,
							GioiTinh = kh.GioiTinh == true ? "Nam" : "Nữ",
							TaiKhoan = taiKhoan,
							NamSinhDate = kh.NamSinh,
							GioiTinhValue = kh.GioiTinh,
							MaTK = kh.MaTK
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
					item.MaKH,
					item.TenKH,
					item.NamSinh,
					item.SDT,
					item.GioiTinh,
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
			using (var dialog = new KhachHangDialog())
			{
				// Bind tài khoản chưa gắn
				var availableTK = _taiKhoanBUS.GetAvailableAccounts()
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();
				dialog.BindTaiKhoan(availableTK);

				dialog.SetMode(KhachHangDialogMode.Add);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var kh = new KhachHang
					{
						TenKH = dialog.TenKH,
						NamSinh = dialog.NamSinhValue,
						SDT = dialog.SDT,
						GioiTinh = dialog.GioiTinhValue,
						MaTK = dialog.SelectedMaTK
					};

					string result = _bus.Add(kh);
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

			int maKH = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maKH);
			if (existing == null) return;

			using (var dialog = new KhachHangDialog())
			{
				var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(existing.MaTK)
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();
				dialog.BindTaiKhoan(availableTK, existing.MaTK);

				dialog.SetMode(KhachHangDialogMode.View);
				dialog.MaKH = existing.MaKH;
				dialog.TenKH = existing.TenKH;
				dialog.NamSinhValue = existing.NamSinh;
				dialog.SDT = existing.SDT;
				dialog.GioiTinhValue = existing.GioiTinh;
				dialog.SelectedMaTK = existing.MaTK;
				dialog.TxtMaKH.Text = existing.MaKH.ToString();
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maKH = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maKH);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new KhachHangDialog())
			{
				// Bind tài khoản: tài khoản hiện tại + tài khoản chưa gắn
				var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(existing.MaTK)
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();
				dialog.BindTaiKhoan(availableTK, existing.MaTK);

				dialog.SetMode(KhachHangDialogMode.Edit);
				dialog.MaKH = existing.MaKH;
				dialog.TenKH = existing.TenKH;
				dialog.NamSinhValue = existing.NamSinh;
				dialog.SDT = existing.SDT;
				dialog.GioiTinhValue = existing.GioiTinh;
				dialog.SelectedMaTK = existing.MaTK;
				dialog.TxtMaKH.Text = existing.MaKH.ToString();

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					existing.TenKH = dialog.TenKH;
					existing.NamSinh = dialog.NamSinhValue;
					existing.SDT = dialog.SDT;
					existing.GioiTinh = dialog.GioiTinhValue;
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
				MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maKH = int.Parse(r.Cells[0].Value.ToString());

			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string tenKH = r.Cells[1].Value?.ToString() ?? "N/A";
			string sdt = r.Cells[3].Value?.ToString() ?? "N/A";
			string taiKhoan = r.Cells[5].Value?.ToString() ?? "Chưa gắn";

			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa khách hàng?\n\n" +
				$"Tên: {tenKH}\n" +
				$"Số điện thoại: {sdt}\n" +
				$"Tài khoản: {taiKhoan}",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maKH);
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
				MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maKH = int.Parse(r.Cells[0].Value.ToString());
			var kh = _bus.GetById(maKH);
			if (kh == null) return;

			using (var dialog = new GanTaiKhoanDialog())
			{
				var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(kh.MaTK)
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();

				dialog.BindTaiKhoan(availableTK, kh.MaTK);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					kh.MaTK = dialog.SelectedMaTK;
					string result = _bus.Update(kh);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
						LoadData();
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
				case 1: // Mã KH
					filtered = _data.Where(x => x.MaKH.ToString().ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 2: // Tên khách hàng
					filtered = _data.Where(x => (x.TenKH ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 3: // Số điện thoại
					filtered = _data.Where(x => (x.SDT ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 4: // Tài khoản
					filtered = _data.Where(x => (x.TaiKhoan ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				default: // Tất cả
					filtered = _data.Where(x => new[]
					{
						x.MaKH.ToString(),
						x.TenKH,
						x.SDT,
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
			btnThem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
			btnGanTaiKhoan.Enabled = enabled;
		}
	}
}
