using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QLBXMD.Utils
{
	public class Validation
	{
		public static bool IsValidEmail(string email)
		{
			string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
			return Regex.IsMatch(email, pattern);
		}

		public static bool IsValidTenDangNhap(string tenDangNhap)
		{
			return tenDangNhap.Length >= 6;
		}
		public static bool IsValidMatKhau(string matKhau)
		{
			if (matKhau.Length < 8) return false;

			bool hasUpper = matKhau.Any(char.IsUpper);
			bool hasLower = matKhau.Any(char.IsLower);
			bool hasDigit = matKhau.Any(char.IsDigit);
			bool hasSpecial = Regex.IsMatch(matKhau, @"[\W_]");
			return hasUpper && hasLower && hasDigit && hasSpecial;
		}
	} 
}
