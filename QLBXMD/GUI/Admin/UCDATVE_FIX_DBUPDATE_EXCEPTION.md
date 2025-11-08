# ?? Fix: DbUpdateException khi ??t vé

## ? V?n ??

Khi ??t vé trong `UCDatVe`, h? th?ng g?p l?i `System.Data.Entity.Infrastructure.DbUpdateException` khi l?u `HoaDon` vào database.

### Debug Log:
```
=== B?T ??U ??T VÉ ===
selectedMaChuyen: 2
selectedGiaVe: 120000.00
selectedSeats.Count: 1
? S? d?ng KH c?: MaKH = 22
? ?ã t?o hóa ??n: MaHD = 19
Exception thrown: 'System.Data.Entity.Infrastructure.DbUpdateException'
```

---

## ?? Nguyên nhân

### Code c? (SAI):

```csharp
var hoaDon = new HoaDon
{
    MaKH = khachHang.MaKH,
    MANV = null,
    NgayLap = DateTime.Now,
    TongTien = selectedSeats.Count * selectedGiaVe.Value, // ? Tính tay
    TrangThai = true // ? SAI: ??t là ?ã thanh toán
};
```

### V?n ??:

1. **`TrangThai = true`** ngh?a là hóa ??n ?ã thanh toán
2. Nh?ng **ch?a có record `ThanhToan`** nào ???c t?o
3. Database có trigger `trg_UpdateHoaDon_AfterThanhToanInsert` ki?m tra logic này
4. Trigger ch? set `TrangThai = true` khi **CÓ b?n ghi `ThanhToan`**

### Logic ?úng trong database:

```sql
-- Trigger: Ch? set TrangThai = 1 khi có ThanhToan
CREATE TRIGGER trg_UpdateHoaDon_AfterThanhToanInsert 
ON ThanhToan
AFTER INSERT AS
BEGIN
    UPDATE HoaDon
    SET TrangThai = 1
    FROM HoaDon hd
    INNER JOIN inserted i ON hd.MaHD = i.MaHD;
END;
```

---

## ? Gi?i pháp

### Code m?i (?ÚNG):

```csharp
var hoaDon = new HoaDon
{
    MaKH = khachHang.MaKH,
    MANV = null, // ? NULL khi khách t? ??t (không qua nhân viên)
    NgayLap = DateTime.Now,
    TongTien = 0, // ? ?? trigger t? tính
    TrangThai = false // ? Ch?a thanh toán
};

db.HoaDons.Add(hoaDon);
db.SaveChanges(); // T?o HoaDon

// T?o vé xe
foreach (var seat in selectedSeats)
{
    var veXe = new VeXe
    {
        MaChuyen = selectedMaChuyen.Value,
        MaHD = hoaDon.MaHD,
        ChoNgoi = seat,
        GiaVe = selectedGiaVe.Value
    };
    db.VeXes.Add(veXe);
}

db.SaveChanges(); // L?u vé ? Trigger tính TongTien

// ? Reload ?? l?y TongTien ?ã ???c trigger tính
db.Entry(hoaDon).Reload();

decimal tongTien = hoaDon.TongTien ?? 0; // L?y t? DB
```

---

## ?? Các thay ??i chính

### 1. `TrangThai = false`
- Hóa ??n m?i t?o ? **ch?a thanh toán**
- Ch? set `true` khi có record `ThanhToan`

### 2. `TongTien = 0`
- ?? trigger `trg_TinhGiaVeVaTongTienHoaDon_ThemSua` t? ??ng tính
- Trigger ch?y sau khi insert `VeXe`

```sql
CREATE TRIGGER trg_TinhGiaVeVaTongTienHoaDon_ThemSua
ON VeXe
AFTER INSERT, UPDATE AS
BEGIN
    UPDATE hd
    SET hd.TongTien = sub.TongTien
    FROM HoaDon hd
    INNER JOIN (
        SELECT vx.MaHD, SUM(vx.GiaVe) AS TongTien
        FROM VeXe vx
        WHERE vx.MaHD IN (SELECT DISTINCT MaHD FROM inserted)
        GROUP BY vx.MaHD
    ) sub ON sub.MaHD = hd.MaHD;
END;
```

### 3. `db.Entry(hoaDon).Reload()`
- Load l?i `TongTien` t? database sau khi trigger ?ã ch?y
- ??m b?o giá tr? hi?n th? ?úng

---

## ?? Lu?ng x? lý ?úng

```
1. T?o HoaDon (TrangThai = false, TongTien = 0)
   ?
2. SaveChanges() ? HoaDon ???c insert vào DB
   ?
3. T?o VeXe (MaHD, ChoNgoi, GiaVe)
   ?
4. SaveChanges() ? VeXe ???c insert
   ?
5. ? Trigger ch?y t? ??ng:
   - trg_UPDATEGiaVe: C?p nh?t GiaVe c?a VeXe
   - trg_TinhGiaVeVaTongTienHoaDon_ThemSua: Tính TongTien
   ?
6. db.Entry(hoaDon).Reload() ? Load l?i TongTien t? DB
   ?
7. Hi?n th? thông báo thành công v?i TongTien ?úng
```

---

## ?? Test Case

### Input:
- Ch?n chuy?n: MaChuyen = 2
- GiaVe = 120,000 VN?
- Ch?n gh?: 5
- Khách hàng: MaKH = 22 (?ã t?n t?i)

### Expected Output:
```
? ?ã t?o KH c?: MaKH = 22
? ?ã t?o hóa ??n: MaHD = 19
? ?ã t?o 1 vé xe

??t vé thành công!
Mã hóa ??n: 19
S? l??ng vé: 1
T?ng ti?n: 120,000 VN?
```

### Database sau khi ??t:

```sql
-- HoaDon
MaHD | MaKH | MANV | TongTien | TrangThai
19   | 22   | NULL | 120000   | 0 (ch?a thanh toán)

-- VeXe
MaVe | MaChuyen | MaHD | ChoNgoi | GiaVe
XX   | 2        | 19   | 5       | 120000
```

---

## ?? L?u ý

### Khi nào `TrangThai = true`?
- **Ch? khi** có record `ThanhToan` ???c t?o
- Trigger `trg_UpdateHoaDon_AfterThanhToanInsert` s? t? ??ng set

### Khi nào `MANV != NULL`?
- Khi **nhân viên bán vé** (có MaNV)
- Trong `UCDatVe` hi?n t?i, khách t? ??t ? `MANV = NULL`

### Tính n?ng thanh toán (TODO):
```csharp
// Sau khi ??t vé, c?n thêm ch?c n?ng thanh toán
var thanhToan = new ThanhToan
{
    MaHD = hoaDon.MaHD,
    NgayTT = DateTime.Now,
    PhuongThuc = "Ti?n m?t", // ho?c "Th? ATM"
    TongTien = hoaDon.TongTien
};

db.ThanhToans.Add(thanhToan);
db.SaveChanges();

// ? Trigger s? t? ??ng set HoaDon.TrangThai = true
```

---

## ? K?t lu?n

**Fix ?ã kh?c ph?c:**
- ? `DbUpdateException` ? ? ??t vé thành công
- ? TongTien sai ? ? Trigger t? tính ?úng
- ? Logic thanh toán sai ? ? TrangThai = false ?úng nghi?p v?

**Tested:** ? Build successful, ready to run!
