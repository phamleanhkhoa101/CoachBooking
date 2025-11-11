using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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

			SetupButtonEffects();
			SetupPhoneValidation();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == NhanVienDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == NhanVienDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == NhanVienDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == NhanVienDialogMode.Edit)
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

			// Hiệu ứng cho nút Gán tài khoản
			btnGanTaiKhoan.MouseEnter += (s, e) => {
				btnGanTaiKhoan.BackColor = Color.FromArgb(53, 170, 255);
			};
			btnGanTaiKhoan.MouseLeave += (s, e) => {
				btnGanTaiKhoan.BackColor = Color.FromArgb(33, 150, 243);
			};
			btnGanTaiKhoan.MouseDown += (s, e) => {
				btnGanTaiKhoan.BackColor = Color.FromArgb(13, 130, 223);
			};
			btnGanTaiKhoan.MouseUp += (s, e) => {
				btnGanTaiKhoan.BackColor = Color.FromArgb(53, 170, 255);
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
					lblHeader.Text = "👨‍💼 THÊM NHÂN VIÊN MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					lblMaNV.Visible = false;
					txtMaNV.Visible = false;
					SetReadOnly(false);
					break;
				case NhanVienDialogMode.Edit:
					Text = "Sửa nhân viên";
					lblHeader.Text = "👨‍💼 CẬP NHẬT NHÂN VIÊN";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					lblMaNV.Visible = true;
					txtMaNV.Visible = true;
					txtMaNV.Enabled = false;
					SetReadOnly(false);
					break;
				case NhanVienDialogMode.View:
					Text = "Xem nhân viên";
					lblHeader.Text = "👨‍💼 THÔNG TIN NHÂN VIÊN";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 575);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
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

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				txtMaNV.BackColor = Color.WhiteSmoke;
				txtTenNV.BackColor = Color.WhiteSmoke;
				dtpNamSinh.BackColor = Color.WhiteSmoke;
				txtDiaChi.BackColor = Color.WhiteSmoke;
				txtSDT.BackColor = Color.WhiteSmoke;
				cbbGioiTinh.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
				cbbTaiKhoan.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaNV.BackColor = Color.WhiteSmoke; // Luôn readonly
				txtTenNV.BackColor = Color.White;
				dtpNamSinh.BackColor = Color.White;
				txtDiaChi.BackColor = Color.White;
				txtSDT.BackColor = Color.White;
				cbbGioiTinh.BackColor = Color.White;
				cbbTrangThai.BackColor = Color.White;
				cbbTaiKhoan.BackColor = Color.White;
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
			if (Mode == NhanVienDialogMode.View)
				return true;

			// Kiểm tra tên nhân viên
			if (string.IsNullOrWhiteSpace(txtTenNV.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập tên nhân viên.\n\nTên nhân viên là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenNV.Focus();
				return false;
			}

			// Kiểm tra độ dài tên
			if (txtTenNV.Text.Trim().Length < 2)
			{
				MessageBox.Show(
					"Tên nhân viên quá ngắn.\n\nVui lòng nhập tên đầy đủ (ít nhất 2 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenNV.Focus();
				return false;
			}

			// Kiểm tra tên có chứa số không
			if (Regex.IsMatch(txtTenNV.Text, @"\d"))
			{
				MessageBox.Show(
					"Tên nhân viên không được chứa số.\n\nVui lòng nhập tên hợp lệ.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenNV.Focus();
				return false;
			}

			// Kiểm tra tuổi nhân viên (phải từ 18 tuổi trở lên)
			int age = DateTime.Now.Year - dtpNamSinh.Value.Year;
			if (dtpNamSinh.Value > DateTime.Now.AddYears(-18))
			{
				MessageBox.Show(
					$"Nhân viên phải từ 18 tuổi trở lên.\n\n" +
					$"Tuổi hiện tại: {age} tuổi\n" +
					$"Yêu cầu: Ít nhất 18 tuổi",
					"Lỗi tuổi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNamSinh.Focus();
				return false;
			}

			// Kiểm tra tuổi nghỉ hưu (không quá 65 tuổi)
			if (age > 65)
			{
				var result = MessageBox.Show(
					$"Nhân viên đã {age} tuổi (quá tuổi nghỉ hưu).\n\n" +
					"Tuổi nghỉ hưu thông thường là 60-65 tuổi.\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận tuổi",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					dtpNamSinh.Focus();
					return false;
				}
			}

			// Kiểm tra địa chỉ
			if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập địa chỉ.\n\nĐịa chỉ là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiaChi.Focus();
				return false;
			}

			// Kiểm tra độ dài địa chỉ
			if (txtDiaChi.Text.Trim().Length < 5)
			{
				MessageBox.Show(
					"Địa chỉ quá ngắn.\n\nVui lòng nhập địa chỉ đầy đủ (ít nhất 5 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiaChi.Focus();
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

			// Kiểm tra giới tính
			if (cbbGioiTinh.SelectedIndex < 0)
			{
				MessageBox.Show(
					"Vui lòng chọn giới tính.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbGioiTinh.Focus();
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
