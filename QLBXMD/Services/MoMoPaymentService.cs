using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QLBXMD.Services
{
	/// <summary>
	/// Service xử lý thanh toán MoMo chuẩn V2.1
	/// </summary>
	public class MoMoPaymentService
	{
		private const string PartnerCode = "MOMO4LG620250601_TEST";
		private const string AccessKey = "e6g7W44vmy1rgT7K";
		private const string SecretKey = "XAAT9fHRPeJD8zhjKsZnTTUSnYEHFISa";
		private const string ApiEndpoint = "https://test-payment.momo.vn/v2/gateway/api/create";

		private const string ReturnUrl = "http://localhost:5000/payment/return";
		private const string NotifyUrl = "http://localhost:5000/payment/notify";

		/// <summary>
		/// Tạo yêu cầu thanh toán MoMo
		/// </summary>
		public async Task<MoMoPaymentResponse> CreatePaymentRequest(
			string orderId,
			long amount,
			string orderInfo,
			string extraData = "")
		{
			try
			{
				if (extraData == null)
					extraData = "";

				var requestId = Guid.NewGuid().ToString();
				var requestType = "captureWallet";

				// ✅ KHÔNG ENCODE rawSignature!
				var rawSignature =
					"accessKey=" + AccessKey +
					"&amount=" + amount +
					"&extraData=" + extraData +
					"&ipnUrl=" + NotifyUrl +                // không encode
					"&orderId=" + orderId +
					"&orderInfo=" + orderInfo +             // không encode
					"&partnerCode=" + PartnerCode +
					"&redirectUrl=" + ReturnUrl +           // không encode
					"&requestId=" + requestId +
					"&requestType=" + requestType;

				Console.WriteLine("=== MOMO SIGNATURE DEBUG ===");
				Console.WriteLine($"Raw Signature: {rawSignature}");

				var signature = ComputeHmacSha256(rawSignature, SecretKey);
				Console.WriteLine($"Computed Signature: {signature}");

				// ✅ BODY cũng KHÔNG encode (MoMo docs yêu cầu raw string)
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

				using (var httpClient = new HttpClient())
				{
					httpClient.Timeout = TimeSpan.FromSeconds(30);

					var json = JsonConvert.SerializeObject(requestBody);
					Console.WriteLine("=== MOMO REQUEST BODY ===");
					Console.WriteLine(json);

					var content = new StringContent(json, Encoding.UTF8, "application/json");
					var response = await httpClient.PostAsync(ApiEndpoint, content);
					var responseContent = await response.Content.ReadAsStringAsync();

					Console.WriteLine("=== MOMO RESPONSE ===");
					Console.WriteLine(responseContent);

					return JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseContent)
						   ?? new MoMoPaymentResponse();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("❌ MoMo Error: " + ex.Message);
				return new MoMoPaymentResponse
				{
					ResultCode = -1,
					Message = "Lỗi kết nối MoMo: " + ex.Message
				};
			}
		}

		/// <summary>
		/// Tính chữ ký HMAC SHA256
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
		/// Verify chữ ký callback từ MoMo
		/// </summary>
		public bool VerifySignature(MoMoCallbackRequest request, string receivedSignature)
		{
			try
			{
				// ✅ Callback cũng theo format raw (không encode)
				var rawSignature =
					"accessKey=" + AccessKey +
					"&amount=" + request.Amount +
					"&extraData=" + request.ExtraData +
					"&message=" + request.Message +
					"&orderId=" + request.OrderId +
					"&orderInfo=" + request.OrderInfo +
					"&orderType=" + request.OrderType +
					"&partnerCode=" + request.PartnerCode +
					"&payType=" + request.PayType +
					"&requestId=" + request.RequestId +
					"&responseTime=" + request.ResponseTime +
					"&resultCode=" + request.ResultCode +
					"&transId=" + request.TransId;

				var computedSignature = ComputeHmacSha256(rawSignature, SecretKey);

				Console.WriteLine("=== VERIFY SIGNATURE ===");
				Console.WriteLine("Received: " + receivedSignature);
				Console.WriteLine("Computed: " + computedSignature);

				return computedSignature == receivedSignature;
			}
			catch
			{
				return false;
			}
		}
		public async Task<MoMoQueryResponse> QueryPaymentStatus(string orderId)
		{
			try
			{
				var requestId = Guid.NewGuid().ToString();

				var rawSignature =
					"accessKey=" + AccessKey +
					"&orderId=" + orderId +
					"&partnerCode=" + PartnerCode +
					"&requestId=" + requestId;

				var signature = ComputeHmacSha256(rawSignature, SecretKey);

				var requestBody = new
				{
					partnerCode = PartnerCode,
					requestId = requestId,
					orderId = orderId,
					signature = signature,
					lang = "vi"
				};

				using (var httpClient = new HttpClient())
				{
					httpClient.Timeout = TimeSpan.FromSeconds(30);

					var json = JsonConvert.SerializeObject(requestBody);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					
					var queryEndpoint = "https://test-payment.momo.vn/v2/gateway/api/query";
					var response = await httpClient.PostAsync(queryEndpoint, content);
					var responseContent = await response.Content.ReadAsStringAsync();

					return JsonConvert.DeserializeObject<MoMoQueryResponse>(responseContent)
						   ?? new MoMoQueryResponse();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("❌ MoMo Query Error: " + ex.Message);
				return new MoMoQueryResponse
				{
					ResultCode = -1,
					Message = "Lỗi kết nối MoMo: " + ex.Message
				};
			}
		}
	}

	#region Response Models

	public class MoMoPaymentResponse
	{
		[JsonProperty("partnerCode")] public string PartnerCode { get; set; }
		[JsonProperty("orderId")] public string OrderId { get; set; }
		[JsonProperty("requestId")] public string RequestId { get; set; }
		[JsonProperty("amount")] public long Amount { get; set; }
		[JsonProperty("responseTime")] public long ResponseTime { get; set; }
		[JsonProperty("message")] public string Message { get; set; }
		[JsonProperty("resultCode")] public int ResultCode { get; set; }
		[JsonProperty("payUrl")] public string PayUrl { get; set; }
		[JsonProperty("deeplink")] public string Deeplink { get; set; }
		[JsonProperty("qrCodeUrl")] public string QrCodeUrl { get; set; }
	}

	public class MoMoCallbackRequest
	{
		[JsonProperty("partnerCode")] public string PartnerCode { get; set; }
		[JsonProperty("orderId")] public string OrderId { get; set; }
		[JsonProperty("requestId")] public string RequestId { get; set; }
		[JsonProperty("amount")] public long Amount { get; set; }
		[JsonProperty("orderInfo")] public string OrderInfo { get; set; }
		[JsonProperty("orderType")] public string OrderType { get; set; }
		[JsonProperty("transId")] public long TransId { get; set; }
		[JsonProperty("resultCode")] public int ResultCode { get; set; }
		[JsonProperty("message")] public string Message { get; set; }
		[JsonProperty("payType")] public string PayType { get; set; }
		[JsonProperty("responseTime")] public long ResponseTime { get; set; }
		[JsonProperty("extraData")] public string ExtraData { get; set; }
		[JsonProperty("signature")] public string Signature { get; set; }
	}

	public class MoMoQueryResponse
	{
		[JsonProperty("partnerCode")] public string PartnerCode { get; set; }
		[JsonProperty("orderId")] public string OrderId { get; set; }
		[JsonProperty("requestId")] public string RequestId { get; set; }
		[JsonProperty("extraData")] public string ExtraData { get; set; }
		[JsonProperty("amount")] public long Amount { get; set; }
		[JsonProperty("transId")] public long TransId { get; set; }
		[JsonProperty("payType")] public string PayType { get; set; }
		[JsonProperty("resultCode")] public int ResultCode { get; set; }
		[JsonProperty("refundTrans")] public object RefundTrans { get; set; }
		[JsonProperty("message")] public string Message { get; set; }
		[JsonProperty("responseTime")] public long ResponseTime { get; set; }
	}

	#endregion
}
