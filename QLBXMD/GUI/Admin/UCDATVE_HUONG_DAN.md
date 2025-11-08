# ?? H??NG D?N S? D?NG GIAO DI?N ??T VÉ XE

## ?? T?ng quan

Giao di?n ??t vé xe (`UCDatVe`) ???c thi?t k? theo mô hình web hi?n ??i v?i 4 b??c ??t vé:

1. **Tìm ki?m chuy?n xe** - Ch?n ?i?m ?i, ?i?m ??n, ngày ?i
2. **Ch?n chuy?n xe** - Xem danh sách chuy?n phù h?p
3. **Ch?n gh?** - S? ?? gh? tr?ng/?ã ??t
4. **Nh?p thông tin khách hàng** - Hoàn t?t ??t vé

---

## ?? Thi?t k? UI

### Màu s?c chính:
- **?? ch? ??o**: `#C62828` (RGB: 198, 40, 40)
- **Xanh gh? tr?ng**: `#00C7AE` (RGB: 0, 199, 174)
- **Cam gh? ?ang ch?n**: `#F58025` (RGB: 245, 128, 37)
- **?? gh? ?ã ??t**: `#D9534F` (RGB: 217, 83, 79)
- **N?n panel**: `#FAFAFA` (RGB: 250, 250, 250)

### Font ch?:
- **Segoe UI** - 10pt cho text th??ng
- **Segoe UI Bold** - 14pt cho tiêu ??
- **Segoe UI Bold** - 20pt cho tiêu ?? chính

---

## ?? Ch?c n?ng chi ti?t

### 1?? Panel Tìm ki?m (`pnlSearch`)

