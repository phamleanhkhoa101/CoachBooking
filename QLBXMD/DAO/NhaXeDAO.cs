using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLBXMD.DAO
{
	public class NhaXeDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.NhaXe> GetAll()
		{
			return db.NhaXes.ToList();
		}
		public EF.NhaXe GetById(int id)
		{
			return db.NhaXes.Find(id);
		}
		public bool Add(EF.NhaXe nx)
		{
			try
			{
				db.NhaXes.Add(nx);
				return db.SaveChanges() > 0;
			}catch (Exception ex)
			{
				return false;
			}
		}
		public bool Update(EF.NhaXe nx)
		{
			var existingNX = db.NhaXes.Find(nx.MaNX);
			if (existingNX != null)
			{
				db.Entry(existingNX).CurrentValues.SetValues(nx);
				return db.SaveChanges() > 0;
			}
			return false;
		}
		public void Delete(int id)
		{
			var nx = db.NhaXes.Find(id);
			if (nx != null)
			{
				db.NhaXes.Remove(nx);
				db.SaveChanges();
			}
		}

		public bool ExistsEmail(string email, int? excludeMaNX = null)
		{
			if (excludeMaNX.HasValue)
			{
				return db.NhaXes.Any(nx => nx.Email == email && nx.MaNX != excludeMaNX.Value);
			}
			return db.NhaXes.Any(nx => nx.Email == email);
		}

		public bool ExistsSDT(string sdt, int? excludeMaNX = null)
		{
			if (excludeMaNX.HasValue)
			{
				return db.NhaXes.Any(nx => nx.SDT == sdt && nx.MaNX != excludeMaNX.Value);
			}
			return db.NhaXes.Any(nx => nx.SDT == sdt);
		}
	}
}
