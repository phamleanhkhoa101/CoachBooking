using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	/// <summary>
	/// Dialog để admin cấp tài khoản mới cho nhân viên
	/// </summary>
	public partial class CapTaiKhoanDialog : Form
	{
		private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();
		private readonly QuyenBUS _quyenBUS = new QuyenBUS();
		private readonly NhanVien _nhanVien;

		private TextBox txtTenDangNhap;
		private TextBox txtEmail;
		private TextBox txtMatKhau;
		private TextBox txtXacNhanMatKhau;
		private ComboBox cboQuyen;
		private CheckBox chkHienMatKhau;
		private Button btnLuu;

		public TaiKhoan TaiKhoanMoi { get; private set; }

		public CapTaiKhoanDialog(NhanVien nhanVien)
		{
			_nhanVien = nhanVien ?? throw new ArgumentNullException(nameof(nhanVien));
			CreateControls();
			LoadQuyenList();
		}

		private void CreateControls()
		{
			// Form settings
			this.Text = "Cấp tài khoản cho nhân viên";
			this.Size = new Size(550, 520);
			this.StartPosition = FormStartPosition.CenterParent;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.BackColor = Color.White;

			// Header Panel
			var pnlHeader = new Panel
			{
				Location = new Point(0, 0),
				Size = new Size(550, 60),
				BackColor = Color.FromArgb(76, 175, 80)
			};
			this.Controls.Add(pnlHeader);

			var lblHeader = new Label
			{
				Text = "🔐 CẤP TÀI KHOẢN CHO NHÂN VIÊN",
				Font = new Font("Segoe UI", 14F, FontStyle.Bold),
				ForeColor = Color.White,
				Location = new Point(20, 15),
				AutoSize = true
			};
			pnlHeader.Controls.Add(lblHeader);

			// Content panel
			var pnlContent = new Panel
			{
				Location = new Point(0, 60),
				Size = new Size(550, 460),
				BackColor = Color.White,
				AutoScroll = true
			};
			this.Controls.Add(pnlContent);

			int y = 20;

			// Thông tin nhân viên
			var grpNhanVien = new GroupBox
			{
				Text = "Thông tin nhân viên",
				Location = new Point(20, y),
				Size = new Size(490, 90),
				Font = new Font("Segoe UI", 9F, FontStyle.Bold)
			};
			pnlContent.Controls.Add(grpNhanVien);

			var lblNhanVien = new Label
			{
				Text = $"Mã NV: {_nhanVien.MaNV}\n" +
					   $"Tên: {_nhanVien.TenNV}\n" +
					   $"SĐT: {_nhanVien.SDT}",
				Location = new Point(15, 25),
				AutoSize = true,
				Font = new Font("Segoe UI", 9F)
			};
			grpNhanVien.Controls.Add(lblNhanVien);

			y += 110;

			// Tên đăng nhập
			pnlContent.Controls.Add(new Label
			{
				Text = "Tên đăng nhập: *",
				Location = new Point(20, y),
				Size = new Size(150, 20),
				Font = new Font("Segoe UI", 9F)
			});

			txtTenDangNhap = new TextBox
			{
				Location = new Point(180, y - 3),
				Size = new Size(330, 25),
				Font = new Font("Segoe UI", 9F),
				Text = GenerateUsername(_nhanVien.TenNV)
			};
			pnlContent.Controls.Add(txtTenDangNhap);

			y += 40;

			// Email
			pnlContent.Controls.Add(new Label
			{
				Text = "Email: *",
				Location = new Point(20, y),
				Size = new Size(150, 20),
				Font = new Font("Segoe UI", 9F)
			});

			txtEmail = new TextBox
			{
				Location = new Point(180, y - 3),
				Size = new Size(330, 25),
				Font = new Font("Segoe UI", 9F)
			};
			pnlContent.Controls.Add(txtEmail);

			y += 40;

			// Mật khẩu
			pnlContent.Controls.Add(new Label
			{
				Text = "Mật khẩu: *",
				Location = new Point(20, y),
				Size = new Size(150, 20),
				Font = new Font("Segoe UI", 9F)
			});

			txtMatKhau = new TextBox
			{
				Location = new Point(180, y - 3),
				Size = new Size(330, 25),
				Font = new Font("Segoe UI", 9F),
				UseSystemPasswordChar = true
			};
			pnlContent.Controls.Add(txtMatKhau);

			y += 40;

			// Xác nhận mật khẩu
			pnlContent.Controls.Add(new Label
			{
				Text = "Xác nhận mật khẩu: *",
				Location = new Point(20, y),
				Size = new Size(150, 20),
				Font = new Font("Segoe UI", 9F)
			});

			txtXacNhanMatKhau = new TextBox
			{
				Location = new Point(180, y - 3),
				Size = new Size(330, 25),
				Font = new Font("Segoe UI", 9F),
				UseSystemPasswordChar = true
			};
			pnlContent.Controls.Add(txtXacNhanMatKhau);

			y += 40;

			// Quyền
			pnlContent.Controls.Add(new Label
			{
				Text = "Quyền: *",
				Location = new Point(20, y),
				Size = new Size(150, 20),
				Font = new Font("Segoe UI", 9F)
			});

			cboQuyen = new ComboBox
			{
				Location = new Point(180, y - 3),
				Size = new Size(330, 25),
				Font = new Font("Segoe UI", 9F),
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			pnlContent.Controls.Add(cboQuyen);

			y += 40;

			// Hiển thị mật khẩu
			chkHienMatKhau = new CheckBox
			{
				Text = "Hiển thị mật khẩu",
				Location = new Point(180, y),
				AutoSize = true,
				Font = new Font("Segoe UI", 9F)
			};
			chkHienMatKhau.CheckedChanged += ChkHienMatKhau_CheckedChanged;
			pnlContent.Controls.Add(chkHienMatKhau);

			y += 50;

			// Buttons
			btnLuu = new Button
			{
				Text = "Tạo tài khoản",
				Location = new Point(250, y),
				Size = new Size(130, 35),
				Font = new Font("Segoe UI", 9F, FontStyle.Bold),
				BackColor = Color.FromArgb(76, 175, 80),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Cursor = Cursors.Hand,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.OK
			};
			btnLuu.FlatAppearance.BorderSize = 0;
			btnLuu.Click += BtnLuu_Click;
			pnlContent.Controls.Add(btnLuu);

			var btnHuy = new Button
			{
				Text = "Hủy",
				Location = new Point(390, y),
				Size = new Size(120, 35),
				Font = new Font("Segoe UI", 9F),
				BackColor = Color.FromArgb(244, 67, 54),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Cursor = Cursors.Hand,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				DialogResult = DialogResult.Cancel
			};
			btnHuy.FlatAppearance.BorderSize = 0;
			btnHuy.Click += (s, ev) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
			pnlContent.Controls.Add(btnHuy);

			// Make Enter/Escape work as accept/cancel
			this.AcceptButton = btnLuu;
			this.CancelButton = btnHuy;

			// Bring panels to front in correct order
			pnlContent.BringToFront();
			pnlHeader.BringToFront();
		}

		private void LoadQuyenList()
		{
			try
			{
				var quyens = _quyenBUS.GetAll();
				cboQuyen.DataSource = quyens;
				cboQuyen.DisplayMember = "TenQuyen";
				cboQuyen.ValueMember = "MaQuyen";

				// Mặc định chọn quyền "Nhân viên"
				var nhanVienQuyen = quyens.FirstOrDefault(q => q.TenQuyen == "Nhân viên");
				if (nhanVienQuyen != null)
				{
					cboQuyen.SelectedValue = nhanVienQuyen.MaQuyen;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải danh sách quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ChkHienMatKhau_CheckedChanged(object sender, EventArgs e)
		{
			txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
			txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
		}

		private void BtnLuu_Click(object sender, EventArgs e)
		{
			if (!ValidateInput()) return;

			try
			{
				// Tạo tài khoản mới
				var taiKhoan = new TaiKhoan
				{
					TenDangNhap = txtTenDangNhap.Text.Trim(),
					Email = txtEmail.Text.Trim(),
					MaQuyen = (int)cboQuyen.SelectedValue,
					TrangThai = true
				};

				string result = _taiKhoanBUS.Add(taiKhoan, txtMatKhau.Text);

				if (!result.Contains("thành công"))
				{
					MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Lấy tài khoản vừa tạo
				var createdTK = _taiKhoanBUS.GetAll()
					.FirstOrDefault(tk => tk.TenDangNhap == taiKhoan.TenDangNhap);

				if (createdTK == null)
				{
					MessageBox.Show("Tạo tài khoản thành công nhưng không thể lấy thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				// Gắn tài khoản cho nhân viên
				_nhanVien.MaTK = createdTK.MaTK;
				var nvBus = new NhanVienBUS();
				string updateResult = nvBus.Update(_nhanVien);

				if (!updateResult.Contains("thành công"))
				{
					MessageBox.Show($"Tạo tài khoản thành công nhưng không thể gắn cho nhân viên:\n{updateResult}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					TaiKhoanMoi = createdTK;
					this.DialogResult = DialogResult.OK;
					return;
				}

				MessageBox.Show(
					$"✅ Cấp tài khoản thành công!\n\n" +
					$"Nhân viên: {_nhanVien.TenNV}\n" +
					$"Tên đăng nhập: {taiKhoan.TenDangNhap}\n" +
					$"Email: {taiKhoan.Email}\n" +
					$"Quyền: {cboQuyen.Text}",
					"Thành công",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);

				TaiKhoanMoi = createdTK;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool ValidateInput()
		{
			// Tên đăng nhập
			if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
			{
				MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenDangNhap.Focus();
				return false;
			}

			if (txtTenDangNhap.Text.Length < 3)
			{
				MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenDangNhap.Focus();
				return false;
			}

			if (!Regex.IsMatch(txtTenDangNhap.Text, @"^[a-zA-Z0-9_\.]+$"))
			{
				MessageBox.Show("Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới (_) và dấu chấm (.).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenDangNhap.Focus();
				return false;
			}

			// Email
			if (string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				MessageBox.Show("Vui lòng nhập email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmail.Focus();
				return false;
			}

			if (!IsValidEmail(txtEmail.Text))
			{
				MessageBox.Show("Email không đúng định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtEmail.Focus();
				return false;
			}

			// Mật khẩu
			if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
			{
				MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhau.Focus();
				return false;
			}

			if (txtMatKhau.Text.Length < 6)
			{
				MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtMatKhau.Focus();
				return false;
			}

			// Xác nhận mật khẩu
			if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
			{
				MessageBox.Show("Mật khẩu xác nhận không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtXacNhanMatKhau.Focus();
				return false;
			}

			// Quyền
			if (cboQuyen.SelectedValue == null)
			{
				MessageBox.Show("Vui lòng chọn quyền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cboQuyen.Focus();
				return false;
			}

			return true;
		}

		private string GenerateUsername(string fullName)
		{
			// Tách họ tên
			var parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0) return "nhanvien";

			// Lấy tên (phần cuối) + chữ cái đầu của họ đệm
			string username = parts[parts.Length - 1].ToLower();
			for (int i = 0; i < parts.Length - 1; i++)
			{
				username += parts[i][0].ToString().ToLower();
			}

			// Remove Vietnamese accents
			username = RemoveVietnameseAccents(username);

			return username;
		}

		private string RemoveVietnameseAccents(string text)
		{
			string[] vietnameseChars = new string[]
			{
				"aAeEoOuUiIdDyY",
				"áàạảãâấầậẩẫăắằặẳẵ",
				"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
				"éèẹẻẽêếềệểễ",
				"ÉÈẸẺẼÊẾỀỆỂỄ",
				"óòọỏõôốồộổỗơớờợởỡ",
				"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
				"úùụủũưứừựửữ",
				"ÚÙỤỦŨƯỨỪỰỬỮ",
				"íìịỉĩ",
				"ÍÌỊỈĨ",
				"đ",
				"Đ",
				"ýỳỵỷỹ",
				"ÝỲỴỶỸ"
			};

			for (int i = 1; i < vietnameseChars.Length; i++)
			{
				for (int j = 0; j < vietnameseChars[i].Length; j++)
				{
					text = text.Replace(vietnameseChars[i][j], vietnameseChars[0][i - 1]);
				}
			}

			return text;
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
	}
}
