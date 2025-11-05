using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum TaiXeDialogMode { Add, View, Edit }

	public class TaiXeDialog : Form
	{
		private TextBox txtMaTX, txtTenTX, txtCCCD, txtSDT, txtDiaChi, txtBangLai;
		private DateTimePicker dtpNamSinh, dtpNgayCap;
		private ComboBox cbbGioiTinh, cbbTrangThai;
		private Button btnPrimary, btnCancel;

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

		private void InitializeComponent()
		{
			this.Text = "Tài x?";
			this.StartPosition = FormStartPosition.CenterParent;
			this.Size = new Size(650, 580);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.AutoScroll = true;

			int leftLabel = 20;
			int leftControl = 160;
			int controlWidth = 450;
			int startTop = 20;
			int rowHeight = 40;

			var lblMaTX = new Label { Text = "Mã tài x?", Left = leftLabel, Top = startTop, AutoSize = true };
			txtMaTX = new TextBox { Left = leftControl, Top = startTop - 4, Width = controlWidth, ReadOnly = true, BackColor = Color.LightGray };

			var lblTenTX = new Label { Text = "Tên tài x?", Left = leftLabel, Top = startTop + rowHeight, AutoSize = true };
			txtTenTX = new TextBox { Left = leftControl, Top = startTop + rowHeight - 4, Width = controlWidth };

			var lblNamSinh = new Label { Text = "N?m sinh", Left = leftLabel, Top = startTop + rowHeight * 2, AutoSize = true };
			dtpNamSinh = new DateTimePicker
			{
				Left = leftControl,
				Top = startTop + rowHeight * 2 - 4,
				Width = 220,
				Format = DateTimePickerFormat.Short,
				Value = DateTime.Now.AddYears(-25)
			};

			var lblCCCD = new Label { Text = "CCCD", Left = leftLabel, Top = startTop + rowHeight * 3, AutoSize = true };
			txtCCCD = new TextBox { Left = leftControl, Top = startTop + rowHeight * 3 - 4, Width = controlWidth };

			var lblGioiTinh = new Label { Text = "Gi?i tính", Left = leftLabel, Top = startTop + rowHeight * 4, AutoSize = true };
			cbbGioiTinh = new ComboBox
			{
				Left = leftControl,
				Top = startTop + rowHeight * 4 - 4,
				Width = 150,
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			cbbGioiTinh.Items.AddRange(new string[] { "Nam", "N?" });
			cbbGioiTinh.SelectedIndex = 0;

			var lblSDT = new Label { Text = "S? ?i?n tho?i", Left = leftLabel, Top = startTop + rowHeight * 5, AutoSize = true };
			txtSDT = new TextBox { Left = leftControl, Top = startTop + rowHeight * 5 - 4, Width = controlWidth };

			var lblDiaChi = new Label { Text = "??a ch?", Left = leftLabel, Top = startTop + rowHeight * 6, AutoSize = true };
			txtDiaChi = new TextBox { Left = leftControl, Top = startTop + rowHeight * 6 - 4, Width = controlWidth };

			var lblBangLai = new Label { Text = "B?ng lái", Left = leftLabel, Top = startTop + rowHeight * 7, AutoSize = true };
			txtBangLai = new TextBox { Left = leftControl, Top = startTop + rowHeight * 7 - 4, Width = controlWidth };

			var lblNgayCap = new Label { Text = "Ngày c?p", Left = leftLabel, Top = startTop + rowHeight * 8, AutoSize = true };
			dtpNgayCap = new DateTimePicker
			{
				Left = leftControl,
				Top = startTop + rowHeight * 8 - 4,
				Width = 220,
				Format = DateTimePickerFormat.Short,
				Value = DateTime.Now
			};

			var lblTrangThai = new Label { Text = "Tr?ng thái", Left = leftLabel, Top = startTop + rowHeight * 9, AutoSize = true };
			cbbTrangThai = new ComboBox
			{
				Left = leftControl,
				Top = startTop + rowHeight * 9 - 4,
				Width = 200,
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			cbbTrangThai.Items.AddRange(new string[] { "Ho?t ??ng", "Ng?ng ho?t ??ng" });
			cbbTrangThai.SelectedIndex = 0;

			btnPrimary = new Button { Left = 420, Top = startTop + rowHeight * 10 + 10, Width = 90, Text = "L?u" };
			btnCancel = new Button { Left = 520, Top = startTop + rowHeight * 10 + 10, Width = 90, Text = "H?y" };

			btnPrimary.Click += (s, e) =>
			{
				if (ValidateInput())
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			};
			btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

			Controls.AddRange(new Control[]
			{
				lblMaTX, txtMaTX,
				lblTenTX, txtTenTX,
				lblNamSinh, dtpNamSinh,
				lblCCCD, txtCCCD,
				lblGioiTinh, cbbGioiTinh,
				lblSDT, txtSDT,
				lblDiaChi, txtDiaChi,
				lblBangLai, txtBangLai,
				lblNgayCap, dtpNgayCap,
				lblTrangThai, cbbTrangThai,
				btnPrimary, btnCancel
			});
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
