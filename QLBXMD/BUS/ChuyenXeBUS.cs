using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class ChuyenXeBUS
	{
		private DAO.ChuyenXeDAO chuyenXeDAO = new DAO.ChuyenXeDAO();

		public string Add(ChuyenXe cx)
		{
			if (cx.MaXe == null || cx.MaTuyen == null || cx.MaTX == null)
			{
				return "Vui lòng chọn xe, tuyến đường và tài xế.";
			}

			if (cx.NgayGioKhoiHanh < DateTime.Now)
			{
				return "Ngày giờ khởi hành phải sau thời điểm hiện tại.";
			}

			if (cx.NgayGioDenDuKien != null && cx.NgayGioDenDuKien <= cx.NgayGioKhoiHanh)
			{
				return "Ngày giờ đến dự kiến phải sau ngày giờ khởi hành.";
			}

			if (cx.GiaVeThucTe == null || cx.GiaVeThucTe <= 0)
			{
				return "Giá vé phải lớn hơn 0.";
			}

			// Prevent same vehicle having a trip at the same minute
			try
			{
				using (var db = new BXMDEntities())
				{
					var target = cx.NgayGioKhoiHanh;
					var start = new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, 0);
					var end = start.AddMinutes(1);

					bool exists = db.ChuyenXes.Any(c => c.MaXe == cx.MaXe && c.NgayGioKhoiHanh >= start && c.NgayGioKhoiHanh < end);
					if (exists)
					{
						return "Xe này đã được lên chuyến vào cùng ngày giờ. Vui lòng chọn xe khác hoặc thay đổi thời gian khởi hành.";
					}
				}
			}
			catch (Exception ex)
			{
				// If DB check fails, return error
				return "Kiểm tra chuyến bị lỗi: " + ex.Message;
			}

			try
			{
				chuyenXeDAO.Add(cx);
				return "Thêm chuyến xe thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm chuyến xe thất bại: " + ex.Message;
			}
		}

		public string Update(ChuyenXe cx)
		{
			if (cx.MaXe == null || cx.MaTuyen == null || cx.MaTX == null)
			{
				return "Vui lòng chọn xe, tuyến đường và tài xế.";
			}

			if (cx.NgayGioDenDuKien != null && cx.NgayGioDenDuKien <= cx.NgayGioKhoiHanh)
			{
				return "Ngày giờ đến dự kiến phải sau ngày giờ khởi hành.";
			}

			if (cx.GiaVeThucTe == null || cx.GiaVeThucTe <= 0)
			{
				return "Giá vé phải lớn hơn 0.";
			}

			// Prevent updating to a datetime that conflicts with another trip for same vehicle (same minute)
			try
			{
				using (var db = new BXMDEntities())
				{
					var target = cx.NgayGioKhoiHanh;
					var start = new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, 0);
					var end = start.AddMinutes(1);

					bool conflict = db.ChuyenXes.Any(c => c.MaXe == cx.MaXe && c.NgayGioKhoiHanh >= start && c.NgayGioKhoiHanh < end && c.MaChuyen != cx.MaChuyen);
					if (conflict)
					{
						return "Không thể cập nhật: cùng xe đã có chuyến vào cùng ngày giờ khác. Vui lòng chọn thời gian khác.";
					}
				}
			}
			catch (Exception ex)
			{
				return "Kiểm tra xung đột chuyến bị lỗi: " + ex.Message;
			}

			try
			{
				chuyenXeDAO.Update(cx);
				return "Cập nhật chuyến xe thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật chuyến xe thất bại: " + ex.Message;
			}
		}

		public string Delete(int maChuyen)
		{
			try
			{
				var chuyenXe = chuyenXeDAO.GetById(maChuyen);
				if (chuyenXe == null)
				{
					return "Chuyến xe không tồn tại.";
				}

				// Kiểm tra chuyến xe có vé không
				if (chuyenXe.VeXes.Count > 0)
				{
					return "Không thể xóa chuyến xe đã có vé.";
				}

				chuyenXeDAO.Delete(maChuyen);
				return "Xóa chuyến xe thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa chuyến xe thất bại: " + ex.Message;
			}
		}

		public List<ChuyenXe> GetAll()
		{
			return chuyenXeDAO.GetAll();
		}

		public ChuyenXe GetById(int id)
		{
			return chuyenXeDAO.GetById(id);
		}
	}
}