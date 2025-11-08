using QLBXMD.DAO;
using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	public class QuyenBUS
	{
		private QuyenDAO quyenDAO = new QuyenDAO();

		public List<Quyen> GetAll()
		{
			return quyenDAO.GetAll();
		}

		public Quyen GetById(int maQuyen)
		{
			return quyenDAO.GetById(maQuyen);
		}
	}
}
