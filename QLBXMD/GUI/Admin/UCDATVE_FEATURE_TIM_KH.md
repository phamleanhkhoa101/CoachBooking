# ?? HOÀN THÀNH - S?a l?i & Thêm tính n?ng ??T VÉ

## ? ?Ã S?A 3 V?N ??:

### 1. ? **S?a l?i GIÁ VÉ = 0**

**V?n ??:** Database có giá vé = 0 ho?c null

**Gi?i pháp:**
```csharp
// Trong dgvChuyenXe_CellContentClick()
var giaVeCell = row.Cells["GiaVe"].Value;
if (giaVeCell != null && giaVeCell != DBNull.Value)
{
    selectedGiaVe = Convert.ToDecimal(giaVeCell);
}
else
{
    selectedGiaVe = 0;
}

// ? Validate giá vé
if (selectedGiaVe == 0)
{
    MessageBox.Show("Chuy?n xe này ch?a có giá vé ho?c giá vé = 0...");
    return; // Không cho phép load s? ?? gh?
}
```

---

### 2. ? **Gh? hi?n th? ??y ??**

Không có v?n ?? v?i code hi?n t?i. Logic `CreateSeatButtons()` ?ã t?o ?? s? gh? t? `startSeat` ??n `endSeat`:

```csharp
private void CreateSeatButtons(FlowLayoutPanel panel, int startSeat, int endSeat)
{
    for (int i = startSeat; i <= endSeat; i++)
    {
        // T?o button gh? s? i
        // ...
        panel.Controls.Add(btnSeat);
    }
}
```

**N?u v?n thi?u gh?, check:**
- Database: `LoaiXe.SoCho` có ?úng s? gh? không?
- FlowLayoutPanel có ?? kích th??c ?? hi?n th??

---

### 3. ? **THÊM TÍNH N?NG TÌM KHÁCH HÀNG THEO SDT**

#### Giao di?n m?i:
```
??????????????????????????????????????????
? S? ?i?n tho?i: [1234567890]  [Tìm]    ?
? H? tên:        [Ph?m Lê Anh Khoa]     ?
? N?m sinh:      [11/ 7/2005] ??        ?
? Gi?i tính:     [Nam ?]                 ?
??????????????????????????????????????????
```

#### Logic:

```csharp
private void btnTimKH_Click(object sender, EventArgs e)
{
    // 1. Validate SDT
    if (string.IsNullOrWhiteSpace(txtSDT.Text)) {
        MessageBox.Show("Vui lòng nh?p SDT ?? tìm...");
        return;
    }

    string sdt = txtSDT.Text.Trim();

    using (var db = new BXMDEntities())
    {
        var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == sdt);

        if (khachHang != null)
        {
            // ? TÌM TH?Y - ?? d? li?u vào form
            txtTenKH.Text = khachHang.TenKH;
            dtpNamSinh.Value = khachHang.NamSinh;
            cboGioiTinh.SelectedIndex = (khachHang.GioiTinh ?? false) ? 0 : 1;

            // Disable ?? không s?a thông tin KH c?
            txtTenKH.ReadOnly = true;
            dtpNamSinh.Enabled = false;
            cboGioiTinh.Enabled = false;
            txtTenKH.BackColor = Color.FromArgb(240, 240, 240);

            MessageBox.Show($"?ã tìm th?y: {khachHang.TenKH}");
        }
        else
        {
            // ? KHÔNG TÌM TH?Y - Enable nh?p m?i
            txtTenKH.Clear();
            dtpNamSinh.Value = DateTime.Now.AddYears(-20);
            cboGioiTinh.SelectedIndex = 0;

            txtTenKH.ReadOnly = false;
            dtpNamSinh.Enabled = true;
            cboGioiTinh.Enabled = true;
            txtTenKH.BackColor = Color.White;

            MessageBox.Show("Không tìm th?y KH. Vui lòng nh?p thông tin m?i.");
            txtTenKH.Focus();
        }
    }
}
```

---

## ?? UI Changes

