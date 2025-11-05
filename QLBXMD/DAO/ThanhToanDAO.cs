using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class ThanhToanDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.ThanhToan> GetAll()
		{
			return db.ThanhToans.ToList();
		}
		public EF.ThanhToan GetById(int id)
		{
			return db.ThanhToans.Find(id);
		}
		public void Add(EF.ThanhToan tt)
		{
			db.ThanhToans.Add(tt);
			db.SaveChanges();
		}
		public void Update(EF.ThanhToan tt)
		{
			var existingTT = db.ThanhToans.Find(tt.MaTT);
			if (existingTT != null)
			{
				existingTT.TongTien = tt.TongTien;
				existingTT.PhuongThuc= tt.PhuongThuc;
				existingTT.NgayTT = tt.NgayTT;
				existingTT.MaHD = tt.MaHD;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var tt = db.ThanhToans.Find(id);
			if (tt != null)
			{
				db.ThanhToans.Remove(tt);
				db.SaveChanges();
			}
		}
	}
}
