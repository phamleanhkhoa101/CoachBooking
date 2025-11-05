using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class NhanVienBUS
	{
		private DAO.NhanVienDAO nhanVienDAO = new DAO.NhanVienDAO();

		public string Add(NhanVien nv)
		{
			if (string.IsNullOrEmpty(nv.TenNV) || string.IsNullOrEmpty(nv.SDT) || string.IsNullOrEmpty(nv.DiaChi))
			{
				return "Vui lòng nhập đủ thông tin.";
			}

			if (nv.NamSinh.Year < 1900 || nv.NamSinh > DateTime.Now.AddYears(-18))
			{
				return "Nhân viên phải từ 18 tuổi trở lên.";
			}

			try
			{
				nhanVienDAO.Add(nv);
				return "Thêm nhân viên thành công.";
			}
			catch (Exception ex)
			{
				return "Thêm nhân viên thất bại: " + ex.Message;
			}
		}

		public string Update(NhanVien nv)
		{
			if (string.IsNullOrEmpty(nv.TenNV) || string.IsNullOrEmpty(nv.SDT) || string.IsNullOrEmpty(nv.DiaChi))
			{
				return "Vui lòng nhập đủ thông tin.";
			}

			if (nv.NamSinh.Year < 1900 || nv.NamSinh > DateTime.Now.AddYears(-18))
			{
				return "Nhân viên phải từ 18 tuổi trở lên.";
			}

			try
			{
				nhanVienDAO.Update(nv);
				return "Cập nhật nhân viên thành công.";
			}
			catch (Exception ex)
			{
				return "Cập nhật nhân viên thất bại: " + ex.Message;
			}
		}

		public string Delete(int maNV)
		{
			try
			{
				var nhanVien = nhanVienDAO.GetById(maNV);
				if (nhanVien == null)
				{
					return "Nhân viên không tồn tại.";
				}

				// Kiểm tra nhân viên có hóa đơn không
				if (nhanVien.HoaDons.Count > 0)
				{
					return "Không thể xóa nhân viên đã có hóa đơn.";
				}

				nhanVienDAO.Delete(maNV);
				return "Xóa nhân viên thành công.";
			}
			catch (Exception ex)
			{
				return "Xóa nhân viên thất bại: " + ex.Message;
			}
		}

		public List<NhanVien> GetAll()
		{
			return nhanVienDAO.GetAll();
		}

		public NhanVien GetById(int id)
		{
			return nhanVienDAO.GetById(id);
		}
	}
}