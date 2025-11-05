using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class GiaVeBUS
	{
		private DAO.GiaVeDAO giaVeDAO = new DAO.GiaVeDAO();

		public string Add(GiaVe gv)
		{
			if (gv.GiaVe1 <= 0)
			{
				return "Giá vé phải lớn hơn 0.";
			}

			// Kiểm tra giá vé đã tồn tại chưa
			if (giaVeDAO.Exists(gv.MaNX, gv.MaLX, gv.MaTuyen))
			{
				return "Giá vé cho nhà xe, loại xe và tuyến đường này đã tồn tại.";
			}

			try
			{
				giaVeDAO.Add(gv);
				return "Thêm giá vé thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm giá vé thất bại: " + ex.Message;
			}
		}

		public string Update(GiaVe gv)
		{
			if (gv.GiaVe1 <= 0)
			{
				return "Giá vé phải lớn hơn 0.";
			}

			try
			{
				giaVeDAO.Update(gv);
				return "Cập nhật giá vé thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật giá vé thất bại: " + ex.Message;
			}
		}

		public string Delete(int maGV)
		{
			try
			{
				var giaVe = giaVeDAO.GetById(maGV);
				if (giaVe == null)
				{
					return "Giá vé không tồn tại.";
				}

				giaVeDAO.Delete(maGV);
				return "Xóa giá vé thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa giá vé thất bại: " + ex.Message;
			}
		}

		public List<GiaVe> GetAll()
		{
			return giaVeDAO.GetAll();
		}

		public GiaVe GetById(int id)
		{
			return giaVeDAO.GetById(id);
		}

		public GiaVe GetByCondition(int maNX, int maLX, int maTuyen)
		{
			return giaVeDAO.GetByCondition(maNX, maLX, maTuyen);
		}
	}
}