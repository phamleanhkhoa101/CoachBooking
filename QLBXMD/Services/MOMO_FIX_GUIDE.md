# ?? H??NG D?N S?A L?I VÀ TEST MOMO

## ? L?i g?p ph?i

**Mã l?i: 11007**
```
L?i t?o yêu c?u thanh toán MoMo:
Mã l?i: 11007
Thông báo: Ch? ký không h?p l?. Vui lòng ki?m tra d? li?u g?c tr??c khi mã hóa
```

## ? Nguyên nhân

Signature (ch? ký) ???c t?o **SAI TH? T?** ho?c **SAI FORMAT** so v?i yêu c?u c?a MoMo API v2.1.

### Sai:
```csharp
// ? Thi?u "accessKey=" prefix và sai th? t?
var rawHash = $"accessKey={AccessKey}" +
              $"&amount={amount}" + // Thi?u prefix
              ...
```

### ?úng:
```csharp
// ? ??y ?? prefix và ?úng th? t? alphabet
var rawSignature = "accessKey=" + AccessKey +
                   "&amount=" + amount +
                   "&extraData=" + extraData +
                   "&ipnUrl=" + NotifyUrl +
                   "&orderId=" + orderId +
                   "&orderInfo=" + orderInfo +
                   "&partnerCode=" + PartnerCode +
                   "&redirectUrl=" + ReturnUrl +
                   "&requestId=" + requestId +
                   "&requestType=" + requestType;
```

## ?? Cách t?o Signature ?úng theo MoMo API v2.1

### 1. **Th? t? tham s? (theo alphabet)**:
```
accessKey
amount
extraData
ipnUrl
orderId
orderInfo
partnerCode
redirectUrl
requestId
requestType
```

### 2. **Format chu?i raw signature**:
```
accessKey=xxx&amount=xxx&extraData=xxx&ipnUrl=xxx&orderId=xxx&orderInfo=xxx&partnerCode=xxx&redirectUrl=xxx&requestId=xxx&requestType=xxx
```

### 3. **Tính HMAC-SHA256**:
```csharp
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
```

## ?? ?ã s?a

### File: `QLBXMD\Services\MoMoPaymentService.cs`

**Thay ??i chính:**
1. ? S?a cách t?o `rawSignature` ?úng format
2. ? ??m b?o ?úng th? t? alphabet
3. ? Thêm debug log ?? ki?m tra
4. ? S?a verify signature cho callback

**Code ?ã s?a:**
```csharp
// ? T?o ch? ký theo ?úng format MoMo API v2.1
var rawSignature = "accessKey=" + AccessKey +
                   "&amount=" + amount +
                   "&extraData=" + extraData +
                   "&ipnUrl=" + NotifyUrl +
                   "&orderId=" + orderId +
                   "&orderInfo=" + orderInfo +
                   "&partnerCode=" + PartnerCode +
                   "&redirectUrl=" + ReturnUrl +
                   "&requestId=" + requestId +
                   "&requestType=" + requestType;

var signature = ComputeHmacSha256(rawSignature, SecretKey);
```

## ?? Cách Test

### 1. **Ch?y l?i ?ng d?ng**
```
F5 ho?c Ctrl+F5 trong Visual Studio
```

### 2. **Th? thanh toán MoMo**
- Ch?n chuy?n xe
- Ch?n gh?
- Click "??t vé"
- Ch?n "MoMo" trong dialog
- Click "Xác nh?n"

### 3. **Ki?m tra Console Log**
Sau khi click "Xác nh?n", ki?m tra Output window (Debug ho?c Console):

```
=== MOMO SIGNATURE DEBUG ===
Raw Signature: accessKey=e6g7W44vmy1rgT7K&amount=250000&extraData=&ipnUrl=...
Computed Signature: abc123def456...

=== MOMO REQUEST ===
Endpoint: https://test-payment.momo.vn/v2/gateway/api/create
Body: {"partnerCode":"MOMO4LG620250601_TEST",...}

=== MOMO RESPONSE ===
Status: OK
Content: {"resultCode":0,"message":"Thành công",...}
```