### Button "Tìm" ???c thêm:
- **V? trí:** Bên c?nh textbox S? ?i?n tho?i
- **Màu:** Xanh d??ng (#3498DB)
- **Size:** 80x34px

### Behavior khi tìm th?y KH:
- ? Các field **TÊN, N?M SINH, GI?I TÍNH** b? **readonly/disabled**
- ? Background màu xám nh?t (240, 240, 240)
- ? Khi ??t vé ? Dùng KH c? (không t?o m?i)

### Behavior khi KHÔNG tìm th?y:
- ? Các field **editable** ?? nh?p thông tin m?i
- ? Background màu tr?ng
- ? Khi ??t vé ? T?o KH m?i

---

## ?? Test Cases

### Test 1: Tìm KH có s?n
**Steps:**
1. Nh?p SDT: `0345442209`
2. Click button **Tìm**

**Expected:**
- ? Hi?n th?: "?ã tìm th?y khách hàng: Ph?m Lê Anh Khoa"
- ? Tên KH, n?m sinh, gi?i tính ???c fill
- ? Các field b? readonly/disabled

---

### Test 2: Tìm KH không t?n t?i
**Steps:**
1. Nh?p SDT: `9999999999`
2. Click button **Tìm**

**Expected:**
- ? Hi?n th?: "Không tìm th?y KH... Vui lòng nh?p thông tin m?i"
- ? Các field tr?ng, editable
- ? Focus vào txtTenKH

---

### Test 3: ??t vé v?i KH c?
**Steps:**
1. Tìm KH có s?n (SDT: 0345442209)
2. Ch?n chuy?n, ch?n gh?
3. Click "??t Vé"

**Expected:**
- ? Không t?o KH m?i
- ? Dùng MaKH c? ?? t?o HoaDon
- ? Success message hi?n ra

---

### Test 4: ??t vé v?i KH m?i
**Steps:**
1. Tìm SDT không t?n t?i
2. Nh?p thông tin KH m?i
3. Ch?n chuy?n, ch?n gh?
4. Click "??t Vé"

**Expected:**
- ? T?o KH m?i trong DB
- ? Dùng MaKH v?a t?o ?? t?o HoaDon
- ? Success message

---

## ?? Bug Fixes

### Fix 1: DateTime nullable
```csharp
// ? L?I:
dtpNamSinh.Value = khachHang.NamSinh.HasValue ? khachHang.NamSinh.Value : ...;

// ? FIX:
dtpNamSinh.Value = khachHang.NamSinh; // DateTime, not DateTime?
```

### Fix 2: bool? nullable
```csharp
// ? L?I:
cboGioiTinh.SelectedIndex = khachHang.GioiTinh.HasValue && khachHang.GioiTinh.Value ? 0 : 1;

// ? FIX:
cboGioiTinh.SelectedIndex = (khachHang.GioiTinh ?? false) ? 0 : 1;
```

### Fix 3: btnLegendDans typo
```csharp
// ? L?I:
this.btnLegendDans.UseVisualStyleBackColor = false;

// ? FIX:
this.btnLegendTrong.UseVisualStyleBackColor = false;
```

---

## ?? Files Changed

1. ? `UCDatVe.cs` - Logic tìm KH + validation giá vé
2. ? `UCDatVe.Designer.cs` - Button Tìm KH + fix typo
3. ? `UCDATVE_FEATURE_TIM_KH.md` - File này

---

## ?? How to Use (cho ng??i dùng)

### Cách 1: Khách hàng c?
1. Nh?p SDT vào textbox
2. Click nút **"Tìm"**
3. N?u tìm th?y ? Thông tin t? ??ng hi?n th?
4. Ch?n gh? ? ??t vé

### Cách 2: Khách hàng m?i
1. Nh?p SDT
2. Click **"Tìm"**
3. Thông báo "Không tìm th?y"
4. Nh?p thông tin KH m?i (tên, n?m sinh, gi?i tính)
5. Ch?n gh? ? ??t vé

---

## ?? Tips & Notes

### Tip 1: Validation SDT
- C?n nh?p ?? 10-11 ch? s?
- Không cho phép ký t? ??c bi?t

### Tip 2: Reset form
Khi ??t vé xong, form ???c reset:
- Các field KH v? tr?ng thái **editable**
- Background v? màu tr?ng

### Tip 3: Database constraint
- SDT là unique trong b?ng `KhachHang`
- Không ???c trùng SDT khi t?o m?i

---

## ? Build Status

```
Build successful ?
No errors
6 warnings (không ?nh h??ng)
```

---

## ?? Screenshots Expected

### Tìm th?y KH:
```
????????????????????????????????????
? S? ?i?n tho?i: 0345442209 [Tìm] ?
? H? tên:        Ph?m Lê Anh Khoa ? (readonly, màu xám)
? N?m sinh:      11/ 7/2005   ??  ? (disabled)
? Gi?i tính:     Nam          ?   ? (disabled)
????????????????????????????????????
```

### Không tìm th?y:
```
????????????????????????????????????
? S? ?i?n tho?i: 9999999999 [Tìm] ?
? H? tên:        _____________    ? (editable, tr?ng)
? N?m sinh:      11/ 7/2005   ??  ? (enabled)
? Gi?i tính:     Nam          ?   ? (enabled)
????????????????????????????????????
```

---

**Version:** 3.0  
**Date:** 2024  
**Status:** ? Complete  
**Build:** ? Successful
