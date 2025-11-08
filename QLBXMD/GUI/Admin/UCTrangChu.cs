using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCTrangChu : UserControl
	{
		public UCTrangChu()
		{
			InitializeComponent();
		}

		private void UCTrangChu_Load(object sender, EventArgs e)
		{
			LoadStatistics();
		}

		private void LoadStatistics()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// Đếm số khách hàng duy nhất
					int soKhachHang = db.KhachHangs.Count();
					lblKhachHangValue.Text = soKhachHang.ToString();

					// Đếm số chuyến xe
					int soChuyenXe = db.ChuyenXes.Count();
					lblSoChuyenXeValue.Text = soChuyenXe.ToString();

					// Tính tổng doanh thu trong tháng hiện tại
					DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
					DateTime endOfMonth = startOfMonth.AddMonths(1);
					
					decimal tongDoanhThu = db.HoaDons
						.Where(h => h.NgayLap >= startOfMonth && h.NgayLap < endOfMonth)
						.Sum(h => (decimal?)h.TongTien) ?? 0;
					
					lblTongDoanhThuValue.Text = tongDoanhThu.ToString("#,##0") + " VNĐ";

					// Đếm số vé bán trong tháng (đếm từ VeXe collection của các HoaDon trong tháng)
					int soVe = db.HoaDons
						.Where(h => h.NgayLap >= startOfMonth && h.NgayLap < endOfMonth)
						.SelectMany(h => h.VeXes)
						.Count();
					lblSoVeValue.Text = soVe.ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi tải thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
