using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI.User
{
	public partial class FrmMainUser : Form
	{
		private TaiKhoan taiKhoan;
		private UCDatVeUser ucDatVe;

		public FrmMainUser()
		{
			InitializeComponent();
			this.Load += FrmMainUser_Load;
		}

		public FrmMainUser(TaiKhoan taiKhoan) : this()
		{
			this.taiKhoan = taiKhoan;
		}

		private void FrmMainUser_Load(object sender, EventArgs e)
		{
			// Load booking interface
			ucDatVe = new UCDatVeUser(taiKhoan);
			ucDatVe.Dock = DockStyle.Fill;
			pnlContent.Controls.Add(ucDatVe);
		}

		private void btnDatVe_Click(object sender, EventArgs e)
		{
			pnlContent.Controls.Clear();
			ucDatVe = new UCDatVeUser(taiKhoan);
			ucDatVe.Dock = DockStyle.Fill;
			pnlContent.Controls.Add(ucDatVe);
		}

		private void btnLichSu_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Chức năng lịch sử đặt vé đang được phát triển.", "Thông báo", 
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnThongTin_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Chức năng thông tin cá nhân đang được phát triển.", "Thông báo", 
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnDangXuat_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			
			if (result == DialogResult.Yes)
			{
				this.Close();
			}
		}
	}
}
