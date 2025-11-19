using System;
using System.Drawing;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;
using QLBXMD.GUI.User;

namespace QLBXMD.GUI.Admin
{
	public partial class UCThongTin : UserControl
	{
		private TaiKhoan _taiKhoan;
		private NhanVien _nhanVien;
		private bool _isEditing = false;

		public UCThongTin()
		{
			InitializeComponent();
		}

		public UCThongTin(TaiKhoan taiKhoan, NhanVien nhanVien)
		{
			InitializeComponent();
			_taiKhoan = taiKhoan;
			_nhanVien = nhanVien;
		}

		private void UCThongTin_Load(object sender, EventArgs e)
		{
			LoadData();
			SetEditMode(false);
		}

		public void SetData(TaiKhoan taiKhoan, NhanVien nhanVien)
		{
			_taiKhoan = taiKhoan;
			_nhanVien = nhanVien;
			LoadData();
			SetEditMode(false);
		}

		private void LoadData()
		{
			if (_nhanVien != null)
			{
				txtMaNV.Text = _nhanVien.MaNV.ToString();
				txtTenNV.Text = _nhanVien.TenNV;
				dtpNamSinh.Value = _nhanVien.NamSinh;
				txtDiaChi.Text = _nhanVien.DiaChi;
				txtSDT.Text = _nhanVien.SDT;
				cbbGioiTinh.SelectedIndex = _nhanVien.GioiTinh == true ? 0 : 1;
				cbbTrangThai.SelectedIndex = _nhanVien.TrangThai == true ? 0 : 1;
			}

			if (_taiKhoan != null)
			{
				txtMaTK.Text = _taiKhoan.MaTK.ToString();
				txtTenDangNhap.Text = _taiKhoan.TenDangNhap;
				txtEmail.Text = _taiKhoan.Email;
				txtQuyen.Text = _taiKhoan.Quyen?.TenQuyen ?? "N/A";
			}
		}

		private void SetEditMode(bool isEditing)
		{
			_isEditing = isEditing;

			txtTenNV.ReadOnly = !isEditing;
			dtpNamSinh.Enabled = isEditing;
			txtDiaChi.ReadOnly = !isEditing;
			txtSDT.ReadOnly = !isEditing;
			cbbGioiTinh.Enabled = isEditing;

			txtMaNV.ReadOnly = true;
			txtMaTK.ReadOnly = true;
			txtTenDangNhap.ReadOnly = true;
			txtEmail.ReadOnly = true;
			txtQuyen.ReadOnly = true;
			cbbTrangThai.Enabled = false;

			txtTenNV.BackColor = isEditing ? Color.White : Color.LightGray;
			txtDiaChi.BackColor = isEditing ? Color.White : Color.LightGray;
			txtSDT.BackColor = isEditing ? Color.White : Color.LightGray;

			btnChinhSua.Visible = !isEditing;
			btnLuu.Visible = isEditing;
			btnHuy.Visible = isEditing;
		}

		private void btnChinhSua_Click(object sender, EventArgs e)
		{
			SetEditMode(true);
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			LoadData();
			SetEditMode(false);
		}

