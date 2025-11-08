using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class XacNhanDatVeDialog : Form
	{
		public string PhuongThucThanhToan { get; private set; } = "Tiền mặt";

		// Thông tin đặt vé
		public string TenKhachHang { get; set; }
		public string SoDienThoai { get; set; }
		public string GioiTinh { get; set; }
		public DateTime NamSinh { get; set; }
		public string DiemDi { get; set; }
		public string DiemDen { get; set; }
		public DateTime NgayKhoiHanh { get; set; }
		public string GheDaChon { get; set; }
		public decimal GiaVe { get; set; }
		public int SoLuongGhe { get; set; }
		public decimal TongTien { get; set; }

		public XacNhanDatVeDialog()
		{
			InitializeComponent();
			this.Load += XacNhanDatVeDialog_Load;
		}

		private void XacNhanDatVeDialog_Load(object sender, EventArgs e)
		{
			// Hiển thị thông tin
			lblTenKH.Text = TenKhachHang;
			lblSDT.Text = SoDienThoai;
			lblGioiTinh.Text = GioiTinh;
			lblNamSinh.Text = NamSinh.ToString("dd/MM/yyyy");
			lblTuyen.Text = $"{DiemDi} → {DiemDen}";
			lblNgayDi.Text = NgayKhoiHanh.ToString("dd/MM/yyyy HH:mm");
			lblGheDaChon.Text = GheDaChon;
			lblGiaVe.Text = $"{GiaVe:#,##0} VNĐ";
			lblSoLuongGhe.Text = SoLuongGhe.ToString();
			lblTongTien.Text = $"{TongTien:#,##0} VNĐ";

			// Mặc định chọn Tiền mặt
			rdoTienMat.Checked = true;
			UpdatePanelStyles();
		}

		private void rdoTienMat_CheckedChanged(object sender, EventArgs e)
		{
			if (rdoTienMat.Checked)
			{
				PhuongThucThanhToan = "Tiền mặt";
				UpdatePanelStyles();
			}
		}

		private void rdoMoMo_CheckedChanged(object sender, EventArgs e)
		{
			if (rdoMoMo.Checked)
			{
				PhuongThucThanhToan = "MoMo";
				UpdatePanelStyles();
			}
		}

		private void pnlTienMat_Click(object sender, EventArgs e)
		{
			rdoTienMat.Checked = true;
		}

		private void pnlMoMo_Click(object sender, EventArgs e)
		{
			rdoMoMo.Checked = true;
		}

		private void UpdatePanelStyles()
		{
			// Cập nhật màu border cho panel được chọn
			if (rdoTienMat.Checked)
			{
				pnlTienMat.BackColor = Color.FromArgb(230, 244, 255); // Light blue
				pnlMoMo.BackColor = Color.White;
			}
			else
			{
				pnlTienMat.BackColor = Color.White;
				pnlMoMo.BackColor = Color.FromArgb(255, 240, 250); // Light pink
			}
		}

		private void btnXacNhan_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
