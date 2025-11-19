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
				if (cbbTrangThai.SelectedIndex == 0) return true; // Hoạt động
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
			cbbTrangThai.Items.Add("Hoạt động");
			cbbTrangThai.Items.Add("Khóa");
			cbbTrangThai.SelectedIndex = 0; // Mặc định hoạt động
			SetupButtonEffects();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
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

			// Hiệu ứng cho nút Cancel (Hủy)
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
					Text = "Thêm tài khoản";
					lblHeader.Text = "THÊM TÀI KHOẢN MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					txtTenDangNhap.Enabled = true;
					lblMatKhau.Text = "Mật khẩu:";
					lblMatKhau.Visible = true;
					txtMatKhau.Visible = true;
					lblXacNhanMatKhau.Visible = true;
					txtXacNhanMatKhau.Visible = true;
					lblXacNhanMatKhau.Text = "Xác nhận mật khẩu:";
					chkHienMatKhau.Visible = true;
					SetReadOnly(false);
					break;
				case TaiKhoanDialogMode.Edit:
					Text = "Sửa tài khoản";
					lblHeader.Text = "CẬP NHẬT TÀI KHOẢN";
					btnPrimary.Text = "Lưu";
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
					Text = "Xem tài khoản";
					lblHeader.Text = "THÔNG TIN TÀI KHOẢN";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 485);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					lblMatKhau.Visible = false;
					txtMatKhau.Visible = false;
					lblXacNhanMatKhau.Visible = false;
					txtXacNhanMatKhau.Visible = false;
					chkHienMatKhau.Visible = false;
					SetReadOnly(false);
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

			// Thay đổi màu nền khi readonly
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
				// Kiểm tra tên đăng nhập
				if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
				{
					MessageBox.Show(
						"Vui lòng nhập tên đăng nhập.\n\nTên đăng nhập là thông tin bắt buộc.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtTenDangNhap.Focus();
					return false;
				}

				// Kiểm tra độ dài tên đăng nhập
				if (txtTenDangNhap.Text.Trim().Length < 6)
				{
					MessageBox.Show(
						$"Tên đăng nhập quá ngắn.\n\n" +
						$"Độ dài hiện tại: {txtTenDangNhap.Text.Trim().Length} ký tự\n" +
						$"Yêu cầu: Ít nhất 6 ký tự",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtTenDangNhap.Focus();
					return false;
				}

				// Kiểm tra tên đăng nhập chỉ chứa chữ, số và _
				if (!Regex.IsMatch(txtTenDangNhap.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show(
						"Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_).\n\n" +
						"Không được chứa khoảng trắng hoặc ký tự đặc biệt khác.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtTenDangNhap.Focus();
					return false;
				}

				// Kiểm tra mật khẩu
				if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
				{
					MessageBox.Show(
						"Vui lòng nhập mật khẩu.\n\nMật khẩu là thông tin bắt buộc.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtMatKhau.Focus();
					return false;
				}

				// Kiểm tra độ dài mật khẩu
				if (txtMatKhau.Text.Length < 8)
				{
					MessageBox.Show(
						$"Mật khẩu quá ngắn.\n\n" +
						$"Độ dài hiện tại: {txtMatKhau.Text.Length} ký tự\n" +
						$"Yêu cầu: Ít nhất 8 ký tự\n\n" +
						$"Mật khẩu mạnh giúp bảo vệ tài khoản tốt hơn.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtMatKhau.Focus();
					return false;
				}

				// Kiểm tra độ mạnh mật khẩu
				bool hasUpper = Regex.IsMatch(txtMatKhau.Text, @"[A-Z]");
				bool hasLower = Regex.IsMatch(txtMatKhau.Text, @"[a-z]");
				bool hasDigit = Regex.IsMatch(txtMatKhau.Text, @"\d");
				bool hasSpecial = Regex.IsMatch(txtMatKhau.Text, @"[!@#$%^&*()_+=\[{}\];:<>|./?,-]");

				if (!hasUpper || !hasLower || !hasDigit)
				{
					var result = MessageBox.Show(
						"Mật khẩu yếu!\n\n" +
						"Mật khẩu mạnh nên có:\n" +
						$"- Chữ hoa (A-Z): {(hasUpper ? "Có" : "Không")}\n" +
						$"- Chữ thường (a-z): {(hasLower ? "Có" : "Không")}\n" +
						$"- Số (0-9): {(hasDigit ? "Có" : "Không")}\n" +
						$"- Ký tự đặc biệt: {(hasSpecial ? "Có" : "Không")} (khuyến nghị)\n\n" +
						"Bạn có muốn tiếp tục với mật khẩu này?",
						"Cảnh báo mật khẩu",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning
						);

					if (result != DialogResult.Yes)
					{
						txtMatKhau.Focus();
						return false;
					}
				}

				// Kiểm tra xác nhận mật khẩu
				if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
				{
					MessageBox.Show(
						"Mật khẩu xác nhận không khớp.\n\n" +
						"Vui lòng nhập lại mật khẩu xác nhận cho giống với mật khẩu.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtXacNhanMatKhau.Focus();
					return false;
				}
			}

			// Kiểm tra email
			if (string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập email.\n\nEmail là thông tin bắt buộc.",
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
					$"Email '{txtEmail.Text}' không đúng định dạng.\n\n" +
					"Định dạng hợp lệ: example@domain.com",
					"Lỗi email",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtEmail.Focus();
				return false;
			}

			// Kiểm tra quyền
			if (SelectedMaQuyen == null || SelectedMaQuyen == 0)
			{
				MessageBox.Show(
					"Vui lòng chọn quyền cho tài khoản.\n\nQuyền xác định những tính năng tài khoản có thể truy cập.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbQuyen.Focus();
				return false;
			}

			// Kiểm tra trạng thái
			if (TrangThaiValue == null)
			{
				MessageBox.Show(
					"Vui lòng chọn trạng thái.",
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
