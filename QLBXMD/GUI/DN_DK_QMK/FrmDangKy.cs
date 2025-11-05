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
		public FrmDangKy()
		{
			InitializeComponent();
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
	}
}
