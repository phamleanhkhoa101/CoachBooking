using QLBXMD.BUS;
using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq; // add
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class FrmMainAdmin : Form
	{
		private TaiKhoan taiKhoan;
		private List<Button> sidebarButtons;
		private NhanVien nhanVien;
		private TaiKhoanBUS taiKhoanBus = new TaiKhoanBUS();

		// Collapsible groups: Chung, DanhMuc, GiaoDich, HeThong
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
			sidebarButtons = new List<Button> { btnTrangChu, btnDatVe, btnNhaXe, btnLoaiXe, btnXe, btnTuyenDuong, btnChuyenXe, btnGiaVe, btnTaiXe, btnHoaDon };
			//lblName.Text = taiKhoan.
			lblRole.Text = taiKhoan.Quyen.TenQuyen;

			// Capture original positions of all controls in the menu for deterministic reflow
			_origTop.Clear();
			foreach (Control c in pnlMenu.Controls)
			{
				_origTop[c] = c.Top;
			}

			// Build collapsible groups and wire click events
			BuildGroups();
			label1.Click += label1_Click; // Chung
			label4.Click += label4_Click; // Giao dịch
			// label3 click is already wired in designer
			label5.Click += label5_Click; // Hệ thống
			btnTrangChu.Click += btnTrangChu_Click; // Home
			btnGiaVe.Click += btnGiaVe_Click; // Giá vé
			btnTaiXe.Click += btnTaiXe_Click; // Tài xế
			btnHoaDon.Click += btnHoaDon_Click; // Hóa đơn

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
				var allowedForStaff = new HashSet<Button> { btnTrangChu, btnDatVe };
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
			pnlMain.Controls.Add(control);
			control.BringToFront();
		}
		private void HightlightButton(Button selectedButton)
		{
			foreach (var btn in sidebarButtons)
			{
				btn.BackColor = System.Drawing.Color.White;
			}
			selectedButton.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
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
			// Simple home action: clear content and highlight button
			pnlMain.Controls.Clear();
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
		private void label4_Click(object sender, EventArgs e) // Giao dịch
		{
			ToggleGroup("GiaoDich");
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
				new[] { "btnTrangChu", "btnDatVe" }
			);

			AddGroup(
				"DanhMuc",
				new[] { "btnNhaXe", "btnLoaiXe", "btnXe", "btnTuyenDuong", "btnChuyenXe", "btnGiaVe", "btnTaiXe" }
			);

			AddGroup(
				"GiaoDich",
				new[] { "btnHoaDon", "btnVe", "button1" } // include legacy 'button1' if present
			);

			AddGroup(
				"HeThong",
				new[] { "button4", "button3", "button2" } // Tài khoản, Phân quyền, Thống kê
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
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var ucThongTin = new UCThongTin();
			ucThongTin.SetData(taiKhoan, nhanVien);
			ShowControl(ucThongTin);
			//HightlightButton(btn);
		}
	}
}
