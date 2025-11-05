using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class XeDAO
	{
		private BXMDEntities  db = new BXMDEntities();
		public List<Xe> GetAll()
		{
			return db.Xes.ToList();
		}
		public Xe GetById(int id)
		{
			return db.Xes.Find(id);
		}
		public void Add(Xe xe)
		{
			db.Xes.Add(xe);
			db.SaveChanges();
		}
		public void Delete(int id) { 
			var xe = db.Xes.Find(id);
			if (xe != null)
			{
				db.Xes.Remove(xe);
				db.SaveChanges();
			}
		}

        public bool ExistsBienSo(string bienSoXe)
        {
            return db.Xes.Any(x => x.BienSoXe == bienSoXe);
        }

        public bool ExistsBienSo(string bienSoXe, int excludeMaXe)
        {
            return db.Xes.Any(x => x.BienSoXe == bienSoXe && x.MaXe != excludeMaXe);
        }
        public void Update(Xe xe)
        {
            var existingXe = db.Xes.Find(xe.MaXe);
            if (existingXe != null)
            {
                existingXe.BienSoXe = xe.BienSoXe;
                existingXe.MaNX = xe.MaNX;
                existingXe.MaLX = xe.MaLX;
                existingXe.TrangThai = xe.TrangThai;
                // Update other properties as needed
                db.SaveChanges();
            }
        }
    }
}
