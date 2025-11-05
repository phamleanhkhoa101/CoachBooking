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
				return "Mật khẩu phải lớn hơn 8 ký tự, bao gồm chữ thường, chữ hoa, số và ký tự đặc biệt";
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
	}
}
