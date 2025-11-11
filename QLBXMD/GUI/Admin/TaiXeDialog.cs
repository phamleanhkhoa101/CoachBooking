using System;
using System.Drawing;
using System.Text.RegularExpressions;
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
			dtpNgayCap.Value = DateTime.Now.AddYears(-2);
			cbbGioiTinh.SelectedIndex = 0;
			cbbTrangThai.SelectedIndex = 0;
			SetupButtonEffects();
			SetupNumericValidation();
		}

		private void SetupButtonEffects()
		{
			// Hi?u ?ng cho nút Primary (L?u/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == TaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == TaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == TaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == TaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};

			// Hi?u ?ng cho nút Cancel (H?y)
			btnCancel.MouseEnter += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(255, 87, 74);
			};
			btnCancel.MouseLeave += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(244, 67, 54);
			};
			btnCancel.MouseDown += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(200, 50, 40);
			};
			btnCancel.MouseUp += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(255, 87, 74);
			};
		}

		private void SetupNumericValidation()
		{
			// Ch? cho phép nh?p s? cho CCCD
			txtCCCD.KeyPress += (s, e) => {
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Ch? cho phép nh?p s? cho SDT
			txtSDT.KeyPress += (s, e) => {
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Gi?i h?n ?? dài
			txtSDT.TextChanged += (s, e) => {
				if (txtSDT.Text.Length > 11)
				{
					txtSDT.Text = txtSDT.Text.Substring(0, 11);
					txtSDT.SelectionStart = txtSDT.Text.Length;
				}
			};
		}

		public void SetMode(TaiXeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case TaiXeDialogMode.Add:
					Text = "Thêm tài x?";
					lblHeader.Text = "?? THÊM TÀI X? M?I";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					lblMaTX.Visible = false;
					txtMaTX.Visible = false;
					SetReadOnly(false);
					break;
				case TaiXeDialogMode.Edit:
					Text = "S?a tài x?";
					lblHeader.Text = "?? C?P NH?T TÀI X?";
					btnPrimary.Text = "L?u";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					lblMaTX.Visible = true;
					txtMaTX.Visible = true;
					txtMaTX.Enabled = false;
					SetReadOnly(false);
					break;
				case TaiXeDialogMode.View:
					Text = "Xem tài x?";
					lblHeader.Text = "?? THÔNG TIN TÀI X?";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 685);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					lblMaTX.Visible = true;
					txtMaTX.Visible = true;
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

			// Thay ??i màu n?n khi readonly
			if (ro)
			{
				txtMaTX.BackColor = Color.WhiteSmoke;
				txtTenTX.BackColor = Color.WhiteSmoke;
				dtpNamSinh.BackColor = Color.WhiteSmoke;
				txtCCCD.BackColor = Color.WhiteSmoke;
				cbbGioiTinh.BackColor = Color.WhiteSmoke;
				txtSDT.BackColor = Color.WhiteSmoke;
				txtDiaChi.BackColor = Color.WhiteSmoke;
				txtBangLai.BackColor = Color.WhiteSmoke;
				dtpNgayCap.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaTX.BackColor = Color.WhiteSmoke; // Luôn readonly
				txtTenTX.BackColor = Color.White;
				dtpNamSinh.BackColor = Color.White;
				txtCCCD.BackColor = Color.White;
				cbbGioiTinh.BackColor = Color.White;
				txtSDT.BackColor = Color.White;
				txtDiaChi.BackColor = Color.White;
				txtBangLai.BackColor = Color.White;
				dtpNgayCap.BackColor = Color.White;
				cbbTrangThai.BackColor = Color.White;
			}
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

			// Ki?m tra tên tài x?
			if (string.IsNullOrWhiteSpace(txtTenTX.Text))
			{
				MessageBox.Show(
					"Vui lòng nh?p tên tài x?.\n\nTên tài x? là thông tin b?t bu?c.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenTX.Focus();
				return false;
			}

			// Ki?m tra ?? dài tên
			if (txtTenTX.Text.Trim().Length < 2)
			{
				MessageBox.Show(
					"Tên tài x? quá ng?n.\n\nVui lòng nh?p tên ??y ?? (ít nh?t 2 ký t?).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenTX.Focus();
				return false;
			}

			// Ki?m tra tên có ch?a s? không
			if (Regex.IsMatch(txtTenTX.Text, @"\d"))
			{
				MessageBox.Show(
					"Tên tài x? không ???c ch?a s?.\n\nVui lòng nh?p tên h?p l?.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenTX.Focus();
				return false;
			}

			// Ki?m tra tu?i tài x? (ph?i t? 21 tu?i tr? lên)
			int age = DateTime.Now.Year - dtpNamSinh.Value.Year;
			if (dtpNamSinh.Value > DateTime.Now.AddYears(-21))
			{
				MessageBox.Show(
					$"Tài x? ph?i t? 21 tu?i tr? lên.\n\n" +
					$"Tu?i hi?n t?i: {age} tu?i\n" +
					$"Yêu c?u: Ít nh?t 21 tu?i\n\n" +
					"Theo quy ??nh pháp lu?t, tài x? ph?i ?? 21 tu?i.",
					"L?i tu?i",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNamSinh.Focus();
				return false;
			}

			// Ki?m tra tu?i quá cao
			if (age > 65)
			{
				var result = MessageBox.Show(
					$"Tài x? ?ã {age} tu?i (v??t tu?i ngh? h?u).\n\n" +
					"Tu?i ngh? h?u thông th??ng là 60-65 tu?i.\n" +
					"B?n có ch?c ch?n mu?n ti?p t?c?",
					"Xác nh?n tu?i",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					dtpNamSinh.Focus();
					return false;
				}
			}

			// Ki?m tra CCCD
			if (string.IsNullOrWhiteSpace(txtCCCD.Text))
			{
				MessageBox.Show(
					"Vui lòng nh?p s? CCCD.\n\nCCCD là thông tin b?t bu?c ?? xác ??nh danh tính.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtCCCD.Focus();
				return false;
			}

			// Ki?m tra ?? dài CCCD (12 s?)
			if (txtCCCD.Text.Length != 12)
			{
				MessageBox.Show(
					$"S? CCCD không h?p l?.\n\n" +
					$"?? dài hi?n t?i: {txtCCCD.Text.Length} s?\n" +
					$"Yêu c?u: 12 s?\n\n" +
					"CCCD Vi?t Nam có ?úng 12 ch? s?.",
					"L?i CCCD",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtCCCD.Focus();
				return false;
			}

			// Ki?m tra CCCD ch? ch?a s?
			if (!Regex.IsMatch(txtCCCD.Text, @"^\d{12}$"))
			{
				MessageBox.Show(
					"S? CCCD ch? ???c ch?a ch? s?.\n\nVui lòng nh?p l?i.",
					"L?i CCCD",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtCCCD.Focus();
				return false;
			}

			// Ki?m tra gi?i tính
			if (GioiTinhValue == null)
			{
				MessageBox.Show(
					"Vui lòng ch?n gi?i tính.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbGioiTinh.Focus();
				return false;
			}

			// Ki?m tra s? ?i?n tho?i
			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show(
					"Vui lòng nh?p s? ?i?n tho?i.\n\nS? ?i?n tho?i là thông tin b?t bu?c.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtSDT.Focus();
				return false;
			}

			// Ki?m tra ??nh d?ng s? ?i?n tho?i
			string phone = txtSDT.Text.Trim();
			if (phone.Length < 10 || phone.Length > 11)
			{
				MessageBox.Show(
					$"S? ?i?n tho?i không h?p l?.\n\n" +
					$"?? dài hi?n t?i: {phone.Length} s?\n" +
					$"Yêu c?u: 10-11 s?",
					"L?i s? ?i?n tho?i",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtSDT.Focus();
				return false;
			}

			// Ki?m tra ??u s? h?p l? (Vi?t Nam)
			if (!Regex.IsMatch(phone, @"^(03|05|07|08|09)\d{8,9}$"))
			{
				var result = MessageBox.Show(
					$"S? ?i?n tho?i '{phone}' có v? không ?úng ??nh d?ng Vi?t Nam.\n\n" +
					"??u s? h?p l?: 03, 05, 07, 08, 09\n" +
					"B?n có ch?c ch?n mu?n ti?p t?c?",
					"Xác nh?n s? ?i?n tho?i",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					txtSDT.Focus();
					return false;
				}
			}

			// Ki?m tra ??a ch?
			if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
			{
				MessageBox.Show(
					"Vui lòng nh?p ??a ch?.\n\n??a ch? là thông tin b?t bu?c.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiaChi.Focus();
				return false;
			}

			// Ki?m tra ?? dài ??a ch?
			if (txtDiaChi.Text.Trim().Length < 5)
			{
				MessageBox.Show(
					"??a ch? quá ng?n.\n\nVui lòng nh?p ??a ch? ??y ?? (ít nh?t 5 ký t?).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiaChi.Focus();
				return false;
			}

			// Ki?m tra b?ng lái
			if (string.IsNullOrWhiteSpace(txtBangLai.Text))
			{
				MessageBox.Show(
					"Vui lòng nh?p s? b?ng lái.\n\nB?ng lái là thông tin b?t bu?c cho tài x?.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtBangLai.Focus();
				return false;
			}

			// Ki?m tra ??nh d?ng b?ng lái (ví d?: B1, B2, C, D, E, F...)
			string bangLai = txtBangLai.Text.Trim().ToUpper();
			if (!Regex.IsMatch(bangLai, @"^[A-F][0-9]?$"))
			{
				var result = MessageBox.Show(
					$"H?ng b?ng lái '{txtBangLai.Text}' có v? không ?úng ??nh d?ng.\n\n" +
					"H?ng b?ng lái h?p l?: A1, A2, B1, B2, C, D, E, F, FB2, FC...\n" +
					"B?n có ch?c ch?n mu?n ti?p t?c?",
					"Xác nh?n b?ng lái",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					txtBangLai.Focus();
					return false;
				}
			}

			// Ki?m tra ngày c?p b?ng lái
			if (dtpNgayCap.Value > DateTime.Now)
			{
				MessageBox.Show(
					"Ngày c?p không ???c l?n h?n ngày hi?n t?i.\n\n" +
					$"Ngày hi?n t?i: {DateTime.Now:dd/MM/yyyy}\n" +
					$"Ngày c?p ?ã ch?n: {dtpNgayCap.Value:dd/MM/yyyy}",
					"L?i ngày c?p",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNgayCap.Focus();
				return false;
			}

			// Ki?m tra ngày c?p ph?i sau ngày sinh ít nh?t 21 n?m
			if (dtpNgayCap.Value < dtpNamSinh.Value.AddYears(21))
			{
				MessageBox.Show(
					"Ngày c?p b?ng lái không h?p l?.\n\n" +
					"B?ng lái ph?i ???c c?p sau khi tài x? ?? 21 tu?i.\n" +
					$"Ngày sinh: {dtpNamSinh.Value:dd/MM/yyyy}\n" +
					$"Ngày c?p t?i thi?u: {dtpNamSinh.Value.AddYears(21):dd/MM/yyyy}\n" +
					$"Ngày c?p hi?n t?i: {dtpNgayCap.Value:dd/MM/yyyy}",
					"L?i ngày c?p",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNgayCap.Focus();
				return false;
			}

			// Ki?m tra tr?ng thái
			if (TrangThaiValue == null)
			{
				MessageBox.Show(
					"Vui lòng ch?n tr?ng thái.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbTrangThai.Focus();
				return false;
			}

			return true;
		}
	}
}
