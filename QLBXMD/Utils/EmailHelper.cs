using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QLBXMD.Utils
{
	public class EmailHelper
	{
		public static bool SendEmail(string toEmail, string subject, string body)
		{
			try
			{
				var fromAddress = new MailAddress("phamleanhkhoa2005@gmail.com", "Hệ thống bán vé");
				var toAddress = new MailAddress(toEmail);
				const string fromPassword = "sbqf mozx wowj ihdx"; // App Password Gmail

				using (var smtp = new SmtpClient
				{
					Host = "smtp.gmail.com",
					Port = 587,
					EnableSsl = true,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
				})
				using (var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = subject,
					Body = body
				})
				{
					smtp.Send(message);
					return true; // ✅ gửi thành công
				}
			}
			catch (Exception ex)
			{
				// Gợi ý: log ra ex.Message nếu muốn debug
				Console.WriteLine("Lỗi gửi mail: " + ex.Message);
				return false; // ❌ gửi thất bại
			}
		}
	}
}

