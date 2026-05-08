using QLBXMD.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI
{
	public partial class FrmDangKy : Form
	{
		private TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();
		private readonly ToolTip passwordToolTip = new ToolTip();
		public FrmDangKy()
		{
			InitializeComponent();
			UpdatePasswordToggleButton(txtMatKhau, btnHienMatKhau);
			UpdatePasswordToggleButton(txtMatKhau2, btnHienMatKhau2);
		}

		private void btnHienMatKhau_Click(object sender, EventArgs e)
		{
			TogglePasswordField(txtMatKhau, btnHienMatKhau);
		}

		private void btnHienMatKhau2_Click(object sender, EventArgs e)
		{
			TogglePasswordField(txtMatKhau2, btnHienMatKhau2);
		}

		private void linkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (e.Link != null) {
				this.Close();
			}
		}

		private void btnDangNhap_Click(object sender, EventArgs e)
		{
			string username = txtTenDangNhap.Text.Trim();
			string email = txtEmail.Text.Trim();
			string password = txtMatKhau.Text.Trim();
			string password2 = txtMatKhau2.Text.Trim();

			var bus = new TaiKhoanBUS();
			string result = bus.DangKy(username, email, password, password2);

			if (result == "success")
			{
				MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			}
			else
			{
				MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void TogglePasswordField(TextBox textBox, Button button)
		{
			SetPasswordVisibility(textBox, button, textBox.PasswordChar != '\0');
		}

		private void SetPasswordVisibility(TextBox textBox, Button button, bool isVisible)
		{
			textBox.PasswordChar = isVisible ? '\0' : '*';
			UpdatePasswordToggleButton(textBox, button);
		}

		private void UpdatePasswordToggleButton(TextBox textBox, Button button)
		{
			bool isVisible = textBox.PasswordChar == '\0';
			button.Image = isVisible ? Properties.Resources.unshow_pass : Properties.Resources.show_pass;
			passwordToolTip.SetToolTip(button, isVisible ? "Ẩn mật khẩu" : "Hiện mật khẩu");
		}
	}
}
