# ? FIX HOÀN T?T: ??t vé thành công nh?ng không t?o ???c VeXe

## ? V?n ?? ban ??u

```
1. Hóa ??n ???c t?o (MaHD = 20)
2. ? VeXe KHÔNG ???c t?o
3. TongTien = 0 (vì không có vé)
4. L?i: "An error occurred while updating the entries"
```

---

## ?? Nguyên nhân

### 1. Code b? duplicate properties
```csharp
var hoaDon = new HoaDon
{
    MANV = null, // ? Dòng này
    MANV = null, // ? Duplicate!
    TongTien = 120000, // ? Dòng c?
    TongTien = 0, // ? Duplicate!
    TrangThai = true, // ? Dòng c?
    TrangThai = false // ? Duplicate!
};
```

### 2. Trigger `trg_KiemTraNgayDatVe` ROLLBACK transaction

```sql
CREATE TRIGGER trg_KiemTraNgayDatVe
ON VeXe
AFTER INSERT AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM VeXe vx
        JOIN HoaDon hd ON hd.MaHD = vx.MaHD
        JOIN ChuyenXe cx ON cx.MaChuyen = vx.MaChuyen
        WHERE hd.NgayLap > cx.NgayGioKhoiHanh  -- ? Check này fail!
    )
    BEGIN
        RAISERROR (N'Ngày l?p hóa ??n ph?i nh? h?n ngày gi? kh?i hành',16,1)
        ROLLBACK TRANSACTION  -- ? ROLLBACK ? Vé không ???c t?o!
        RETURN;
    END
END;
```

**Ví d? l?i:**
```
NgayLap = 08/11/2025 00:00 (hôm nay)
NgayGioKhoiHanh = 22/04/2025 12:00 (trong quá kh?)
? 08/11/2025 > 22/04/2025 ? ROLLBACK!
```

### 3. Không có GiaVe record trong database

Trigger `trg_UPDATEGiaVe` c?n record trong b?ng `GiaVe`:
```sql
INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX
    AND gv.MaLX = x.MaLX
    AND gv.MaTuyen = cx.MaTuyen;
```

N?u không có ? trigger không update gì ? có th? fail

---

## ? Gi?i pháp ?ã áp d?ng

### Fix 1: Xóa duplicate properties

```csharp
var hoaDon = new HoaDon
{
    MaKH = khachHang.MaKH,
    MANV = null, // ? Ch? 1 l?n
    NgayLap = DateTime.Now,
    TongTien = 0, // ? Ch? 1 l?n
    TrangThai = false // ? Ch? 1 l?n
};
```

### Fix 2: Validate ngày kh?i hành TR??C KHI ??t vé

```csharp
// ? 1.5. Ki?m tra ngày kh?i hành ph?i l?n h?n hi?n t?i
if (chuyenXe.NgayGioKhoiHanh <= DateTime.Now)
{
    MessageBox.Show(
        $"Không th? ??t vé cho chuy?n xe ?ã kh?i hành.\n\n" +
        $"Ngày kh?i hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}\n" +
        $"Hi?n t?i: {DateTime.Now:dd/MM/yyyy HH:mm}",
        "L?i",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
    return;
}
```

### Fix 3: Validate có GiaVe record không

```csharp
// ? 1.6. Ki?m tra có c?u hình giá vé trong database không
var xe = db.Xes.FirstOrDefault(x => x.MaXe == chuyenXe.MaXe);
var giaVeRecord = db.GiaVes.FirstOrDefault(gv => 
    gv.MaNX == xe.MaNX && 
    gv.MaLX == xe.MaLX && 
    gv.MaTuyen == chuyenXe.MaTuyen
);

if (giaVeRecord == null)
{
    MessageBox.Show(
        $"Chuy?n xe này ch?a có giá vé ???c c?u hình.\n\n" +
        $"Vui lòng liên h? qu?n tr? viên.",
        "L?i",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
    return;
}
```

### Fix 4: Ch? hi?n th? chuy?n xe h?p l? trong LoadChuyenXe()

