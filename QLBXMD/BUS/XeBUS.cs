using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class XeBUS
	{
		private DAO.XeDAO xeDAO = new DAO.XeDAO();

		public string Add(Xe xe)
		{
			if (string.IsNullOrEmpty(xe.BienSoXe))
			{
				return "Vui lòng nhập biển số xe.";
			}

			if (xe.MaLX == null || xe.MaNX == null)
			{
				return "Vui lòng chọn loại xe và nhà xe.";
			}

			// Kiểm tra trùng biển số
			if (xeDAO.ExistsBienSo(xe.BienSoXe))
			{
				return "Biển số xe đã tồn tại.";
			}

			try
			{
				xeDAO.Add(xe);
				return "Thêm xe thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm xe thất bại: " + ex.Message;
			}
		}

		public string Update(Xe xe)
		{
			if (string.IsNullOrEmpty(xe.BienSoXe))
			{
				return "Vui lòng nhập biển số xe.";
			}

			if (xe.MaLX == null || xe.MaNX == null)
			{
				return "Vui lòng chọn loại xe và nhà xe.";
			}

			// Kiểm tra trùng biển số (loại trừ chính nó)
			if (xeDAO.ExistsBienSo(xe.BienSoXe, xe.MaXe))
			{
				return "Biển số xe đã tồn tại.";
			}

			try
			{
				xeDAO.Update(xe);
				return "Cập nhật xe thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật xe thất bại: " + ex.Message;
			}
		}

		public string Delete(int maXe)
		{
			try
			{
				var xe = xeDAO.GetById(maXe);
				if (xe == null)
				{
					return "Xe không tồn tại.";
				}

				// Kiểm tra xe có chuyến xe không
				if (xe.ChuyenXes.Count > 0)
				{
					return "Không thể xóa xe đang có chuyến xe.";
				}

				xeDAO.Delete(maXe);
				return "Xóa xe thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa xe thất bại: " + ex.Message;
			}
		}

		public List<Xe> GetAll()
		{
			return xeDAO.GetAll();
		}

		public Xe GetById(int id)
		{
			return xeDAO.GetById(id);
		}
	}
}