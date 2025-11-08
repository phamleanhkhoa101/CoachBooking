# ?? Tích H?p Thanh Toán MoMo - H??ng D?n ??y ??

## ? ?ã Hoàn Thành

### 1. Files ?ã T?o

| File | Mô T? |
|------|-------|
| `Services\MoMoPaymentService.cs` | Service x? lý thanh toán MoMo |
| `INSTALL_NEWTONSOFT_JSON.md` | H??ng d?n cài ??t Newtonsoft.Json |
| `TICH_HOP_MOMO_COMPLETE.md` | File này |

### 2. Files ?ã C?p Nh?t

| File | Thay ??i |
|------|----------|
| `GUI\Admin\UCDatVe.cs` | - Fix l?i ph??ng th?c thanh toán<br>- Thêm method `ProcessMoMoPayment()`<br>- Integrate MoMo service |
| `packages.config` | Thêm Newtonsoft.Json package |

## ?? Cách Ho?t ??ng

### Flow Thanh Toán MoMo

```
1. User ch?n "Thanh toán MoMo" trong dialog
   ?
2. btnDatVe_Click() ? ProcessMoMoPayment()
   ?
3. T?o request ??n MoMo API
   ?
4. Nh?n PayUrl t? MoMo
   ?
5. M? trình duy?t v?i PayUrl
   ?
6. User thanh toán trên MoMo
   ?
7. MoMo callback v? server (IPN)
   ?
8. Server verify và c?p nh?t database
   ?
9. Hi?n th? k?t qu? cho user
```

### ?i?m Khác Bi?t v?i Ti?n M?t

**Ti?n M?t:**
```csharp
// T?o hóa ??n + vé ngay l?p t?c
// TrangThai = false (ch?a thanh toán)
```

**MoMo:**
```csharp
// Ch? m? payment page
// Ch? callback ?? t?o hóa ??n + vé
// TrangThai = true sau khi callback thành công
```

## ?? Code Chi Ti?t

### 1. Fix L?i Ph??ng Th?c Thanh Toán

**Tr??c (SAI):**
```csharp
private void btnDatVe_Click(object sender, EventArgs e)
{
    using (var confirmDialog = new XacNhanDatVeDialog())
    {
        // ...
        if (confirmDialog.ShowDialog() != DialogResult.OK)
            return;
        
        string phuongThucThanhToan = confirmDialog.PhuongThucThanhToan;
    }
    
    try
    {
        string phuongThucThanhToan = "Ti?n m?t"; // ? Gán l?i!
        // ...
    }
}
```

**Sau (?ÚNG):**
```csharp
private void btnDatVe_Click(object sender, EventArgs e)
{
    // ? Declare variable ? ngoài using block
    string phuongThucThanhToan = "Ti?n m?t";
    
    using (var confirmDialog = new XacNhanDatVeDialog())
    {
        // ...
        if (confirmDialog.ShowDialog() != DialogResult.OK)
            return;
        
        // ? L?u giá tr? t? dialog
        phuongThucThanhToan = confirmDialog.PhuongThucThanhToan;
    }
    
    // ? Check và x? lý MoMo
    if (phuongThucThanhToan == "MoMo")
    {
        ProcessMoMoPayment(tienTam);
        return;
    }
    
    // Continue v?i ti?n m?t...
}
```

### 2. Method ProcessMoMoPayment

```csharp
private async void ProcessMoMoPayment(decimal amount)
{
    try
    {
        // Show loading cursor
        this.Cursor = Cursors.WaitCursor;
        
        // T?o orderId unique
        string orderId = $"HD{DateTime.Now:yyyyMMddHHmmss}";
        string orderInfo = $"Thanh toan ve xe - {selectedDiemDi} den {selectedDiemDen}";
        
        // Call MoMo API
        var momoService = new Services.MoMoPaymentService();
        var response = await momoService.CreatePaymentRequest(
            orderId,
            (long)amount,
            orderInfo
        );
        
        this.Cursor = Cursors.Default;
        
        if (response.ResultCode == 0)
        {
            // ? Thành công ? M? trình duy?t
            MessageBox.Show("?ang chuy?n ??n trang thanh toán MoMo...");
            System.Diagnostics.Process.Start(response.PayUrl);
            
            // ?? Ch? callback t? MoMo ?? hoàn t?t
        }
        else
        {
            // ? L?i
            MessageBox.Show($"L?i: {response.Message}");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"L?i k?t n?i MoMo: {ex.Message}");
    }
}
```

