using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum TaiXeDialogMode { Add, View, Edit }

	public partial class TaiXeDialog : Form
	{
		public TaiXeDialogMode Mode { get; private set; } = TaiXeDialogMode.Add;

		public string MaTX { get => txtMaTX.Text; set => txtMaTX.Text = value; }
		public string TenTX { get => txtTenTX.Text; set => txtTenTX.Text = value; }
		public DateTime NamSinhValue { get => dtpNamSinh.Value; set => dtpNamSinh.Value = value; }
		public string CCCD { get => txtCCCD.Text; set => txtCCCD.Text = value; }
		public string SDT { get => txtSDT.Text; set => txtSDT.Text = value; }
		public string DiaChi { get => txtDiaChi.Text; set => txtDiaChi.Text = value; }
		public string BangLai { get => txtBangLai.Text; set => txtBangLai.Text = value; }
		public DateTime NgayCapValue { get => dtpNgayCap.Value; set => dtpNgayCap.Value = value; }

		public bool? GioiTinhValue
		{
			get
			{
				if (cbbGioiTinh.SelectedIndex == 0) return true; // Nam
				if (cbbGioiTinh.SelectedIndex == 1) return false; // N?
				return null;
			}
			set
			{
				if (value == true) cbbGioiTinh.SelectedIndex = 0;
				else if (value == false) cbbGioiTinh.SelectedIndex = 1;
				else cbbGioiTinh.SelectedIndex = -1;
			}
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0) return true; // Ho?t ??ng
				if (cbbTrangThai.SelectedIndex == 1) return false; // Ng?ng ho?t ??ng
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public TaiXeDialog()
		{
			InitializeComponent();
			dtpNamSinh.Value = DateTime.Now.AddYears(-25);
			cbbGioiTinh.SelectedIndex = 0;
			cbbTrangThai.SelectedIndex = 0;
		}

		public void SetMode(TaiXeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case TaiXeDialogMode.Add:
					Text = "Thêm tài x?";
					btnPrimary.Text = "Thêm";
					txtMaTX.Enabled = false;
					SetReadOnly(false);
					break;
				case TaiXeDialogMode.Edit:
					Text = "S?a tài x?";
					btnPrimary.Text = "L?u";
					txtMaTX.Enabled = false;
					SetReadOnly(false);
					break;
				case TaiXeDialogMode.View:
					Text = "Xem tài x?";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaTX.Enabled = !ro;
			txtTenTX.ReadOnly = ro;
			dtpNamSinh.Enabled = !ro;
			txtCCCD.ReadOnly = ro;
			cbbGioiTinh.Enabled = !ro;
			txtSDT.ReadOnly = ro;
			txtDiaChi.ReadOnly = ro;
			txtBangLai.ReadOnly = ro;
			dtpNgayCap.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;

			txtTenTX.TabStop = !ro;
			dtpNamSinh.TabStop = !ro;
			txtCCCD.TabStop = !ro;
			cbbGioiTinh.TabStop = !ro;
			txtSDT.TabStop = !ro;
			txtDiaChi.TabStop = !ro;
			txtBangLai.TabStop = !ro;
			dtpNgayCap.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;
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
			if (Mode == TaiXeDialogMode.View)
				return true;

			if (string.IsNullOrWhiteSpace(txtTenTX.Text))
			{
				MessageBox.Show("Vui lòng nh?p tên tài x?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenTX.Focus();
				return false;
			}

			if (dtpNamSinh.Value > DateTime.Now.AddYears(-21))
			{
				MessageBox.Show("Tài x? ph?i t? 21 tu?i tr? lên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				dtpNamSinh.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtCCCD.Text))
			{
				MessageBox.Show("Vui lòng nh?p CCCD.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtCCCD.Focus();
				return false;
			}

			if (txtCCCD.Text.Length != 12)
			{
				MessageBox.Show("CCCD ph?i có 12 s?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtCCCD.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui lòng nh?p s? ?i?n tho?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return false;
			}

			if (txtSDT.Text.Length < 10 || txtSDT.Text.Length > 11)
			{
				MessageBox.Show("S? ?i?n tho?i ph?i có 10-11 s?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtBangLai.Text))
			{
				MessageBox.Show("Vui lòng nh?p s? b?ng lái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtBangLai.Focus();
				return false;
			}

			if (dtpNgayCap.Value > DateTime.Now)
			{
				MessageBox.Show("Ngày c?p không ???c l?n h?n ngày hi?n t?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				dtpNgayCap.Focus();
				return false;
			}

			return true;
		}
	}
}
