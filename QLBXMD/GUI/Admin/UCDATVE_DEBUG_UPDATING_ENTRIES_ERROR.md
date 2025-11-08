# ?? Ch?n ?oán l?i "An error occurred while updating the entries"

## ? Tri?u ch?ng

```
L?i t?o vé xe:
An error occurred while updating the entries. See the inner exception for details.
An error occurred while updating the entries. See the inner exception for details.
```

- ? Hóa ??n ???c t?o thành công (MaHD = 20)
- ? VeXe không ???c t?o
- TongTien = 0 (vì không có vé)

---

## ?? Nguyên nhân có th?

### 1. Trigger `trg_UPDATEGiaVe` không tìm th?y GiaVe

**SQL Trigger:**
```sql
CREATE TRIGGER trg_UPDATEGiaVe
ON VeXe
AFTER INSERT, UPDATE AS
BEGIN
    UPDATE vx 
    SET vx.GiaVe = gv.GiaVe  -- ? N?u gv.GiaVe không t?n t?i ? NULL ? fail
    FROM VeXe vx
    INNER JOIN inserted i ON i.MaVe = vx.MaVe
    INNER JOIN ChuyenXe cx ON cx.MaChuyen = vx.MaChuyen
    INNER JOIN Xe x ON x.MaXe = cx.MaXe
    INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX
        AND gv.MaLX = x.MaLX
        AND gv.MaTuyen = cx.MaTuyen;  -- ? INNER JOIN ? không match = không update
END;
```

**V?n ??:** 
- N?u b?ng `GiaVe` không có record v?i `(MaNX, MaLX, MaTuyen)` match v?i chuy?n xe
- Trigger s? không update `GiaVe` ? có th? ?? `NULL`
- Nh?ng code ?ang set `GiaVe = selectedGiaVe.Value` nên không ph?i NULL issue

### 2. Trigger `trg_KiemTraNgayDatVe` fail

**SQL Trigger:**
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
        WHERE hd.NgayLap > cx.NgayGioKhoiHanh  -- ? Ki?m tra này
    )
    BEGIN
        RAISERROR (N'Ngày l?p hóa ??n ph?i nh? h?n ngày gi? kh?i hành c?a chuy?n xe',16,1)
        ROLLBACK TRANSACTION
        RETURN;
    END
