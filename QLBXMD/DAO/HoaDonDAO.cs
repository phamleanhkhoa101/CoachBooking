using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class HoaDonDAO
	{
		private BXMDEntities db = new BXMDEntities();
		public List<EF.HoaDon> GetAll()
		{
			return db.HoaDons.ToList();
		}
		public EF.HoaDon GetById(int id)
		{
			return db.HoaDons.Find(id);
		}
		public void Add(EF.HoaDon hd)
		{
			db.HoaDons.Add(hd);
			db.SaveChanges();
		}
		public void Update(EF.HoaDon hd)
		{
			var existingHD = db.HoaDons.Find(hd.MaHD);
			if (existingHD != null)
			{
				existingHD.MaKH = hd.MaKH;
				existingHD.MANV = hd.MANV;
				existingHD.NgayLap = hd.NgayLap;
				existingHD.TongTien = hd.TongTien;
				existingHD.TrangThai = hd.TrangThai;
				db.SaveChanges();
			}
		}
		public void Delete(int id)
		{
			var hd = db.HoaDons.Find(id);
			if (hd != null)
			{
				db.HoaDons.Remove(hd);
				db.SaveChanges();
			}
		}
        public List<HoaDon> GetByKhachHang(int maKH)
        {
            return db.HoaDons.Where(hd => hd.MaKH == maKH).ToList();
        }
        public List<HoaDon> GetByNhanVien(int maNV)
        {
            return db.HoaDons.Where(hd => hd.MANV == maNV).ToList();
        }
    }
}
