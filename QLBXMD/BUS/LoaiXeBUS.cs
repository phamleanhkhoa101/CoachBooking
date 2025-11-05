using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class LoaiXeBUS
	{
		private DAO.LoaiXeDAO loaiXeDAO = new DAO.LoaiXeDAO();

		public string Add(LoaiXe lx)
		{
			if (string.IsNullOrEmpty(lx.TenLX))
			{
				return "Vui lòng nhập tên loại xe.";
			}

			try
			{
				loaiXeDAO.Add(lx);
				return "Thêm loại xe thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm loại xe thất bại: " + ex.Message;
			}
		}

		public string Update(LoaiXe lx)
		{
			if (string.IsNullOrEmpty(lx.TenLX))
			{
				return "Vui lòng nhập tên loại xe.";
			}

			try
			{
				loaiXeDAO.Update(lx);
				return "Cập nhật loại xe thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật loại xe thất bại: " + ex.Message;
			}
		}

		public string Delete(int maLX)
		{
			try
			{
				var loaiXe = loaiXeDAO.GetById(maLX);
				if (loaiXe == null)
				{
					return "Loại xe không tồn tại.";
				}

				// Kiểm tra xem loại xe có đang được sử dụng không
				if (loaiXe.Xes.Count > 0)
				{
					return "Không thể xóa loại xe đang có xe.";
				}
				if (loaiXe.GiaVes.Count > 0)
				{
					return "Không thể xóa loại xe đang có giá vé.";
				}

				loaiXeDAO.Delete(maLX);
				return "Xóa loại xe thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa loại xe thất bại: " + ex.Message;
			}
		}
	}
}