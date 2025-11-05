using QLBXMD.BUS;
using QLBXMD.Utils;
using QLBXMD.EF;
using QLBXMD.GUI.Admin;
using QLBXMD.GUI.User;
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
	public partial class FrmDangNhap : Form
	{
		private TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();
		public FrmDangNhap()
		{
			InitializeComponent();
		}

		private void linkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			FrmDangKy frm = new FrmDangKy();
			frm.ShowDialog();
			this.Show();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			FrmQuenMatKhau frm = new FrmQuenMatKhau();
			frm.ShowDialog();
			this.Show();
		}

		private void btnDangNhap_Click(object sender, EventArgs e)
		{
			string tenDangNhap = txtTenDangNhap.Text;
			string matKhau = txtMatKhau.Text;
			if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
			{
				MessageBox.Show("Vui lòng nhập đầy đủ thông tin đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			var taiKhoan = taiKhoanBUS.DangNhap(tenDangNhap,HashHelper.ComputeHash(matKhau));

			if (taiKhoan == null )
			{
				MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if ((bool)!taiKhoan.TrangThai)
			{
				MessageBox.Show("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				switch (taiKhoan.MaQuyen)
				{
					case 1:
					case 2: 
						this.Hide();
						FrmMainAdmin frmAdmin = new FrmMainAdmin(taiKhoan);
						frmAdmin.ShowDialog();

						txtTenDangNhap.Clear();
						txtTenDangNhap.Focus();
						txtMatKhau.Clear();
						this.Show();
						break;
					case 3:
						this.Hide();
						FrmMainUser frmMainUser = new FrmMainUser(taiKhoan);
						frmMainUser.ShowDialog();

						txtTenDangNhap.Clear();
						txtTenDangNhap.Focus();
						txtMatKhau.Clear();
						this.Show();
						break;
				}
			}

		}
	}
}
