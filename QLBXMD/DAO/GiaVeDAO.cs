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
		/// L?y t?t c? các t? h?p NhaXe x LoaiXe x TuyenDuong mà nhà xe th?c s? có xe ch?y,
		/// bao g?m c? nh?ng t? h?p chýa có giá vé
		/// </summary>
		public List<dynamic> GetAllCombinations()
		{
			// ? L?y các t? h?p TH?C T? t? b?ng Xe và ChuyenXe
			// Ch? l?y nh?ng Nhà xe x Lo?i xe x Tuy?n mà nhà xe ðó có xe lo?i ðó và ch?y tuy?n ðó
			var actualCombinations = (from cx in db.ChuyenXes
									  join x in db.Xes on cx.MaXe equals x.MaXe
									  join nx in db.NhaXes on x.MaNX equals nx.MaNX
									  join lx in db.LoaiXes on x.MaLX equals lx.MaLX
									  join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
									  where x.MaNX != 0 // ? Ch? l?y xe có nhà xe
									  select new
									  {
										  MaNX = x.MaNX,
										  TenNX = nx.TenNX,
										  MaLX = x.MaLX,
										  TenLX = lx.TenLX,
										  MaTuyen = cx.MaTuyen,
										  DiemDi = td.DiemDi,
										  DiemDen = td.DiemDen
									  })
									  .Distinct() // ? Lo?i b? trùng l?p (v? nhi?u chuy?n có th? cùng t? h?p)
									  .ToList();

			// L?y t?t c? giá vé hi?n có
			var existingPrices = db.GiaVes.ToList();

			// K?t h?p d? li?u
			var result = actualCombinations.Select(combo =>
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
