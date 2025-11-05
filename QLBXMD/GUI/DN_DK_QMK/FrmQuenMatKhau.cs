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

namespace QLBXMD.GUI.DN_DK_QMK
{
	public partial class FrmQuenMatKhau : Form
	{
		private TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();
		public FrmQuenMatKhau()
		{
			InitializeComponent();
		}

		private void linkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (e.Link != null)
			{
				this.Close();
			}
		}

		private void btnDangNhap_Click(object sender, EventArgs e)
		{
			string email = txtEmail.Text.Trim();
			string tenDangNhap = txtTenDangNhap.Text.Trim();
			string result = taiKhoanBUS.QuenMatKhau( email,tenDangNhap);

			if (result=="success")
			{
				MessageBox.Show("Mật khẩu mới đã được gửi đến email của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			}
			else
			{
				MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
	}
}
