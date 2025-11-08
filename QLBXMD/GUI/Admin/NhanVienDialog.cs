using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;

namespace QLBXMD.GUI.Admin
{
	public enum NhanVienDialogMode { Add, View, Edit }

	public partial class NhanVienDialog : Form
	{
		private TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();

		public NhanVienDialogMode Mode { get; private set; } = NhanVienDialogMode.Add;

		// Make txtMaNV publicly accessible
		public TextBox TxtMaNV => txtMaNV;

		public int MaNV { get; set; }
		public string TenNV { get => txtTenNV.Text; set => txtTenNV.Text = value; }
		public DateTime NamSinhValue { get => dtpNamSinh.Value; set => dtpNamSinh.Value = value; }
		public string DiaChi { get => txtDiaChi.Text; set => txtDiaChi.Text = value; }
		public string SDT { get => txtSDT.Text; set => txtSDT.Text = value; }

		public bool? GioiTinhValue
		{
			get
			{
				if (cbbGioiTinh.SelectedIndex == 0) return true; // Nam
				if (cbbGioiTinh.SelectedIndex == 1) return false; // Nữ
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
				if (cbbTrangThai.SelectedIndex == 0) return true; // Hoạt động
				if (cbbTrangThai.SelectedIndex == 1) return false; // Nghỉ việc
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public int? SelectedMaTK
		{
			get
			{
				if (cbbTaiKhoan.SelectedValue == null || (int)cbbTaiKhoan.SelectedValue == 0)
					return null;
				return cbbTaiKhoan.SelectedValue is int v ? (int?)v : null;
			}
			set
			{
				if (value.HasValue && value.Value > 0)
					cbbTaiKhoan.SelectedValue = value.Value;
				else
					cbbTaiKhoan.SelectedIndex = 0;
			}
		}

		public NhanVienDialog()
		{
			InitializeComponent();
			cbbGioiTinh.Items.Add("Nam");
			cbbGioiTinh.Items.Add("Nữ");
			cbbGioiTinh.SelectedIndex = 0;

			cbbTrangThai.Items.Add("Hoạt động");
			cbbTrangThai.Items.Add("Nghỉ việc");
			cbbTrangThai.SelectedIndex = 0;
		}

		public void BindTaiKhoan(IEnumerable<KeyValuePair<int, string>> items, int? currentMaTK = null)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không chọn --"));
			cbbTaiKhoan.DataSource = list;
			cbbTaiKhoan.DisplayMember = "Value";
			cbbTaiKhoan.ValueMember = "Key";

			if (currentMaTK.HasValue && currentMaTK.Value > 0)
				cbbTaiKhoan.SelectedValue = currentMaTK.Value;
			else
				cbbTaiKhoan.SelectedIndex = 0;
		}

		public void SetMode(NhanVienDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case NhanVienDialogMode.Add:
					Text = "Thêm nhân viên";
					btnPrimary.Text = "Thêm";
					lblMaNV.Visible = false;
					txtMaNV.Visible = false;
					SetReadOnly(false);
					break;
				case NhanVienDialogMode.Edit:
					Text = "Sửa nhân viên";
					btnPrimary.Text = "Lưu";
					lblMaNV.Visible = true;
					txtMaNV.Visible = true;
					txtMaNV.Enabled = false;
					SetReadOnly(false);
					break;
				case NhanVienDialogMode.View:
					Text = "Xem nhân viên";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					lblMaNV.Visible = true;
					txtMaNV.Visible = true;
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaNV.Enabled = !ro;
			txtTenNV.Enabled = !ro;
			dtpNamSinh.Enabled = !ro;
			txtDiaChi.Enabled = !ro;
			txtSDT.Enabled = !ro;
			cbbGioiTinh.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;
			cbbTaiKhoan.Enabled = !ro;
			btnGanTaiKhoan.Enabled = !ro;

			txtTenNV.TabStop = !ro;
			dtpNamSinh.TabStop = !ro;
			txtDiaChi.TabStop = !ro;
			txtSDT.TabStop = !ro;
			cbbGioiTinh.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;
			cbbTaiKhoan.TabStop = !ro;
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
			if (Mode == NhanVienDialogMode.View)
				return true;

			if (string.IsNullOrWhiteSpace(txtTenNV.Text))
			{
				MessageBox.Show("Vui lòng nhập tên nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenNV.Focus();
				return false;
			}

			if (dtpNamSinh.Value > DateTime.Now.AddYears(-18))
			{
				MessageBox.Show("Nhân viên phải từ 18 tuổi trở lên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				dtpNamSinh.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
			{
				MessageBox.Show("Vui lòng nhập địa chỉ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtDiaChi.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return false;
			}

			if (txtSDT.Text.Length < 10 || txtSDT.Text.Length > 11)
			{
				MessageBox.Show("Số điện thoại phải có 10-11 số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return false;
			}

			if (cbbGioiTinh.SelectedIndex < 0)
			{
				MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbGioiTinh.Focus();
				return false;
			}

			return true;
		}

		private void btnGanTaiKhoan_Click(object sender, EventArgs e)
		{
			using (var dialog = new GanTaiKhoanDialog())
			{
				// Lấy danh sách tài khoản chưa gắn
				var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(SelectedMaTK)
					.Select(tk => new KeyValuePair<int, string>(
						tk.MaTK,
						$"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
					))
					.ToList();

				dialog.BindTaiKhoan(availableTK, SelectedMaTK);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					SelectedMaTK = dialog.SelectedMaTK;
					// Refresh ComboBox
					BindTaiKhoan(availableTK, SelectedMaTK);
				}
			}
		}
	}
}
