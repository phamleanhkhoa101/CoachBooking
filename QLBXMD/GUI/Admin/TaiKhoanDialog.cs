using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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
			SetupButtonEffects();
		}

		private void SetupButtonEffects()
		{
			// Hi?u ?ng cho nút Primary (L?u/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == TaiKhoanDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == TaiKhoanDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == TaiKhoanDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == TaiKhoanDialogMode.Edit)
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
					lblHeader.Text = "?? THÊM TÀI KHO?N M?I";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					txtTenDangNhap.Enabled = true;
					lblMatKhau.Text = "M?t kh?u:";
					lblMatKhau.Visible = true;
					txtMatKhau.Visible = true;
					lblXacNhanMatKhau.Visible = true;
					txtXacNhanMatKhau.Visible = true;
					lblXacNhanMatKhau.Text = "Xác nh?n m?t kh?u:";
					chkHienMatKhau.Visible = true;
					SetReadOnly(false);
					break;
				case TaiKhoanDialogMode.Edit:
					Text = "S?a tài kho?n";
					lblHeader.Text = "?? C?P NH?T TÀI KHO?N";
					btnPrimary.Text = "L?u";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					txtTenDangNhap.Enabled = false;
					txtTenDangNhap.BackColor = Color.WhiteSmoke;
					lblMatKhau.Visible = false;
					txtMatKhau.Visible = false;
					lblXacNhanMatKhau.Visible = false;
					txtXacNhanMatKhau.Visible = false;
					chkHienMatKhau.Visible = false;
					SetReadOnly(false);
					break;
				case TaiKhoanDialogMode.View:
					Text = "Xem tài kho?n";
					lblHeader.Text = "?? THÔNG TIN TÀI KHO?N";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 485);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					lblMatKhau.Visible = false;
					txtMatKhau.Visible = false;
					lblXacNhanMatKhau.Visible = false;
					txtXacNhanMatKhau.Visible = false;
					chkHienMatKhau.Visible = false;
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

			// Thay ??i màu n?n khi readonly
			if (ro)
			{
				txtTenDangNhap.BackColor = Color.WhiteSmoke;
				txtEmail.BackColor = Color.WhiteSmoke;
				cbbQuyen.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
				txtMatKhau.BackColor = Color.WhiteSmoke;
				txtXacNhanMatKhau.BackColor = Color.WhiteSmoke;
			}
			else
			{
				if (Mode == TaiKhoanDialogMode.Edit)
					txtTenDangNhap.BackColor = Color.WhiteSmoke; // Disabled in edit mode
				else
					txtTenDangNhap.BackColor = Color.White;
				
				txtEmail.BackColor = Color.White;
				cbbQuyen.BackColor = Color.White;
				cbbTrangThai.BackColor = Color.White;
				txtMatKhau.BackColor = Color.White;
				txtXacNhanMatKhau.BackColor = Color.White;
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
			if (Mode == TaiKhoanDialogMode.View)
				return true;

			if (Mode == TaiKhoanDialogMode.Add)
			{
				// Ki?m tra tên ??ng nh?p
				if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
				{
					MessageBox.Show(
						"Vui lòng nh?p tên ??ng nh?p.\n\nTên ??ng nh?p là thông tin b?t bu?c.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtTenDangNhap.Focus();
					return false;
				}

				// Ki?m tra ?? dài tên ??ng nh?p
				if (txtTenDangNhap.Text.Trim().Length < 6)
				{
					MessageBox.Show(
						$"Tên ??ng nh?p quá ng?n.\n\n" +
						$"?? dài hi?n t?i: {txtTenDangNhap.Text.Trim().Length} ký t?\n" +
						$"Yêu c?u: Ít nh?t 6 ký t?",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtTenDangNhap.Focus();
					return false;
				}

				// Ki?m tra tên ??ng nh?p ch? ch?a ch?, s? và _
				if (!Regex.IsMatch(txtTenDangNhap.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show(
						"Tên ??ng nh?p ch? ???c ch?a ch? cái, s? và d?u g?ch d??i (_).\n\n" +
						"Không ???c ch?a kho?ng tr?ng ho?c ký t? ??c bi?t khác.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtTenDangNhap.Focus();
					return false;
				}

				// Ki?m tra m?t kh?u
				if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
				{
					MessageBox.Show(
						"Vui lòng nh?p m?t kh?u.\n\nM?t kh?u là thông tin b?t bu?c.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtMatKhau.Focus();
					return false;
				}

				// Ki?m tra ?? dài m?t kh?u
				if (txtMatKhau.Text.Length < 8)
				{
					MessageBox.Show(
						$"M?t kh?u quá ng?n.\n\n" +
						$"?? dài hi?n t?i: {txtMatKhau.Text.Length} ký t?\n" +
						$"Yêu c?u: Ít nh?t 8 ký t?\n\n" +
						$"M?t kh?u m?nh giúp b?o v? tài kho?n t?t h?n.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtMatKhau.Focus();
					return false;
				}

				// Ki?m tra ?? m?nh m?t kh?u
				bool hasUpper = Regex.IsMatch(txtMatKhau.Text, @"[A-Z]");
				bool hasLower = Regex.IsMatch(txtMatKhau.Text, @"[a-z]");
				bool hasDigit = Regex.IsMatch(txtMatKhau.Text, @"\d");
				bool hasSpecial = Regex.IsMatch(txtMatKhau.Text, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

				if (!hasUpper || !hasLower || !hasDigit)
				{
					var result = MessageBox.Show(
						"M?t kh?u y?u!\n\n" +
						"M?t kh?u m?nh nên có:\n" +
						$"? Ch? hoa (A-Z): {(hasUpper ? "Có" : "Không")}\n" +
						$"? Ch? th??ng (a-z): {(hasLower ? "Có" : "Không")}\n" +
						$"? S? (0-9): {(hasDigit ? "Có" : "Không")}\n" +
						$"? Ký t? ??c bi?t: {(hasSpecial ? "Có" : "Không")} (khuy?n ngh?)\n\n" +
						"B?n có mu?n ti?p t?c v?i m?t kh?u này?",
						"C?nh báo m?t kh?u",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning
					);

					if (result != DialogResult.Yes)
					{
						txtMatKhau.Focus();
						return false;
					}
				}

				// Ki?m tra xác nh?n m?t kh?u
				if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
				{
					MessageBox.Show(
						"M?t kh?u xác nh?n không kh?p.\n\n" +
						"Vui lòng nh?p l?i m?t kh?u xác nh?n cho gi?ng v?i m?t kh?u.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtXacNhanMatKhau.Focus();
					return false;
				}
			}

			// Ki?m tra email
			if (string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				MessageBox.Show(
					"Vui lòng nh?p email.\n\nEmail là thông tin b?t bu?c.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtEmail.Focus();
				return false;
			}

			// Validate email format
			if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
			{
				MessageBox.Show(
					$"Email '{txtEmail.Text}' không ?úng ??nh d?ng.\n\n" +
					"??nh d?ng h?p l?: example@domain.com",
					"L?i email",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtEmail.Focus();
				return false;
			}

			// Ki?m tra quy?n
			if (SelectedMaQuyen == null || SelectedMaQuyen == 0)
			{
				MessageBox.Show(
					"Vui lòng ch?n quy?n cho tài kho?n.\n\nQuy?n xác ??nh nh?ng tính n?ng tài kho?n có th? truy c?p.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbQuyen.Focus();
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

		private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
		{
			txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
			txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
		}
	}
}
