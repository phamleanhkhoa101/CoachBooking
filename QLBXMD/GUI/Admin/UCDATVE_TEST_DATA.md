# ?? DEMO DATA CHO UCDATVE

## ?? D? li?u c?n có trong database ?? test

### 1. Tuy?n ???ng (TuyenDuong)

```sql
INSERT INTO TuyenDuong (DiemDi, DiemDen, KhoangCach, ThoiGianDuKien)
VALUES 
('Hà N?i', '?à N?ng', 800, 12),
('Hà N?i', 'TP.HCM', 1700, 24),
('?à N?ng', 'TP.HCM', 900, 14),
('Hà N?i', 'Hu?', 650, 10),
('TP.HCM', 'Nha Trang', 450, 8);
```

---

### 2. Lo?i xe (LoaiXe)

```sql
INSERT INTO LoaiXe (TenLX, SoCho, TrangThai)
VALUES 
('Gh? ng?i 45 ch?', 45, 1),
('Gi??ng n?m 40 ch?', 40, 1),
('Gi??ng n?m VIP 32 ch?', 32, 1),
('Gh? ng?i Limousine 16 ch?', 16, 1);
```

---

### 3. Nhà xe (NhaXe)

```sql
INSERT INTO NhaXe (TenNX, DiaChi, SDT, Email)
VALUES 
('Hoàng Long', 'Hà N?i', '0243123456', 'hoanglong@bus.vn'),
('Ph??ng Trang', 'TP.HCM', '0283123456', 'phuongtrang@bus.vn'),
('Mai Linh', '?à N?ng', '0236123456', 'mailinh@bus.vn'),
('Thành B??i', 'Hà N?i', '0243999888', 'thanhbuoi@bus.vn');
```

---

### 4. Xe (Xe)

```sql
-- Xe c?a Hoàng Long (MaNX = 1)
INSERT INTO Xe (BienSoXe, MaNX, MaLX)
VALUES 
('29A-12345', 1, 1),  -- Gh? ng?i 45
('29B-23456', 1, 2),  -- Gi??ng 40
('29C-34567', 1, 3);  -- Gi??ng VIP 32

-- Xe c?a Ph??ng Trang (MaNX = 2)
INSERT INTO Xe (BienSoXe, MaNX, MaLX)
VALUES 
('51D-45678', 2, 2),  -- Gi??ng 40
('51E-56789', 2, 4);  -- Limousine 16

-- Xe c?a Mai Linh (MaNX = 3)
INSERT INTO Xe (BienSoXe, MaNX, MaLX)
VALUES 
('43F-67890', 3, 1),  -- Gh? ng?i 45
('43G-78901', 3, 2);  -- Gi??ng 40
```

---

### 5. Giá vé (GiaVe)

```sql
-- Giá cho tuy?n Hà N?i - ?à N?ng (MaTuyen = 1)
INSERT INTO GiaVe (MaNX, MaLX, MaTuyen, GiaVe)
VALUES 
(1, 1, 1, 350000),  -- Hoàng Long, Gh? 45
(1, 2, 1, 450000),  -- Hoàng Long, Gi??ng 40
(1, 3, 1, 550000),  -- Hoàng Long, Gi??ng VIP
(3, 1, 1, 340000),  -- Mai Linh, Gh? 45
(3, 2, 1, 440000);  -- Mai Linh, Gi??ng 40

-- Giá cho tuy?n Hà N?i - TP.HCM (MaTuyen = 2)
INSERT INTO GiaVe (MaNX, MaLX, MaTuyen, GiaVe)
VALUES 
(1, 2, 2, 650000),  -- Hoàng Long, Gi??ng 40
(1, 3, 2, 800000),  -- Hoàng Long, Gi??ng VIP
(2, 2, 2, 700000),  -- Ph??ng Trang, Gi??ng 40
(2, 4, 2, 1200000); -- Ph??ng Trang, Limousine

-- Giá cho tuy?n ?à N?ng - TP.HCM (MaTuyen = 3)
INSERT INTO GiaVe (MaNX, MaLX, MaTuyen, GiaVe)
VALUES 
(2, 2, 3, 500000),  -- Ph??ng Trang, Gi??ng 40
(3, 1, 3, 400000),  -- Mai Linh, Gh? 45
(3, 2, 3, 480000);  -- Mai Linh, Gi??ng 40
```

---

### 6. Tài x? (TaiXe)

```sql
INSERT INTO TaiXe (TenTX, NamSinh, DiaChi, SDT, GPLX)
VALUES 
('Nguy?n V?n A', '1985-05-15', 'Hà N?i', '0987654321', 'B2-123456'),
('Tr?n V?n B', '1980-08-20', 'TP.HCM', '0976543210', 'D-234567'),
('Lê V?n C', '1990-03-10', '?à N?ng', '0965432109', 'B2-345678'),
('Ph?m V?n D', '1988-12-05', 'Hà N?i', '0954321098', 'D-456789');
```

