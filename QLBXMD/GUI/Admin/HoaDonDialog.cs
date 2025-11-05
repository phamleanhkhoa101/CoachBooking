using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum HoaDonDialogMode { Add, View, Edit }

	public class HoaDonDialog : Form
	{
		private TextBox txtMaHD;
		private ComboBox cbbKhachHang, cbbNhanVien, cbbTrangThai;
		private DateTimePicker dtpNgayLap;
		private NumericUpDown numTongTien;
		private Button btnPrimary, btnCancel;

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

		private void InitializeComponent()
		{
			this.Text = "Hóa ??n";
			this.StartPosition = FormStartPosition.CenterParent;
			this.Size = new Size(600, 450);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			int leftLabel = 20;
			int leftControl = 160;
			int controlWidth = 400;
			int startTop = 20;
			int rowHeight = 50;

			var lblMaHD = new Label { Text = "Mã hóa ??n", Left = leftLabel, Top = startTop, AutoSize = true };
			txtMaHD = new TextBox { Left = leftControl, Top = startTop - 4, Width = controlWidth, ReadOnly = true, BackColor = Color.LightGray };

			var lblKhachHang = new Label { Text = "Khách hàng", Left = leftLabel, Top = startTop + rowHeight, AutoSize = true };
			cbbKhachHang = new ComboBox 
			{ 
				Left = leftControl, 
				Top = startTop + rowHeight - 4, 
				Width = controlWidth, 
				DropDownStyle = ComboBoxStyle.DropDownList 
			};

			var lblNhanVien = new Label { Text = "Nhân viên", Left = leftLabel, Top = startTop + rowHeight * 2, AutoSize = true };
			cbbNhanVien = new ComboBox 
			{ 
				Left = leftControl, 
				Top = startTop + rowHeight * 2 - 4, 
				Width = controlWidth, 
				DropDownStyle = ComboBoxStyle.DropDownList 
			};

			var lblNgayLap = new Label { Text = "Ngày l?p", Left = leftLabel, Top = startTop + rowHeight * 3, AutoSize = true };
			dtpNgayLap = new DateTimePicker
			{
				Left = leftControl,
				Top = startTop + rowHeight * 3 - 4,
				Width = 250,
				Format = DateTimePickerFormat.Custom,
				CustomFormat = "dd/MM/yyyy HH:mm",
				Value = DateTime.Now
			};

			var lblTongTien = new Label { Text = "T?ng ti?n (?)", Left = leftLabel, Top = startTop + rowHeight * 4, AutoSize = true };
			numTongTien = new NumericUpDown
			{
				Left = leftControl,
				Top = startTop + rowHeight * 4 - 4,
				Width = 250,
				Maximum = 1000000000,
				Minimum = 0,
				Increment = 1000,
				ThousandsSeparator = true,
				Value = 0
			};

			var lblTrangThai = new Label { Text = "Tr?ng thái", Left = leftLabel, Top = startTop + rowHeight * 5, AutoSize = true };
			cbbTrangThai = new ComboBox
			{
				Left = leftControl,
				Top = startTop + rowHeight * 5 - 4,
				Width = 250,
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			cbbTrangThai.Items.AddRange(new string[] { "?ã thanh toán", "Ch?a thanh toán" });
			cbbTrangThai.SelectedIndex = 1; // M?c ??nh ch?a thanh toán

			btnPrimary = new Button { Left = 370, Top = startTop + rowHeight * 6 + 10, Width = 90, Text = "L?u" };
			btnCancel = new Button { Left = 470, Top = startTop + rowHeight * 6 + 10, Width = 90, Text = "H?y" };

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
				lblMaHD, txtMaHD,
				lblKhachHang, cbbKhachHang,
				lblNhanVien, cbbNhanVien,
				lblNgayLap, dtpNgayLap,
				lblTongTien, numTongTien,
				lblTrangThai, cbbTrangThai,
				btnPrimary, btnCancel
			});
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
				MessageBox.Show("Vui lòng ch?n ít nh?t khách hàng ho?c nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
