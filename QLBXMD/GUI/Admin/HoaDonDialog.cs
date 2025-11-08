using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum HoaDonDialogMode { Add, View, Edit }

	public partial class HoaDonDialog : Form
	{
		public HoaDonDialogMode Mode { get; private set; } = HoaDonDialogMode.Add;

		public string MaHD { get => txtMaHD.Text; set => txtMaHD.Text = value; }
		public DateTime NgayLapValue { get => dtpNgayLap.Value; set => dtpNgayLap.Value = value; }
		public decimal TongTienValue { get => numTongTien.Value; set => numTongTien.Value = value; }

		public int? SelectedMaKH 
		{ 
			get { return cbbKhachHang.SelectedValue is int v ? (int?)v : (cbbKhachHang.SelectedValue as int?); } 
			set { if (value.HasValue) cbbKhachHang.SelectedValue = value.Value; else cbbKhachHang.SelectedIndex = -1; } 
		}
		
		public int? SelectedMANV 
		{ 
			get { return cbbNhanVien.SelectedValue is int v ? (int?)v : (cbbNhanVien.SelectedValue as int?); } 
			set { if (value.HasValue) cbbNhanVien.SelectedValue = value.Value; else cbbNhanVien.SelectedIndex = -1; } 
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0) return true; // ?ã thanh toán
				if (cbbTrangThai.SelectedIndex == 1) return false; // Ch?a thanh toán
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public HoaDonDialog()
		{
			InitializeComponent();
			cbbTrangThai.SelectedIndex = 1; // M?c ??nh ch?a thanh toán
		}

		public void BindKhachHang(IEnumerable<KeyValuePair<int, string>> items)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không ch?n --"));
			cbbKhachHang.DataSource = list;
			cbbKhachHang.DisplayMember = "Value";
			cbbKhachHang.ValueMember = "Key";
		}

		public void BindNhanVien(IEnumerable<KeyValuePair<int, string>> items)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không ch?n --"));
			cbbNhanVien.DataSource = list;
			cbbNhanVien.DisplayMember = "Value";
			cbbNhanVien.ValueMember = "Key";
		}

		public void SetMode(HoaDonDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case HoaDonDialogMode.Add:
					Text = "Thêm hóa ??n";
					btnPrimary.Text = "Thêm";
					txtMaHD.Enabled = false;
					SetReadOnly(false);
					break;
				case HoaDonDialogMode.Edit:
					Text = "S?a hóa ??n";
					btnPrimary.Text = "L?u";
					txtMaHD.Enabled = false;
					SetReadOnly(false);
					break;
				case HoaDonDialogMode.View:
					Text = "Xem hóa ??n";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaHD.Enabled = !ro;
			cbbKhachHang.Enabled = !ro;
			cbbNhanVien.Enabled = !ro;
			dtpNgayLap.Enabled = !ro;
			numTongTien.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;

			cbbKhachHang.TabStop = !ro;
			cbbNhanVien.TabStop = !ro;
			dtpNgayLap.TabStop = !ro;
			numTongTien.TabStop = !ro;
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
			if (Mode == HoaDonDialogMode.View)
				return true;

			// Ph?i ch?n ít nh?t khách hàng ho?c nhân viên
			int? maKH = SelectedMaKH;
			int? maNV = SelectedMANV;
			
			if ((maKH == null || maKH == 0) && (maNV == null || maNV == 0))
			{
				MessageBox.Show("Vui lòng chọn ít nhất khách hàng ho?c nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbKhachHang.Focus();
				return false;
			}

			if (dtpNgayLap.Value > DateTime.Now)
			{
				MessageBox.Show("Ngày l?p không ???c l?n h?n ngày hi?n t?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				dtpNgayLap.Focus();
				return false;
			}

			if (numTongTien.Value <= 0)
			{
				MessageBox.Show("T?ng ti?n ph?i l?n h?n 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				numTongTien.Focus();
				return false;
			}

			return true;
		}
	}
}
