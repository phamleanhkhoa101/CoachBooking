using System;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;
using QLBXMD.Utils;

namespace QLBXMD.GUI.User
{
	public partial class DoiMatKhauDialog : Form
	{
		private TaiKhoan currentUser;

		public DoiMatKhauDialog(TaiKhoan user)
		{
			InitializeComponent();
			this.currentUser = user;
		}

		private void btnXacNhan_Click(object sender, EventArgs e)
		{
			// Validate
			if (string.IsNullOrWhiteSpace(txtMatKhauMoi.Text))
			{
				MessageBox.Show("Vui lòng nhập mật khẩu mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhauMoi.Focus();
				return;
			}

			if (txtMatKhauMoi.Text.Length < 6)
			{
				MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhauMoi.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
			{
				MessageBox.Show("Vui lòng xác nhận mật khẩu mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtXacNhanMatKhau.Focus();
				return;
			}

			if (txtMatKhauMoi.Text != txtXacNhanMatKhau.Text)
			{
				MessageBox.Show("Mật khẩu xác nhận không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtXacNhanMatKhau.Focus();
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTK == currentUser.MaTK);
					if (taiKhoan == null)
					{
						MessageBox.Show("Không tìm thấy tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Cập nhật mật khẩu mới
					string matKhauMoiHash = HashHelper.ComputeHash(txtMatKhauMoi.Text);
					taiKhoan.MatKhau = matKhauMoiHash;

					db.SaveChanges();

					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi đổi mật khẩu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
