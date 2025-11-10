using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLBXMD.GUI.User
{
	public partial class XacNhanDatVeUserDialog : Form
	{
		public string PhuongThucThanhToan { get; private set; } = "MoMo"; // Mặc định MoMo

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

		public XacNhanDatVeUserDialog()
		{
			InitializeComponent();
			this.Load += XacNhanDatVeUserDialog_Load;
		}

		private void XacNhanDatVeUserDialog_Load(object sender, EventArgs e)
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
