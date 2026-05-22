using QLBXMD.DAO;
using QLBXMD.EF;
using QLBXMD.Utils;
using QLBXMD.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.BUS
{
	public class TaiKhoanBUS
	{
		private TaiKhoanDAO taiKhoanDAO = new TaiKhoanDAO();

		public TaiKhoan DangNhap(string tenDangNhap, string matKhau)
		{
			return taiKhoanDAO.DangNhap(tenDangNhap, matKhau);
		}

		public string DangKy(string tenDangNhap, string email, string matKhau, string matKhau2)
		{
			if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(matKhau2))
			{
				return "Vui lòng nhập đầy đủ thông tin.";
			}

			if (!Validation.IsValidTenDangNhap(tenDangNhap))
			{
				return "Tên đăng nhập phải có ít nhất 6 ký tự.";
			}
			
			if (!Validation.IsValidEmail(email))
			{
				return "Email không đúng định dạng.";
			}
			if (!Validation.IsValidMatKhau(matKhau))
			{
				return "Mật khẩu phải từ 6 ký tự trở lên, bao gồm chữ thường, chữ hoa, số và ký tự đặc biệt.";
			}
			if (!matKhau.Equals(matKhau2))
			{
				return "Mật khẩu không khớp.";
			}

			if (taiKhoanDAO.ExistsEmail(email)) return "Email đã được sử dụng.";
			if (taiKhoanDAO.ExistsTenDangNhap(tenDangNhap)) return "Tên đăng nhập đã được sử dụng.";

			string matKhauHash = HashHelper.ComputeHash(matKhau);

			var tk = new TaiKhoan
			{
				TenDangNhap = tenDangNhap,
				Email = email,
				MatKhau = matKhauHash,
				MaQuyen = 3, // Mặc định là nhóm User
				TrangThai = true // Mặc định tài khoản mới là hoạt động
			};
			bool success = taiKhoanDAO.Add(tk);
			return success ? "success" : "Đăng ký thất bại. Vui lòng thử lại.";
		}

		public string QuenMatKhau(string email, string tenDangNhap)
		{
			if (string.IsNullOrEmpty(email))
			{
				return "Vui lòng nhập email.";
			}
			if (!Validation.IsValidEmail(email))
			{
				return "Email không đúng định dạng.";
			}
			if (string.IsNullOrEmpty(tenDangNhap)) return "Vui lòng nhập tên đăng nhập.";
			if (tenDangNhap.Length<6) return "Tên đăng nhập phải có ít nhất 6 ký tự.";
			var tk = taiKhoanDAO.GetByTenDangNhapAndEmail(tenDangNhap,email);
			if (tk == null)
			{
				return "Tên đăng nhập hoặc Email không tồn tại.";
			}
			string newPassword = GenerateRandomPassword(10);
			tk.MatKhau = HashHelper.ComputeHash(newPassword);
			bool success = taiKhoanDAO.Update(tk);
			if (!success) return "Đặt lại mật khẩu thất bại. Vui lòng thử lại.";
			bool emailSent = EmailHelper.SendEmail(
				email,
				"Yêu cầu đặt lại mật khẩu",
				$"Mật khẩu mới của bạn là: {newPassword}\nVui lòng đăng nhập và đổi mật khẩu ngay."
			);
			return emailSent ? "success" : "Gửi email thất bại. Vui lòng thử lại.";
		}
		
		private string GenerateRandomPassword(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
			var random = new Random();
			var sb = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				sb.Append(chars[random.Next(chars.Length)]);
			}
			return sb.ToString();
		}

		public NhanVien GetNhanVienByTaiKhoan(TaiKhoan taiKhoan)
		{
			if (taiKhoan == null) return null;
			return taiKhoanDAO.GetNhanVienByMaTK(taiKhoan.MaTK);
		}
		
		public KhachHang GetKhachHangByTaiKhoan(TaiKhoan taiKhoan)
		{
			if (taiKhoan == null) return null;
			return taiKhoanDAO.GetKhachHangByMaTK(taiKhoan.MaTK);
		}

		// CRUD operations for admin
		public List<TaiKhoan> GetAll()
		{
			return taiKhoanDAO.GetAll();
		}

		public TaiKhoan GetById(int maTK)
		{
			return taiKhoanDAO.GetById(maTK);
		}

		public string Add(TaiKhoan tk, string matKhau)
		{
			if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
				return "Vui lòng nhập tên đăng nhập.";

			if (!Validation.IsValidTenDangNhap(tk.TenDangNhap))
				return "Tên đăng nhập phải có ít nhất 6 ký tự.";

			if (string.IsNullOrWhiteSpace(tk.Email))
				return "Vui lòng nhập email.";

			if (!Validation.IsValidEmail(tk.Email))
				return "Email không đúng định dạng.";

			if (string.IsNullOrWhiteSpace(matKhau))
				return "Vui lòng nhập mật khẩu.";

			if (!Validation.IsValidMatKhau(matKhau))
				return "Mật khẩu phải từ 6 ký tự trở lên, bao gồm chữ thường, chữ hoa, số và ký tự đặc biệt.";

			if (tk.MaQuyen == null || tk.MaQuyen == 0)
				return "Vui lòng chọn quyền.";

			if (taiKhoanDAO.ExistsTenDangNhap(tk.TenDangNhap))
				return "Tên đăng nhập đã được sử dụng.";

			if (taiKhoanDAO.ExistsEmail(tk.Email))
				return "Email đã được sử dụng.";

			tk.MatKhau = HashHelper.ComputeHash(matKhau);

			try
			{
				bool success = taiKhoanDAO.Add(tk);
				return success ? "Thêm tài khoản thành công." : "Thêm tài khoản thất bại.";
			}
			catch (Exception ex)
			{
				return "Lỗi: " + ex.Message;
			}
		}

		public string Update(TaiKhoan tk)
		{
			if (string.IsNullOrWhiteSpace(tk.Email))
				return "Vui lòng nhập email.";

			if (!Validation.IsValidEmail(tk.Email))
				return "Email không đúng định dạng.";

			if (tk.MaQuyen == null || tk.MaQuyen == 0)
				return "Vui lòng chọn quyền.";

			var existing = taiKhoanDAO.GetById(tk.MaTK);
			if (existing == null)
				return "Tài khoản không tồn tại.";

			// Check email duplicate (excluding current account)
			var existingEmail = taiKhoanDAO.GetByEmail(tk.Email);
			if (existingEmail != null && existingEmail.MaTK != tk.MaTK)
				return "Email đã được sử dụng bởi tài khoản khác.";

			try
			{
				existing.Email = tk.Email;
				existing.MaQuyen = tk.MaQuyen;
				existing.TrangThai = tk.TrangThai;

				bool success = taiKhoanDAO.Update(existing);
				return success ? "Cập nhật tài khoản thành công." : "Cập nhật tài khoản thất bại.";
			}
			catch (Exception ex)
			{
				return "Lỗi: " + ex.Message;
			}
		}

		public string ResetPassword(int maTK)
		{
			var tk = taiKhoanDAO.GetById(maTK);
			if (tk == null)
				return "Tài khoản không tồn tại.";

			string newPassword = GenerateRandomPassword(10);
			tk.MatKhau = HashHelper.ComputeHash(newPassword);

			try
			{
				bool success = taiKhoanDAO.Update(tk);
				if (!success)
					return "Đặt lại mật khẩu thất bại.";

				bool emailSent = EmailHelper.SendEmail(
					tk.Email,
					"Đặt lại mật khẩu",
					$"Mật khẩu mới của bạn là: {newPassword}\nVui lòng đăng nhập và đổi mật khẩu ngay."
				);

				return emailSent 
					? "Đặt lại mật khẩu thành công. Mật khẩu mới đã được gửi qua email." 
					: "Đặt lại mật khẩu thành công nhưng không thể gửi email.";
			}
			catch (Exception ex)
			{
				return "Lỗi: " + ex.Message;
			}
		}

		public string Delete(int maTK)
		{
			var tk = taiKhoanDAO.GetById(maTK);
			if (tk == null)
				return "Tài khoản không tồn tại.";

			// Check if account is linked to employee or customer
			var nhanVien = taiKhoanDAO.GetNhanVienByMaTK(maTK);
			var khachHang = taiKhoanDAO.GetKhachHangByMaTK(maTK);

			if (nhanVien != null)
				return "Không thể xóa tài khoản này vì đã được liên kết với nhân viên.";

			if (khachHang != null)
				return "Không thể xóa tài khoản này vì đã được liên kết với khách hàng.";

			try
			{
				bool success = taiKhoanDAO.Delete(maTK);
				return success ? "Xóa tài khoản thành công." : "Xóa tài khoản thất bại.";
			}
			catch (Exception ex)
			{
				return "Lỗi: " + ex.Message;
			}
		}

		/// <summary>
		/// Lấy danh sách tài khoản chưa được gắn với nhân viên hoặc khách hàng
		/// </summary>
		public List<TaiKhoan> GetAvailableAccounts()
		{
			var all = taiKhoanDAO.GetAll();
			return all.Where(tk => 
				taiKhoanDAO.GetNhanVienByMaTK(tk.MaTK) == null && 
				taiKhoanDAO.GetKhachHangByMaTK(tk.MaTK) == null
			).ToList();
		}

		/// <summary>
		/// Lấy tài khoản chưa gắn HOẶC tài khoản hiện tại của entity này (dùng cho Edit)
		/// </summary>
		public List<TaiKhoan> GetAvailableAccountsForEdit(int? currentMaTK)
		{
			var all = taiKhoanDAO.GetAll();
			return all.Where(tk => 
				tk.MaTK == currentMaTK ||
				(taiKhoanDAO.GetNhanVienByMaTK(tk.MaTK) == null && 
				 taiKhoanDAO.GetKhachHangByMaTK(tk.MaTK) == null)
			).ToList();
		}
	}
}
