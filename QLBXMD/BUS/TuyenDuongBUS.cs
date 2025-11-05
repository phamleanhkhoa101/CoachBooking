using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class TuyenDuongBUS
	{
		private DAO.TuyenDuongDAO tuyenDuongDAO = new DAO.TuyenDuongDAO();

		public string Add(TuyenDuong td)
		{
			if (string.IsNullOrEmpty(td.DiemDi) || string.IsNullOrEmpty(td.DiemDen))
			{
				return "Vui lòng nhập điểm khởi hành và điểm đến.";
			}

			if (td.QuangDuong == null || td.QuangDuong <= 0)
			{
				return "Khoảng cách phải lớn hơn 0.";
			}

			try
			{
				tuyenDuongDAO.Add(td);
				return "Thêm tuyến đường thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm tuyến đường thất bại: " + ex.Message;
			}
		}

		public string Update(TuyenDuong td)
		{
			if (string.IsNullOrEmpty(td.DiemDi) || string.IsNullOrEmpty(td.DiemDen))
			{
				return "Vui lòng nhập điểm khởi hành và điểm đến.";
			}

			if (td.QuangDuong == null || td.QuangDuong <= 0)
			{
				return "Khoảng cách phải lớn hơn 0.";
			}

			try
			{
				tuyenDuongDAO.Update(td);
				return "Cập nhật tuyến đường thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật tuyến đường thất bại: " + ex.Message;
			}
		}

		public string Delete(int maTuyen)
		{
			try
			{
				var tuyenDuong = tuyenDuongDAO.GetById(maTuyen);
				if (tuyenDuong == null)
				{
					return "Tuyến đường không tồn tại.";
				}

				// Kiểm tra tuyến đường có chuyến xe không
				if (tuyenDuong.ChuyenXes.Count > 0)
				{
					return "Không thể xóa tuyến đường đang có chuyến xe.";
				}

				// Kiểm tra tuyến đường có giá vé không
				if (tuyenDuong.GiaVes.Count > 0)
				{
					return "Không thể xóa tuyến đường đang có giá vé.";
				}

				tuyenDuongDAO.Delete(maTuyen);
				return "Xóa tuyến đường thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa tuyến đường thất bại: " + ex.Message;
			}
		}

		public List<TuyenDuong> GetAll()
		{
			return tuyenDuongDAO.GetAll();
		}

		public TuyenDuong GetById(int id)
		{
			return tuyenDuongDAO.GetById(id);
		}
	}
}