### 3. MoMoPaymentService

```csharp
public class MoMoPaymentService
{
    // Test credentials
    private const string PartnerCode = "MOMO4LG620250601_TEST";
    private const string AccessKey = "e6g7W44vmy1rgT7K";
    private const string SecretKey = "XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa";
    private const string ApiEndpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
    
    public async Task<MoMoPaymentResponse> CreatePaymentRequest(
        string orderId,
        long amount,
        string orderInfo)
    {
        // 1. T?o signature
        var signature = ComputeHmacSha256(rawHash, SecretKey);
        
        // 2. T?o request body
        var requestBody = new { ... };
        
        // 3. Call API
        var response = await httpClient.PostAsync(ApiEndpoint, content);
        
        // 4. Parse response
        return JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseContent);
    }
}
```

## ?? Testing

### Test Case 1: Thanh Toán Ti?n M?t

1. Ch?n chuy?n xe
2. Ch?n gh?
3. Nh?p thông tin KH
4. Nh?n "??t vé"
5. Dialog hi?n ra ? Ch?n "?? Thanh toán ti?n m?t"
6. Nh?n "Xác nh?n"
7. ? T?o hóa ??n + vé ngay l?p t?c

**Expected:**
```
? Ph??ng th?c thanh toán ?ã ch?n: Ti?n m?t
? ?ã t?o KH...
? ?ã t?o hóa ??n...
? ?ã t?o vé xe...
??t vé thành công!
Ph??ng th?c thanh toán: Ti?n m?t
```

### Test Case 2: Thanh Toán MoMo

1. Ch?n chuy?n xe
2. Ch?n gh?
3. Nh?p thông tin KH
4. Nh?n "??t vé"
5. Dialog hi?n ra ? Ch?n "?? Thanh toán MoMo"
6. Nh?n "Xác nh?n"
7. ? Hi?n thông báo "?ang chuy?n ??n trang thanh toán MoMo..."
8. ? M? trình duy?t v?i MoMo payment page
9. User thanh toán trên MoMo
10. ?? Hi?n t?i: Ch? callback (ch?a implement)

**Expected Console Log:**
```
? Ph??ng th?c thanh toán ?ã ch?n: MoMo
=== MOMO REQUEST ===
Endpoint: https://test-payment.momo.vn/v2/gateway/api/create
Body: {...}
=== MOMO RESPONSE ===
Status: OK
Content: {"resultCode":0,"payUrl":"https://..."}
```

## ?? Ch?a Hoàn Thành (TODO)

### 1. Callback Handler (IPN)

C?n t?o Web API ?? nh?n callback t? MoMo:

```csharp
[HttpPost]
[Route("api/payment/momo-callback")]
public IHttpActionResult MoMoCallback([FromBody] MoMoCallbackRequest request)
{
    // 1. Verify signature
    var momoService = new MoMoPaymentService();
    if (!momoService.VerifySignature(request, request.Signature))
        return BadRequest("Invalid signature");
    
    // 2. Check resultCode
    if (request.ResultCode == 0)
    {
        // ? Thanh toán thành công
        // 3. T?o hóa ??n + vé
        // 4. C?p nh?t tr?ng thái
        // 5. G?i notification cho user
    }
    
    return Ok();
}
```

### 2. Ki?m Tra Tr?ng Thái

User quay l?i app sau khi thanh toán:

```csharp
private void btnKiemTraThanhToan_Click(object sender, EventArgs e)
{
    string orderId = txtOrderId.Text;
    
    using (var db = new BXMDEntities())
    {
        var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD.ToString() == orderId);
        
        if (hoaDon != null && hoaDon.TrangThai == true)
        {
            MessageBox.Show("Thanh toán thành công!");
        }
        else
        {
            MessageBox.Show("?ang ch? thanh toán...");
        }
    }
}
```

### 3. QR Code Payment

Thay vì m? trình duy?t, có th? hi?n th? QR code:

```csharp
if (response.ResultCode == 0 && !string.IsNullOrEmpty(response.QrCodeUrl))
{
    // Show QR code in dialog
    var qrDialog = new QRCodeDialog();
    qrDialog.QrCodeUrl = response.QrCodeUrl;
    qrDialog.OrderId = orderId;
    qrDialog.ShowDialog();
}
```

