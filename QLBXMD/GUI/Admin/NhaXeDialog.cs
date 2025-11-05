using QLBXMD.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class NhaXeDialog : Form
	{
		public string MaNhaXe
		{
			get => txtMaNhaXe.Text.Trim();
			set => txtMaNhaXe.Text = value; 
		}
		public string TenNhaXe
		{
			get => txtTenNhaXe.Text.Trim();
			set => txtTenNhaXe.Text = value;
		}
		public string SDT
		{
			get => txtSDT.Text.Trim();
			set => txtSDT.Text = value;
		}

		public string Email
		{
			get => txtEmail.Text.Trim();
			set => txtEmail.Text = value;
		}
		public string TrangThai
		{
			get => cbbTrangThai.SelectedItem?.ToString();
			set => cbbTrangThai.SelectedItem = value;
		}
		public NhaXeDialog()
		{
			InitializeComponent();
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			// Validate dữ liệu trước khi đóng dialog
			if (string.IsNullOrWhiteSpace(TenNhaXe))
			{
				MessageBox.Show("Vui lòng nhập tên nhà xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenNhaXe.Focus();
				return; // Không đóng dialog
			}
			
			if (string.IsNullOrWhiteSpace(SDT))
			{
				MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return; // Không đóng dialog
			}
			
			if (string.IsNullOrWhiteSpace(Email))
			{
				MessageBox.Show("Vui lòng nhập email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmail.Focus();
				return; // Không đóng dialog
			}
			
			if (cbbTrangThai.SelectedItem == null)
			{
				MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbTrangThai.Focus();
				return; // Không đóng dialog
			}
			
			// Validate email format
			if (!Validation.IsValidEmail(Email))
			{
				MessageBox.Show("Email không đúng định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmail.Focus();
				return; // Không đóng dialog
			}
			
			// Nếu tất cả validation pass, đóng dialog với OK
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void NhaXeDialog_Load(object sender, EventArgs e)
		{
		}

		private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}
	}
}
