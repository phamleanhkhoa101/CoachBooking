using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class NhanVienDAO
	{
		private BXMDEntities db = new BXMDEntities();

		public List<NhanVien> GetAll()
		{
			return db.NhanViens.ToList();
		}
		public NhanVien GetById(int id)
		{
			return db.NhanViens.Find(id);
		}	
		public void Add(NhanVien nv)
		{
			db.NhanViens.Add(nv);
			db.SaveChanges();
		}
		public void Update(NhanVien nv)
		{
			var existingNV = db.NhanViens.Find(nv.MaNV);
			if (existingNV != null)
			{
				existingNV.TenNV = nv.TenNV;
				existingNV.GioiTinh = nv.GioiTinh;
				existingNV.DiaChi = nv.DiaChi;
				existingNV.SDT = nv.SDT;
				existingNV.NamSinh = nv.NamSinh;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var nv = db.NhanViens.Find(id);
			if (nv != null)
			{
				db.NhanViens.Remove(nv);
				db.SaveChanges();
			}
		}
	}
}
