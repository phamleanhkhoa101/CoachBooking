using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class TuyenDuongDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.TuyenDuong> GetAll()
		{
			return db.TuyenDuongs.ToList();
		}
		public EF.TuyenDuong GetById(int id)
		{
			return db.TuyenDuongs.Find(id);
		}
		public void Add(EF.TuyenDuong td)
		{
			db.TuyenDuongs.Add(td);
			db.SaveChanges();
		}
		public void Update(EF.TuyenDuong td)
		{
			var existingTD = db.TuyenDuongs.Find(td.MaTuyen);
			if (existingTD != null)
			{
				existingTD.DiemDi = td.DiemDi;
				existingTD.DiemDen = td.DiemDen;
				existingTD.QuangDuong = td.QuangDuong;
				existingTD.TrangThai = td.TrangThai;
				existingTD.ThoiGianDuKien = td.ThoiGianDuKien;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var td = db.TuyenDuongs.Find(id);
			if (td != null)
			{
				db.TuyenDuongs.Remove(td);
				db.SaveChanges();
			}
		}
	}
}