```csharp
var query = (from cx in db.ChuyenXes
            join x in db.Xes on cx.MaXe equals x.MaXe
            join nx in db.NhaXes on x.MaNX equals nx.MaNX
            join lx in db.LoaiXes on x.MaLX equals lx.MaLX
            join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
            where td.DiemDi == selectedDiemDi
                  && td.DiemDen == selectedDiemDen
                  && System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
                  && cx.NgayGioKhoiHanh > DateTime.Now // ? CH? CHUY?N CH?A KH?I HÀNH
            select new { ... }).ToList();

// ? Filter ch? l?y chuy?n có GiaVe
var resultsWithPrice = query.Where(q =>
{
    var hasGiaVe = db.GiaVes.Any(gv =>
        gv.MaNX == q.MaNX &&
        gv.MaLX == q.MaLoaiXe &&
        gv.MaTuyen == q.MaTuyen
    );
    return hasGiaVe;
}).Select(q => new
{
    // ...
    GiaVe = db.GiaVes
        .Where(gv => gv.MaNX == q.MaNX && gv.MaLX == q.MaLoaiXe && gv.MaTuyen == q.MaTuyen)
        .Select(gv => (decimal?)gv.GiaVe1) // ? GiaVe1 là property th?c t? trong EF
        .FirstOrDefault() ?? 0
}).ToList();
```

### Fix 5: C?i thi?n error handling

```csharp
catch (Exception exVe)
{
    var innerMsg = exVe.InnerException?.InnerException?.Message 
                   ?? exVe.InnerException?.Message 
                   ?? "";
    MessageBox.Show(
        $"L?i t?o vé xe:\n{exVe.Message}\n\nChi ti?t:\n{innerMsg}",
        "L?i",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
    Console.WriteLine($"? L?i VeXe: {exVe.ToString()}");
    return;
}
```

---

## ?? Ki?m tra l?i database

### 1. Xóa hóa ??n rác (MaHD = 20, TongTien = 0)

```sql
-- Xóa hóa ??n không có vé
DELETE FROM HoaDon WHERE MaHD = 20 AND TongTien = 0;
```

### 2. Ki?m tra d? li?u chuy?n xe

```sql
-- Xem chuy?n xe nào còn h?p l? (trong t??ng lai)
SELECT 
    cx.MaChuyen,
    td.DiemDi,
    td.DiemDen,
    cx.NgayGioKhoiHanh,
    GETDATE() AS NgayHienTai,
    CASE 
        WHEN cx.NgayGioKhoiHanh > GETDATE() THEN N'OK - Có th? ??t'
        ELSE N'FAIL - ?ã kh?i hành'
    END AS TrangThai
FROM ChuyenXe cx
JOIN TuyenDuong td ON td.MaTuyen = cx.MaTuyen;
```

### 3. Ki?m tra GiaVe

```sql
-- Xem chuy?n nào có giá vé
SELECT 
    cx.MaChuyen,
    x.MaNX,
    x.MaLX,
    cx.MaTuyen,
    gv.GiaVe,
    CASE 
        WHEN gv.MaGV IS NULL THEN N'MISSING - Ch?a có giá'
        ELSE N'OK - Có giá'
    END AS TrangThaiGiaVe
FROM ChuyenXe cx
JOIN Xe x ON x.MaXe = cx.MaXe
LEFT JOIN GiaVe gv ON gv.MaNX = x.MaNX 
    AND gv.MaLX = x.MaLX 
    AND gv.MaTuyen = cx.MaTuyen;
```

### 4. Thêm chuy?n xe test trong t??ng lai

```sql
-- Thêm chuy?n m?i trong t??ng lai
DECLARE @NgayMai DATE = DATEADD(DAY, 1, GETDATE());

INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh)
VALUES 
(1, 1, 1, CAST(@NgayMai AS DATETIME) + CAST('08:00:00' AS DATETIME)),
(2, 2, 2, CAST(@NgayMai AS DATETIME) + CAST('10:00:00' AS DATETIME)),
(3, 3, 3, CAST(@NgayMai AS DATETIME) + CAST('14:00:00' AS DATETIME));

-- Ki?m tra ?ã insert ch?a
SELECT * FROM ChuyenXe WHERE CAST(NgayGioKhoiHanh AS DATE) = @NgayMai;
```

---

## ?? Các thay ??i trong code

### File: `QLBXMD\GUI\Admin\UCDatVe.cs`

