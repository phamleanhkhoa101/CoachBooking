using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	internal class QuyenDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.Quyen> GetAll()
		{
			return db.Quyens.ToList();
		}
		public EF.Quyen GetById(int id)
		{
			return db.Quyens.Find(id);
		}
		public void Add(EF.Quyen q)
		{
			db.Quyens.Add(q);
			db.SaveChanges();
		}
		public void Update(EF.Quyen q)
		{
			var existingCX = db.Quyens.Find(q.MaQuyen);
			if (existingCX != null)
			{
				existingCX.MaQuyen = q.MaQuyen;
				existingCX.TenQuyen = q.TenQuyen;
				existingCX.MoTa = q.MoTa;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var q = db.Quyens.Find(id);
			if (q != null)
			{
				db.Quyens.Remove(q);
				db.SaveChanges();
			}
		}
	}
}
