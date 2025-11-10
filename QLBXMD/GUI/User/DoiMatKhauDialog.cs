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
			if (string.IsNullOrWhiteSpace(txtMatKhauCu.Text))
			{
				MessageBox.Show("Vui lòng nh?p m?t kh?u c?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhauCu.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtMatKhauMoi.Text))
			{
				MessageBox.Show("Vui lòng nh?p m?t kh?u m?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhauMoi.Focus();
				return;
			}

			if (txtMatKhauMoi.Text.Length < 6)
			{
				MessageBox.Show("M?t kh?u m?i ph?i có ít nh?t 6 ký t?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhauMoi.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
			{
				MessageBox.Show("Vui lòng xác nh?n m?t kh?u m?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtXacNhanMatKhau.Focus();
				return;
			}

			if (txtMatKhauMoi.Text != txtXacNhanMatKhau.Text)
			{
				MessageBox.Show("M?t kh?u xác nh?n không kh?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
						MessageBox.Show("Không tìm th?y tài kho?n.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Ki?m tra m?t kh?u c?
					string matKhauCuHash = HashHelper.ComputeHash(txtMatKhauCu.Text);
					if (taiKhoan.MatKhau != matKhauCuHash)
					{
						MessageBox.Show("M?t kh?u c? không chính xác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						txtMatKhauCu.Focus();
						return;
					}

					// C?p nh?t m?t kh?u m?i
					string matKhauMoiHash = HashHelper.ComputeHash(txtMatKhauMoi.Text);
					taiKhoan.MatKhau = matKhauMoiHash;

					db.SaveChanges();

					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"L?i ??i m?t kh?u: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
