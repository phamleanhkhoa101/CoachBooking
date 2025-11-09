# H??NG D?N S? D?NG GIAO DI?N ??T VÉ KHÁCH HÀNG

## Tính n?ng chính

### 1. Giao di?n trang ch? (FrmMainUser)
- **Menu bên trái** v?i các ch?c n?ng:
  - ?? ??t vé (UCDatVeUser)
  - ?? L?ch s? ??t vé (?ang phát tri?n)
  - ?? Thông tin cá nhân (?ang phát tri?n)
  - ?? ??ng xu?t

### 2. Giao di?n ??t vé (UCDatVeUser)

#### **B??c 1: Tìm chuy?n xe**
- Ch?n **?i?m ?i** và **?i?m ??n** t? dropdown
- Nút **? (Swap)**: ??i ng??c ?i?m ?i và ?i?m ??n
- Ch?n **Ngày ?i** (Ngày v? t?m th?i b? disable)
- Nh?n **TÌM CHUY?N XE**

#### **B??c 2: Xem danh sách chuy?n xe**
Hi?n th? các th? chuy?n xe v?i thông tin:
- ?? Icon xe bus
- Tên nhà xe
- Lo?i xe - S? ch?
- Gi? ?i ? Gi? ??n
- Ngày kh?i hành
- S? ch? tr?ng
- **Giá vé** (màu ??, n?i b?t)
- Nút **"Ch?n chuy?n"** (màu xanh)

**Hi?u ?ng:**
- Hover vào th?: ??i màu n?n sang xám nh?t
- N?u h?t ch?: Nút "Ch?n chuy?n" b? disable

#### **B??c 3: Ch?n gh?**
Sau khi ch?n chuy?n, hi?n th? s? ?? gh?:

**Chú thích:**
- ? **Tr?ng vi?n xanh**: Gh? tr?ng
- ? **Xám**: Gh? ?ã ??t (không th? ch?n)
- ?? **Cam**: Gh? ?ang ch?n

**Lo?i xe:**
- **Xe gh? ng?i**: Hi?n th? 1 t?ng
- **Xe gi??ng n?m**: Hi?n th? 2 t?ng (T?ng d??i + T?ng trên)

**Thao tác:**
- Click vào gh? ?? ch?n/b? ch?n
- Có th? ch?n nhi?u gh?
- Hi?n th? danh sách gh? ?ã ch?n ? phía d??i

#### **B??c 4: Xác nh?n ??t vé**
- Hi?n th?:
  - **Gh? ?ã ch?n**: 1, 5, 7, ...
  - **T?ng ti?n**: (màu ??, font l?n)
- Nh?n nút **??T VÉ** (màu ??)
- Xác nh?n trong popup:
  - Tên khách hàng
  - S? ?i?n tho?i
  - Gi?i tính
  - Ngày sinh
  - Tuy?n ?i
  - Ngày kh?i hành
  - Gh? ?ã ch?n
  - T?ng ti?n
- Nh?n **Yes** ?? hoàn t?t

## Tích h?p vào h? th?ng

### Cách g?i giao di?n t? form ??ng nh?p:

```csharp
// Sau khi ??ng nh?p thành công
var frmMainUser = new FrmMainUser(taiKhoan);
frmMainUser.Show();
this.Hide(); // ?n form ??ng nh?p
```

### Kh?i t?o riêng UserControl:

```csharp
// Trong form ho?c panel
var ucDatVe = new UCDatVeUser(taiKhoan);
ucDatVe.Dock = DockStyle.Fill;
panel.Controls.Add(ucDatVe);
```

## Yêu c?u h? th?ng

### Database:
- B?ng **ChuyenXe** ph?i có:
  - `GiaVeThucTe` > 0
  - `NgayGioKhoiHanh` > DateTime.Now
- B?ng **KhachHang** ph?i có `MaTK` liên k?t v?i `TaiKhoan`
- Trigger t? ??ng tính `TongTien` trong b?ng `HoaDon`

### Quy?n:
- Ch? khách hàng ?ã ??ng nh?p m?i có th? ??t vé
- H? th?ng t? ??ng l?y thông tin t? `TaiKhoan` ? `KhachHang`

## L?u ý k? thu?t

### 1. Responsive Design
- Giao di?n t? ??ng scale theo kích th??c màn hình
- FlowLayoutPanel t? ??ng xu?ng dòng khi h?t ch?
- Scroll bar t? ??ng xu?t hi?n khi n?i dung dài

### 2. Validation
- Ki?m tra gh? ?ã ???c ??t tr??c khi xác nh?n
- Ki?m tra ngày kh?i hành > hi?n t?i
- Ki?m tra giá vé > 0

### 3. Performance
- S? d?ng `using` ?? dispose DbContext
- Ch? load d? li?u khi c?n thi?t
- S? d?ng LINQ query optimization

### 4. UX/UI
- Màu s?c theo Material Design:
  - Primary: `#009688` (Teal 500)
  - Accent: `#F44336` (Red 500)
  - Warning: `#FF9800` (Orange 500)
- Font: Segoe UI (Windows modern font)
- Icon: Unicode emoji (không c?n file ?nh)

## Tính n?ng s?p t?i (TODO)

- [ ] **L?ch s? ??t vé**: Xem các vé ?ã ??t, h?y vé
- [ ] **Thông tin cá nhân**: S?a profile, ??i m?t kh?u
- [ ] **Thanh toán MoMo**: Tích h?p payment gateway
- [ ] **Vé kh? h?i**: Ch?n c? vé ?i và vé v?
- [ ] **Tìm ki?m nâng cao**: L?c theo giá, gi?, nhà xe
- [ ] **?ánh giá chuy?n xe**: Rating & review
- [ ] **Notification**: Email/SMS xác nh?n ??t vé

## Troubleshooting

### L?i "Không tìm th?y thông tin khách hàng"
? ??m b?o trong b?ng `KhachHang` có record v?i `MaTK` trùng v?i `TaiKhoan.MaTK`

### L?i "Chuy?n xe không có giá vé"
? Vào Admin ? Chuy?n xe ? C?p nh?t `GiaVeThucTe` > 0

### Giao di?n b? l?i font/icon
? Ki?m tra font Segoe UI ?ã ???c cài ??t trên Windows
? Unicode emoji yêu c?u Windows 10+ ho?c font h? tr? emoji

## Liên h?

N?u có v?n ?? ho?c góp ý, vui lòng liên h? team phát tri?n.
