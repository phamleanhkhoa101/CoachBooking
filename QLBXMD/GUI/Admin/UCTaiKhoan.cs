using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCTaiKhoan : UserControl
	{
		private readonly TaiKhoanBUS _bus = new TaiKhoanBUS();
		private readonly QuyenBUS _quyenBUS = new QuyenBUS();
		private List<dynamic> _data;

		public UCTaiKhoan()
		{
			InitializeComponent();
			this.Load += UCTaiKhoan_Load;
			this.dgv.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has;
			btnSua.Enabled = has;
			btnXoa.Enabled = has;
			btnResetPassword.Enabled = has;
		}

		private void UCTaiKhoan_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();

				var list = _bus.GetAll() ?? new List<TaiKhoan>();
				_data = list
					.Select(tk => {
						string lienKet = "Chưa gắn";
						var nv = _bus.GetNhanVienByTaiKhoan(tk);
						var kh = _bus.GetKhachHangByTaiKhoan(tk);
						if (nv != null) lienKet = $"NV: {nv.TenNV}";
						else if (kh != null) lienKet = $"KH: {kh.TenKH}";
						
						return new
						{
							MaTK = tk.MaTK,
							TenDangNhap = tk.TenDangNhap,
							Email = tk.Email,
							Quyen = tk.Quyen?.TenQuyen ?? "N/A",
							TrangThai = tk.TrangThai == true ? "Hoạt động" : "Khóa",
							LienKet = lienKet,
							MaQuyen = tk.MaQuyen,
							TrangThaiValue = tk.TrangThai
						};
					})
					.ToList<dynamic>();

				LoadData(_data);
				InitCbb();
				btnXem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnResetPassword.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgv.Columns.Clear();
			dgv.Columns.Add("MaTK", "Mã TK");
			dgv.Columns.Add("TenDangNhap", "Tên đăng nhập");
			dgv.Columns.Add("Email", "Email");
			dgv.Columns.Add("Quyen", "Quyền");
			dgv.Columns.Add("TrangThai", "Trạng thái");
			dgv.Columns.Add("LienKet", "Liên kết");
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã TK");
			cbbLoai.Items.Add("Tên đăng nhập");
			cbbLoai.Items.Add("Email");
			cbbLoai.Items.Add("Quyền");
			cbbLoai.Items.Add("Trạng thái");
			cbbLoai.Items.Add("Liên kết");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;
		}

		private void LoadData(List<dynamic> list)
		{
			dgv.Rows.Clear();

			foreach (var item in list)
			{
				dgv.Rows.Add(
					item.MaTK,
					item.TenDangNhap,
					item.Email,
					item.Quyen,
					item.TrangThai,
					item.LienKet
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
			LoadData(_data);
			txtSearch.Clear();
		}

		private IEnumerable<KeyValuePair<int, string>> GetQuyenItems()
		{
			try
			{
				var quyens = _quyenBUS.GetAll();
				return quyens.Select(q => new KeyValuePair<int, string>(q.MaQuyen, q.TenQuyen)).ToList();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải danh sách quyền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return new List<KeyValuePair<int, string>>();
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new TaiKhoanDialog())
			{
				dialog.BindQuyen(GetQuyenItems());
				dialog.SetMode(TaiKhoanDialogMode.Add);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var tk = new TaiKhoan
					{
						TenDangNhap = dialog.TenDangNhap,
						Email = dialog.Email,
						MaQuyen = dialog.SelectedMaQuyen ?? 3, // Mặc định User
						TrangThai = dialog.TrangThaiValue
					};

					string result = _bus.Add(tk, dialog.MatKhau);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
						UCTaiKhoan_Load(null, null);
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maTK = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maTK);
			if (existing == null) return;

			using (var dialog = new TaiKhoanDialog())
			{
				dialog.BindQuyen(GetQuyenItems());
				dialog.SetMode(TaiKhoanDialogMode.View);
				dialog.MaTK = existing.MaTK;
				dialog.TenDangNhap = existing.TenDangNhap;
				dialog.Email = existing.Email;
				dialog.SelectedMaQuyen = existing.MaQuyen;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;

			int maTK = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maTK);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new TaiKhoanDialog())
			{
				dialog.BindQuyen(GetQuyenItems());
				dialog.SetMode(TaiKhoanDialogMode.Edit);
				dialog.MaTK = existing.MaTK;
				dialog.TenDangNhap = existing.TenDangNhap;
				dialog.Email = existing.Email;
				dialog.SelectedMaQuyen = existing.MaQuyen;
				dialog.TrangThaiValue = existing.TrangThai;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var update = new TaiKhoan
					{
						MaTK = existing.MaTK,
						TenDangNhap = existing.TenDangNhap, // Keep original
						MatKhau = existing.MatKhau, // Keep original
						Email = dialog.Email,
						MaQuyen = dialog.SelectedMaQuyen ?? existing.MaQuyen,
						TrangThai = dialog.TrangThaiValue
					};

					string result = _bus.Update(update);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
						UCTaiKhoan_Load(null, null);
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn tài khoản cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn tài khoản cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maTK = int.Parse(r.Cells[0].Value.ToString());

			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string tenDangNhap = r.Cells[1].Value?.ToString() ?? "N/A";
			string email = r.Cells[2].Value?.ToString() ?? "N/A";
			string quyen = r.Cells[3].Value?.ToString() ?? "N/A";

			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa tài khoản?\n\n" +
				$"Tên đăng nhập: {tenDangNhap}\n" +
				$"Email: {email}\n" +
				$"Quyền: {quyen}",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maTK);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("thành công"))
				{
					UCTaiKhoan_Load(null, null);
				}
			}
		}

		private void btnResetPassword_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn tài khoản cần đặt lại mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn tài khoản cần đặt lại mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int maTK = int.Parse(r.Cells[0].Value.ToString());
			string tenDangNhap = r.Cells[1].Value?.ToString() ?? "N/A";
			string email = r.Cells[2].Value?.ToString() ?? "N/A";

			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn đặt lại mật khẩu cho tài khoản?\n\n" +
				$"Tên đăng nhập: {tenDangNhap}\n" +
				$"Email: {email}\n\n" +
				$"Mật khẩu mới sẽ được gửi qua email.",
				"Xác nhận đặt lại mật khẩu",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.ResetPassword(maTK);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
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
				case 1: // Mã TK
					filtered = _data.Where(x => x.MaTK.ToString().ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 2: // Tên đăng nhập
					filtered = _data.Where(x => (x.TenDangNhap ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 3: // Email
					filtered = _data.Where(x => (x.Email ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 4: // Quyền
					filtered = _data.Where(x => (x.Quyen ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 5: // Trạng thái
					filtered = _data.Where(x => (x.TrangThai ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				case 6: // Liên kết
					filtered = _data.Where(x => (x.LienKet ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;
				default: // Tất cả
					filtered = _data.Where(x => new[]
					{
						x.MaTK.ToString(),
						x.TenDangNhap,
						x.Email,
						x.Quyen,
						x.TrangThai,
						x.LienKet
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
			btnResetPassword.Enabled = enabled;
		}
	}
}
