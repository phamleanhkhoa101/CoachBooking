using System;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.DAO;
using QLBXMD.EF;
using QLBXMD.Utils;

namespace QLBXMD.GUI.User
{
	public partial class UCThongTinUser : UserControl
	{
		private TaiKhoan currentUser;

		public UCThongTinUser()
		{
			InitializeComponent();
		}

		public UCThongTinUser(TaiKhoan user) : this()
		{
			this.currentUser = user;
			this.Load += UCThongTinUser_Load;
		}

		private void UCThongTinUser_Load(object sender, EventArgs e)
		{
			LoadThongTinUser();
		}

		private void LoadThongTinUser()
		{
			if (currentUser == null)
			{
				MessageBox.Show("Vui lòng đăng nhập để xem thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					// Lấy thông tin tài khoản
					var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTK == currentUser.MaTK);
					if (taiKhoan == null)
					{
						MessageBox.Show("Không tìm thấy thông tin tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Lấy thông tin khách hàng; nếu chưa có thì Tạo mới 1 record liên kết với tài khoản
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						// Tạo khách hàng mặc định liên kết với tài khoản
						khachHang = new KhachHang
						{
							MaTK = currentUser.MaTK,
							TenKH = taiKhoan.TenDangNhap ?? "",
							SDT = "",
							NamSinh = DateTime.Now.AddYears(-21),
							GioiTinh = null
						};
						db.KhachHangs.Add(khachHang);
						db.SaveChanges();
					}

					// Hiển thị thông tin
					txtTenDangNhap.Text = taiKhoan.TenDangNhap;
					txtTenKH.Text = khachHang.TenKH;
					txtSDT.Text = khachHang.SDT;
					txtEmail.Text = taiKhoan.Email ?? ""; // Lấy email từ TaiKhoan
					dtpNamSinh.Value = khachHang.NamSinh;
					cboGioiTinh.SelectedIndex = (khachHang.GioiTinh ?? true) ? 0 : 1;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnCapNhat_Click(object sender, EventArgs e)
		{
			// Validate
			if (string.IsNullOrWhiteSpace(txtTenKH.Text))
			{
				MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenKH.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			if (txtSDT.Text.Length < 10 || txtSDT.Text.Length > 11)
			{
				MessageBox.Show("Số điện thoại phải có 10-11 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			// Validate email
			if (!string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				if (!Validation.IsValidEmail(txtEmail.Text.Trim()))
				{
					MessageBox.Show("Email không đúng định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtEmail.Focus();
					return;
				}
			}

			if (cboGioiTinh.SelectedItem == null)
			{
				MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Confirm
			var result = MessageBox.Show(
				"Bạn có chắc chắn muốn cập nhật thông tin?",
				"Xác nhận",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (result != DialogResult.Yes) return;

			try
			{
				using (var db = new BXMDEntities())
				{
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Lấy thông tin tài khoản
					var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTK == currentUser.MaTK);
					if (taiKhoan == null)
					{
						MessageBox.Show("Không tìm thấy thông tin tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Kiểm tra trùng số điện thoại
					var existingSDT = db.KhachHangs.FirstOrDefault(kh => kh.SDT == txtSDT.Text.Trim() && kh.MaKH != khachHang.MaKH);
					if (existingSDT != null)
					{
						MessageBox.Show("Số điện thoại đã được sử dụng bởi khách hàng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						txtSDT.Focus();
						return;
					}

					// Kiểm tra trùng email
					if (!string.IsNullOrWhiteSpace(txtEmail.Text))
					{
						var existingEmail = db.TaiKhoans.FirstOrDefault(tk => tk.Email == txtEmail.Text.Trim() && tk.MaTK != currentUser.MaTK);
						if (existingEmail != null)
						{
							MessageBox.Show("Email đã được sử dụng bởi tài khoản khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							txtEmail.Focus();
							return;
						}
					}

					// Cập nhật thông tin khách hàng
					khachHang.TenKH = txtTenKH.Text.Trim();
					khachHang.SDT = txtSDT.Text.Trim();
					khachHang.NamSinh = dtpNamSinh.Value.Date;
					khachHang.GioiTinh = cboGioiTinh.SelectedItem.ToString() == "Nam";

					// Cập nhật email trong tài khoản
					taiKhoan.Email = txtEmail.Text.Trim();

					db.SaveChanges();

					MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi cập nhật thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDoiMatKhau_Click(object sender, EventArgs e)
		{
			using (var form = new DoiMatKhauDialog(currentUser))
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}
	}
}
