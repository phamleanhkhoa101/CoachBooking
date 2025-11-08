# ? HOÀN THÀNH: Trang Qu?n lý Nhân viên & Khách hàng v?i G?n Tài kho?n

## ?? Các file ?ã t?o/c?p nh?t (Priority 2)

### Dialog Forms v?i ComboBox Tài kho?n

**1. NhanVienDialog**
- ? `QLBXMD\GUI\Admin\NhanVienDialog.cs` - Dialog qu?n lý nhân viên
- ? `QLBXMD\GUI\Admin\NhanVienDialog.Designer.cs` - Designer
- ? `QLBXMD\GUI\Admin\NhanVienDialog.resx` - Resources
- **Tính n?ng:**
  - ComboBox tài kho?n tích h?p s?n
  - Button "..." ?? m? GanTaiKhoanDialog
  - Mode: Add, Edit, View
  - Validation ??y ?? (tu?i >= 18, SDT 10-11 s?)

**2. KhachHangDialog**
- ? `QLBXMD\GUI\Admin\KhachHangDialog.cs` - Dialog qu?n lý khách hàng
- ? `QLBXMD\GUI\Admin\KhachHangDialog.Designer.cs` - Designer
- ? `QLBXMD\GUI\Admin\KhachHangDialog.resx` - Resources
- **Tính n?ng:**
  - ComboBox tài kho?n tích h?p s?n
  - Button "..." ?? m? GanTaiKhoanDialog
  - Mode: Add, Edit, View
  - Validation ??y ?? (n?m sinh, SDT 10-11 s?)

### User Controls (Trang qu?n lý)

**3. UCNhanVien**
- ? `QLBXMD\GUI\Admin\UCNhanVien.cs` - Trang qu?n lý nhân viên
- ? `QLBXMD\GUI\Admin\UCNhanVien.Designer.cs` - Designer
- **Ch?c n?ng:**
  - DataGridView hi?n th? 8 c?t
  - CRUD: Thêm, Xem, S?a, Xóa
  - **Button "G?n TK"** - G?n/??i tài kho?n nhanh
  - Tìm ki?m theo 7 tiêu chí
  - Hi?n th? c?t "Tài kho?n" (tên ??ng nh?p ho?c "Ch?a g?n")

**4. UCKhachHang**
- ? `QLBXMD\GUI\Admin\UCKhachHang.cs` - Trang qu?n lý khách hàng
- ? `QLBXMD\GUI\Admin\UCKhachHang.Designer.cs` - Designer
- **Ch?c n?ng:**
  - DataGridView hi?n th? 6 c?t
  - CRUD: Thêm, Xem, S?a, Xóa
  - **Button "G?n TK"** - G?n/??i tài kho?n nhanh
  - Tìm ki?m theo 5 tiêu chí
  - Hi?n th? c?t "Tài kho?n"

## ?? Cách s? d?ng (2 Option)

### **Option 1: G?n tài kho?n trong Dialog (Khuy?n ngh?)**

#### Khi thêm m?i:
```
1. Click "Thêm" ? NhanVienDialog m?
2. Nh?p thông tin nhân viên
3. Ch?n tài kho?n t? ComboBox (ho?c click "..." ?? ch?n chi ti?t)
   - ComboBox hi?n th?: "TenDangNhap (Email) - Quy?n"
   - Ch? hi?n th? tài kho?n ch?a g?n
4. Click "Thêm" ? L?u nhân viên + g?n tài kho?n ngay
```

#### Khi s?a:
```
1. Ch?n nhân viên ? Click "S?a"
2. Dialog hi?n th? tài kho?n hi?n t?i trong ComboBox
3. Có th?:
   - Gi? nguyên tài kho?n c?
   - ??i sang tài kho?n khác (ch?a g?n)
   - G? g?n (ch?n "-- Không ch?n --")
4. Click "L?u"
```

### **Option 2: Dùng button "G?n TK" riêng**

```
1. Ch?n nhân viên trong DataGridView
2. Click "G?n TK"
3. GanTaiKhoanDialog m? ra
4. Ch?n tài kho?n ho?c g? g?n
5. Click "Ch?n" ? C?p nh?t
```

## ?? DataGridView Columns

### UCNhanVien (8 c?t):
```
| MaNV | TenNV | NamSinh | DiaChi | SDT | GioiTinh | TrangThai | Tài kho?n |
|------|-------|---------|--------|-----|----------|-----------|-----------|
| 1    | Nguy?n| 2000... | HN     | 098 | Nam      | Ho?t ??ng | admin123  |
| 2    | Tr?n  | 1995... | HCM    | 091 | N?       | Ngh? vi?c| Ch?a g?n  |
```

