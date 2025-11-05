using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class VeXeDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.VeXe> GetAll()
		{
			return db.VeXes.ToList();
		}
		public EF.VeXe GetById(int id)
		{
			return db.VeXes.Find(id);
		}
		public void Add(EF.VeXe vx)
		{
			db.VeXes.Add(vx);
			db.SaveChanges();
		}
		public void Update(EF.VeXe vx)
		{
			var existingVX = db.VeXes.Find(vx.MaVe);
			if (existingVX != null)
			{
				existingVX.ChoNgoi = vx.ChoNgoi;
				existingVX.MaChuyen = vx.MaChuyen;
				existingVX.MaHD = vx.MaHD;
				existingVX.GiaVe = vx.GiaVe;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var vx = db.VeXes.Find(id);
			if (vx != null)
			{
				db.VeXes.Remove(vx);
				db.SaveChanges();
			}
		}
	}
}
