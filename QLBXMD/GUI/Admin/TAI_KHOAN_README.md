# Tính n?ng Qu?n lý Tài kho?n - Hoàn thành ?

## T?ng quan

H? th?ng qu?n lý tài kho?n v?i ??y ?? ch?c n?ng CRUD và tích h?p v?i Nhân viên/Khách hàng.

## Các file ?ã t?o/c?p nh?t

### 1. Business Logic Layer (BUS)
- ? `QLBXMD\BUS\QuyenBUS.cs` - Qu?n lý quy?n
- ? `QLBXMD\BUS\TaiKhoanBUS.cs` - ?ã thêm:
  - `GetAvailableAccounts()` - L?y tài kho?n ch?a g?n
  - `GetAvailableAccountsForEdit(int? currentMaTK)` - L?y tài kho?n ch?a g?n + tài kho?n hi?n t?i

### 2. Data Access Layer (DAO)
- ? `QLBXMD\DAO\TaiKhoanDAO.cs` - ?ã thêm:
  - `GetByEmail(string email)` - L?y tài kho?n theo email

### 3. User Interface (GUI)
- ? `QLBXMD\GUI\Admin\TaiKhoanDialog.cs` - Dialog thêm/s?a/xem tài kho?n
- ? `QLBXMD\GUI\Admin\TaiKhoanDialog.Designer.cs` - Designer file
- ? `QLBXMD\GUI\Admin\TaiKhoanDialog.resx` - Resource file
- ? `QLBXMD\GUI\Admin\UCTaiKhoan.cs` - UserControl qu?n lý tài kho?n
- ? `QLBXMD\GUI\Admin\UCTaiKhoan.Designer.cs` - Designer file
- ? `QLBXMD\GUI\Admin\GanTaiKhoanDialog.cs` - Dialog g?n tài kho?n
- ? `QLBXMD\GUI\Admin\GanTaiKhoanDialog.Designer.cs` - Designer file
- ? `QLBXMD\GUI\Admin\GanTaiKhoanDialog.resx` - Resource file

### 4. Documentation
- ? `QLBXMD\GUI\Admin\HUONG_DAN_GAN_TAI_KHOAN.md` - H??ng d?n chi ti?t cách g?n tài kho?n

## Ch?c n?ng ?ã implement

### UCTaiKhoan (Trang qu?n lý)

#### Hi?n th?
- ? DataGridView v?i 6 c?t:
  - Mã TK
  - Tên ??ng nh?p
  - Email
  - Quy?n
  - Tr?ng thái (Ho?t ??ng/Khóa)
  - **Liên k?t** (Hi?n th? NV/KH ?ã g?n ho?c "Ch?a g?n")

#### Tìm ki?m
- ? Theo Mã TK
- ? Theo Tên ??ng nh?p
- ? Theo Email
- ? Theo Quy?n
- ? Theo Tr?ng thái
- ? Theo Liên k?t
- ? T?t c?

#### CRUD Operations

**1. Thêm tài kho?n** ?
- Validation:
  - Tên ??ng nh?p >= 6 ký t?
  - Email ?úng ??nh d?ng
  - M?t kh?u >= 8 ký t?
  - Xác nh?n m?t kh?u kh?p
  - Ch?n quy?n b?t bu?c
  - Ch?n tr?ng thái
- Ki?m tra trùng l?p:
  - Tên ??ng nh?p không trùng
  - Email không trùng
- M?t kh?u ???c hash tr??c khi l?u

**2. Xem chi ti?t** ?
- Read-only mode
- Hi?n th? ??y ?? thông tin
- Không cho phép ch?nh s?a

**3. S?a tài kho?n** ?
- Không cho s?a:
  - Tên ??ng nh?p (gi? nguyên)
  - M?t kh?u (dùng Reset Password riêng)
- Cho phép s?a:
  - Email
  - Quy?n
  - Tr?ng thái
- Validation email và quy?n

**4. ??t l?i m?t kh?u** ?
- T? ??ng t?o m?t kh?u ng?u nhiên 10 ký t?
- G?i qua email
- Có xác nh?n tr??c khi th?c hi?n
- Hi?n th? thông báo thành công/th?t b?i

**5. Xóa tài kho?n** ?
- Ki?m tra ràng bu?c:
  - Không cho xóa n?u ?ã g?n v?i nhân viên
  - Không cho xóa n?u ?ã g?n v?i khách hàng
- Hi?n th? xác nh?n v?i thông tin chi ti?t
- Thông báo lý do không th? xóa

### TaiKhoanDialog

