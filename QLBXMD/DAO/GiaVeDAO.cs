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

		/// <summary>
		/// Lấy tất cả các tổ hợp NhaXe x LoaiXe x TuyenDuong, bao gồm cả những tổ hợp chưa có giá vé
		/// </summary>
		public List<dynamic> GetAllCombinations()
		{
			// Lấy tất cả các tổ hợp có thể
			var allCombinations = (from nx in db.NhaXes
								   from lx in db.LoaiXes
								   from td in db.TuyenDuongs
								   select new
								   {
									   MaNX = nx.MaNX,
									   TenNX = nx.TenNX,
									   MaLX = lx.MaLX,
									   TenLX = lx.TenLX,
									   MaTuyen = td.MaTuyen,
									   DiemDi = td.DiemDi,
									   DiemDen = td.DiemDen
								   }).ToList();

			// Lấy tất cả giá vé hiện có
			var existingPrices = db.GiaVes.ToList();

			// Kết hợp dữ liệu
			var result = allCombinations.Select(combo =>
			{
				var price = existingPrices.FirstOrDefault(gv =>
					gv.MaNX == combo.MaNX &&
					gv.MaLX == combo.MaLX &&
					gv.MaTuyen == combo.MaTuyen);

				return new
				{
					MaGV = price?.MaGV,
					MaNX = combo.MaNX,
					TenNX = combo.TenNX,
					MaLX = combo.MaLX,
					TenLX = combo.TenLX,
					MaTuyen = combo.MaTuyen,
					DiemDi = combo.DiemDi,
					DiemDen = combo.DiemDen,
					GiaVe = price?.GiaVe1,
					HasPrice = price != null
				};
			}).OrderBy(x => x.TenNX)
			  .ThenBy(x => x.TenLX)
			  .ThenBy(x => x.DiemDi)
			  .ToList<dynamic>();

			return result;
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