1. ? **LoadChuyenXe()**: 
   - Thêm filter `cx.NgayGioKhoiHanh > DateTime.Now`
   - Join v?i `GiaVes` ?? ch? hi?n th? chuy?n có giá vé
   - Fix property name: `gv.GiaVe` ? `gv.GiaVe1`

2. ? **btnDatVe_Click()**:
   - Xóa duplicate property assignments
   - Thêm validation check `NgayGioKhoiHanh > DateTime.Now`
   - Thêm validation check `GiaVe` record t?n t?i
   - C?i thi?n error message v?i innermost exception

3. ? **Error handling**:
   - Hi?n th? chi ti?t l?i t? `InnerException.InnerException.Message`
   - Log full stack trace vào Console

---

## ? K?t qu?

### Tr??c khi fix:
```
? Hóa ??n t?o thành công (MaHD = 20)
? VeXe KHÔNG ???c t?o
? TongTien = 0
? Error: "An error occurred while updating the entries"
```

### Sau khi fix:
```
? Ch? hi?n th? chuy?n xe h?p l? (ch?a kh?i hành + có giá vé)
? Validate ??y ?? tr??c khi ??t vé
? Hóa ??n ???c t?o v?i TrangThai = false (ch?a thanh toán)
? VeXe ???c t?o thành công
? Trigger t? ??ng tính TongTien
? Hi?n th? thông báo thành công v?i ??y ?? thông tin
```

---

## ?? Test Case

### Input:
```
- ?i?m ?i: B?n xe Mi?n ?ông
- ?i?m ??n: ?à L?t
- Ngày ?i: [Ngày mai]
- Gh?: 5, 6
- Khách hàng: pham le anh khoa (0123456789)
```

### Expected Output:
```
? ?ã t?o KH m?i: MaKH = 23
? ?ã t?o hóa ??n: MaHD = 21
? ?ã t?o 2 vé xe

??t vé thành công!
Mã hóa ??n: 21
S? l??ng vé: 2
T?ng ti?n: 240,000 VN?
```

### Database Result:
```sql
-- HoaDon
MaHD | MaKH | MANV | TongTien | TrangThai
21   | 23   | NULL | 240000   | 0 (ch?a thanh toán)

-- VeXe
MaVe | MaChuyen | MaHD | ChoNgoi | GiaVe
XX   | [ID]     | 21   | 5       | 120000
XX   | [ID]     | 21   | 6       | 120000
```

---

## ?? Công c? debug

### 1. Console logs

```csharp
Console.WriteLine("=== B?T ??U ??T VÉ ===");
Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}");
Console.WriteLine($"? ?ã t?o KH m?i: MaKH = {khachHang.MaKH}");
Console.WriteLine($"? ?ã t?o hóa ??n: MaHD = {hoaDon.MaHD}");
Console.WriteLine($"? ?ã t?o {selectedSeats.Count} vé xe");
```

### 2. SQL queries ?? debug

```sql
-- 1. Ki?m tra chuy?n xe
SELECT * FROM ChuyenXe WHERE MaChuyen = [ID];

-- 2. Ki?m tra có GiaVe không
SELECT gv.* 
FROM ChuyenXe cx
JOIN Xe x ON x.MaXe = cx.MaXe
LEFT JOIN GiaVe gv ON gv.MaNX = x.MaNX 
    AND gv.MaLX = x.MaLX 
    AND gv.MaTuyen = cx.MaTuyen
WHERE cx.MaChuyen = [ID];

-- 3. Ki?m tra ngày gi?
SELECT 
    cx.NgayGioKhoiHanh,
    GETDATE() AS NgayHienTai,
    CASE 
        WHEN cx.NgayGioKhoiHanh > GETDATE() THEN 'OK'
        ELSE 'FAIL'
    END
FROM ChuyenXe cx
WHERE cx.MaChuyen = [ID];
```

---

## ? Checklist hoàn thành

- [x] ? Fix duplicate property assignment
- [x] ? Validate `NgayGioKhoiHanh > DateTime.Now`
- [x] ? Validate `GiaVe` record t?n t?i
- [x] ? C?i thi?n error message
- [x] ? Ch? hi?n th? chuy?n xe h?p l?
- [x] ? Fix property name `GiaVe1` trong Entity Framework
- [x] ? Build successful

**Status:** ? **READY TO TEST!**
