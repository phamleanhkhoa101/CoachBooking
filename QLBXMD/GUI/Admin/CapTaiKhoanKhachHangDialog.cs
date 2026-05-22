using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	/// <summary>
	/// Dialog để admin cấp tài khoản mới cho khách hàng (chỉ quyền User)
	/// </summary>
	public partial class CapTaiKhoanKhachHangDialog : Form
	{
		private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();
		private readonly QuyenBUS _quyenBUS = new QuyenBUS();
		private readonly KhachHang _khachHang;

		private TextBox txtTenDangNhap;
		private TextBox txtEmail;
		private TextBox txtMatKhau;
		private TextBox txtXacNhanMatKhau;
		private Label lblQuyen;
		private CheckBox chkHienMatKhau;
		private Button btnLuu;

		public TaiKhoan TaiKhoanMoi { get; private set; }

		public CapTaiKhoanKhachHangDialog(KhachHang khachHang)
		{
			_khachHang = khachHang ?? throw new ArgumentNullException(nameof(khachHang));
			CreateControls();
		}

		private void CreateControls()
		{
			// Form settings
			this.Text = "Cấp tài khoản cho khách hàng";
			this.Size = new Size(550, 520);
			this.StartPosition = FormStartPosition.CenterParent;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.BackColor = Color.White;

			// Header Panel
			var pnlHeader = new Panel
			{
				Dock = DockStyle.Top,
				Height = 60,
				BackColor = Color.FromArgb(33, 150, 243) // Blue cho khách hàng
			};
			this.Controls.Add(pnlHeader);

			var lblHeader = new Label
			{
				Text = "CẤP TÀI KHOẢN CHO KHÁCH HÀNG",
				Font = new Font("Segoe UI", 14F, FontStyle.Bold),
				ForeColor = Color.White,
				Location = new Point(20, 15),
				AutoSize = true
			};
			pnlHeader.Controls.Add(lblHeader);

			// Footer Panel (fixed at bottom) - ensures buttons are always visible
			var pnlFooter = new Panel
			{
				Dock = DockStyle.Bottom,
				Height = 70,
				BackColor = Color.WhiteSmoke
			};
			this.Controls.Add(pnlFooter);

			// Content panel
			var pnlContent = new Panel
			{
				Dock = DockStyle.Fill,
				BackColor = Color.White,
				AutoScroll = true,
				Padding = new Padding(0, 10, 0, 10)
			};
			this.Controls.Add(pnlContent);

			int y = 10;

			// Thông tin khách hàng
			var grpKhachHang = new GroupBox
			{
				Text = "Thông tin khách hàng",
				Location = new Point(20, y),
				Size = new Size(490, 90),
				Font = new Font("Segoe UI", 9F, FontStyle.Bold)
			};
			//pnlContent.Controls.Add(grpKhachHang);

			var lblKhachHang = new Label
			{
				Text = $"Mã KH: {_khachHang.MaKH}\n" +
					   $"Tên: {_khachHang.TenKH}\n" +
					   $"SĐT: {_khachHang.SDT}",
				Location = new Point(15, 25),
				AutoSize = true,
				Font = new Font("Segoe UI", 9F)
			};
			grpKhachHang.Controls.Add(lblKhachHang);

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
				Text = GenerateUsername(_khachHang.TenKH)
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

			// Quyền (chỉ hiển thị, không cho chọn)
			pnlContent.Controls.Add(new Label
			{
				Text = "Quyền:",
				Location = new Point(20, y),
				Size = new Size(150, 20),
				Font = new Font("Segoe UI", 9F)
			});

			lblQuyen = new Label
			{
				Text = "Khách hàng (User)",
				Location = new Point(180, y),
				AutoSize = true,
				Font = new Font("Segoe UI", 9F, FontStyle.Bold),
				ForeColor = Color.FromArgb(33, 150, 243)
			};
			pnlContent.Controls.Add(lblQuyen);

			y += 40;

			// Hiện mật khẩu
			chkHienMatKhau = new CheckBox
			{
				Text = "Hiện mật khẩu",
				Location = new Point(180, y),
				AutoSize = true,
				Font = new Font("Segoe UI", 9F)
			};
			chkHienMatKhau.CheckedChanged += ChkHienMatKhau_CheckedChanged;
			pnlContent.Controls.Add(chkHienMatKhau);

			// Footer buttons using FlowLayoutPanel docked to right to ensure visibility
			var pnlButtons = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.RightToLeft,
				Dock = DockStyle.Right,
				Width = 320,
				Padding = new Padding(10),
				WrapContents = false,
				AutoSize = false
			};
			pnlFooter.Controls.Add(pnlButtons);

			btnLuu = new Button
			{
				Text = "Tạo tài khoản",
				Size = new Size(130, 35),
				Font = new Font("Segoe UI", 9F, FontStyle.Bold),
				BackColor = Color.FromArgb(33, 150, 243),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Cursor = Cursors.Hand,
				Margin = new Padding(10, 10, 0, 10)
			};
			btnLuu.FlatAppearance.BorderSize = 0;
			btnLuu.Click += BtnLuu_Click;
			pnlButtons.Controls.Add(btnLuu);

			var btnHuy = new Button
			{
				Text = "Hủy",
				Size = new Size(120, 35),
				Font = new Font("Segoe UI", 9F),
				BackColor = Color.FromArgb(244, 67, 54),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Cursor = Cursors.Hand,
				Margin = new Padding(10, 10, 10, 10)
			};
			btnHuy.FlatAppearance.BorderSize = 0;
			btnHuy.Click += (s, ev) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
			pnlButtons.Controls.Add(btnHuy);

			// Make Enter/Escape work as accept/cancel
			this.AcceptButton = btnLuu;
			this.CancelButton = btnHuy;

			// Ensure footer is visible above content (do NOT bring content to front)
			pnlFooter.BringToFront();
			pnlHeader.BringToFront();

			// Force layout update
			this.PerformLayout();
			this.Invalidate();
			this.Update();
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
				// Lấy quyền "User" (Khách hàng)
				var userQuyen = _quyenBUS.GetAll().FirstOrDefault(q => q.TenQuyen == "KhachHang");
				if (userQuyen == null)
				{
					MessageBox.Show("Không tìm thấy quyền 'khachhang' trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Tạo tài khoản mới
				var taiKhoan = new TaiKhoan
				{
					TenDangNhap = txtTenDangNhap.Text.Trim(),
					Email = txtEmail.Text.Trim(),
					MaQuyen = userQuyen.MaQuyen, // Luôn là quyền User
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

				// Gắn tài khoản cho khách hàng
				_khachHang.MaTK = createdTK.MaTK;
				var khBus = new KhachHangBUS();
				string updateResult = khBus.Update(_khachHang);

				if (!updateResult.Contains("thành công"))
				{
					MessageBox.Show($"Tạo tài khoản thành công nhưng không thể gắn cho khách hàng:\n{updateResult}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					TaiKhoanMoi = createdTK;
					this.DialogResult = DialogResult.OK;
					return;
				}

				MessageBox.Show(
					$"Cấp tài khoản thành công!\n\n" +
					$"Khách hàng: {_khachHang.TenKH}\n" +
					$"Tên đăng nhập: {taiKhoan.TenDangNhap}\n" +
					$"Email: {taiKhoan.Email}\n" +
					$"Quyền: Khách hàng (User)",
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

			if (!QLBXMD.Utils.Validation.IsValidMatKhau(txtMatKhau.Text))
			{
				MessageBox.Show("Mật khẩu phải từ 6 ký tự trở lên, bao gồm chữ thường, chữ hoa, số và ký tự đặc biệt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

			return true;
		}

		private string GenerateUsername(string fullName)
		{
			// Tách họ tên
			var parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0) return "khachhang";

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
			if (string.IsNullOrEmpty(text)) return text;

			// Normalize to decomposed form (letters + diacritics)
			string normalized = text.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();
			foreach (var ch in normalized)
			{
				var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
				if (uc != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(ch);
				}
			}

			// Compose back and replace special Vietnamese characters
			string result = sb.ToString().Normalize(NormalizationForm.FormC);
			result = result.Replace('đ', 'd').Replace('Đ', 'D');
			return result;
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
