# ?? H??NG D?N: ??i "G?n tài kho?n" thành "C?p tài kho?n"

## ? **?ã hoàn thành:**

### **1. C?p nh?t UCNhanVien.cs**
- ? ??i text "Ch?a g?n" ? "Ch?a c?p"
- ? Ch? enable button "C?p TK" khi nhân viên ch?a có tài kho?n
- ? Ki?m tra nhân viên ?ã có tài kho?n hay ch?a tr??c khi m? form

### **2. C?p nh?t UCNhanVien.Designer.cs**
- ? ??i text button: "G?n TK" ? "?? C?p TK"
- ? Thêm style màu xanh lá cho button

## ?? **C?n t?o: CapTaiKhoanDialog.cs**

Do gi?i h?n token, tôi cung c?p template ??y ??:

```csharp
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;
using QLBXMD.GUI; // EmailHelper

namespace QLBXMD.GUI.Admin
{
    public partial class CapTaiKhoanDialog : Form
    {
        private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();
        private readonly QuyenBUS _quyenBUS = new QuyenBUS();
        private readonly NhanVien _nhanVien;

        private TextBox txtTenDangNhap, txtEmail, txtMatKhau, txtXacNhanMatKhau;
        private ComboBox cboQuyen;
        private CheckBox chkHienMatKhau, chkGuiEmail;
        private Button btnLuu;

        public TaiKhoan TaiKhoanMoi { get; private set; }

        public CapTaiKhoanDialog(NhanVien nhanVien)
        {
            _nhanVien = nhanVien ?? throw new ArgumentNullException(nameof(nhanVien));
            InitializeComponent();
            CreateControls();
            LoadQuyenList();
        }

        private void CreateControls()
        {
            // Form settings
            this.Text = "C?p tài kho?n cho nhân viên";
            this.Size = new Size(550, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Header Panel
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(76, 175, 80)
            };
            this.Controls.Add(pnlHeader);

            var lblHeader = new Label
            {
                Text = "?? C?P TÀI KHO?N CHO NHÂN VIÊN",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(500, 30),
                Location = new Point(20, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblHeader);

            int y = 80;

            // Thông tin nhân viên
            var grpNhanVien = new GroupBox
            {
                Text = "Thông tin nhân viên",
                Location = new Point(20, y),
                Size = new Size(490, 80),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            this.Controls.Add(grpNhanVien);

            var lblNhanVien = new Label
            {
                Text = $"Mã NV: {_nhanVien.MaNV} | Tên: {_nhanVien.TenNV} | S?T: {_nhanVien.SDT}",
                Location = new Point(15, 25),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };
            grpNhanVien.Controls.Add(lblNhanVien);

            y += 90;

            // Tên ??ng nh?p
            this.Controls.Add(new Label
            {
                Text = "Tên ??ng nh?p: *",
                Location = new Point(20, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9F)
            });

            txtTenDangNhap = new TextBox
            {
                Location = new Point(180, y - 3),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 9F)
            };
            this.Controls.Add(txtTenDangNhap);

            y += 35;

            // Email
            this.Controls.Add(new Label
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
            this.Controls.Add(txtEmail);

            y += 35;

            // M?t kh?u
            this.Controls.Add(new Label
            {
                Text = "M?t kh?u: *",
                Location = new Point(20, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9F)
            });

            txtMatKhau = new TextBox
            {
                Location = new Point(180, y - 3),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 9F),
                UseSystemPasswordChar = true
            };
            this.Controls.Add(txtMatKhau);

            // Button t?o m?t kh?u t? ??ng
            var btnGenerate = new Button
            {
                Text = "??",
                Location = new Point(465, y - 3),
                Size = new Size(45, 25),
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click += (s, ev) =>
            {
                string password = GenerateRandomPassword(12);
                txtMatKhau.Text = password;
                txtXacNhanMatKhau.Text = password;
                MessageBox.Show($"M?t kh?u: {password}\n\nVui lòng l?u l?i!", "M?t kh?u t? ??ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            this.Controls.Add(btnGenerate);

            y += 35;

            // Xác nh?n m?t kh?u
            this.Controls.Add(new Label
            {
                Text = "Xác nh?n m?t kh?u: *",
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
            this.Controls.Add(txtXacNhanMatKhau);

            y += 35;

            // Quy?n
            this.Controls.Add(new Label
            {
                Text = "Quy?n: *",
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
            this.Controls.Add(cboQuyen);

            y += 35;

            // Hi?n th? m?t kh?u
            chkHienMatKhau = new CheckBox
            {
                Text = "Hi?n th? m?t kh?u",
                Location = new Point(180, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };
            chkHienMatKhau.CheckedChanged += (s, ev) =>
            {
                txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
                txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            };
            this.Controls.Add(chkHienMatKhau);

            y += 25;

            // G?i email
            chkGuiEmail = new CheckBox
            {
                Text = "G?i thông tin ??ng nh?p qua email",
                Location = new Point(180, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                Checked = true
            };
            this.Controls.Add(chkGuiEmail);

            y += 35;

            // Buttons
            btnLuu = new Button
            {
                Text = "T?o tài kho?n",
                Location = new Point(250, y),
                Size = new Size(130, 35),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            this.Controls.Add(btnLuu);

            var btnHuy = new Button
            {
                Text = "H?y",
                Location = new Point(390, y),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += (s, ev) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnHuy);
        }

        private void LoadQuyenList()
        {
            try
            {
                var quyens = _quyenBUS.GetAll();
                cboQuyen.DataSource = quyens;
                cboQuyen.DisplayMember = "TenQuyen";
                cboQuyen.ValueMember = "MaQuyen";

                // M?c ??nh ch?n quy?n "Nhân viên"
                var nhanVienQuyen = quyens.FirstOrDefault(q => q.TenQuyen == "Nhân viên");
                if (nhanVienQuyen != null)
                {
                    cboQuyen.SelectedValue = nhanVienQuyen.MaQuyen;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i t?i danh sách quy?n: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                // T?o tài kho?n m?i
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
                    MessageBox.Show(result, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // L?y tài kho?n v?a t?o
                var createdTK = _taiKhoanBUS.GetAll()
                    .FirstOrDefault(tk => tk.TenDangNhap == taiKhoan.TenDangNhap);

                if (createdTK == null)
                {
                    MessageBox.Show("T?o tài kho?n thành công nh?ng không th? l?y thông tin.", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // G?n tài kho?n cho nhân viên
                _nhanVien.MaTK = createdTK.MaTK;
                var nvBus = new NhanVienBUS();
                string updateResult = nvBus.Update(_nhanVien);

                if (!updateResult.Contains("thành công"))
                {
                    MessageBox.Show($"T?o tài kho?n thành công nh?ng không th? g?n cho nhân viên:\n{updateResult}", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TaiKhoanMoi = createdTK;
                    this.DialogResult = DialogResult.OK;
                    return;
                }

                // G?i email n?u check
                if (chkGuiEmail.Checked)
                {
                    SendWelcomeEmail(taiKhoan.Email, taiKhoan.TenDangNhap, txtMatKhau.Text);
                }

                MessageBox.Show(
                    $"? C?p tài kho?n thành công!\n\n" +
                    $"Nhân viên: {_nhanVien.TenNV}\n" +
                    $"Tên ??ng nh?p: {taiKhoan.TenDangNhap}\n" +
                    $"Email: {taiKhoan.Email}\n" +
                    $"Quy?n: {cboQuyen.Text}",
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
                MessageBox.Show($"L?i: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nh?p tên ??ng nh?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            if (txtTenDangNhap.Text.Length < 3)
            {
                MessageBox.Show("Tên ??ng nh?p ph?i có ít nh?t 3 ký t?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nh?p email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nh?p m?t kh?u.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("M?t kh?u ph?i có ít nh?t 6 ký t?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
            {
                MessageBox.Show("M?t kh?u xác nh?n không kh?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhanMatKhau.Focus();
                return false;
            }

            if (cboQuyen.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng ch?n quy?n.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboQuyen.Focus();
                return false;
            }

            return true;
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }

        private void SendWelcomeEmail(string email, string username, string password)
        {
            try
            {
                string subject = "Thông tin tài kho?n h? th?ng qu?n lý b?n xe";
                string body = $@"
Xin chào {_nhanVien.TenNV},

Tài kho?n c?a b?n ?ã ???c t?o thành công!

Thông tin ??ng nh?p:
- Tên ??ng nh?p: {username}
- M?t kh?u: {password}
- Email: {email}

Vui lòng ??ng nh?p và ??i m?t kh?u ngay sau l?n ??ng nh?p ??u tiên.

Trân tr?ng,
Ban qu?n tr? h? th?ng
";

                bool sent = EmailHelper.SendEmail(email, subject, body);
                if (!sent)
                {
                    MessageBox.Show("Không th? g?i email thông báo. Vui lòng g?i thông tin ??ng nh?p cho nhân viên th? công.", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"L?i g?i email: {ex.Message}");
            }
        }
    }
}
```

## ?? **Cách s? d?ng:**

1. Copy code trên vào file `QLBXMD/GUI/Admin/CapTaiKhoanDialog.cs`
2. Build l?i project
3. Test ch?c n?ng:
   - M? UCNhanVien
   - Ch?n nhân viên ch?a có tài kho?n
   - Click nút "?? C?p TK"
   - ?i?n thông tin và t?o tài kho?n

## ?? **L?u ý:**

- ? Ch? hi?n th? button "C?p TK" cho nhân viên ch?a có tài kho?n
- ? T? ??ng g?n tài kho?n vào nhân viên sau khi t?o
- ? Có tính n?ng t?o m?t kh?u t? ??ng
- ? Có option g?i email thông báo cho nhân viên
- ? Validate ??y ?? các tr??ng input

---

**Date:** 2025-01-28  
**Status:** ? Completed (c?n t?o file CapTaiKhoanDialog.cs)
