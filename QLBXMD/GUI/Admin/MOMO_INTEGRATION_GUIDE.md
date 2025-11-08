# H??ng D?n Tích H?p Thanh Toán MoMo

## T?ng Quan

Dialog xác nh?n ??t vé ?ã có s?n option ch?n MoMo. ?? tích h?p th?c t? v?i MoMo Payment Gateway, c?n th?c hi?n các b??c sau.

## Thông Tin T? Hình ?nh

### 1. Partner Code (TEST)
```
MOMO4LG620250601_TEST
```

### 2. Access Key
```
e6g7W44vmy1rgT7K
```

### 3. Secret Key
```
XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa
```

### 4. API Endpoints

#### Website/Application
```
https://test-payment.momo.vn/v2/gateway/api/create
```

#### POS
```
https://test-payment.momo.vn/v2/gateway/api/pos
```

## Các B??c Tích H?p

### B??c 1: Cài ??t Th? Vi?n

```bash
# Cài ??t Newtonsoft.Json n?u ch?a có
Install-Package Newtonsoft.Json
```

### B??c 2: T?o Class MoMoPaymentService

```csharp
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QLBXMD.Services
{
    public class MoMoPaymentService
    {
        private const string PartnerCode = "MOMO4LG620250601_TEST";
        private const string AccessKey = "e6g7W44vmy1rgT7K";
        private const string SecretKey = "XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa";
        private const string ApiEndpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
        private const string ReturnUrl = "http://localhost/payment/return"; // Thay b?ng URL th?c t?
        private const string NotifyUrl = "http://localhost/payment/notify"; // Thay b?ng URL th?c t?

        public async Task<MoMoPaymentResponse> CreatePaymentRequest(
            string orderId,
            decimal amount,
            string orderInfo)
        {
            try
            {
                var requestId = Guid.NewGuid().ToString();
                var extraData = "";
                var requestType = "captureWallet";

                // T?o ch? ký (signature)
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
                    amount = amount.ToString("0"),
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
                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(ApiEndpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var momoResponse = JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseContent);
                    return momoResponse;
                }
            }
            catch (Exception ex)
            {
                return new MoMoPaymentResponse
                {
                    ResultCode = -1,
                    Message = $"L?i: {ex.Message}"
                };
            }
        }

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
    }

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
}
```

### B??c 3: C?p Nh?t UCDatVe.cs

```csharp
private async void btnDatVe_Click(object sender, EventArgs e)
{
    // ... validate code ...

    using (var confirmDialog = new XacNhanDatVeDialog())
    {
        // ... setup dialog ...

        if (confirmDialog.ShowDialog() != DialogResult.OK)
            return;

        string phuongThucThanhToan = confirmDialog.PhuongThucThanhToan;

        // N?u ch?n MoMo
        if (phuongThucThanhToan == "MoMo")
        {
            await ProcessMoMoPayment(tienTam, orderId);
            return; // Ch? callback t? MoMo
        }

        // Ti?p t?c x? lý ??t vé v?i ti?n m?t
        // ...
    }
}

private async Task ProcessMoMoPayment(decimal amount, string orderId)
{
    try
    {
        var momoService = new MoMoPaymentService();
        var orderInfo = $"Thanh toán vé xe - Mã ??n {orderId}";

        var response = await momoService.CreatePaymentRequest(
            orderId, 
            amount, 
            orderInfo
        );

        if (response.ResultCode == 0)
        {
            // M? trình duy?t v?i PayUrl ho?c hi?n th? QR Code
            System.Diagnostics.Process.Start(response.PayUrl);

            MessageBox.Show(
                "Vui lòng hoàn t?t thanh toán trên MoMo.\n" +
                "H? th?ng s? t? ??ng c?p nh?t sau khi thanh toán thành công.",
                "Ch? thanh toán",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        else
        {
            MessageBox.Show(
                $"L?i t?o yêu c?u thanh toán MoMo:\n{response.Message}",
                "L?i",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            $"L?i k?t n?i MoMo: {ex.Message}",
            "L?i",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }
}
```

### B??c 4: X? Lý Callback (IPN - Instant Payment Notification)

T?o Web API ?? nh?n callback t? MoMo:

```csharp
[HttpPost]
[Route("api/payment/momo-callback")]
public IHttpActionResult MoMoCallback([FromBody] MoMoCallbackRequest request)
{
    try
    {
        // Verify signature
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

        if (signature != request.Signature)
        {
            return BadRequest("Invalid signature");
        }

        // Ki?m tra resultCode
        if (request.ResultCode == 0)
        {
            // Thanh toán thành công
            // C?p nh?t tr?ng thái hóa ??n
            using (var db = new BXMDEntities())
            {
                var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD.ToString() == request.OrderId);
                if (hoaDon != null)
                {
                    hoaDon.TrangThai = true; // ?ã thanh toán
                    db.SaveChanges();
                }
            }
        }

        return Ok();
    }
    catch (Exception ex)
    {
        return InternalServerError(ex);
    }
}
```

## Flow Hoàn Ch?nh

### 1. Desktop App (Client)
```
User ch?n MoMo ? T?o request ? Nh?n PayUrl ? M? browser
```

### 2. MoMo App/Web
```
User thanh toán ? MoMo x? lý ? Redirect v? ReturnUrl
```

### 3. Server (API)
```
Nh?n IPN callback ? Verify signature ? C?p nh?t database ? Response OK
```

### 4. Desktop App (Return)
```
User quay l?i app ? Check tr?ng thái ? Hi?n th? k?t qu?
```

## Testing

### Test v?i MoMo Test Environment

1. **S? ?i?n tho?i test**: 0969123456
2. **OTP test**: 123456
3. **Tài kho?n test**: Theo tài li?u MoMo

### Test Cases

- [ ] Thanh toán thành công
- [ ] Thanh toán th?t b?i (h?y)
- [ ] Thanh toán timeout
- [ ] Verify signature ?úng
- [ ] Verify signature sai
- [ ] X? lý duplicate callback
- [ ] X? lý callback delay

## L?u Ý B?o M?t

?? **QUAN TR?NG**:
1. **KHÔNG** commit Secret Key lên Git
2. L?u keys trong config file ho?c environment variables
3. S? d?ng HTTPS cho API endpoints
4. Validate signature t? MoMo callback
5. Log t?t c? transactions ?? audit

## Chuy?n Sang Production

Khi chuy?n sang môi tr??ng th?c:

1. ??ng ký tài kho?n MoMo Business
2. L?y PartnerCode, AccessKey, SecretKey production
3. Thay ??i API endpoint:
   ```
   https://payment.momo.vn/v2/gateway/api/create
   ```
4. C?p nh?t ReturnUrl và NotifyUrl th?c t?
5. Test k? trên môi tr??ng staging tr??c

## Tài Li?u Tham Kh?o

- [MoMo Payment Gateway Documentation](https://developers.momo.vn/)
- [API Integration Guide](https://developers.momo.vn/v3/docs/payment/api/wallet/onetime)
- [IPN/Webhook Guide](https://developers.momo.vn/v3/docs/payment/api/result-handling/notification)

## Status
?? **Ch? Tích H?p** - ?ã chu?n b? s?n UI, ch? implement logic thanh toán MoMo th?c t?
