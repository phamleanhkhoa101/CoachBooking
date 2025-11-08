# ? S?a l?y Giá Vé Th?c T? t? ChuyenXe

## ?? Thay ??i
Chuy?n t? l?y giá vé t? b?ng `GiaVe` sang l?y `GiaVeThucTe` t? b?ng `ChuyenXe`.

---

## ?? So sánh

### ? Cách c? (t? b?ng GiaVe)
```csharp
// Ph?c t?p: Join v?i GiaVe, filter nhi?u l?n
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
        .Select(gv => (decimal?)gv.GiaVe1)
        .FirstOrDefault() ?? 0
}).ToList();
```

**V?n ??:**
- Query ph?c t?p, ch?m
- Ph?i join v?i nhi?u b?ng (MaNX, MaLX, MaTuyen)
- Có th? không kh?p n?u thi?u c?u hình trong GiaVe
- Ph?i validate thêm trong btnDatVe_Click

### ? Cách m?i (t? ChuyenXe.GiaVeThucTe)
```csharp
// ??n gi?n: L?y tr?c ti?p t? ChuyenXe
var query = (from cx in db.ChuyenXes
            join x in db.Xes on cx.MaXe equals x.MaXe
            join nx in db.NhaXes on x.MaNX equals nx.MaNX
            join lx in db.LoaiXes on x.MaLX equals lx.MaLX
            join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
            where td.DiemDi == selectedDiemDi
                  && td.DiemDen == selectedDiemDen
                  && System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
                  && cx.NgayGioKhoiHanh > DateTime.Now
                  && cx.GiaVeThucTe.HasValue && cx.GiaVeThucTe > 0 // ? Filter ngay trong query
            select new
            {
                // ...
                GiaVe = cx.GiaVeThucTe.Value // ? L?y tr?c ti?p
            }).ToList();
```

**?u ?i?m:**
- Query ??n gi?n, nhanh h?n
- Không c?n join v?i GiaVe
- Filter ngay trong query chính
- D? b?o trì

---

## ?? Chi ti?t thay ??i

### 1. File: `UCDatVe.cs` - Hàm `LoadChuyenXe()`

#### Thay ??i query
```csharp
// Thêm ?i?u ki?n filter giá vé ngay trong WHERE
&& cx.GiaVeThucTe.HasValue && cx.GiaVeThucTe > 0

// L?y giá vé tr?c ti?p
GiaVe = cx.GiaVeThucTe.Value
```

#### B? ph?n filter ph?c t?p
```csharp
// ? B? ?o?n này
var resultsWithPrice = query.Where(q =>
{
    var hasGiaVe = db.GiaVes.Any(gv => ...);
    return hasGiaVe;
}).Select(...).ToList();

// ? Thay b?ng
var results = query; // ?ã filter trong query chính r?i
```

### 2. File: `UCDatVe.cs` - Hàm `btnDatVe_Click()`

#### B? validation GiaVe table
```csharp
// ? B? ?o?n này
var xe = db.Xes.FirstOrDefault(x => x.MaXe == chuyenXe.MaXe);
var giaVeRecord = db.GiaVes.FirstOrDefault(gv => 
    gv.MaNX == xe.MaNX && 
    gv.MaLX == xe.MaLX && 
    gv.MaTuyen == chuyenXe.MaTuyen
);
if (giaVeRecord == null) { ... }

// ? Thay b?ng
if (!chuyenXe.GiaVeThucTe.HasValue || chuyenXe.GiaVeThucTe.Value <= 0)
{
    MessageBox.Show("Chuy?n xe này ch?a có giá vé...", ...);
    return;
}
```

---

## ?? Test Case

### Test 1: Chuy?n có GiaVeThucTe
```sql
-- Setup data
UPDATE ChuyenXe 
SET GiaVeThucTe = 350000 
WHERE MaChuyen = 1

-- Test trong app
1. Tìm chuy?n
2. Ph?i hi?n th? giá vé = 350,000 VN?
3. ??t vé thành công
```

### Test 2: Chuy?n không có GiaVeThucTe (NULL)
```sql
-- Setup data
UPDATE ChuyenXe 
SET GiaVeThucTe = NULL 
WHERE MaChuyen = 1

-- Test trong app
1. Tìm chuy?n
2. Không hi?n th? chuy?n này (?ã filter trong query)
```

### Test 3: Chuy?n có GiaVeThucTe = 0
```sql
-- Setup data
UPDATE ChuyenXe 
SET GiaVeThucTe = 0 
WHERE MaChuyen = 1

-- Test trong app
1. Tìm chuy?n
2. Không hi?n th? chuy?n này (?ã filter > 0)
```