#### Mode Add (Thêm)
- T?t c? field có th? nh?p
- Hi?n th? field m?t kh?u và xác nh?n m?t kh?u
- Checkbox "Hi?n m?t kh?u"
- Button "Thêm"

#### Mode Edit (S?a)
- Không cho s?a tên ??ng nh?p
- ?n field m?t kh?u (dùng reset password)
- Cho phép s?a email, quy?n, tr?ng thái
- Button "L?u"

#### Mode View (Xem)
- T?t c? field read-only
- ?n button chính
- Button "?óng"

### GanTaiKhoanDialog

- ComboBox hi?n th? tài kho?n ch?a g?n
- Format hi?n th?: `TenDangNhap (Email) - Quy?n`
- Có option "-- Không ch?n --"
- Button Ch?n/H?y

## Tích h?p v?i Nhân viên/Khách hàng

### Ph??ng th?c m?i trong TaiKhoanBUS

```csharp
// L?y tài kho?n ch?a ???c g?n
public List<TaiKhoan> GetAvailableAccounts()

// L?y tài kho?n ch?a g?n + tài kho?n hi?n t?i (dùng cho Edit)
public List<TaiKhoan> GetAvailableAccountsForEdit(int? currentMaTK)
```

### Cách s? d?ng

#### Option 1: Thêm vào Dialog Nhân viên/Khách hàng
Xem file `HUONG_DAN_GAN_TAI_KHOAN.md` ?? bi?t chi ti?t cách thêm ComboBox tài kho?n vào NhanVienDialog/KhachHangDialog

#### Option 2: Dùng GanTaiKhoanDialog riêng
Thêm button "G?n tài kho?n" trong UCNhanVien/UCKhachHang ?? m? GanTaiKhoanDialog

### Ví d? s? d?ng trong UCNhanVien

```csharp
using QLBXMD.BUS;

private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();

// Khi thêm nhân viên
private void btnThem_Click(object sender, EventArgs e)
{
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
        
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var nv = new NhanVien
            {
                // ... các field khác
                MaTK = dialog.SelectedMaTK  // G?n tài kho?n
            };
            
            string result = _bus.Add(nv);
            // ...
        }
    }
}

// Khi s?a nhân viên
private void btnSua_Click(object sender, EventArgs e)
{
    // Bind tài kho?n: tài kho?n hi?n t?i + tài kho?n ch?a g?n
    var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(existing.MaTK)
        .Select(tk => new KeyValuePair<int, string>(
            tk.MaTK, 
            $"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
        ))
        .ToList();
    dialog.BindTaiKhoan(availableTK, existing.MaTK);
    // ...
}
```

## Quy t?c kinh doanh

### 1. Tài kho?n
- ? M?t tài kho?n ch? g?n v?i **1 nhân viên** HO?C **1 khách hàng**
- ? Không ???c g?n cùng lúc c? hai
- ? Tài kho?n ch?a g?n có th? g?n cho b?t k? ai
- ? Khi s?a, có th? gi? nguyên ho?c ch?n tài kho?n khác ch?a g?n

### 2. Xóa
- ? Không xóa ???c tài kho?n ?ã g?n v?i nhân viên/khách hàng
- ? N?u mu?n xóa, ph?i g? liên k?t tr??c (set MaTK = null)

### 3. Quy?n
- ?? **Khuy?n ngh?**:
  - Nhân viên: Quy?n "Admin" ho?c "Staff"
  - Khách hàng: Quy?n "User"
- ?? Ch?a có validation c?ng, có th? thêm sau

## B?o m?t

? M?t kh?u ???c hash b?ng HashHelper tr??c khi l?u
? Không hi?n th? m?t kh?u trong grid
? Không cho s?a m?t kh?u tr?c ti?p (dùng reset password)
? G?i m?t kh?u m?i qua email khi reset

## Build Status

? **Build successful** - T?t c? file ?ã ???c compile thành công

## Các b??c ti?p theo

1. ? Hoàn thành trang qu?n lý tài kho?n
2. ? Thêm ch?c n?ng g?n tài kho?n
3. ?? **Ti?p theo**: C?p nh?t trang Nhân viên và Khách hàng
   - Thêm ComboBox/Button g?n tài kho?n vào Dialog
   - Hi?n th? c?t "Tài kho?n" trong DataGridView
   - Implement logic g?n/g? tài kho?n

## Ghi chú k? thu?t

- Framework: .NET Framework 4.8
- C# Version: 7.3
- Pattern: 3-Layer Architecture (GUI - BUS - DAO)
- ORM: Entity Framework
- UI: Windows Forms
