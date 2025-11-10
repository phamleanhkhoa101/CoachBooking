using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI.User
{
	public partial class FrmMainUser : Form
	{
		private TaiKhoan taiKhoan;
		private UCDatVeUser ucDatVe;
		private UCLichSuMuaVe ucLichSu;
		private UCThongTinUser ucThongTin;

		// Màu gốc của các nút
		private Color menuBackColor = Color.FromArgb(33, 37, 41);
		private Color activeButtonColor = Color.FromArgb(0, 150, 136);
		private Color hoverButtonColor = Color.FromArgb(52, 58, 64);
		private Color clickButtonColor = Color.FromArgb(0, 120, 110);
		private Color logoutHoverColor = Color.FromArgb(244, 67, 54);
		private Color logoutClickColor = Color.FromArgb(200, 50, 40);

		// Lưu trữ nút đang được chọn
		private Button currentActiveButton;

		public FrmMainUser()
		{
			InitializeComponent();
			this.Load += FrmMainUser_Load;
			SetupButtonEffects();
		}

		public FrmMainUser(TaiKhoan taiKhoan) : this()
		{
			this.taiKhoan = taiKhoan;
		}

		private void SetupButtonEffects()
		{
			// Thiết lập hiệu ứng cho nút Đặt vé
			btnDatVe.MouseEnter += (s, e) => {
				if (currentActiveButton != btnDatVe)
					btnDatVe.BackColor = hoverButtonColor;
			};
			btnDatVe.MouseLeave += (s, e) => {
				if (currentActiveButton != btnDatVe)
					btnDatVe.BackColor = menuBackColor;
			};
			btnDatVe.MouseDown += (s, e) => {
				if (currentActiveButton == btnDatVe)
					btnDatVe.BackColor = clickButtonColor;
				else
					btnDatVe.BackColor = hoverButtonColor;
			};
			btnDatVe.MouseUp += (s, e) => {
				if (currentActiveButton == btnDatVe)
					btnDatVe.BackColor = activeButtonColor;
				else
					btnDatVe.BackColor = hoverButtonColor;
			};

			// Thiết lập hiệu ứng cho nút Lịch sử
			btnLichSu.MouseEnter += (s, e) => {
				if (currentActiveButton != btnLichSu)
					btnLichSu.BackColor = hoverButtonColor;
			};
			btnLichSu.MouseLeave += (s, e) => {
				if (currentActiveButton != btnLichSu)
					btnLichSu.BackColor = menuBackColor;
			};
			btnLichSu.MouseDown += (s, e) => {
				if (currentActiveButton == btnLichSu)
					btnLichSu.BackColor = clickButtonColor;
				else
					btnLichSu.BackColor = hoverButtonColor;
			};
			btnLichSu.MouseUp += (s, e) => {
				if (currentActiveButton == btnLichSu)
					btnLichSu.BackColor = activeButtonColor;
				else
					btnLichSu.BackColor = hoverButtonColor;
			};

			// Thiết lập hiệu ứng cho nút Thông tin
			btnThongTin.MouseEnter += (s, e) => {
				if (currentActiveButton != btnThongTin)
					btnThongTin.BackColor = hoverButtonColor;
			};
			btnThongTin.MouseLeave += (s, e) => {
				if (currentActiveButton != btnThongTin)
					btnThongTin.BackColor = menuBackColor;
			};
			btnThongTin.MouseDown += (s, e) => {
				if (currentActiveButton == btnThongTin)
					btnThongTin.BackColor = clickButtonColor;
				else
					btnThongTin.BackColor = hoverButtonColor;
			};
			btnThongTin.MouseUp += (s, e) => {
				if (currentActiveButton == btnThongTin)
					btnThongTin.BackColor = activeButtonColor;
				else
					btnThongTin.BackColor = hoverButtonColor;
			};

			// Thiết lập hiệu ứng cho nút Đăng xuất
			btnDangXuat.MouseEnter += (s, e) => {
				btnDangXuat.BackColor = logoutHoverColor;
			};
			btnDangXuat.MouseLeave += (s, e) => {
				btnDangXuat.BackColor = menuBackColor;
			};
			btnDangXuat.MouseDown += (s, e) => {
				btnDangXuat.BackColor = logoutClickColor;
			};
			btnDangXuat.MouseUp += (s, e) => {
				btnDangXuat.BackColor = logoutHoverColor;
			};
		}

		private void SetActiveButton(Button activeButton)
		{
			// Reset tất cả các nút về màu mặc định
			if (currentActiveButton != null)
			{
				currentActiveButton.BackColor = menuBackColor;
				currentActiveButton.Font = new Font("Segoe UI", 11F);
			}

			// Đặt nút hiện tại là active
			currentActiveButton = activeButton;
			currentActiveButton.BackColor = activeButtonColor;
			currentActiveButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
		}

		private void FrmMainUser_Load(object sender, EventArgs e)
		{
			// Load booking interface by default
			LoadDatVe();
		}

		private void btnDatVe_Click(object sender, EventArgs e)
		{
			LoadDatVe();
		}

		private void btnLichSu_Click(object sender, EventArgs e)
		{
			LoadLichSu();
		}

		private void btnThongTin_Click(object sender, EventArgs e)
		{
			LoadThongTin();
		}

		private void LoadDatVe()
		{
			SetActiveButton(btnDatVe);
			pnlContent.Controls.Clear();
			if (ucDatVe == null)
			{
				ucDatVe = new UCDatVeUser(taiKhoan);
				ucDatVe.Dock = DockStyle.Fill;
			}
			pnlContent.Controls.Add(ucDatVe);
		}

		private void LoadLichSu()
		{
			SetActiveButton(btnLichSu);
			pnlContent.Controls.Clear();
			// Always create new instance to refresh data
			ucLichSu = new UCLichSuMuaVe(taiKhoan);
			ucLichSu.Dock = DockStyle.Fill;
			pnlContent.Controls.Add(ucLichSu);
		}

		private void LoadThongTin()
		{
			SetActiveButton(btnThongTin);
			pnlContent.Controls.Clear();
			// Always create new instance to refresh data
			ucThongTin = new UCThongTinUser(taiKhoan);
			ucThongTin.Dock = DockStyle.Fill;
			pnlContent.Controls.Add(ucThongTin);
		}

		private void btnDangXuat_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			
			if (result == DialogResult.Yes)
			{
				this.Close();
			}
		}
	}
}
