using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class KhachHangBUS
	{
		private DAO.KhachHangDAO khachHangDAO = new DAO.KhachHangDAO();

		public string Add(KhachHang kh)
		{
			if (string.IsNullOrEmpty(kh.TenKH) || string.IsNullOrEmpty(kh.SDT))
			{
				return "Vui lòng nhập đủ thông tin.";
			}

			if (kh.NamSinh.Year < 1900 || kh.NamSinh > DateTime.Now)
			{
				return "Năm sinh không hợp lệ.";
			}

			// Kiểm tra trùng số điện thoại
			if (khachHangDAO.ExistsSDT(kh.SDT))
			{
				return "Số điện thoại đã tồn tại.";
			}

			try
			{
				khachHangDAO.Add(kh);
				return "Thêm khách hàng thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm khách hàng thất bại: " + ex.Message;
			}
		}

		public string Update(KhachHang kh)
		{
			if (string.IsNullOrEmpty(kh.TenKH) || string.IsNullOrEmpty(kh.SDT))
			{
				return "Vui lòng nhập đủ thông tin.";
			}

			if (kh.NamSinh.Year < 1900 || kh.NamSinh > DateTime.Now)
			{
				return "Năm sinh không hợp lệ.";
			}

			// Kiểm tra trùng số điện thoại (loại trừ chính nó)
			if (khachHangDAO.ExistsSDT(kh.SDT, kh.MaKH))
			{
				return "Số điện thoại đã tồn tại.";
			}

			try
			{
				khachHangDAO.Update(kh);
				return "Cập nhật khách hàng thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật khách hàng thất bại: " + ex.Message;
			}
		}

		public string Delete(int maKH)
		{
			try
			{
				var khachHang = khachHangDAO.GetById(maKH);
				if (khachHang == null)
				{
					return "Khách hàng không tồn tại.";
				}

				// Kiểm tra khách hàng có hóa đơn không
				if (khachHang.HoaDons.Count > 0)
				{
					return "Không thể xóa khách hàng đã có hóa đơn.";
				}

				khachHangDAO.Delete(maKH);
				return "Xóa khách hàng thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa khách hàng thất bại: " + ex.Message;
			}
		}

		public List<KhachHang> GetAll()
		{
			return khachHangDAO.GetAll();
		}

		public KhachHang GetById(int id)
		{
			return khachHangDAO.GetById(id);
		}
	}
}