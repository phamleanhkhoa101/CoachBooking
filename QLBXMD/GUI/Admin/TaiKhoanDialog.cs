using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum TaiKhoanDialogMode { Add, View, Edit }

	public partial class TaiKhoanDialog : Form
	{
		public TaiKhoanDialogMode Mode { get; private set; } = TaiKhoanDialogMode.Add;

		public int MaTK { get; set; }
		public string TenDangNhap { get => txtTenDangNhap.Text; set => txtTenDangNhap.Text = value; }
		public string Email { get => txtEmail.Text; set => txtEmail.Text = value; }
		public string MatKhau { get => txtMatKhau.Text; set => txtMatKhau.Text = value; }
		public string XacNhanMatKhau { get => txtXacNhanMatKhau.Text; set => txtXacNhanMatKhau.Text = value; }

		public int? SelectedMaQuyen
		{
			get { return cbbQuyen.SelectedValue is int v ? (int?)v : (cbbQuyen.SelectedValue as int?); }
			set { if (value.HasValue) cbbQuyen.SelectedValue = value.Value; else cbbQuyen.SelectedIndex = -1; }
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0) return true; // Ho?t ??ng
				if (cbbTrangThai.SelectedIndex == 1) return false; // Khóa
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public TaiKhoanDialog()
		{
			InitializeComponent();
			cbbTrangThai.Items.Add("Ho?t ??ng");
			cbbTrangThai.Items.Add("Khóa");
			cbbTrangThai.SelectedIndex = 0; // M?c ??nh ho?t ??ng
		}

		public void BindQuyen(IEnumerable<KeyValuePair<int, string>> items)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			cbbQuyen.DataSource = list;
			cbbQuyen.DisplayMember = "Value";
			cbbQuyen.ValueMember = "Key";
		}

		public void SetMode(TaiKhoanDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case TaiKhoanDialogMode.Add:
					Text = "Thêm tài kho?n";
					btnPrimary.Text = "Thêm";
					txtTenDangNhap.Enabled = true;
					lblMatKhau.Text = "M?t kh?u *";
					lblXacNhanMatKhau.Visible = true;
					txtMatKhau.Visible = true;
					txtXacNhanMatKhau.Visible = true;
					lblXacNhanMatKhau.Text = "Xác nh?n m?t kh?u *";
					SetReadOnly(false);
					break;
				case TaiKhoanDialogMode.Edit:
					Text = "S?a tài kho?n";
					btnPrimary.Text = "L?u";
					txtTenDangNhap.Enabled = false;
					lblMatKhau.Visible = false;
					txtMatKhau.Visible = false;
					lblXacNhanMatKhau.Visible = false;
					txtXacNhanMatKhau.Visible = false;
					SetReadOnly(false);
					break;
				case TaiKhoanDialogMode.View:
					Text = "Xem tài kho?n";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					lblMatKhau.Visible = false;
					txtMatKhau.Visible = false;
					lblXacNhanMatKhau.Visible = false;
					txtXacNhanMatKhau.Visible = false;
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtTenDangNhap.Enabled = !ro;
			txtEmail.Enabled = !ro;
			cbbQuyen.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;
			txtMatKhau.Enabled = !ro;
			txtXacNhanMatKhau.Enabled = !ro;

			txtEmail.TabStop = !ro;
			cbbQuyen.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;
			txtMatKhau.TabStop = !ro;
			txtXacNhanMatKhau.TabStop = !ro;
		}

		private void btnPrimary_Click(object sender, EventArgs e)
		{
			if (ValidateInput())
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private bool ValidateInput()
		{
			if (Mode == TaiKhoanDialogMode.View)
				return true;

			if (Mode == TaiKhoanDialogMode.Add)
			{
				if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
				{
					MessageBox.Show("Vui lòng nh?p tên ??ng nh?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtTenDangNhap.Focus();
					return false;
				}

				if (txtTenDangNhap.Text.Length < 6)
				{
					MessageBox.Show("Tên ??ng nh?p ph?i có ít nh?t 6 ký t?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtTenDangNhap.Focus();
					return false;
				}

				if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
				{
					MessageBox.Show("Vui lòng nh?p m?t kh?u.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtMatKhau.Focus();
					return false;
				}

				if (txtMatKhau.Text.Length < 8)
				{
					MessageBox.Show("M?t kh?u ph?i có ít nh?t 8 ký t?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtMatKhau.Focus();
					return false;
				}

				if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
				{
					MessageBox.Show("M?t kh?u xác nh?n không kh?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtXacNhanMatKhau.Focus();
					return false;
				}
			}

			if (string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				MessageBox.Show("Vui lòng nh?p email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmail.Focus();
				return false;
			}

			if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
			{
				MessageBox.Show("Email không ?úng ??nh d?ng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmail.Focus();
				return false;
			}

			if (SelectedMaQuyen == null || SelectedMaQuyen == 0)
			{
				MessageBox.Show("Vui lòng ch?n quy?n.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbQuyen.Focus();
				return false;
			}

			return true;
		}

		private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
		{
			txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
			txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
		}
	}
}
