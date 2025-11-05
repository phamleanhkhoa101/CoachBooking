using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class TaiXeDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.TaiXe> GetAll()
		{
			return db.TaiXes.ToList();
		}
		public EF.TaiXe GetById(int id)
		{
			return db.TaiXes.Find(id);
		}
		public void Add(EF.TaiXe tx)
		{
			db.TaiXes.Add(tx);
			db.SaveChanges();
		}
		public void Update(EF.TaiXe tx)
		{
			var existingTX = db.TaiXes.Find(tx.MaTX);
			if (existingTX != null)
			{
				existingTX.TenTX = tx.TenTX;
				existingTX.SDT = tx.SDT;
				existingTX.DiaChi = tx.DiaChi;
				existingTX.TrangThai = tx.TrangThai;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var tx = db.TaiXes.Find(id);
			if (tx != null)
			{
				db.TaiXes.Remove(tx);
				db.SaveChanges();
			}
		}
	}
}
