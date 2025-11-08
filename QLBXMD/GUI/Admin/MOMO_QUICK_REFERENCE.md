# ?? MoMo Integration - Quick Reference

## ?? Quick Start

### 1. Cài ??t Package

Trong Visual Studio Package Manager Console:
```powershell
Install-Package Newtonsoft.Json -Version 13.0.3
```

Ho?c restore packages:
```powershell
Update-Package -reinstall
```

### 2. Test Thanh Toán MoMo

1. Run application
2. Ch?n chuy?n xe và gh?
3. Nh?p thông tin khách hàng
4. Click "??t vé"
5. Trong dialog ? Ch?n "?? Thanh toán MoMo"
6. Click "Xác nh?n"
7. Browser s? m? v?i MoMo payment page

### 3. Test Credentials

```
S? ?i?n tho?i: 0969123456
OTP: 123456
```

## ?? Files Quan Tr?ng

| File | Mô T? |
|------|-------|
| `Services\MoMoPaymentService.cs` | MoMo API service |
| `GUI\Admin\UCDatVe.cs` | Logic ??t vé |
| `GUI\Admin\XacNhanDatVeDialog.cs` | Dialog ch?n ph??ng th?c |

## ?? API Credentials (Test)

```csharp
PartnerCode: MOMO4LG620250601_TEST
AccessKey: e6g7W44vmy1rgT7K
SecretKey: XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa
Endpoint: https://test-payment.momo.vn/v2/gateway/api/create
```

## ?? Code Snippets

### T?o Payment Request

```csharp
var momoService = new MoMoPaymentService();
var response = await momoService.CreatePaymentRequest(
    orderId: "HD20241220123456",
    amount: 300000,
    orderInfo: "Thanh toan ve xe"
);

if (response.ResultCode == 0)
{
    // Success - Open payment page
    System.Diagnostics.Process.Start(response.PayUrl);
}
```

### Verify Callback Signature

```csharp
var momoService = new MoMoPaymentService();
bool isValid = momoService.VerifySignature(
    callbackRequest, 
    receivedSignature
);

if (isValid && callbackRequest.ResultCode == 0)
{
    // Payment successful
    // Update database
}
```

## ?? Troubleshooting

### L?i: "Could not load file Newtonsoft.Json"

**Fix:**
1. Close Visual Studio
2. Delete `bin` and `obj` folders
3. Restore packages
4. Rebuild

### L?i: Ph??ng th?c v?n là "Ti?n m?t"

**Fix:** ?ã s?a trong code m?i
```csharp
// ? L?y t? dialog ra ngoài using block
string phuongThucThanhToan = confirmDialog.PhuongThucThanhToan;
```

### MoMo Response: ResultCode != 0

**Check:**
1. Signature ?úng ch?a?
2. Amount > 0?
3. OrderId unique?
4. Internet connection?

## ?? Testing Checklist

- [ ] Cài ??t Newtonsoft.Json
- [ ] Build successful
- [ ] Ch?n MoMo trong dialog
- [ ] Console log: "Ph??ng th?c thanh toán ?ã ch?n: MoMo"
- [ ] Browser m? payment page
- [ ] Test payment v?i OTP 123456
- [ ] Check MoMo response trong console

## ?? Console Logs Example

### Success Flow

```
? Ph??ng th?c thanh toán ?ã ch?n: MoMo
=== MOMO REQUEST ===
Endpoint: https://test-payment.momo.vn/v2/gateway/api/create
Body: {"partnerCode":"MOMO4LG620250601_TEST",...}
=== MOMO RESPONSE ===
Status: OK
Content: {"resultCode":0,"payUrl":"https://test-payment.momo.vn/..."}
```

### Error Flow

```
? MoMo Error: ...
L?i k?t n?i MoMo: [Error Message]
```

## ?? Links

- [Full Documentation](TICH_HOP_MOMO_COMPLETE.md)
- [Installation Guide](INSTALL_NEWTONSOFT_JSON.md)
- [MoMo API Docs](https://developers.momo.vn/)

## ?? Limitations (Current)

1. ?? Ch?a có callback handler ? Không t? ??ng c?p nh?t DB
2. ?? Ch? test environment
3. ?? Ch?a có QR code option
4. ?? Ch?a có transaction logging

## ?? Next Steps

1. Implement callback API
2. Deploy to server
3. Config production credentials
4. Add transaction table
5. Implement status check

---

**Quick Links:**
- [MoMo Test Account](https://developers.momo.vn/v3/docs/payment/testing/test-credentials)
- [API Reference](https://developers.momo.vn/v3/docs/payment/api/wallet/onetime)