### 4. **K?t qu? mong ??i**
- ? `resultCode = 0` ? Thành công
- ? `payUrl` ???c tr? v?
- ? Trình duy?t m? trang thanh toán MoMo

## ?? Debug n?u v?n l?i

### N?u v?n l?i 11007:

1. **Ki?m tra credentials**:
```csharp
// Trong MoMoPaymentService.cs
private const string PartnerCode = "MOMO4LG620250601_TEST";
private const string AccessKey = "e6g7W44vmy1rgT7K";
private const string SecretKey = "XAAT9fHRPeJD8zhjKsZnTTUSnyEHFISa";
```

? ??m b?o **?ÚNG** v?i thông tin MoMo cung c?p.

2. **Ki?m tra format amount**:
```csharp
// Amount ph?i là s? nguyên (VN?)
long amount = 250000; // ? ?úng
decimal amount = 250000.5m; // ? Sai
```

3. **Ki?m tra extraData**:
```csharp
// extraData có th? ?? tr?ng ""
string extraData = "";
```

4. **So sánh v?i example MoMo**:
```
Raw signature MoMo example:
accessKey=F8BBA842ECF85&amount=50000&extraData=&ipnUrl=https://webhook.site/...

Raw signature c?a b?n:
accessKey=e6g7W44vmy1rgT7K&amount=250000&extraData=&ipnUrl=http://localhost:5000/...
```

## ?? L?u ý quan tr?ng

### 1. **URL ph?i là HTTPS khi production**
```csharp
// ? Không dùng HTTP trong production
private const string ReturnUrl = "http://localhost:5000/payment/return";

// ? Dùng HTTPS
private const string ReturnUrl = "https://yourdomain.com/payment/return";
```

### 2. **Test credentials**
- `MOMO4LG620250601_TEST` là tài kho?n TEST
- Ch? dùng ?? test, không dùng production
- Khi production c?n ??ng ký tài kho?n MoMo Business

### 3. **IPN/Callback URL**
- Ph?i là URL public (không ph?i localhost)
- MoMo s? g?i ??n URL này khi thanh toán thành công
- C?n deploy API endpoint ?? nh?n callback

## ?? Flow thanh toán MoMo hoàn ch?nh

```
1. User click "Thanh toán MoMo"
   ?
2. App t?o signature và g?i MoMo API
   ?
3. MoMo tr? v? payUrl
   ?
4. M? trình duy?t v?i payUrl
   ?
5. User thanh toán trên MoMo
   ?
6. MoMo redirect v? returnUrl (User th?y)
   ?
7. MoMo g?i IPN ??n notifyUrl (Server x? lý)
   ?
8. App c?p nh?t database và thông báo user
```

## ? Checklist sau khi s?a

- [x] S?a format signature
- [x] Thêm debug log
- [x] Build thành công
- [ ] Test thanh toán
- [ ] Ki?m tra console log
- [ ] Verify payUrl ???c t?o
- [ ] Test thanh toán trên MoMo sandbox

## ?? Next Steps

1. **Test ngay bây gi?** - Ch?y l?i app và th? thanh toán
2. **Ki?m tra log** - Xem console có signature ?úng không
3. **Deploy callback API** - T?o endpoint nh?n IPN t? MoMo
4. **Implement callback handler** - C?p nh?t database khi thanh toán thành công

## ?? H? tr?

N?u v?n l?i, cung c?p thông tin:
1. Console log (Raw Signature + Response)
2. Request body JSON
3. Response t? MoMo
4. Mã l?i c? th?

---

**? L?u ý:** Sau khi s?a, signature s? ???c t?o ?úng format và l?i 11007 s? ???c gi?i quy?t!
