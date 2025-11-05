using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class LoaiXeDAO
	{
		private BXMDEntities db = new BXMDEntities();
		
		public List<EF.LoaiXe> GetAll()
		{
			return db.LoaiXes.ToList();
		}
		
		public EF.LoaiXe GetById(int id)
		{
			return db.LoaiXes.Find(id);
		}
		
		public bool Add(EF.LoaiXe lx)
		{
			try
			{
				db.LoaiXes.Add(lx);
				return db.SaveChanges() > 0;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		
		public bool Update(EF.LoaiXe lx)
		{
			var existingLX = db.LoaiXes.Find(lx.MaLX);
			if (existingLX != null)
			{
				db.Entry(existingLX).CurrentValues.SetValues(lx);
				return db.SaveChanges() > 0;
			}
			return false;
		}
		
		public void Delete(int id)
		{
			var lx = db.LoaiXes.Find(id);
			if (lx != null)
			{
				db.LoaiXes.Remove(lx);
				db.SaveChanges();
			}
		}

		public bool ExistsTenLX(string tenLX, int? excludeMaLX = null)
		{
			if (excludeMaLX.HasValue)
			{
				return db.LoaiXes.Any(lx => lx.TenLX == tenLX && lx.MaLX != excludeMaLX.Value);
			}
			return db.LoaiXes.Any(lx => lx.TenLX == tenLX);
		}
	}
}
