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
		public string BangLai
		{
			get => cbbBangLai.SelectedItem?.ToString() ?? "";
			set
			{
				int index = cbbBangLai.FindStringExact(value);
				if (index >= 0)
				{
					cbbBangLai.SelectedIndex = index;
				}
				else
				{
					if (!string.IsNullOrEmpty(value))
					{
						cbbBangLai.Items.Add(value);
						cbbBangLai.SelectedItem = value;
					}
					else
					{
						cbbBangLai.SelectedIndex = -1;
					}
				}
			}
		}
		public DateTime NgayCapValue { get => dtpNgayCap.Value; set => dtpNgayCap.Value = value; }

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

		private int _savedGioiTinhIndex = -1;
		private int _savedBangLaiIndex = -1;
		private int _savedTrangThaiIndex = -1;
		private DateTime _savedNamSinhValue = DateTime.Now;
		private DateTime _savedNgayCapValue = DateTime.Now;

		public TaiXeDialog()
		{
			InitializeComponent();
			dtpNamSinh.Value = DateTime.Now.AddYears(-25);
			dtpNgayCap.Value = DateTime.Now.AddYears(-2);
			cbbGioiTinh.SelectedIndex = 0;
			cbbTrangThai.SelectedIndex = 0;
			SetupButtonEffects();
			SetupNumericValidation();

			cbbGioiTinh.KeyDown += ComboBox_KeyDown_ReadOnly;
			cbbGioiTinh.KeyPress += ComboBox_KeyPress_ReadOnly;
			cbbGioiTinh.DropDown += ComboBox_DropDown_ReadOnly;
			cbbGioiTinh.SelectedIndexChanged += ComboBox_SelectedIndexChanged_ReadOnly;

			cbbBangLai.KeyDown += ComboBox_KeyDown_ReadOnly;
			cbbBangLai.KeyPress += ComboBox_KeyPress_ReadOnly;
			cbbBangLai.DropDown += ComboBox_DropDown_ReadOnly;
			cbbBangLai.SelectedIndexChanged += ComboBox_SelectedIndexChanged_ReadOnly;

			cbbTrangThai.KeyDown += ComboBox_KeyDown_ReadOnly;
			cbbTrangThai.KeyPress += ComboBox_KeyPress_ReadOnly;
			cbbTrangThai.DropDown += ComboBox_DropDown_ReadOnly;
			cbbTrangThai.SelectedIndexChanged += ComboBox_SelectedIndexChanged_ReadOnly;

			dtpNamSinh.KeyDown += DateTimePicker_KeyDown_ReadOnly;
			dtpNamSinh.KeyPress += DateTimePicker_KeyPress_ReadOnly;
			dtpNamSinh.ValueChanged += DateTimePicker_ValueChanged_ReadOnly;

			dtpNgayCap.KeyDown += DateTimePicker_KeyDown_ReadOnly;
			dtpNgayCap.KeyPress += DateTimePicker_KeyPress_ReadOnly;
			dtpNgayCap.ValueChanged += DateTimePicker_ValueChanged_ReadOnly;

			this.Load += TaiXeDialog_Load;
		}

		private void TaiXeDialog_Load(object sender, EventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				SetReadOnly(true);
			}
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
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

		private void SetupNumericValidation()
		{
			// Chỉ cho phép nhập số cho CCCD
			txtCCCD.KeyPress += (s, e) => {
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Chỉ cho phép nhập số cho SDT
			txtSDT.KeyPress += (s, e) => {
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Giới hạn độ dài
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
					Text = "Thêm tài xế";
					lblHeader.Text = "THÊM TÀI XẾ MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					lblMaTX.Visible = false;
					txtMaTX.Visible = false;
					SetReadOnly(false);
					break;
				case TaiXeDialogMode.Edit:
					Text = "Sửa tài xế";
					lblHeader.Text = "CẬP NHẬT TÀI XẾ";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					lblMaTX.Visible = true;
					txtMaTX.Visible = true;
					txtMaTX.Enabled = false;
					SetReadOnly(false);
					break;
				case TaiXeDialogMode.View:
					Text = "Xem tài xế";
					lblHeader.Text = "THÔNG TIN TÀI XẾ";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 575);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					lblMaTX.Visible = true;
					txtMaTX.Visible = true;
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaTX.ReadOnly = true;
			txtMaTX.Enabled = true;
			txtTenTX.ReadOnly = ro;
			txtTenTX.Enabled = true;
			dtpNamSinh.Enabled = true;
			txtCCCD.ReadOnly = ro;
			txtCCCD.Enabled = true;
			cbbGioiTinh.Enabled = true;
			txtSDT.ReadOnly = ro;
			txtSDT.Enabled = true;
			txtDiaChi.ReadOnly = ro;
			txtDiaChi.Enabled = true;
			cbbBangLai.Enabled = true;
			dtpNgayCap.Enabled = true;
			cbbTrangThai.Enabled = true;

			txtTenTX.TabStop = !ro;
			dtpNamSinh.TabStop = !ro;
			txtCCCD.TabStop = !ro;
			cbbGioiTinh.TabStop = !ro;
			txtSDT.TabStop = !ro;
			txtDiaChi.TabStop = !ro;
			cbbBangLai.TabStop = !ro;
			dtpNgayCap.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				_savedGioiTinhIndex = cbbGioiTinh.SelectedIndex;
				_savedBangLaiIndex = cbbBangLai.SelectedIndex;
				_savedTrangThaiIndex = cbbTrangThai.SelectedIndex;
				_savedNamSinhValue = dtpNamSinh.Value;
				_savedNgayCapValue = dtpNgayCap.Value;

				txtMaTX.BackColor = Color.WhiteSmoke;
				txtTenTX.BackColor = Color.WhiteSmoke;
				dtpNamSinh.BackColor = Color.WhiteSmoke;
				txtCCCD.BackColor = Color.WhiteSmoke;
				cbbGioiTinh.BackColor = Color.WhiteSmoke;
				txtSDT.BackColor = Color.WhiteSmoke;
				txtDiaChi.BackColor = Color.WhiteSmoke;
				cbbBangLai.BackColor = Color.WhiteSmoke;
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
				cbbBangLai.BackColor = Color.White;
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

			// Kiểm tra tên tài xế
			if (string.IsNullOrWhiteSpace(txtTenTX.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập tên tài xế.\n\nTên tài xế là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenTX.Focus();
				return false;
			}

			// Kiểm tra độ dài tên
			if (txtTenTX.Text.Trim().Length < 2)
			{
				MessageBox.Show(
					"Tên tài xế quá ngắn.\n\nVui lòng nhập tên đầy đủ (ít nhất 2 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenTX.Focus();
				return false;
			}

			// Kiểm tra tên có chứa số không
			if (Regex.IsMatch(txtTenTX.Text, @"\d"))
			{
				MessageBox.Show(
					"Tên tài xế không được chứa số.\n\nVui lòng nhập tên hợp lệ.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenTX.Focus();
				return false;
			}

			// Kiểm tra tuổi tài xế (phải từ 21 tuổi trở lên)
			int age = DateTime.Now.Year - dtpNamSinh.Value.Year;
			if (dtpNamSinh.Value > DateTime.Now.AddYears(-21))
			{
				MessageBox.Show(
					$"Tài xế phải từ 21 tuổi trở lên.\n\n" +
					$"Tuổi hiện tại: {age} tuổi\n" +
					$"Yêu cầu: Ít nhất 21 tuổi\n\n" +
					"Theo quy định pháp luật, tài xế phải đủ 21 tuổi.",
					"Lỗi tuổi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNamSinh.Focus();
				return false;
			}

			// Kiểm tra tuổi quá cao
			if (age > 65)
			{
				var result = MessageBox.Show(
					$"Tài xế đã {age} tuổi (vượt tuổi nghỉ hưu).\n\n" +
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

			// Kiểm tra CCCD
			if (string.IsNullOrWhiteSpace(txtCCCD.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập số CCCD.\n\nCCCD là thông tin bắt buộc để xác định danh tính.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtCCCD.Focus();
				return false;
			}

			// Kiểm tra độ dài CCCD (12 số)
			if (txtCCCD.Text.Length != 12)
			{
				MessageBox.Show(
					$"Số CCCD không hợp lệ.\n\n" +
					$"Độ dài hiện tại: {txtCCCD.Text.Length} số\n" +
					$"Yêu cầu: 12 số\n\n" +
					"CCCD Việt Nam có đúng 12 chữ số.",
					"Lỗi CCCD",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtCCCD.Focus();
				return false;
			}

			// Kiểm tra CCCD chỉ chứa số
			if (!Regex.IsMatch(txtCCCD.Text, @"^\d{12}$"))
			{
				MessageBox.Show(
					"Số CCCD chỉ được chứa chữ số.\n\nVui lòng nhập lại.",
					"Lỗi CCCD",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtCCCD.Focus();
				return false;
			}

			// Kiểm tra giới tính
			if (GioiTinhValue == null)
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

			// Kiểm tra bằng lái
			if (string.IsNullOrWhiteSpace(BangLai))
			{
				MessageBox.Show(
					"Vui lòng chọn hạng bằng lái.\n\nBằng lái là thông tin bắt buộc cho tài xế.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbBangLai.Focus();
				return false;
			}

			// Kiểm tra ngày cấp bằng lái
			if (dtpNgayCap.Value > DateTime.Now)
			{
				MessageBox.Show(
					"Ngày cấp không được lớn hơn ngày hiện tại.\n\n" +
					$"Ngày hiện tại: {DateTime.Now:dd/MM/yyyy}\n" +
					$"Ngày cấp đã chọn: {dtpNgayCap.Value:dd/MM/yyyy}",
					"Lỗi ngày cấp",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNgayCap.Focus();
				return false;
			}

			// Kiểm tra ngày cấp phải sau ngày sinh ít nhất 21 năm
			if (dtpNgayCap.Value < dtpNamSinh.Value.AddYears(21))
			{
				MessageBox.Show(
					"Ngày cấp bằng lái không hợp lệ.\n\n" +
					"Bằng lái phải được cấp sau khi tài xế đủ 21 tuổi.\n" +
					$"Ngày sinh: {dtpNamSinh.Value:dd/MM/yyyy}\n" +
					$"Ngày cấp tối thiểu: {dtpNamSinh.Value.AddYears(21):dd/MM/yyyy}\n" +
					$"Ngày cấp hiện tại: {dtpNgayCap.Value:dd/MM/yyyy}",
					"Lỗi ngày cấp",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNgayCap.Focus();
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

		private void ComboBox_KeyDown_ReadOnly(object sender, KeyEventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void ComboBox_KeyPress_ReadOnly(object sender, KeyPressEventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				e.Handled = true;
			}
		}

		private void ComboBox_DropDown_ReadOnly(object sender, EventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				ComboBox cbb = sender as ComboBox;
				if (cbb != null)
				{
					cbb.BeginInvoke(new Action(() => cbb.DroppedDown = false));
				}
			}
		}

		private void ComboBox_SelectedIndexChanged_ReadOnly(object sender, EventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				ComboBox cbb = sender as ComboBox;
				if (cbb == cbbGioiTinh)
				{
					if (cbbGioiTinh.Focused)
					{
						cbbGioiTinh.SelectedIndex = _savedGioiTinhIndex;
					}
					else
					{
						_savedGioiTinhIndex = cbbGioiTinh.SelectedIndex;
					}
				}
				else if (cbb == cbbBangLai)
				{
					if (cbbBangLai.Focused)
					{
						cbbBangLai.SelectedIndex = _savedBangLaiIndex;
					}
					else
					{
						_savedBangLaiIndex = cbbBangLai.SelectedIndex;
					}
				}
				else if (cbb == cbbTrangThai)
				{
					if (cbbTrangThai.Focused)
					{
						cbbTrangThai.SelectedIndex = _savedTrangThaiIndex;
					}
					else
					{
						_savedTrangThaiIndex = cbbTrangThai.SelectedIndex;
					}
				}
			}
		}

		private void DateTimePicker_KeyDown_ReadOnly(object sender, KeyEventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void DateTimePicker_KeyPress_ReadOnly(object sender, KeyPressEventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				e.Handled = true;
			}
		}

		private void DateTimePicker_ValueChanged_ReadOnly(object sender, EventArgs e)
		{
			if (Mode == TaiXeDialogMode.View)
			{
				DateTimePicker dtp = sender as DateTimePicker;
				if (dtp == dtpNamSinh)
				{
					if (dtpNamSinh.Focused)
					{
						dtpNamSinh.Value = _savedNamSinhValue;
					}
					else
					{
						_savedNamSinhValue = dtpNamSinh.Value;
					}
				}
				else if (dtp == dtpNgayCap)
				{
					if (dtpNgayCap.Focused)
					{
						dtpNgayCap.Value = _savedNgayCapValue;
					}
					else
					{
						_savedNgayCapValue = dtpNgayCap.Value;
					}
				}
			}
		}
	}
}
