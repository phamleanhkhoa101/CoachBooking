using QLBXMD.BUS;
using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // add
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class FrmMainAdmin : Form
	{
		private TaiKhoan taiKhoan;
		private List<Button> sidebarButtons;
		private Button activeSidebarButton;
		private Panel sidebarIndicator;
		private NhanVien nhanVien;
		private TaiKhoanBUS taiKhoanBus = new TaiKhoanBUS();
		private readonly Color menuBackColor = Color.White;
		private readonly Color menuCardColor = Color.White;
		private readonly Color menuItemColor = Color.White;
		private readonly Color menuItemHoverColor = Color.FromArgb(248, 250, 252);
		private readonly Color menuItemActiveColor = Color.White;
		private readonly Color menuTextColor = Color.FromArgb(30, 41, 59);
		private readonly Color menuMutedTextColor = Color.Black;
		private readonly Color menuAccentColor = Color.FromArgb(29, 78, 216);
		private readonly Color borderColor = Color.FromArgb(226, 232, 240);
		private readonly Color headerBackColor = Color.FromArgb(37, 99, 235);
		private readonly Color headerTextColor = Color.White;
		private readonly Color mainBackColor = Color.FromArgb(248, 250, 252);

		// Collapsible groups: Chung, DanhMuc, HeThong
		private class GroupState
		{
			public string Name { get; set; }
			public List<Control> Controls { get; set; } = new List<Control>();
			public bool Expanded { get; set; } = true;
			public int OrigTop { get; set; }
			public int OrigBottom { get; set; }
			public int OrigHeight => OrigBottom - OrigTop;
		}
		private readonly Dictionary<string, GroupState> _groups = new Dictionary<string, GroupState>(StringComparer.OrdinalIgnoreCase);
		private readonly Dictionary<Control, int> _origTop = new Dictionary<Control, int>();

		public FrmMainAdmin()
		{
			InitializeComponent();
		}
		public FrmMainAdmin(TaiKhoan taiKhoan)
		{
			this.taiKhoan = taiKhoan;
			nhanVien = taiKhoanBus.GetNhanVienByTaiKhoan(taiKhoan);
			InitializeComponent();
		}

		private void btnNhaXe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCNhaXe());
			HightlightButton(btnNhaXe);
		}

		private void FrmMainAdmin_Load(object sender, EventArgs e)
		{
			sidebarButtons = new List<Button> { 
				btnTrangChu, btnDatVe, btnHoaDon, btnKhachHang,
				btnNhaXe, btnLoaiXe, btnXe, 
				btnTuyenDuong, btnChuyenXe, btnGiaVe, btnTaiXe,
				btnTaiKhoan, btnThongKe, btnPhanQuyen
			};
			ApplySidebarTheme();
			//lblName.Text = taiKhoan.
			lblRole.Text = taiKhoan.Quyen.TenQuyen;
			lblName.Text = nhanVien != null
				? $"Xin chào, {nhanVien.TenNV}"
				: $"Xin chào, {taiKhoan.TenDangNhap}";

			// Capture original positions of all controls in the menu for deterministic reflow
			_origTop.Clear();
			foreach (Control c in pnlMenu.Controls)
			{
				_origTop[c] = c.Top;
			}

			// Build collapsible groups and wire click events
			BuildGroups();
			label1.Click += label1_Click; // Chung
			// label3 click is already wired in designer
			label5.Click += label5_Click; // Hệ thống
			btnTrangChu.Click += btnTrangChu_Click; // Home
			btnGiaVe.Click += btnGiaVe_Click; // Giá vé
			btnTaiXe.Click += btnTaiXe_Click; // Tài xế
			btnHoaDon.Click += btnHoaDon_Click; // Hóa đơn
			btnKhachHang.Click += btnKhachHang_Click; // Khách hàng

			btnTrangChu_Click(this, EventArgs.Empty); // Show home by default
			ApplyPermissions();
		}

		private void ApplyPermissions()
		{
			// Determine role from account
			var roleName = taiKhoan?.Quyen?.TenQuyen?.Trim();
			bool isAdmin = string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase);

			// If not admin, treat as ticket seller: only allow Home and Booking
			if (!isAdmin)
			{
				var allowedForStaff = new HashSet<Button> { btnTrangChu, btnDatVe, btnHoaDon, btnKhachHang };
				foreach (var btn in sidebarButtons)
				{
					btn.Enabled = allowedForStaff.Contains(btn);
				}
			}
			else
			{
				foreach (var btn in sidebarButtons)
				{
					btn.Enabled = true;
				}
			}
		}

		private void ShowControl(System.Windows.Forms.UserControl control)
		{
			pnlMain.Controls.Clear();
			control.Dock = DockStyle.Fill;
			AdminGridTheme.ApplyToControlTree(control);
			pnlMain.Controls.Add(control);
			control.BringToFront();
		}
		private void HightlightButton(Button selectedButton)
		{
			foreach (var btn in sidebarButtons)
			{
				btn.BackColor = menuItemColor;
				btn.ForeColor = menuTextColor;
			}

			activeSidebarButton = selectedButton;
			selectedButton.BackColor = menuItemActiveColor;
			selectedButton.ForeColor = Color.FromArgb(15, 23, 42);

			if (sidebarIndicator != null)
			{
				sidebarIndicator.Visible = true;
				sidebarIndicator.Height = selectedButton.Height - 8;
				sidebarIndicator.Top = selectedButton.Top + 4;
				sidebarIndicator.Left = Math.Max(2, selectedButton.Left - 6);
				sidebarIndicator.BringToFront();
				selectedButton.BringToFront();
			}
		}

		private void btnLoaiXe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCLoaiXe());
			HightlightButton(btnLoaiXe);
		}

		private void btnLogout_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(
				"Bạn có chắc chắn muốn đăng xuất?",
				"Xác nhận đăng xuất",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (result == DialogResult.Yes)
			{
				this.Close();
			}
		}

		private void btnXe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCXe());
			HightlightButton(btnXe);
		}

		private void btnTuyenDuong_Click(object sender, EventArgs e)
		{
			ShowControl(new UCTuyenDuong());
			HightlightButton(btnTuyenDuong);
		}

		private void btnChuyenXe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCChuyenXe());
			HightlightButton(btnChuyenXe);
		}

		private void btnGiaVe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCGiaVe());
			HightlightButton(btnGiaVe);
		}

		private void btnTaiXe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCTaiXe());
			HightlightButton(btnTaiXe);
		}

		private void btnHoaDon_Click(object sender, EventArgs e)
		{
			ShowControl(new UCHoaDon());
			HightlightButton(btnHoaDon);
		}

		private void btnTrangChu_Click(object sender, EventArgs e)
		{
			ShowControl(new UCTrangChu());
			HightlightButton(btnTrangChu);
		}

		// Label clicks -> toggle groups
		private void label3_Click(object sender, EventArgs e) // Danh mục
		{
			ToggleGroup("DanhMuc");
		}
		private void label1_Click(object sender, EventArgs e) // Chung
		{
			ToggleGroup("Chung");
		}
		private void label5_Click(object sender, EventArgs e) // Hệ thống
		{
			ToggleGroup("HeThong");
		}

		// Group helpers
		private void BuildGroups()
		{
			_groups.Clear();

			AddGroup(
				"Chung",
				new[] { "btnTrangChu", "btnDatVe", "btnHoaDon", "btnKhachHang", "btnThanhToan" }
			);

			AddGroup(
				"DanhMuc",
				new[] { "btnNhaXe", "btnLoaiXe", "btnXe", "btnTuyenDuong", "btnChuyenXe", "btnGiaVe", "btnTaiXe" }
			);

			AddGroup(
				"HeThong",
				new[] { "btnTaiKhoan", "btnThongKe", "btnPhanQuyen" }
			);

			// After groups are defined, record original geometry for each group based on original Tops
			foreach (var kv in _groups)
			{
				var g = kv.Value;
				g.OrigTop = g.Controls.Min(c => _origTop.ContainsKey(c) ? _origTop[c] : c.Top);
				g.OrigBottom = g.Controls.Max(c => (_origTop.ContainsKey(c) ? _origTop[c] : c.Top) + c.Height);
			}
		}

		private void AddGroup(string key, string[] controlNames)
		{
			var found = new List<Control>();
			foreach (var n in controlNames)
			{
				var arr = pnlMenu.Controls.Find(n, true);
				if (arr != null && arr.Length > 0)
				{
					found.Add(arr[0]);
				}
			}

			found = found.Distinct().OrderBy(c => c.Top).ToList();
			if (found.Count == 0) return;

			var state = new GroupState
			{
				Name = key,
				Controls = found,
				Expanded = true
			};
			_groups[key] = state;
		}

		private void ToggleGroup(string key)
		{
			if (!_groups.TryGetValue(key, out var state)) return;
			if (state.Controls.Count == 0) return;

			state.Expanded = !state.Expanded;
			ReflowMenu();
		}

		private void ReflowMenu()
		{
			// Deterministic layout: place each control based on original Top minus
			// the total height of collapsed groups that were originally above it.
			foreach (Control c in pnlMenu.Controls)
			{
				if (!_origTop.ContainsKey(c))
					_origTop[c] = c.Top; // fallback
			}

			foreach (Control c in pnlMenu.Controls)
			{
				// Visibility: hide if inside a collapsed group, else show
				bool inCollapsedGroup = _groups.Values.Any(g => !g.Expanded && g.Controls.Contains(c));
				c.Visible = !inCollapsedGroup;

				int collapsedAbove = _groups.Values
					.Where(g => !g.Expanded && g.OrigTop < _origTop[c])
					.Sum(g => g.OrigHeight);

				c.Top = _origTop[c] - collapsedAbove;
			}

			if (activeSidebarButton != null && sidebarIndicator != null)
			{
				sidebarIndicator.Top = activeSidebarButton.Top + 5;
				sidebarIndicator.Visible = activeSidebarButton.Visible;
				sidebarIndicator.BringToFront();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var ucThongTin = new UCThongTin();
			ucThongTin.SetData(taiKhoan, nhanVien);
			ShowControl(ucThongTin);
			//HightlightButton(btn);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			ShowControl(new UCTaiKhoan());
			//HightlightButton(btnTrangChu);
		}

		private void btnPhanQuyen_Click(object sender, EventArgs e)
		{
			ShowControl(new UCNhanVien());
			HightlightButton(btnPhanQuyen);
		}

		private void btnKhachHang_Click(object sender, EventArgs e)
		{
			ShowControl(new UCKhachHang());
			HightlightButton(btnKhachHang);
		}

		private void btnDatVe_Click(object sender, EventArgs e)
		{
			var ucDatVe = new UCDatVe();
			ucDatVe.SetNhanVien(nhanVien); // ✅ Truyền thông tin nhân viên vào UCDatVe
			ShowControl(ucDatVe);
			HightlightButton(btnDatVe);
		}

		private void thôngTinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//ShowControl(new UCThongTin());
			var ucThongTin = new UCThongTin();
			ucThongTin.SetData(taiKhoan, nhanVien);
			ShowControl(ucThongTin);
		}

		private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(
				"Bạn có chắc chắn muốn đăng xuất?",
				"Xác nhận đăng xuất",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (result == DialogResult.Yes)
			{
				this.Close();
			}
		}

		private void btnSetting_Click(object sender, EventArgs e)
		{
			contextMenuStrip1.Show(Cursor.Position);
		}

		private void pnlMenu_Paint(object sender, PaintEventArgs e)
		{

		}

		private void btnTuyenDuong_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCTuyenDuong());
			HightlightButton(btnTuyenDuong);
		}

		private void btnChuyenXe_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCChuyenXe());
			HightlightButton(btnChuyenXe);
		}

		private void btnGiaVe_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCGiaVe());
			HightlightButton(btnGiaVe);
		}

		private void btnTaiXe_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCTaiXe());
			HightlightButton(btnTaiXe);
		}

		private void btnTaiKhoan_Click(object sender, EventArgs e)
		{
			ShowControl(new UCTaiKhoan());
			HightlightButton(btnTaiKhoan);
		}

		private void btnPhanQuyen_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCNhanVien());
			HightlightButton(btnPhanQuyen);
		}

		private void btnThongKe_Click(object sender, EventArgs e)
		{
			ShowControl(new UCThongKe());
			HightlightButton(btnThongKe);
		}

		private void btnKhachHang_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCKhachHang());
			HightlightButton(btnKhachHang);
		}

		private void btnHoaDon_Click_1(object sender, EventArgs e)
		{
			ShowControl(new UCHoaDon());
			HightlightButton(btnHoaDon);
		}

		private void btnThanhToan_Click(object sender, EventArgs e)
		{
			//ShowControl(new UCThanhToan());
			//HightlightButton(btnThanhToan);
		}

		private void ApplySidebarTheme()
		{
			pnlMenu.BackColor = menuBackColor;
			pnlHeader.BackColor = headerBackColor;
			pnlMain.BackColor = mainBackColor;
			BackColor = mainBackColor;

			label2.ForeColor = headerTextColor;
			lblName.ForeColor = headerTextColor;
			lblRole.ForeColor = Color.FromArgb(191, 219, 254);
			btnSetting.BackColor = headerBackColor;
			btnSetting.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 130, 246);
			btnSetting.FlatAppearance.MouseDownBackColor = Color.FromArgb(59, 130, 246);

			panel1.BackColor = menuCardColor;
			panel1.BorderStyle = BorderStyle.FixedSingle;

			label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			label1.ForeColor = menuMutedTextColor;
			label3.ForeColor = menuMutedTextColor;
			label5.ForeColor = menuMutedTextColor;

			panel3.BackColor = borderColor;
			panel4.BackColor = borderColor;

			StyleSectionIcon(guna2PictureBox1);
			StyleSectionIcon(guna2PictureBox2);
			StyleSectionIcon(guna2PictureBox3);

			foreach (var btn in sidebarButtons)
			{
				btn.FlatStyle = FlatStyle.Flat;
				btn.FlatAppearance.BorderSize = 0;
				btn.FlatAppearance.MouseDownBackColor = menuItemHoverColor;
				btn.FlatAppearance.MouseOverBackColor = menuItemHoverColor;
				btn.BackColor = menuItemColor;
				btn.ForeColor = menuTextColor;
				btn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Regular);
				btn.Padding = new Padding(36, 0, 0, 0);
				btn.TextAlign = ContentAlignment.MiddleLeft;
				btn.Cursor = Cursors.Hand;
				btn.UseVisualStyleBackColor = false;
			}

			if (sidebarIndicator == null)
			{
				sidebarIndicator = new Panel
				{
					Size = new Size(4, 32),
					BackColor = menuAccentColor,
					Visible = false
				};
				pnlMenu.Controls.Add(sidebarIndicator);
			}
		}

		private void StyleSectionIcon(Control icon)
		{
			icon.BackColor = Color.Transparent;
		}
    }
}