**Các control:**
- `cboDiemDi` - ComboBox ch?n ?i?m ?i (load t? `TuyenDuong.DiemDi`)
- `cboDiemDen` - ComboBox ch?n ?i?m ??n (load t? `TuyenDuong.DiemDen`)
- `dtpNgayDi` - DateTimePicker ch?n ngày ?i
- `btnTimChuyen` - Button tìm ki?m (màu ?? #C62828)

**Query SQL tìm chuy?n:**
```sql
SELECT cx.MaChuyen, cx.NgayGioKhoiHanh, cx.NgayGioDenDuKien,
       nx.TenNX, lx.TenLX, lx.SoCho, cx.GiaVeThucTe
FROM ChuyenXe cx
INNER JOIN Xe x ON x.MaXe = cx.MaXe
INNER JOIN NhaXe nx ON nx.MaNX = x.MaNX
INNER JOIN LoaiXe lx ON lx.MaLX = x.MaLX
INNER JOIN TuyenDuong td ON td.MaTuyen = cx.MaTuyen
WHERE td.DiemDi = @DiemDi 
  AND td.DiemDen = @DiemDen
  AND CAST(cx.NgayGioKhoiHanh AS DATE) = @NgayDi
```

**Validation:**
- Ph?i ch?n c? ?i?m ?i và ?i?m ??n
- ?i?m ?i ? ?i?m ??n
- Hi?n th? thông báo n?u không tìm th?y chuy?n

---

### 2?? Panel Danh sách chuy?n (`pnlResults`)

**DataGridView columns:**
| C?t | Hi?n th? | Mô t? |
|-----|----------|-------|
| MaChuyen | ?n | ID chuy?n xe |
| MaXe | ?n | ID xe |
| MaLoaiXe | ?n | ID lo?i xe |
| KhoiHanh | Hi?n | Th?i gian kh?i hành (dd/MM/yyyy HH:mm) |
| DenDuKien | Hi?n | Th?i gian ??n d? ki?n |
| TenNX | Hi?n | Tên nhà xe |
| TenLX | Hi?n | Lo?i xe (gh? ng?i/gi??ng n?m) |
| SoCho | Hi?n | S? ch? |
| GiaVe | Hi?n | Giá vé (format: #,##0 VN?) |
| btnChon | Button | Button "Ch?n" |

**S? ki?n `CellContentClick`:**
- Khi click button "Ch?n"
- L?u: `selectedMaChuyen`, `selectedMaXe`, `selectedMaLoaiXe`, `selectedGiaVe`
- G?i `LoadSeatMap()` ?? hi?n th? s? ?? gh?

---

### 3?? Panel S? ?? gh? (`pnlSeatMap`)

#### Logic phân lo?i xe:

**A. Xe gi??ng n?m** (TenLX ch?a "gi??ng"):
```
??????????????????????????
?   T?ng d??i (1-N/2)    ?
??????????????????????????
?   T?ng trên (N/2+1-N)  ?
??????????????????????????
```

**B. Xe gh? ng?i**:
```
??????????????????????????
?   S? ?? gh? (1-N)      ?
??????????????????????????
```

#### Tr?ng thái gh?:

| Tr?ng thái | Màu | Enabled | Action |
|------------|-----|---------|--------|
| Tr?ng | Xanh `#00C7AE` | ? | Click ?? ch?n |
| ?ang ch?n | Cam `#F58025` | ? | Click ?? b? ch?n |
| ?ã ??t | ?? `#D9534F` | ? | Không th? ch?n |

**Query l?y gh? ?ã ??t:**
```sql
SELECT ChoNgoi 
FROM VeXe 
WHERE MaChuyen = @MaChuyen
```

#### T?o button gh? ??ng:
```csharp
Button btnSeat = new Button
{
    Name = $"btnGhe_{i}",
    Text = i.ToString(),
    Width = 50,
    Height = 50,
    Margin = new Padding(5),
    FlatStyle = FlatStyle.Flat,
    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
    Tag = i  // L?u s? gh?
};
```

#### S? ki?n click gh?:
```csharp
private void BtnSeat_Click(object sender, EventArgs e)
{
    Button btn = (Button)sender;
    int seatNumber = (int)btn.Tag;

    if (selectedSeats.Contains(seatNumber))
    {
        // B? ch?n ? màu xanh
        selectedSeats.Remove(seatNumber);
        btn.BackColor = Color.FromArgb(0, 199, 174);
    }
    else
    {
        // Ch?n gh? ? màu cam
        selectedSeats.Add(seatNumber);
        btn.BackColor = Color.FromArgb(245, 128, 37);
    }

    UpdateThongTinChuyen();
    pnlThongTinKH.Visible = selectedSeats.Count > 0;
}
```

---

### 4?? Panel Thông tin khách hàng (`pnlThongTinKH`)

**Các tr??ng nh?p li?u:**
| Control | Field | Validation |
|---------|-------|------------|
| `txtTenKH` | H? tên | Không ???c r?ng |
| `txtSDT` | S? ?i?n tho?i | 10-11 ch? s? |
| `dtpNamSinh` | N?m sinh | DateTimePicker |
| `cboGioiTinh` | Gi?i tính | Nam/N? |

**Hi?n th? thông tin:**
- `lblThongTinChuyen` - Tuy?n ???ng, ngày ?i, gh? ?ã ch?n
- `lblTongTien` - T?ng ti?n = S? gh? × Giá vé
- `btnDatVe` - Button ??t vé (màu ?? #C62828)

---

## ?? Quy trình ??t vé (Database Transaction)

### B??c 1: Tìm ho?c t?o khách hàng

```csharp
var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == txtSDT.Text);

if (khachHang == null)
{
    khachHang = new KhachHang
    {
        TenKH = txtTenKH.Text.Trim(),
        NamSinh = dtpNamSinh.Value,
        SDT = txtSDT.Text.Trim(),
        GioiTinh = cboGioiTinh.SelectedItem.ToString() == "Nam",
        MaTK = null
    };
    db.KhachHangs.Add(khachHang);
    db.SaveChanges();
}
```

**Logic:**
- Check `SDT` ?ã t?n t?i ? Dùng l?i khách hàng
- N?u m?i ? T?o m?i `KhachHang`
- `MaTK = null` (khách vãng lai ch?a có tài kho?n)

---

### B??c 2: T?o hóa ??n

```csharp
var hoaDon = new HoaDon
{
    MaKH = khachHang.MaKH,
    MANV = null,  // Có th? gán nhân viên ?ang ??ng nh?p
    NgayLap = DateTime.Now,
    TongTien = selectedSeats.Count * selectedGiaVe.Value,
    TrangThai = true  // ?ã thanh toán
};
db.HoaDons.Add(hoaDon);
db.SaveChanges();
```

---

### B??c 3: T?o vé xe

```csharp
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
db.SaveChanges();
```

---

### Rollback n?u l?i:

```csharp
using (var transaction = db.Database.BeginTransaction())
{
    try
    {
        // ... các b??c trên
        transaction.Commit();
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw ex;
    }
}
```

---

## ? Validation ??y ??

### Tr??c khi ??t vé:

| ?i?u ki?n | Thông báo |
|-----------|-----------|
| Ch?a ch?n chuy?n | "Vui lòng ch?n chuy?n xe." |
| Ch?a ch?n gh? | "Vui lòng ch?n ít nh?t m?t gh?." |
| Tên tr?ng | "Vui lòng nh?p tên khách hàng." |
| SDT tr?ng | "Vui lòng nh?p s? ?i?n tho?i." |
| SDT < 10 ho?c > 11 s? | "S? ?i?n tho?i ph?i có 10-11 ch? s?." |
| Ch?a ch?n gi?i tính | "Vui lòng ch?n gi?i tính." |

### Xác nh?n tr??c khi l?u:
```
B?n có ch?c ch?n mu?n ??t {N} vé?
T?ng ti?n: {X} VN?
```

---

## ?? Reset Form sau khi ??t vé

```csharp
private void ResetForm()
{
    selectedMaChuyen = null;
    selectedMaXe = null;
    selectedMaLoaiXe = null;
    selectedGiaVe = null;
    selectedSeats.Clear();
    bookedSeats.Clear();

    txtTenKH.Clear();
    txtSDT.Clear();
    dtpNamSinh.Value = DateTime.Now.AddYears(-20);
    cboGioiTinh.SelectedIndex = 0;

    pnlResults.Visible = false;
    pnlSeatMap.Visible = false;
    pnlThongTinKH.Visible = false;

    lblThongTinChuyen.Text = "Ch?a ch?n chuy?n";
    lblTongTien.Text = "T?ng: 0 VN?";
}
```

---

## ?? Database Schema

### B?ng liên quan:

```
TuyenDuong (DiemDi, DiemDen)
    ?
ChuyenXe (MaChuyen, MaTuyen, MaXe, NgayGioKhoiHanh, GiaVeThucTe)
    ?
Xe (MaXe, MaNX, MaLX)
    ?
LoaiXe (MaLX, TenLX, SoCho)
NhaXe (MaNX, TenNX)
    ?
VeXe (MaVe, MaChuyen, MaHD, ChoNgoi, GiaVe)
    ?
HoaDon (MaHD, MaKH, MANV, NgayLap, TongTien)
    ?
KhachHang (MaKH, TenKH, SDT, NamSinh, GioiTinh)
```

---

## ?? ?i?m n?i b?t

? **UI/UX hi?n ??i** - Gi?ng web ??t vé tr?c tuy?n
? **Responsive** - Panel AutoScroll, ?i?u ch?nh linh ho?t
? **Real-time** - Gh? ?ã ??t c?p nh?t t?c thì
? **Transaction safety** - Rollback n?u l?i
? **T? ??ng t?o KH** - N?u ch?a có trong DB
? **Multi-seat booking** - ??t nhi?u gh? cùng lúc
? **Validation ??y ??** - Ki?m tra t?t c? input

---

## ?? Tích h?p vào FrmMainAdmin

Trong `FrmMainAdmin.cs`:

```csharp
private void btnDatVe_Click(object sender, EventArgs e)
{
    ShowControl(new UCDatVe());
    HightlightButton(btnDatVe);
}
```

Trong `FrmMainAdmin.Designer.cs`:

```csharp
this.btnDatVe.Click += new System.EventHandler(this.btnDatVe_Click);
```

---

## ?? Notes

1. **Gh? gi??ng n?m**: T? ??ng chia 2 t?ng (50-50)
2. **Gh? ng?i**: Hi?n th? 1 t?ng duy nh?t
3. **Màu gh?**: Update real-time khi click
4. **T?ng ti?n**: T? ??ng tính khi ch?n/b? ch?n gh?
5. **Transaction**: ??m b?o data integrity khi l?u vé

---

## ?? Troubleshooting

### L?i: "Không tìm th?y chuy?n xe"
? Ki?m tra có chuy?n xe trong database v?i tuy?n ???ng và ngày ?ã ch?n

### L?i: "Gh? không hi?n th?"
? Ki?m tra `LoaiXe.SoCho` có giá tr? h?p l? (> 0)

### L?i: "Không l?u ???c vé"
? Ki?m tra foreign key constraints (MaChuyen, MaKH t?n t?i)

---

## ?? Hoàn thành!

Giao di?n ??t vé ?ã s?n sàng s? d?ng v?i ??y ?? ch?c n?ng và validation chuyên nghi?p! ???