END;
```

**V?n ??:**
- N?u `NgayLap` (hi?n t?i) > `NgayGioKhoiHanh` (trong quá kh?)
- Trigger s? **ROLLBACK** ? Vé không ???c t?o!

**Ví d?:**
```
NgayLap = 08/11/2025 00:00:00 (hôm nay)
NgayGioKhoiHanh = 22/04/2025 12:00:00 (trong quá kh?)
? NgayLap > NgayGioKhoiHanh ? ROLLBACK!
```

---

## ? Gi?i pháp

### Fix 1: Validate tr??c khi ??t vé (trong C#)

```csharp
// ? 1. Ki?m tra chuy?n xe còn t?n t?i không
var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
if (chuyenXe == null)
{
    MessageBox.Show("Chuy?n xe không t?n t?i.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}

// ? 2. Ki?m tra ngày kh?i hành ph?i l?n h?n hi?n t?i
if (chuyenXe.NgayGioKhoiHanh <= DateTime.Now)
{
    MessageBox.Show(
        $"Chuy?n xe này ?ã kh?i hành (ho?c s?p kh?i hành).\n" +
        $"Ngày kh?i hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}\n" +
        $"Không th? ??t vé.",
        "L?i",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
    return;
}

// ? 3. Ki?m tra có GiaVe trong database không
var xe = db.Xes.FirstOrDefault(x => x.MaXe == chuyenXe.MaXe);
if (xe == null)
{
    MessageBox.Show("Không tìm th?y thông tin xe.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}

var giaVeRecord = db.GiaVes.FirstOrDefault(gv => 
    gv.MaNX == xe.MaNX && 
    gv.MaLX == xe.MaLX && 
    gv.MaTuyen == chuyenXe.MaTuyen
);

if (giaVeRecord == null)
{
    MessageBox.Show(
        "Chuy?n xe này ch?a có giá vé ???c c?u hình.\n" +
        "Vui lòng liên h? qu?n tr? viên.",
        "L?i",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
    return;
}
```

### Fix 2: Ch? hi?n th? chuy?n xe h?p l? trong `LoadChuyenXe()`

```csharp
var query = from cx in db.ChuyenXes
            join x in db.Xes on cx.MaXe equals x.MaXe
            join nx in db.NhaXes on x.MaNX equals nx.MaNX
            join lx in db.LoaiXes on x.MaLX equals lx.MaLX
            join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
            join gv in db.GiaVes on new { x.MaNX, x.MaLX, cx.MaTuyen } equals new { gv.MaNX, gv.MaLX, gv.MaTuyen }  // ? Thêm join GiaVe
            where td.DiemDi == selectedDiemDi
                  && td.DiemDen == selectedDiemDen
                  && System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
                  && cx.NgayGioKhoiHanh > DateTime.Now  // ? Ch? hi?n th? chuy?n ch?a kh?i hành
            select new
            {
                MaChuyen = cx.MaChuyen,
                MaXe = x.MaXe,
                MaLoaiXe = lx.MaLX,
                KhoiHanh = cx.NgayGioKhoiHanh,
                DenDuKien = cx.NgayGioDenDuKien,
                TenNX = nx.TenNX,
                TenLX = lx.TenLX,
                SoCho = lx.SoCho,
                GiaVe = gv.GiaVe  // ? L?y t? GiaVe table thay vì ChuyenXe.GiaVeThucTe
            };
```

---

## ?? Debug Steps

### 1. Ki?m tra d? li?u trong database

```sql
-- 1. Ki?m tra chuy?n xe b?n ?ang ??t
SELECT * FROM ChuyenXe WHERE MaChuyen = 2;

-- 2. Ki?m tra xe c?a chuy?n ?ó
SELECT x.*, cx.MaTuyen 
FROM ChuyenXe cx
JOIN Xe x ON x.MaXe = cx.MaXe
WHERE cx.MaChuyen = 2;

-- 3. Ki?m tra có GiaVe không
SELECT gv.* 
FROM ChuyenXe cx
JOIN Xe x ON x.MaXe = cx.MaXe
LEFT JOIN GiaVe gv ON gv.MaNX = x.MaNX 
    AND gv.MaLX = x.MaLX 
    AND gv.MaTuyen = cx.MaTuyen
WHERE cx.MaChuyen = 2;

-- 4. Ki?m tra ngày gi?
SELECT 
    cx.MaChuyen,
    cx.NgayGioKhoiHanh,
    GETDATE() AS NgayHienTai,
    CASE 
        WHEN cx.NgayGioKhoiHanh > GETDATE() THEN N'OK - Có th? ??t'
        ELSE N'FAIL - ?ã kh?i hành'
    END AS TrangThai
FROM ChuyenXe cx
WHERE cx.MaChuyen = 2;
```

### 2. Test trigger riêng

```sql
-- Test insert tr?c ti?p vào VeXe
BEGIN TRANSACTION;

INSERT INTO VeXe (MaChuyen, MaHD, ChoNgoi, GiaVe)
VALUES (2, 20, 10, 120000);

-- N?u l?i ? xem message
-- N?u OK ? ROLLBACK ?? không làm ?nh h??ng data
ROLLBACK;
```

---

## ?? Checklist

### Trong code C#:
- [x] ? Fix duplicate property assignment
- [ ] ? Thêm validation `NgayGioKhoiHanh > DateTime.Now`
- [ ] ? Thêm validation check `GiaVe` record t?n t?i
- [ ] ? C?i thi?n error message ?? hi?n th? inner exception

### Trong database:
- [ ] ? Ki?m tra trigger `trg_KiemTraNgayDatVe`
- [ ] ? Ki?m tra data `GiaVe` có ?? records không
- [ ] ? Xóa ho?c update chuy?n xe trong quá kh?

---

## ?? Hành ??ng ti?p theo

1. **Ch?y debug query SQL** ?? xác ??nh v?n ??
2. **Update code** v?i validation ??y ??
3. **Test l?i** v?i chuy?n xe trong t??ng lai