### UCKhachHang (6 c?t):
```
| MaKH | TenKH | NamSinh | SDT | GioiTinh | Tài kho?n |
|------|-------|---------|-----|----------|-----------|
| 1    | Lê V?n| 1990... | 097 | Nam      | user001   |
| 2    | Ph?m  | 1985... | 093 | N?       | Ch?a g?n  |
```

## ?? Tìm ki?m

### UCNhanVien - 7 tiêu chí:
1. T?t c? (tìm trong t?t c? c?t)
2. Mã NV
3. Tên nhân viên
4. S? ?i?n tho?i
5. ??a ch?
6. Tr?ng thái
7. **Tài kho?n** (tìm theo tên ??ng nh?p)

### UCKhachHang - 5 tiêu chí:
1. T?t c?
2. Mã KH
3. Tên khách hàng
4. S? ?i?n tho?i
5. **Tài kho?n**

## ? Validation Rules

### NhanVienDialog:
```csharp
? Tên nhân viên: B?t bu?c
? N?m sinh: T? 18 tu?i tr? lên
? ??a ch?: B?t bu?c
? SDT: B?t bu?c, 10-11 s?
? Gi?i tính: B?t bu?c
? Tài kho?n: Optional (có th? không ch?n)
```

### KhachHangDialog:
```csharp
? Tên khách hàng: B?t bu?c
? N?m sinh: Không ???c > hi?n t?i
? SDT: B?t bu?c, 10-11 s?
? Gi?i tính: B?t bu?c
? Tài kho?n: Optional
```

### Quy t?c g?n tài kho?n:
```
? M?t tài kho?n ch? g?n 1 nhân viên HO?C 1 khách hàng
? Khi s?a: có th? ??i tài kho?n ho?c g? g?n
? Khi xóa NV/KH: t? ??ng g? g?n tài kho?n (MaTK = null)
? ComboBox ch? hi?n th?:
  - Tài kho?n ch?a g?n (khi Add)
  - Tài kho?n ch?a g?n + tài kho?n hi?n t?i (khi Edit)
```

## ?? UI Components

### Dialog Layout (NhanVienDialog):
```
???????????????????????????????????????
? [Thêm nhân viên]              [X]   ?
???????????????????????????????????????
? Mã nhân viên:   [________] (disabled)?
? Tên nhân viên*: [________]          ?
? N?m sinh*:      [?? dd/MM/yyyy]    ?
? ??a ch?*:       [________________] ?
?                 [________________] ?
? S? ?i?n tho?i*: [________]          ?
? Gi?i tính*:     [Nam ?]             ?
? Tr?ng thái*:    [Ho?t ??ng ?]      ?
? Tài kho?n:      [user123 (em... ?][...]?
?                                      ?
?         [Thêm]  [H?y]               ?
???????????????????????????????????????
```

### Button "..." (btnGanTaiKhoan):
- Kích th??c: 24x28 px
- Text: "..."
- Ch?c n?ng: M? GanTaiKhoanDialog ?? ch?n chi ti?t h?n
- Hi?n th? danh sách v?i format ??y ??: "TenDangNhap (Email) - Quy?n"

## ?? Flow Diagram

### Flow 1: Thêm nhân viên + G?n tài kho?n
```
[Click Thêm]
    ?
[NhanVienDialog m? (Add mode)]
    ?
[Nh?p thông tin NV]
    ?
[Ch?n tài kho?n t? ComboBox ho?c click "..."]
    ?
[Validation]
    ?
[L?u NV v?i MaTK]
    ?
[Refresh DataGridView]
```

### Flow 2: G?n tài kho?n cho NV có s?n
```
[Ch?n NV trong DataGridView]
    ?
[Click "G?n TK"]
    ?
[GanTaiKhoanDialog m?]
    ?
[Ch?n TK ho?c "Không ch?n" (g? g?n)]
    ?
[Update NhanVien.MaTK]
    ?
[Refresh DataGridView]
```

### Flow 3: ??i tài kho?n
```
[Ch?n NV ? Click S?a]
    ?
[Dialog hi?n th? TK hi?n t?i]
    ?
[Ch?n TK khác ho?c g? g?n]
    ?
[L?u ? Update MaTK]
```

## ?? Ví d? S? d?ng

### T?o nhân viên m?i v?i tài kho?n:

