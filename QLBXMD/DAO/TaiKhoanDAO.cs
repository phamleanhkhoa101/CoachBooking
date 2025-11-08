using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.DAO
{
	public class TaiKhoanDAO
	{
		private BXMDEntities db = new BXMDEntities();

		public TaiKhoan DangNhap(string TenDangNhap, string MatKhau)
		{
			var user = db.TaiKhoans.FirstOrDefault(tk => tk.TenDangNhap == TenDangNhap && tk.MatKhau == MatKhau);
			return user;
		}
		public TaiKhoan GetByTenDangNhapAndEmail(string TenDangNhap, string Email)
		{
			var user = db.TaiKhoans.FirstOrDefault(tk => tk.TenDangNhap == TenDangNhap && tk.Email == Email);
			return user;
		}
		public TaiKhoan GetByEmail(string email)
		{
			return db.TaiKhoans.FirstOrDefault(tk => tk.Email == email);
		}
		public List<TaiKhoan> GetAll()
		{
			return db.TaiKhoans.ToList();
		}
		public TaiKhoan GetById(int id)
		{
			return db.TaiKhoans.Find(id);
		}
		public bool Add(TaiKhoan tk)
		{
			db.TaiKhoans.Add(tk);
			return db.SaveChanges() >0;
		}
		public bool Update(TaiKhoan tk)
		{
			var existingTK = db.TaiKhoans.Find(tk.MaTK);
			if (existingTK != null)
			{
				existingTK.TenDangNhap = tk.TenDangNhap;
				existingTK.MatKhau = tk.MatKhau;
				existingTK.MaQuyen = tk.MaQuyen;
				existingTK.TrangThai = tk.TrangThai;
				existingTK.Email = tk.Email;
				return db.SaveChanges()>0;
			}
			return false;
		}
		public bool Delete(int id)
		{
			var tk = db.TaiKhoans.Find(id);
			if (tk != null)
			{
				db.TaiKhoans.Remove(tk);
				return db.SaveChanges() >0;
			}
			return false;
		}

		public bool ExistsEmail(string email)
		{
			return db.TaiKhoans.Any(t => t.Email == email);
		}
		public bool ExistsTenDangNhap(string email)
		{
			return db.TaiKhoans.Any(t => t.TenDangNhap == email);
		}

		public NhanVien GetNhanVienByMaTK(int MaTK)
		{
			return db.NhanViens.FirstOrDefault(nv => nv.MaTK == MaTK);
		}
		public KhachHang GetKhachHangByMaTK(int MaTK)
		{
			return db.KhachHangs.FirstOrDefault(kh => kh.MaTK == MaTK);
		}
	}
}
