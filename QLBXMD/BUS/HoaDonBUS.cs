using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class HoaDonBUS
	{
		private DAO.HoaDonDAO hoaDonDAO = new DAO.HoaDonDAO();

		public string Add(HoaDon hd)
		{
			if (hd.MaKH == null && hd.MANV == null)
			{
				return "Vui lòng chọn khách hàng hoặc nhân viên.";
			}

			if (hd.TongTien == null || hd.TongTien <= 0)
			{
				return "Tổng tiền phải lớn hơn 0.";
			}

			try
			{
				hoaDonDAO.Add(hd);
				return "Thêm hóa đơn thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm hóa đơn thất bại: " + ex.Message;
			}
		}

		public string Update(HoaDon hd)
		{
			if (hd.TongTien == null || hd.TongTien <= 0)
			{
				return "Tổng tiền phải lớn hơn 0.";
			}

			try
			{
				hoaDonDAO.Update(hd);
				return "Cập nhật hóa đơn thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật hóa đơn thất bại: " + ex.Message;
			}
		}

		public string Delete(int maHD)
		{
			try
			{
				var hoaDon = hoaDonDAO.GetById(maHD);
				if (hoaDon == null)
				{
					return "Hóa đơn không tồn tại.";
				}

				// Kiểm tra hóa đơn có thanh toán hoặc vé xe không
				//if (hoaDon.ThanhToans.Count > 0)
				//{
				//	return "Không thể xóa hóa đơn đã có thanh toán.";
				//}
				//if (hoaDon.VeXes.Count > 0)
				//{
				//	return "Không thể xóa hóa đơn đã có vé xe.";
				//}

				hoaDonDAO.Delete(maHD);
				return "Xóa hóa đơn thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa hóa đơn thất bại: " + ex.Message;
			}
		}

		public List<HoaDon> GetAll()
		{
			return hoaDonDAO.GetAll();
		}

		public HoaDon GetById(int id)
		{
			return hoaDonDAO.GetById(id);
		}

		public List<HoaDon> GetByKhachHang(int maKH)
		{
			return hoaDonDAO.GetByKhachHang(maKH);
		}

		public List<HoaDon> GetByNhanVien(int maNV)
		{
			return hoaDonDAO.GetByNhanVien(maNV);
		}
	}
}