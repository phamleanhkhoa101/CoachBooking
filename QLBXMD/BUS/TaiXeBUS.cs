using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class TaiXeBUS
	{
		private DAO.TaiXeDAO taiXeDAO = new DAO.TaiXeDAO();

		public string Add(TaiXe tx)
		{
			if (string.IsNullOrEmpty(tx.TenTX) || string.IsNullOrEmpty(tx.SDT))
			{
				return "Vui lòng nhập đủ thông tin.";
			}

			if (tx.NamSinh.Year < 1900 || tx.NamSinh > DateTime.Now.AddYears(-21))
			{
				return "Tài xế phải từ 21 tuổi trở lên.";
			}

			// Kiểm tra trùng GPLX
			//if (taiXeDAO.ExistsGPLX(tx.GPLX))
			//{
			//	return "Giấy phép lái xe đã tồn tại.";
			//}

			try
			{
				taiXeDAO.Add(tx);
				return "Thêm tài xế thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm tài xế thất bại: " + ex.Message;
			}
		}

		public string Update(TaiXe tx)
		{
			if (string.IsNullOrEmpty(tx.TenTX) || string.IsNullOrEmpty(tx.SDT))
			{
				return "Vui lòng nhập đủ thông tin.";
			}

			if (tx.NamSinh.Year < 1900 || tx.NamSinh > DateTime.Now.AddYears(-21))
			{
				return "Tài xế phải từ 21 tuổi trở lên.";
			}

			// Kiểm tra trùng GPLX (loại trừ chính nó)
			//if (taiXeDAO.ExistsGPLX(tx.GPLX, tx.MaTX))
			//{
			//	return "Giấy phép lái xe đã tồn tại.";
			//}

			try
			{
				taiXeDAO.Update(tx);
				return "Cập nhật tài xế thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật tài xế thất bại: " + ex.Message;
			}
		}

		public string Delete(int maTX)
		{
			try
			{
				var taiXe = taiXeDAO.GetById(maTX);
				if (taiXe == null)
				{
					return "Tài xế không tồn tại.";
				}

				// Kiểm tra tài xế có chuyến xe không
				if (taiXe.ChuyenXes.Count > 0)
				{
					return "Không thể xóa tài xế đang có chuyến xe.";
				}

				taiXeDAO.Delete(maTX);
				return "Xóa tài xế thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa tài xế thất bại: " + ex.Message;
			}
		}

		public List<TaiXe> GetAll()
		{
			return taiXeDAO.GetAll();
		}

		public TaiXe GetById(int id)
		{
			return taiXeDAO.GetById(id);
		}
	}
}