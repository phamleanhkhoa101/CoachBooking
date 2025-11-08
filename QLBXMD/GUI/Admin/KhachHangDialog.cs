using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;

namespace QLBXMD.GUI.Admin
{
	public enum KhachHangDialogMode { Add, View, Edit }

	public partial class KhachHangDialog : Form
	{
		private TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();

		public KhachHangDialogMode Mode { get; private set; } = KhachHangDialogMode.Add;

		// Make txtMaKH publicly accessible
		public TextBox TxtMaKH => txtMaKH;

		public int MaKH { get; set; }
		public string TenKH { get => txtTenKH.Text; set => txtTenKH.Text = value; }
		public DateTime NamSinhValue { get => dtpNamSinh.Value; set => dtpNamSinh.Value = value; }
		public string SDT { get => txtSDT.Text; set => txtSDT.Text = value; }

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

		public KhachHangDialog()
		{
			InitializeComponent();
			cbbGioiTinh.Items.Add("Nam");
			cbbGioiTinh.Items.Add("N?");
			cbbGioiTinh.SelectedIndex = 0;
		}

		public void BindTaiKhoan(IEnumerable<KeyValuePair<int, string>> items, int? currentMaTK = null)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không ch?n --"));
			cbbTaiKhoan.DataSource = list;
			cbbTaiKhoan.DisplayMember = "Value";
			cbbTaiKhoan.ValueMember = "Key";

			if (currentMaTK.HasValue && currentMaTK.Value > 0)
				cbbTaiKhoan.SelectedValue = currentMaTK.Value;
			else
				cbbTaiKhoan.SelectedIndex = 0;
		}

		public void SetMode(KhachHangDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case KhachHangDialogMode.Add:
					Text = "Thêm khách hàng";
					btnPrimary.Text = "Thêm";
					lblMaKH.Visible = false;
					txtMaKH.Visible = false;
					SetReadOnly(false);
					break;
				case KhachHangDialogMode.Edit:
					Text = "S?a khách hàng";
					btnPrimary.Text = "L?u";
					lblMaKH.Visible = true;
					txtMaKH.Visible = true;
					txtMaKH.Enabled = false;
					SetReadOnly(false);
					break;
				case KhachHangDialogMode.View:
					Text = "Xem khách hàng";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					lblMaKH.Visible = true;
					txtMaKH.Visible = true;
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaKH.Enabled = !ro;
			txtTenKH.Enabled = !ro;
			dtpNamSinh.Enabled = !ro;
			txtSDT.Enabled = !ro;
			cbbGioiTinh.Enabled = !ro;
			cbbTaiKhoan.Enabled = !ro;
			btnGanTaiKhoan.Enabled = !ro;

			txtTenKH.TabStop = !ro;
			dtpNamSinh.TabStop = !ro;
			txtSDT.TabStop = !ro;
			cbbGioiTinh.TabStop = !ro;
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
			if (Mode == KhachHangDialogMode.View)
				return true;

			if (string.IsNullOrWhiteSpace(txtTenKH.Text))
			{
				MessageBox.Show("Vui lòng nh?p tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenKH.Focus();
				return false;
			}

			if (dtpNamSinh.Value > DateTime.Now)
			{
				MessageBox.Show("N?m sinh không ???c l?n h?n ngày hi?n t?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				dtpNamSinh.Focus();
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

			if (cbbGioiTinh.SelectedIndex < 0)
			{
				MessageBox.Show("Vui lòng ch?n gi?i tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbGioiTinh.Focus();
				return false;
			}

			return true;
		}

		private void btnGanTaiKhoan_Click(object sender, EventArgs e)
		{
			using (var dialog = new GanTaiKhoanDialog())
			{
				// L?y danh sách tài kho?n ch?a g?n
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