```csharp
// Trong btnThem_Click c?a UCNhanVien:

using (var dialog = new NhanVienDialog())
{
    // Bind tài kho?n ch?a g?n
    var availableTK = _taiKhoanBUS.GetAvailableAccounts()
        .Select(tk => new KeyValuePair<int, string>(
            tk.MaTK,
            $"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
        ))
        .ToList();
    dialog.BindTaiKhoan(availableTK);
    
    dialog.SetMode(NhanVienDialogMode.Add);
    
    if (dialog.ShowDialog() == DialogResult.OK)
    {
        var nv = new NhanVien
        {
            TenNV = dialog.TenNV,
            NamSinh = dialog.NamSinhValue,
            DiaChi = dialog.DiaChi,
            SDT = dialog.SDT,
            GioiTinh = dialog.GioiTinhValue,
            TrangThai = dialog.TrangThaiValue,
            MaTK = dialog.SelectedMaTK  // ? G?n tài kho?n
        };
        
        string result = _bus.Add(nv);
        // ...
    }
}
```

### G?n tài kho?n cho NV có s?n:

```csharp
// Trong btnGanTaiKhoan_Click:

var nv = _bus.GetById(maNV);

using (var dialog = new GanTaiKhoanDialog())
{
    var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(nv.MaTK)
        .Select(tk => new KeyValuePair<int, string>(
            tk.MaTK,
            $"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
        ))
        .ToList();
    
    dialog.BindTaiKhoan(availableTK, nv.MaTK);
    
    if (dialog.ShowDialog() == DialogResult.OK)
    {
        nv.MaTK = dialog.SelectedMaTK;  // null n?u ch?n "Không ch?n"
        string result = _bus.Update(nv);
        // ...
    }
}
```

## ?? Business Rules

1. **Tài kho?n ??c quy?n:**
   - 1 tài kho?n ch? g?n v?i 1 nhân viên HO?C 1 khách hàng
   - Không cho phép g?n c? hai

2. **Hi?n th? trong ComboBox:**
   - Add mode: Ch? tài kho?n ch?a g?n
   - Edit mode: Tài kho?n ch?a g?n + tài kho?n hi?n t?i

3. **Xóa:**
   - Xóa NV/KH ? T? ??ng g? g?n (MaTK = null)
   - Tài kho?n tr? l?i tr?ng thái "ch?a g?n"
   - Có th? g?n cho entity khác

4. **Validation:**
   - Check trùng SDT (trong KhachHangBUS)
   - Check tu?i >= 18 (cho nhân viên)
   - Check format SDT (10-11 s?)

## ?? Notes

- **Format hi?n th? tài kho?n:** `TenDangNhap (Email) - TenQuyen`
  - VD: `admin123 (admin@example.com) - Admin`
- **C?t "Tài kho?n" trong grid:**
  - Hi?n th? tên ??ng nh?p n?u ?ã g?n
  - Hi?n th? "Ch?a g?n" n?u MaTK = null
  - Màu ch? có th? custom: Xám nh?t + Italic cho "Ch?a g?n"
- **Button "G?n TK":**
  - Vi?t t?t ?? ti?t ki?m không gian
  - Tooltip: "G?n ho?c thay ??i tài kho?n"
  - Enable khi ch?n 1 dòng trong DataGridView

## ?? Build Status

? **Build successful** - T?t c? file compile thành công!

## ?? Checklist

- [x] NhanVienDialog v?i ComboBox tài kho?n
- [x] KhachHangDialog v?i ComboBox tài kho?n
- [x] UCNhanVien v?i button "G?n TK"
- [x] UCKhachHang v?i button "G?n TK"
- [x] Hi?n th? c?t "Tài kho?n" trong DataGridView
- [x] Tìm ki?m theo tài kho?n
- [x] Validation ??y ??
- [x] Integration v?i GanTaiKhoanDialog
- [x] Integration v?i TaiKhoanBUS.GetAvailableAccounts()
- [x] X? lý g? g?n khi xóa
- [x] Build thành công

## ?? T?ng k?t

**Priority 2 hoàn thành 100%!**

H? th?ng ?ã có:
1. ? Dialog v?i ComboBox tài kho?n tích h?p (Priority 2)
2. ? Button "G?n TK" riêng trong UC (Priority 2)
3. ? Hi?n th? tr?ng thái g?n tài kho?n (Priority 2)
4. ? Tìm ki?m theo tài kho?n (Priority 2)

**UX ???c c?i thi?n:**
- G?n tài kho?n ngay khi t?o m?i (ti?n l?i)
- ??i/g? tài kho?n b?t c? lúc nào (linh ho?t)
- Hi?n th? rõ ràng tr?ng thái (minh b?ch)
- 2 cách g?n tài kho?n (tùy s? thích)
