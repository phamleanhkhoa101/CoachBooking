using QLBXMD.EF;
using QLBXMD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	internal class NhaXeBUS
	{
		private DAO.NhaXeDAO nhaXeDAO = new DAO.NhaXeDAO();

		public string Update(NhaXe nx)
		{
			if (string.IsNullOrEmpty(nx.TenNX) || string.IsNullOrEmpty(nx.SDT) || string.IsNullOrEmpty(nx.Email))
			{
				return "Vui lòng nhập đủ thông tin.";
			}
			
			// Kiểm tra trùng email và SDT, loại trừ chính bản ghi đang sửa
			if (nhaXeDAO.ExistsEmail(nx.Email, nx.MaNX)) return "Email đã tồn tại.";
			if (nhaXeDAO.ExistsSDT(nx.SDT, nx.MaNX)) return "Số điện thoại đã tồn tại.";

			if (nhaXeDAO.Update(nx))
				return "Cập nhật nhà xe thành công.";
			else
				return "Cập nhật nhà xe thất bại.";

		}

        public string Delete(int maNX)
        {
            try
            {
                var nhaXe = nhaXeDAO.GetById(maNX);
                if (nhaXe == null)
                {
                    return "Nhà xe không tồn tại.";
                }

                // Kiểm tra xem nhà xe có đang được sử dụng không (có xe hoặc giá vé liên quan)
                if (nhaXe.Xes.Count > 0)
                {
                    return "Không thể xóa nhà xe đang có xe.";
                }
                if (nhaXe.GiaVes.Count > 0)
                {
                    return "Không thể xóa nhà xe đang có giá vé.";
                }

                nhaXeDAO.Delete(maNX);
                return "Xóa nhà xe thành công.";
            }
            catch (Exception ex)
            {
                return "Xóa nhà xe thất bại: " + ex.Message;
            }
        }

        public string Add(NhaXe nx)
        {
            if (string.IsNullOrEmpty(nx.TenNX) || string.IsNullOrEmpty(nx.SDT) || string.IsNullOrEmpty(nx.Email))
            {
                return "Vui lòng nhập đủ thông tin.";
            }

            // Kiểm tra trùng email và SDT
            if (nhaXeDAO.ExistsEmail(nx.Email)) return "Email đã tồn tại.";
            if (nhaXeDAO.ExistsSDT(nx.SDT)) return "Số điện thoại đã tồn tại.";

            if (nhaXeDAO.Add(nx))
                return "Thêm nhà xe thành công.";
            else
                return "Thêm nhà xe thất bại.";
        }
	}
}
