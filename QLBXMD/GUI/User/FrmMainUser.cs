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
		private UCLichSuMuaVe ucLichSu;
		private UCThongTinUser ucThongTin;

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
			// Load booking interface by default
			LoadDatVe();
		}

		private void btnDatVe_Click(object sender, EventArgs e)
		{
			LoadDatVe();
		}

		private void btnLichSu_Click(object sender, EventArgs e)
		{
			LoadLichSu();
		}

		private void btnThongTin_Click(object sender, EventArgs e)
		{
			LoadThongTin();
		}

		private void LoadDatVe()
		{
			pnlContent.Controls.Clear();
			if (ucDatVe == null)
			{
				ucDatVe = new UCDatVeUser(taiKhoan);
				ucDatVe.Dock = DockStyle.Fill;
			}
			pnlContent.Controls.Add(ucDatVe);
		}

		private void LoadLichSu()
		{
			pnlContent.Controls.Clear();
			// Always create new instance to refresh data
			ucLichSu = new UCLichSuMuaVe(taiKhoan);
			ucLichSu.Dock = DockStyle.Fill;
			pnlContent.Controls.Add(ucLichSu);
		}

		private void LoadThongTin()
		{
			pnlContent.Controls.Clear();
			// Always create new instance to refresh data
			ucThongTin = new UCThongTinUser(taiKhoan);
			ucThongTin.Dock = DockStyle.Fill;
			pnlContent.Controls.Add(ucThongTin);
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