---

### 7. Chuy?n xe (ChuyenXe) - HÔM NAY & NGÀY MAI

```sql
-- L?y ngày hi?n t?i
DECLARE @Today DATE = CAST(GETDATE() AS DATE)
DECLARE @Tomorrow DATE = DATEADD(DAY, 1, @Today)

-- Chuy?n HÀ N?I - ?À N?NG (hôm nay)
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh, NgayGioDenDuKien, GiaVeThucTe)
VALUES 
(1, 1, 1, CAST(@Today AS DATETIME) + CAST('08:00:00' AS DATETIME), 
         CAST(@Today AS DATETIME) + CAST('20:00:00' AS DATETIME), 350000),
         
(2, 1, 2, CAST(@Today AS DATETIME) + CAST('22:00:00' AS DATETIME), 
         CAST(@Tomorrow AS DATETIME) + CAST('10:00:00' AS DATETIME), 450000),
         
(3, 1, 3, CAST(@Today AS DATETIME) + CAST('18:00:00' AS DATETIME), 
         CAST(@Tomorrow AS DATETIME) + CAST('06:00:00' AS DATETIME), 550000);

-- Chuy?n HÀ N?I - TP.HCM (hôm nay)
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh, NgayGioDenDuKien, GiaVeThucTe)
VALUES 
(2, 2, 1, CAST(@Today AS DATETIME) + CAST('20:00:00' AS DATETIME), 
         CAST(@Tomorrow AS DATETIME) + CAST('20:00:00' AS DATETIME), 650000),
         
(5, 2, 2, CAST(@Today AS DATETIME) + CAST('19:00:00' AS DATETIME), 
         CAST(@Tomorrow AS DATETIME) + CAST('19:00:00' AS DATETIME), 1200000);

-- Chuy?n ?À N?NG - TP.HCM (hôm nay)
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh, NgayGioDenDuKien, GiaVeThucTe)
VALUES 
(4, 3, 3, CAST(@Today AS DATETIME) + CAST('07:00:00' AS DATETIME), 
         CAST(@Today AS DATETIME) + CAST('21:00:00' AS DATETIME), 500000),
         
(7, 3, 4, CAST(@Today AS DATETIME) + CAST('23:00:00' AS DATETIME), 
         CAST(@Tomorrow AS DATETIME) + CAST('13:00:00' AS DATETIME), 480000);

-- Thêm chuy?n cho NGÀY MAI
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh, NgayGioDenDuKien, GiaVeThucTe)
VALUES 
(1, 1, 1, CAST(@Tomorrow AS DATETIME) + CAST('08:00:00' AS DATETIME), 
         CAST(@Tomorrow AS DATETIME) + CAST('20:00:00' AS DATETIME), 350000),
         
(2, 1, 2, CAST(@Tomorrow AS DATETIME) + CAST('22:00:00' AS DATETIME), 
         DATEADD(DAY, 2, @Today) + CAST('10:00:00' AS DATETIME), 450000);
```

---

### 8. Vé ?ã ??t m?u (?? test gh? ?ã ??t)

```sql
-- T?o khách hàng demo
INSERT INTO KhachHang (TenKH, NamSinh, SDT, GioiTinh, MaTK)
VALUES 
('Nguy?n V?n X', '1990-01-01', '0912345678', 1, NULL),
('Tr?n Th? Y', '1995-05-15', '0923456789', 0, NULL);

-- T?o hóa ??n demo
INSERT INTO HoaDon (MaKH, MANV, NgayLap, TongTien, TrangThai)
VALUES 
(1, NULL, GETDATE(), 700000, 1);

-- ??t m?t vài gh? cho chuy?n s? 1 (MaChuyen = 1)
-- ?? test UI hi?n th? gh? ?ã ??t
INSERT INTO VeXe (MaChuyen, MaHD, ChoNgoi, GiaVe)
VALUES 
(1, 1, 5, 350000),
(1, 1, 6, 350000);

-- MaHD = 2
INSERT INTO HoaDon (MaKH, MANV, NgayLap, TongTien, TrangThai)
VALUES 
(2, NULL, GETDATE(), 450000, 1);

-- Chuy?n s? 2 (Gi??ng n?m 40 ch?)
INSERT INTO VeXe (MaChuyen, MaHD, ChoNgoi, GiaVe)
VALUES 
(2, 2, 10, 450000);
```

---

## ?? Test Cases

### Test Case 1: Tìm chuy?n c? b?n

**Input:**
- ?i?m ?i: Hà N?i
- ?i?m ??n: ?à N?ng
- Ngày: Hôm nay

**Expected:**
- Hi?n 3 chuy?n xe
- Nhà xe: Hoàng Long (x2), Mai Linh (x1)
- Giá t? 350k ? 550k

---

### Test Case 2: Ch?n gh? xe gi??ng n?m

