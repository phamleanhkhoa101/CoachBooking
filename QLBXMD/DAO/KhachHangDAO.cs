using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class KhachHangDAO
	{
		private BXMDEntities db = new BXMDEntities();

		public List<KhachHang> GetAll()
		{
			return db.KhachHangs.ToList();
		}

		public KhachHang GetById(int id)
		{
			return db.KhachHangs.Find(id);
		}

		public void Add(KhachHang kh)
		{
			db.KhachHangs.Add(kh);
			db.SaveChanges();
		}

		public void Update(KhachHang kh)
		{
			var existingKH = db.KhachHangs.Find(kh.MaKH);
			if (existingKH != null)
			{
				existingKH.TenKH = kh.TenKH;
				existingKH.NamSinh = kh.NamSinh;
				existingKH.SDT = kh.SDT;
				existingKH.GioiTinh = kh.GioiTinh;
				existingKH.MaTK = kh.MaTK;
				db.SaveChanges();
			}
		}

		public void Delete(int id)
		{
			var kh = db.KhachHangs.Find(id);
			if (kh != null)
			{
				db.KhachHangs.Remove(kh);
				db.SaveChanges();
			}
		}

		public bool ExistsSDT(string sdt, int? excludeMaKH = null)
		{
			if (excludeMaKH.HasValue)
			{
				return db.KhachHangs.Any(kh => kh.SDT == sdt && kh.MaKH != excludeMaKH.Value);
			}
			return db.KhachHangs.Any(kh => kh.SDT == sdt);
		}
	}
}
