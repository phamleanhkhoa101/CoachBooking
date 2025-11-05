using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class ChuyenXeDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.ChuyenXe> GetAll()
		{
			return db.ChuyenXes.ToList();
		}
		public EF.ChuyenXe GetById(int id)
		{
			return db.ChuyenXes.Find(id);
		}
		public void Add(EF.ChuyenXe cx)
		{
			db.ChuyenXes.Add(cx);
			db.SaveChanges();
		}
		public void Update(EF.ChuyenXe cx)
		{
			var existingCX = db.ChuyenXes.Find(cx.MaChuyen);
			if (existingCX != null)
			{
				existingCX.MaXe = cx.MaXe;
				existingCX.MaTuyen = cx.MaTuyen;
				existingCX.MaTX = cx.MaTX;
				existingCX.NgayGioKhoiHanh = cx.NgayGioKhoiHanh;
				existingCX.NgayGioDenDuKien = cx.NgayGioDenDuKien;
				existingCX.GiaVeThucTe = cx.GiaVeThucTe;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var cx = db.ChuyenXes.Find(id);
			if (cx != null)
			{
				db.ChuyenXes.Remove(cx);
				db.SaveChanges();
			}
		}
	}
}