		private void btnLuu_Click(object sender, EventArgs e)
		{
			if (!ValidateInput()) return;

			try
			{
				using (var db = new BXMDEntities())
				{
					var nv = db.NhanViens.Find(_nhanVien.MaNV);
					if (nv != null)
					{
						nv.TenNV = txtTenNV.Text.Trim();
						nv.NamSinh = dtpNamSinh.Value;
						nv.DiaChi = txtDiaChi.Text.Trim();
						nv.SDT = txtSDT.Text.Trim();
						nv.GioiTinh = cbbGioiTinh.SelectedIndex == 0;

						db.SaveChanges();

						_nhanVien.TenNV = nv.TenNV;
						_nhanVien.NamSinh = nv.NamSinh;
						_nhanVien.DiaChi = nv.DiaChi;
						_nhanVien.SDT = nv.SDT;
						_nhanVien.GioiTinh = nv.GioiTinh;

						MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						SetEditMode(false);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDoiMatKhau_Click(object sender, EventArgs e)
		{
			if (_taiKhoan == null)
			{
				MessageBox.Show("Không có thông tin tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			using (var dialog = new DoiMatKhauDialog(_taiKhoan))
			{
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void btnDoiEmail_Click(object sender, EventArgs e)
		{
			if (_taiKhoan == null)
			{
				MessageBox.Show("Không có thông tin tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			using (var dialog = new DoiEmailDialog(_taiKhoan))
			{
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					txtEmail.Text = _taiKhoan.Email;
				}
			}
		}

		private bool ValidateInput()
		{
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

			return true;
		}
	}

	public class DoiEmailDialog : Form
	{
		private TaiKhoan _taiKhoan;
		private TextBox txtEmailHienTai, txtEmailMoi, txtXacNhanEmail, txtMatKhau;
		private Button btnXacNhan, btnHuy;
		private CheckBox chkHienMatKhau;

		public DoiEmailDialog(TaiKhoan taiKhoan)
		{
			_taiKhoan = taiKhoan;
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.Text = "Đổi Email";
			this.StartPosition = FormStartPosition.CenterParent;
			this.Size = new Size(500, 400);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			int leftLabel = 20;
			int leftControl = 180;
			int controlWidth = 280;
			int startTop = 20;
			int rowHeight = 50;

			var lblTitle = new Label
			{
				Text = "Đổi Email",
				Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold),
				Left = leftLabel,
				Top = startTop,
				Width = 450,
				TextAlign = ContentAlignment.MiddleCenter
			};

			var lblEmailHienTai = new Label { Text = "Email hiện tại:", Left = leftLabel, Top = startTop + rowHeight, AutoSize = true };
			txtEmailHienTai = new TextBox 
			{ 
				Left = leftControl, 
				Top = startTop + rowHeight - 4, 
				Width = controlWidth, 
				ReadOnly = true,
				BackColor = Color.LightGray,
				Text = _taiKhoan.Email
			};

			var lblEmailMoi = new Label { Text = "Email mới:", Left = leftLabel, Top = startTop + rowHeight * 2, AutoSize = true };
			txtEmailMoi = new TextBox { Left = leftControl, Top = startTop + rowHeight * 2 - 4, Width = controlWidth };

			var lblXacNhan = new Label { Text = "Xác nhận email:", Left = leftLabel, Top = startTop + rowHeight * 3, AutoSize = true };
			txtXacNhanEmail = new TextBox { Left = leftControl, Top = startTop + rowHeight * 3 - 4, Width = controlWidth };

			var lblMatKhau = new Label { Text = "Mật khẩu:", Left = leftLabel, Top = startTop + rowHeight * 4, AutoSize = true };
			txtMatKhau = new TextBox { Left = leftControl, Top = startTop + rowHeight * 4 - 4, Width = controlWidth, UseSystemPasswordChar = true };

			chkHienMatKhau = new CheckBox
			{
				Text = "Hiện mật khẩu",
				Left = leftControl,
				Top = startTop + rowHeight * 5 - 4,
				AutoSize = true
			};
			chkHienMatKhau.CheckedChanged += (s, e) =>
			{
				txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
			};

			btnXacNhan = new Button 
			{ 
				Left = 270, 
				Top = startTop + rowHeight * 6, 
				Width = 90, 
				Height = 35, 
				Text = "Xác nhận",
				BackColor = Color.FromArgb(40, 167, 69),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnHuy = new Button 
			{ 
				Left = 370, 
				Top = startTop + rowHeight * 6, 
				Width = 90, 
				Height = 35, 
				Text = "Hủy",
				BackColor = Color.FromArgb(220, 53, 69),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};

			btnXacNhan.Click += BtnXacNhan_Click;
			btnHuy.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

			Controls.AddRange(new Control[]
			{
				lblTitle,
				lblEmailHienTai, txtEmailHienTai,
				lblEmailMoi, txtEmailMoi,
				lblXacNhan, txtXacNhanEmail,
				lblMatKhau, txtMatKhau,
				chkHienMatKhau,
				btnXacNhan, btnHuy
			});
		}

		private void BtnXacNhan_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtEmailMoi.Text))
			{
				MessageBox.Show("Vui lòng nhập email mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmailMoi.Focus();
				return;
			}

			if (!txtEmailMoi.Text.Contains("@") || !txtEmailMoi.Text.Contains("."))
			{
				MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmailMoi.Focus();
				return;
			}

			if (txtEmailMoi.Text.Trim() != txtXacNhanEmail.Text.Trim())
			{
				MessageBox.Show("Xác nhận email không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtXacNhanEmail.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
			{
				MessageBox.Show("Vui lòng nhập mật khẩu để xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhau.Focus();
				return;
			}

			if (_taiKhoan.MatKhau != txtMatKhau.Text)
			{
				MessageBox.Show("Mật khẩu không đúng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtMatKhau.Focus();
				return;
			}

			if (txtEmailMoi.Text.Trim() == _taiKhoan.Email)
			{
				MessageBox.Show("Email mới phải khác email hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmailMoi.Focus();
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					var tk = db.TaiKhoans.Find(_taiKhoan.MaTK);
					if (tk != null)
					{
						tk.Email = txtEmailMoi.Text.Trim();
						db.SaveChanges();
						_taiKhoan.Email = txtEmailMoi.Text.Trim();
						MessageBox.Show("Đổi email thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.DialogResult = DialogResult.OK;
						this.Close();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi đổi email: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