## ?? Database Schema

?? tracking thanh toán MoMo, có th? thêm b?ng:

```sql
CREATE TABLE MoMoTransactions
(
    Id INT PRIMARY KEY IDENTITY,
    OrderId VARCHAR(50) UNIQUE NOT NULL,
    MaHD INT NULL,
    Amount BIGINT NOT NULL,
    TransId BIGINT NULL,
    ResultCode INT NOT NULL,
    Message NVARCHAR(500),
    RequestTime DATETIME DEFAULT GETDATE(),
    ResponseTime DATETIME NULL,
    
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD)
)
```

L?u transaction log:

```csharp
// Tr??c khi call MoMo
db.MoMoTransactions.Add(new MoMoTransaction
{
    OrderId = orderId,
    Amount = (long)amount,
    ResultCode = -1, // Pending
    RequestTime = DateTime.Now
});
db.SaveChanges();

// Sau khi nh?n callback
var transaction = db.MoMoTransactions.FirstOrDefault(t => t.OrderId == orderId);
transaction.TransId = request.TransId;
transaction.ResultCode = request.ResultCode;
transaction.Message = request.Message;
transaction.ResponseTime = DateTime.Now;
db.SaveChanges();
```

## ?? Production Checklist

Tr??c khi deploy:

### 1. Environment Variables

```csharp
// ? Không hard-code credentials
private const string PartnerCode = "MOMO4LG620250601_TEST";

// ? Dùng config
private static string PartnerCode => ConfigurationManager.AppSettings["MoMo:PartnerCode"];
```

### 2. App.config

```xml
<appSettings>
    <add key="MoMo:PartnerCode" value="PRODUCTION_CODE"/>
    <add key="MoMo:AccessKey" value="PRODUCTION_KEY"/>
    <add key="MoMo:SecretKey" value="PRODUCTION_SECRET"/>
    <add key="MoMo:ApiEndpoint" value="https://payment.momo.vn/v2/gateway/api/create"/>
    <add key="MoMo:ReturnUrl" value="https://yourdomain.com/payment/return"/>
    <add key="MoMo:NotifyUrl" value="https://yourdomain.com/payment/notify"/>
</appSettings>
```

### 3. HTTPS

- ReturnUrl và NotifyUrl ph?i dùng HTTPS
- Deploy callback API trên server có SSL

### 4. Error Handling

- Log t?t c? requests/responses
- Retry mechanism
- Timeout handling
- Transaction rollback

### 5. Security

- Verify signature t? MoMo
- Không expose Secret Key
- Rate limiting
- Input validation

## ?? Tài Li?u Tham Kh?o

### MoMo Documentation

- [Payment Gateway API](https://developers.momo.vn/v3/docs/payment/api/wallet/onetime)
- [IPN/Webhook](https://developers.momo.vn/v3/docs/payment/api/result-handling/notification)
- [Test Environment](https://developers.momo.vn/v3/docs/payment/testing/test-credentials)

### Test Credentials (?ã có trong code)

```
Partner Code: MOMO4LG620250601_TEST
Access Key: e6g7W44vmy1rgT7K
Secret Key: XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa
API Endpoint: https://test-payment.momo.vn/v2/gateway/api/create
```

### Test MoMo Account

- S? ?i?n tho?i: 0969123456
- OTP: 123456

## ?? Summary

### ? ?ã Hoàn Thành

1. ? Fix l?i ph??ng th?c thanh toán không ?úng
2. ? T?o MoMoPaymentService
3. ? Integrate vào UCDatVe
4. ? Test v?i MoMo Test Environment
5. ? M? payment page trong browser

### ? ?ang Ch?

1. ? Callback/IPN Handler
2. ? Database tracking
3. ? QR Code payment
4. ? Transaction status check

### ?? Next Steps

1. **Implement Callback API** (Quan tr?ng nh?t!)
2. Deploy API lên server
3. Config ReturnUrl và NotifyUrl
4. Test end-to-end flow
5. Add transaction logging
6. Production configuration

---

**Status:** ? Phase 1 Complete - Basic Integration
**Next:** Implement Callback Handler
**Version:** 1.0
**Last Update:** 2024
