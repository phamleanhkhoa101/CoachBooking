using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QLBXMD.Services
{
	/// <summary>
	/// Service x? lý thanh toán MoMo
	/// </summary>
	public class MoMoPaymentService
	{
		// ?? TEST Credentials - Thay ??i khi chuy?n sang Production
		private const string PartnerCode = "MOMO4LG620250601_TEST";
		private const string AccessKey = "e6g7W44vmy1rgT7K";
		private const string SecretKey = "XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa";
		private const string ApiEndpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
		
		// ?? C?n thay ??i thành URL th?c t? c?a b?n
		private const string ReturnUrl = "http://localhost:5000/payment/return";
		private const string NotifyUrl = "http://localhost:5000/payment/notify";

		/// <summary>
		/// T?o yêu c?u thanh toán MoMo
		/// </summary>
		public async Task<MoMoPaymentResponse> CreatePaymentRequest(
			string orderId,
			long amount,
			string orderInfo,
			string extraData = "")
		{
			try
			{
				var requestId = Guid.NewGuid().ToString();
				var requestType = "captureWallet";

				// T?o ch? ký (signature) theo th? t? alphabet
				var rawHash = $"accessKey={AccessKey}" +
							  $"&amount={amount}" +
							  $"&extraData={extraData}" +
							  $"&ipnUrl={NotifyUrl}" +
							  $"&orderId={orderId}" +
							  $"&orderInfo={orderInfo}" +
							  $"&partnerCode={PartnerCode}" +
							  $"&redirectUrl={ReturnUrl}" +
							  $"&requestId={requestId}" +
							  $"&requestType={requestType}";

				var signature = ComputeHmacSha256(rawHash, SecretKey);

				// T?o request body
				var requestBody = new
				{
					partnerCode = PartnerCode,
					accessKey = AccessKey,
					requestId = requestId,
					amount = amount,
					orderId = orderId,
					orderInfo = orderInfo,
					redirectUrl = ReturnUrl,
					ipnUrl = NotifyUrl,
					extraData = extraData,
					requestType = requestType,
					signature = signature,
					lang = "vi"
				};

				// G?i request ??n MoMo
				using (var httpClient = new HttpClient())
				{
					httpClient.Timeout = TimeSpan.FromSeconds(30);
					
					var json = JsonConvert.SerializeObject(requestBody);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					Console.WriteLine($"=== MOMO REQUEST ===");
					Console.WriteLine($"Endpoint: {ApiEndpoint}");
					Console.WriteLine($"Body: {json}");

					var response = await httpClient.PostAsync(ApiEndpoint, content);
					var responseContent = await response.Content.ReadAsStringAsync();

					Console.WriteLine($"=== MOMO RESPONSE ===");
					Console.WriteLine($"Status: {response.StatusCode}");
					Console.WriteLine($"Content: {responseContent}");

					var momoResponse = JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseContent);
					return momoResponse;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"? MoMo Error: {ex.ToString()}");
				return new MoMoPaymentResponse
				{
					ResultCode = -1,
					Message = $"L?i k?t n?i MoMo: {ex.Message}"
				};
			}
		}

		/// <summary>
		/// Tính HMAC SHA256 signature
		/// </summary>
		private string ComputeHmacSha256(string message, string secretKey)
		{
			var keyBytes = Encoding.UTF8.GetBytes(secretKey);
			var messageBytes = Encoding.UTF8.GetBytes(message);

			using (var hmac = new HMACSHA256(keyBytes))
			{
				var hashBytes = hmac.ComputeHash(messageBytes);
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

		/// <summary>
		/// Verify signature t? MoMo callback
		/// </summary>
		public bool VerifySignature(MoMoCallbackRequest request, string receivedSignature)
		{
			try
			{
				var rawHash = $"accessKey={AccessKey}" +
							  $"&amount={request.Amount}" +
							  $"&extraData={request.ExtraData}" +
							  $"&message={request.Message}" +
							  $"&orderId={request.OrderId}" +
							  $"&orderInfo={request.OrderInfo}" +
							  $"&orderType={request.OrderType}" +
							  $"&partnerCode={request.PartnerCode}" +
							  $"&payType={request.PayType}" +
							  $"&requestId={request.RequestId}" +
							  $"&responseTime={request.ResponseTime}" +
							  $"&resultCode={request.ResultCode}" +
							  $"&transId={request.TransId}";

				var signature = ComputeHmacSha256(rawHash, SecretKey);
				return signature == receivedSignature;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"? Verify Signature Error: {ex.Message}");
				return false;
			}
		}
	}

	#region Response Models

	/// <summary>
	/// Response t? MoMo khi t?o payment request
	/// </summary>
	public class MoMoPaymentResponse
	{
		[JsonProperty("partnerCode")]
		public string PartnerCode { get; set; }

		[JsonProperty("orderId")]
		public string OrderId { get; set; }

		[JsonProperty("requestId")]
		public string RequestId { get; set; }

		[JsonProperty("amount")]
		public long Amount { get; set; }

		[JsonProperty("responseTime")]
		public long ResponseTime { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("resultCode")]
		public int ResultCode { get; set; }

		[JsonProperty("payUrl")]
		public string PayUrl { get; set; }

		[JsonProperty("deeplink")]
		public string Deeplink { get; set; }

		[JsonProperty("qrCodeUrl")]
		public string QrCodeUrl { get; set; }
	}

	/// <summary>
	/// Callback request t? MoMo (IPN)
	/// </summary>
	public class MoMoCallbackRequest
	{
		[JsonProperty("partnerCode")]
		public string PartnerCode { get; set; }

		[JsonProperty("orderId")]
		public string OrderId { get; set; }

		[JsonProperty("requestId")]
		public string RequestId { get; set; }

		[JsonProperty("amount")]
		public long Amount { get; set; }

		[JsonProperty("orderInfo")]
		public string OrderInfo { get; set; }

		[JsonProperty("orderType")]
		public string OrderType { get; set; }

		[JsonProperty("transId")]
		public long TransId { get; set; }

		[JsonProperty("resultCode")]
		public int ResultCode { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("payType")]
		public string PayType { get; set; }

		[JsonProperty("responseTime")]
		public long ResponseTime { get; set; }

		[JsonProperty("extraData")]
		public string ExtraData { get; set; }

		[JsonProperty("signature")]
		public string Signature { get; set; }
	}

	#endregion
}
