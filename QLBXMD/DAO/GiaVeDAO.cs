using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class GiaVeDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.GiaVe> GetAll()
		{
			return db.GiaVes.ToList();
		}
		public EF.GiaVe GetById(int id)
		{
			return db.GiaVes.Find(id);
		}
		public void Add(EF.GiaVe gv)
		{
			db.GiaVes.Add(gv);
			db.SaveChanges();
		}
		public void Update(EF.GiaVe gv)
		{
			var existingGV = db.GiaVes.Find(gv.MaGV);
			if (existingGV != null)
			{
				existingGV.MaNX = gv.MaNX;
				existingGV.MaLX = gv.MaLX;
				existingGV.MaTuyen = gv.MaTuyen;
				existingGV.GiaVe1 = gv.GiaVe1;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var gv = db.GiaVes.Find(id);
			if (gv != null)
			{
				db.GiaVes.Remove(gv);
				db.SaveChanges();
			}
		}
        public bool Exists(int maNX, int maLX, int maTuyen)
        {
            return db.GiaVes.Any(gv => gv.MaNX == maNX && gv.MaLX == maLX && gv.MaTuyen == maTuyen);
        }

        public EF.GiaVe GetByCondition(int maNX, int maLX, int maTuyen)
        {
            return db.GiaVes.FirstOrDefault(gv => gv.MaNX == maNX && gv.MaLX == maLX && gv.MaTuyen == maTuyen);
        }
    }
}
