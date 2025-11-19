using QLBXMD.Utils;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum NhaXeDialogMode { Add, View, Edit }

	public partial class NhaXeDialog : Form
	{
		public NhaXeDialogMode Mode { get; private set; } = NhaXeDialogMode.Add;

		public string MaNhaXe
		{
			get => txtMaNhaXe.Text.Trim();
			set => txtMaNhaXe.Text = value;
		}

		public string TenNhaXe
		{
			get => txtTenNhaXe.Text.Trim();
			set => txtTenNhaXe.Text = value;
		}

		public string SDT
		{
			get => txtSDT.Text.Trim();
			set => txtSDT.Text = value;
		}

		public string Email
		{
			get => txtEmail.Text.Trim();
			set => txtEmail.Text = value;
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0) return true; // Hoạt động
				if (cbbTrangThai.SelectedIndex == 1) return false; // Ngừng hoạt động
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public NhaXeDialog()
		{
			InitializeComponent();
			SetupButtonEffects();
			SetupPhoneValidation();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == NhaXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == NhaXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == NhaXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == NhaXeDialogMode.Edit)
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

		private void SetupPhoneValidation()
		{
			// Chỉ cho phép nhập số
			txtSDT.KeyPress += (s, e) => {
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Tự động format khi nhập
			txtSDT.TextChanged += (s, e) => {
				// Giới hạn độ dài
				if (txtSDT.Text.Length > 11)
				{
					txtSDT.Text = txtSDT.Text.Substring(0, 11);
					txtSDT.SelectionStart = txtSDT.Text.Length;
				}
			};
		}

		public void SetMode(NhaXeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case NhaXeDialogMode.Add:
					Text = "Thêm nhà xe";
					lblHeader.Text = "🚍 THÊM NHÀ XE MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					lblMaNhaXe.Visible = false;
					txtMaNhaXe.Visible = false;
					SetReadOnly(false);
					break;
				case NhaXeDialogMode.Edit:
					Text = "Sửa nhà xe";
					lblHeader.Text = "🚍 CẬP NHẬT NHÀ XE";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					lblMaNhaXe.Visible = true;
					txtMaNhaXe.Visible = true;
					txtMaNhaXe.Enabled = false;
					SetReadOnly(false);
					break;
				case NhaXeDialogMode.View:
					Text = "Xem nhà xe";
					lblHeader.Text = "🚍 THÔNG TIN NHÀ XE";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 395);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					lblMaNhaXe.Visible = true;
					txtMaNhaXe.Visible = true;
					SetReadOnly(false);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaNhaXe.Enabled = !ro;
			txtTenNhaXe.Enabled = !ro;
			txtEmail.Enabled = !ro;
			txtSDT.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;

			txtTenNhaXe.TabStop = !ro;
			txtEmail.TabStop = !ro;
			txtSDT.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				txtMaNhaXe.BackColor = Color.WhiteSmoke;
				txtTenNhaXe.BackColor = Color.WhiteSmoke;
				txtEmail.BackColor = Color.WhiteSmoke;
				txtSDT.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaNhaXe.BackColor = Color.WhiteSmoke; // Luôn readonly
				txtTenNhaXe.BackColor = Color.White;
				txtEmail.BackColor = Color.White;
				txtSDT.BackColor = Color.White;
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
			if (Mode == NhaXeDialogMode.View)
				return true;

			// Kiểm tra tên nhà xe
			if (string.IsNullOrWhiteSpace(txtTenNhaXe.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập tên nhà xe.\n\nTên nhà xe là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenNhaXe.Focus();
				return false;
			}

			// Kiểm tra độ dài tên
			if (txtTenNhaXe.Text.Trim().Length < 3)
			{
				MessageBox.Show(
					"Tên nhà xe quá ngắn.\n\nVui lòng nhập tên đầy đủ (ít nhất 3 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenNhaXe.Focus();
				return false;
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
			if (!Validation.IsValidEmail(Email))
			{
				MessageBox.Show(
					$"Email '{Email}' không đúng định dạng.\n\n" +
					"Định dạng hợp lệ: example@domain.com",
					"Lỗi email",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtEmail.Focus();
				return false;
			}

			// Kiểm tra số điện thoại
			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập số điện thoại.\n\nSố điện thoại là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtSDT.Focus();
				return false;
			}

			// Kiểm tra định dạng số điện thoại
			string phone = txtSDT.Text.Trim();
			if (phone.Length < 10 || phone.Length > 11)
			{
				MessageBox.Show(
					$"Số điện thoại không hợp lệ.\n\n" +
					$"Độ dài hiện tại: {phone.Length} số\n" +
					$"Yêu cầu: 10-11 số",
					"Lỗi số điện thoại",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtSDT.Focus();
				return false;
			}

			// Kiểm tra số điện thoại chỉ chứa số
			if (!Regex.IsMatch(phone, @"^\d+$"))
			{
				MessageBox.Show(
					"Số điện thoại chỉ được chứa chữ số.\n\nVui lòng nhập lại.",
					"Lỗi số điện thoại",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtSDT.Focus();
				return false;
			}

			// Kiểm tra đầu số hợp lệ (Việt Nam)
			if (!Regex.IsMatch(phone, @"^(03|05|07|08|09)\d{8,9}$"))
			{
				var result = MessageBox.Show(
					$"Số điện thoại '{phone}' có vẻ không đúng định dạng Việt Nam.\n\n" +
					"Đầu số hợp lệ: 03, 05, 07, 08, 09\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận số điện thoại",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					txtSDT.Focus();
					return false;
				}
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

		private void NhaXeDialog_Load(object sender, EventArgs e)
		{
			// Khởi tạo combobox trạng thái nếu chưa có
			if (cbbTrangThai.Items.Count == 0)
			{
				cbbTrangThai.Items.Add("Hoạt động");
				cbbTrangThai.Items.Add("Ngừng hoạt động");
				cbbTrangThai.SelectedIndex = 0;
			}
		}
	}
}