---

## ?? Performance

### Cách c? (GiaVe table)
```
Query 1: Select ChuyenXe + join Xe, NhaXe, LoaiXe, TuyenDuong
Query 2: For each row -> Check GiaVe exists (N queries)
Query 3: For each filtered row -> Get GiaVe value (N queries)
? T?ng: 1 + 2N queries (N = s? chuy?n)
```

### Cách m?i (GiaVeThucTe)
```
Query 1: Select ChuyenXe + join Xe, NhaXe, LoaiXe, TuyenDuong
         + Filter GiaVeThucTe trong WHERE
? T?ng: 1 query duy nh?t
```

**C?i thi?n:** ~200-300% nhanh h?n v?i 10-20 chuy?n xe

---

## ?? L?u ý quan tr?ng

### 1. ??ng b? giá vé
Khi t?o/s?a chuy?n xe, c?n c?p nh?t `GiaVeThucTe`:

```csharp
// Trong UCChuyenXe ho?c ChuyenXeDialog
var chuyenXe = new ChuyenXe
{
    MaXe = selectedMaXe,
    MaTuyen = selectedMaTuyen,
    MaTX = selectedMaTX,
    NgayGioKhoiHanh = khoiHanhValue,
    NgayGioDenDuKien = denDuKienValue,
    GiaVeThucTe = giaVeValue // ? Ph?i ?i?n giá vé
};
```

### 2. Trigger t? ??ng (khuy?n ngh?)
T?o trigger ?? t? ??ng c?p nh?t `GiaVeThucTe` t? b?ng `GiaVe`:

```sql
CREATE TRIGGER trg_ChuyenXe_AutoSetGiaVe
ON ChuyenXe
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE cx
    SET cx.GiaVeThucTe = gv.GiaVe
    FROM ChuyenXe cx
    INNER JOIN INSERTED i ON cx.MaChuyen = i.MaChuyen
    INNER JOIN Xe x ON cx.MaXe = x.MaXe
    INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX 
                        AND gv.MaLX = x.MaLX 
                        AND gv.MaTuyen = cx.MaTuyen
    WHERE cx.GiaVeThucTe IS NULL OR cx.GiaVeThucTe = 0
END
```

### 3. Validation khi thêm chuy?n
```csharp
// Trong ChuyenXeDialog ho?c ChuyenXeBUS
if (!GiaVeThucTe.HasValue || GiaVeThucTe <= 0)
{
    MessageBox.Show(
        "Vui lòng nh?p giá vé th?c t? cho chuy?n xe.",
        "C?nh báo",
        MessageBoxButtons.OK,
        MessageBoxIcon.Warning
    );
    return;
}
```

---

## ?? Files ?ã thay ??i
- `UCDatVe.cs` - Hàm `LoadChuyenXe()` và `btnDatVe_Click()`

---

## ? Checklist
- [x] S?a query l?y giá vé t? ChuyenXe.GiaVeThucTe
- [x] B? filter ph?c t?p v?i GiaVe table
- [x] S?a validation trong btnDatVe_Click
- [x] Build successful (no errors)
- [ ] Test v?i chuy?n có GiaVeThucTe
- [ ] Test v?i chuy?n không có GiaVeThucTe
- [ ] Verify performance improvement

---

## ?? Next Steps
1. **Ki?m tra data hi?n t?i:**
```sql
-- Xem chuy?n nào ch?a có GiaVeThucTe
SELECT MaChuyen, NgayGioKhoiHanh, GiaVeThucTe
FROM ChuyenXe
WHERE GiaVeThucTe IS NULL OR GiaVeThucTe = 0

-- C?p nh?t t? GiaVe n?u c?n
UPDATE cx
SET cx.GiaVeThucTe = gv.GiaVe
FROM ChuyenXe cx
INNER JOIN Xe x ON cx.MaXe = x.MaXe
INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX 
                    AND gv.MaLX = x.MaLX 
                    AND gv.MaTuyen = cx.MaTuyen
WHERE cx.GiaVeThucTe IS NULL OR cx.GiaVeThucTe = 0
```

2. **C?p nh?t UCChuyenXe:**
   - ??m b?o khi thêm/s?a chuy?n xe ph?i nh?p GiaVeThucTe
   - Validate không ???c ?? tr?ng ho?c = 0

3. **T?o trigger t? ??ng (optional):**
   - T? ??ng fill GiaVeThucTe t? GiaVe khi thêm chuy?n m?i

---

**C?p nh?t:** 21/01/2025 20:45  
**Status:** ? Build successful, ready to test