**Steps:**
1. Tìm chuy?n Hà N?i ? TP.HCM
2. Ch?n chuy?n xe gi??ng 40 ch?
3. Xem s? ?? gh?

**Expected:**
- 2 GroupBox: T?ng d??i (1-20), T?ng trên (21-40)
- Gh? 10 màu ?? (?ã ??t)
- Các gh? khác màu xanh (tr?ng)

---

### Test Case 3: Ch?n nhi?u gh?

**Steps:**
1. Ch?n chuy?n xe gh? ng?i 45 ch?
2. Click ch?n gh?: 1, 2, 3, 10, 15
3. Check lblTongTien

**Expected:**
- 5 gh? màu cam
- lblTongTien = 5 × 350,000 = 1,750,000 VN?
- pnlThongTinKH.Visible = true

---

### Test Case 4: ??t vé khách hàng m?i

**Input:**
- Tên: Ph?m V?n Test
- SDT: 0999888777
- N?m sinh: 01/01/2000
- Gi?i tính: Nam
- Gh?: 7, 8

**Expected:**
- T?o m?i KhachHang
- T?o HoaDon
- T?o 2 VeXe (gh? 7, 8)
- MessageBox thành công
- Reset form

---

### Test Case 5: ??t vé khách hàng ?ã có

**Input:**
- SDT: 0912345678 (Nguy?n V?n X - ?ã có)
- Tên: (có th? ?? khác)
- Gh?: 20

**Expected:**
- KHÔNG t?o m?i KhachHang
- Dùng l?i MaKH = 1
- T?o HoaDon m?i
- T?o VeXe

---

### Test Case 6: Validation

**Test 6.1 - Ch?a ch?n gh?:**
- Click "??t Vé" khi ch?a ch?n gh?
- Expected: "Vui lòng ch?n ít nh?t m?t gh?."

**Test 6.2 - SDT ng?n:**
- Nh?p SDT: 0123456
- Expected: "S? ?i?n tho?i ph?i có 10-11 ch? s?."

**Test 6.3 - Tên tr?ng:**
- ?? tr?ng txtTenKH
- Expected: "Vui lòng nh?p tên khách hàng."

---

### Test Case 7: Gh? ?ã ??t

**Steps:**
1. Ch?n chuy?n MaChuyen = 1
2. Xem s? ?? gh?

**Expected:**
- Gh? 5, 6 màu ??
- Gh? 5, 6 disabled (không click ???c)
- Các gh? khác màu xanh, có th? click

---

### Test Case 8: B? ch?n gh?

**Steps:**
1. Ch?n gh? 10 (màu cam)
2. Click l?i gh? 10

**Expected:**
- Gh? 10 ??i v? màu xanh
- selectedSeats.Remove(10)
- lblTongTien gi?m ?i giá c?a 1 gh?

---

## ?? Expected Database State sau test

### KhachHang
```
MaKH | TenKH          | SDT        | NamSinh    | GioiTinh
-----|----------------|------------|------------|----------
1    | Nguy?n V?n X   | 0912345678 | 1990-01-01 | 1
2    | Tr?n Th? Y     | 0923456789 | 1995-05-15 | 0
3    | Ph?m V?n Test  | 0999888777 | 2000-01-01 | 1
```

### HoaDon
```
MaHD | MaKH | NgayLap    | TongTien | TrangThai
-----|------|------------|----------|----------
1    | 1    | 2024-..    | 700000   | 1
2    | 2    | 2024-..    | 450000   | 1
3    | 3    | 2024-..    | 700000   | 1
```

### VeXe
```
MaVe | MaChuyen | MaHD | ChoNgoi | GiaVe
-----|----------|------|---------|-------
1    | 1        | 1    | 5       | 350000
2    | 1        | 1    | 6       | 350000
3    | 2        | 2    | 10      | 450000
4    | 1        | 3    | 7       | 350000
5    | 1        | 3    | 8       | 350000
```

---

## ?? Reset Database

N?u mu?n reset và test l?i:

```sql
-- Xóa theo th? t? foreign key
DELETE FROM VeXe;
DELETE FROM HoaDon;
DELETE FROM KhachHang WHERE MaKH > 2; -- Gi? l?i 2 demo
DELETE FROM ChuyenXe;
DELETE FROM GiaVe;
DELETE FROM Xe;
DELETE FROM TaiXe;
DELETE FROM LoaiXe;
DELETE FROM NhaXe;
DELETE FROM TuyenDuong;

-- Sau ?ó ch?y l?i các INSERT trên
```

---

## ?? Notes

- **MaChuyen, MaKH, MaHD**: T? ??ng increment, không c?n set
- **Ngày tháng**: Script dùng `GETDATE()` ?? l?y ngày hi?n t?i
- **GiaVeThucTe**: Ph?i match v?i b?ng `GiaVe`
- **Gi??ng n?m**: `SoCho` ph?i ch?n ?? chia 2 t?ng

---

**Happy Testing! ??**
