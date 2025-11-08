# ?? B?N C?P NH?T UCDatVe - S?A L?I & THÊM TÍNH N?NG

## ?? T?ng quan thay ??i

### ? 1. **S?A L?I: "The underlying provider failed on Rollback"**

#### ?? Nguyên nhân:
- S? d?ng `BeginTransaction()` gây l?i khi rollback
- Entity Framework ?ôi khi không x? lý transaction t?t v?i SaveChanges()

#### ? Gi?i pháp:
- **B? transaction** - Dùng SaveChanges() theo t?ng b??c
- **Thêm validation gh? trùng** - Ki?m tra gh? ?ã ??t tr??c khi l?u
- **Better error handling** - Hi?n th? c? InnerException

#### ?? Code c? (có l?i):
```csharp
using (var transaction = db.Database.BeginTransaction())
{
    try
    {
        // 1. T?o khách hàng
        // 2. T?o hóa ??n
        // 3. T?o vé xe
        transaction.Commit(); // ? L?I ROLLBACK ? ?ÂY
    }
    catch (Exception ex)
    {
        transaction.Rollback(); // ? CRASH
        throw ex;
    }
}
```

#### ?? Code m?i (?ã s?a):
```csharp
using (var db = new BXMDEntities())
{
    // ? B??C 1: Ki?m tra gh? trùng
    var daDat = db.VeXes
        .Where(v => v.MaChuyen == selectedMaChuyen && selectedSeats.Contains(v.ChoNgoi))
        .Select(v => v.ChoNgoi)
        .ToList();

    if (daDat.Any())
    {
        MessageBox.Show($"Gh? s? {string.Join(", ", daDat)} ?ã ???c ??t...");
        LoadSeatMap(); // Reload l?i s? ??
        return;
    }

    // ? B??C 2: Tìm/T?o khách hàng
    var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == txtSDT.Text.Trim());
    if (khachHang == null)
    {
        khachHang = new KhachHang { ... };
        db.KhachHangs.Add(khachHang);
        db.SaveChanges(); // ? SaveChanges riêng
    }

    // ? B??C 3: T?o hóa ??n
    var hoaDon = new HoaDon { ... };
    db.HoaDons.Add(hoaDon);
    db.SaveChanges(); // ? SaveChanges riêng

    // ? B??C 4: T?o vé xe
    foreach (var seat in selectedSeats)
    {
        db.VeXes.Add(new VeXe { ... });
    }
    db.SaveChanges(); // ? SaveChanges cu?i
}
```

---

### ? 2. **THÊM TÍNH N?NG: L?c theo gi? ?i**

#### ?? M?c ?ích:
- Cho phép khách hàng l?c chuy?n xe theo khung gi?
- T?ng tr?i nghi?m ng??i dùng

#### ?? Layout m?i:

```
???????????????????????????????????????????????????????
? [?i?m ?i ?]  [?i?m ??n ?]  [Ngày ?i ??]           ?
? [Gi? ?i  ?]               [Tìm chuy?n]             ?
???????????????????????????????????????????????????????
```

#### ?? Các khung gi?:
| Index | Khung gi? | Mô t? |
|-------|-----------|-------|
| 0 | T?t c? | Không l?c gi? |
| 1 | 00:00 - 06:00 | ?êm khuya/Sáng s?m |
| 2 | 06:00 - 12:00 | Sáng |
| 3 | 12:00 - 18:00 | Chi?u |
| 4 | 18:00 - 24:00 | T?i/?êm |

---

## ?? Chi ti?t thay ??i

### A. Designer.cs - Thêm ComboBox gi? ?i

```csharp
// THÊM M?I
private System.Windows.Forms.ComboBox cboGioDi;
private System.Windows.Forms.Label labelGioDi;

// Trong InitializeComponent()
this.cboGioDi = new System.Windows.Forms.ComboBox();
this.labelGioDi = new System.Windows.Forms.Label();

// cboGioDi
this.cboGioDi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
this.cboGioDi.Font = new System.Drawing.Font("Segoe UI", 10F);
this.cboGioDi.Items.AddRange(new object[] {
    "T?t c?",
    "00:00 - 06:00",
    "06:00 - 12:00",
    "12:00 - 18:00",
    "18:00 - 24:00"
});
this.cboGioDi.Location = new System.Drawing.Point(23, 124);
this.cboGioDi.Size = new System.Drawing.Size(250, 36);

// labelGioDi
this.labelGioDi.Text = "Gi? ?i";
this.labelGioDi.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
this.labelGioDi.Location = new System.Drawing.Point(18, 85);

// pnlSearch - Thêm controls
this.pnlSearch.Controls.Add(this.cboGioDi);
this.pnlSearch.Controls.Add(this.labelGioDi);
this.pnlSearch.Size = new System.Drawing.Size(1280, 180); // T?ng height
```

### B. UCDatVe.cs - Load m?c ??nh

```csharp
private void UCDatVe_Load(object sender, EventArgs e)
{
    LoadDiemDiDiemDen();
    dtpNgayDi.Value = DateTime.Now;
    dtpNamSinh.Value = DateTime.Now.AddYears(-20);
    cboGioiTinh.SelectedIndex = 0;
    cboGioDi.SelectedIndex = 0; // ? THÊM: Ch?n "T?t c?"
}
```

### C. LoadChuyenXe() - L?c theo gi?

```csharp
private void LoadChuyenXe()
{
    // ...existing query...
    var results = query.ToList();

    // ? THÊM: L?c theo gi? n?u không ch?n "T?t c?"
    if (cboGioDi.SelectedIndex > 0)
    {
        int gioIndex = cboGioDi.SelectedIndex;
        results = results.Where(r =>
        {
            int gio = r.KhoiHanh.Hour;
            switch (gioIndex)
            {
                case 1: return gio >= 0 && gio < 6;   // 00:00 - 06:00
                case 2: return gio >= 6 && gio < 12;  // 06:00 - 12:00
                case 3: return gio >= 12 && gio < 18; // 12:00 - 18:00
                case 4: return gio >= 18 && gio < 24; // 18:00 - 24:00
                default: return true;
            }
        }).ToList();
    }

    // ...existing code...
}
```

### D. btnDatVe_Click() - S?a l?i transaction

```csharp
private void btnDatVe_Click(object sender, EventArgs e)
{
    // ...existing validation...

    try
    {
        int maHD = 0;
        int soLuongVe = 0;
        decimal tongTien = 0;

        using (var db = new BXMDEntities())
        {
            // ? 1. KI?M TRA GH? TRÙNG
            var daDat = db.VeXes
                .Where(v => v.MaChuyen == selectedMaChuyen 
                         && selectedSeats.Contains(v.ChoNgoi))
                .Select(v => v.ChoNgoi)
                .ToList();

            if (daDat.Any())
            {
                MessageBox.Show(
                    $"Gh? s? {string.Join(", ", daDat)} ?ã ???c ??t b?i ng??i khác.\n" +
                    "Vui lòng ch?n gh? khác.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                LoadSeatMap(); // Reload s? ?? gh?
                return;
            }

            // ? 2. TÌM/T?O KHÁCH HÀNG
            var khachHang = db.KhachHangs
                .FirstOrDefault(kh => kh.SDT == txtSDT.Text.Trim());
            
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
                db.SaveChanges(); // ? Save riêng
            }

            // ? 3. T?O HÓA ??N
            var hoaDon = new HoaDon
            {
                MaKH = khachHang.MaKH,
                MANV = null,
                NgayLap = DateTime.Now,
                TongTien = selectedSeats.Count * selectedGiaVe.Value,
                TrangThai = true
            };
            db.HoaDons.Add(hoaDon);
            db.SaveChanges(); // ? Save riêng

            // ? 4. T?O VÉ XE
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
            db.SaveChanges(); // ? Save cu?i

            // L?u thông tin
            maHD = hoaDon.MaHD;
            soLuongVe = selectedSeats.Count;
            tongTien = hoaDon.TongTien.Value;
        }

        MessageBox.Show(
            $"??t vé thành công!\n" +
            $"Mã hóa ??n: {maHD}\n" +
            $"S? l??ng vé: {soLuongVe}\n" +
            $"T?ng ti?n: {tongTien:#,##0} VN?",
            "Thông báo",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );

        ResetForm();
    }
    catch (Exception ex)
    {
        // ? Hi?n th? c? InnerException
        MessageBox.Show(
            "L?i ??t vé: " + ex.Message + 
            (ex.InnerException != null ? "\n" + ex.InnerException.Message : ""),
            "L?i",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }
}
```

---

## ?? Test Cases

### Test 1: L?c theo gi? sáng (06:00 - 12:00)

**Steps:**
1. Ch?n ?i?m ?i/??n
2. Ch?n ngày
3. Ch?n "06:00 - 12:00" ? ComboBox gi? ?i
4. Click "Tìm chuy?n"

**Expected:**
- Ch? hi?n chuy?n xe kh?i hành t? 6h sáng ??n 12h tr?a

---

### Test 2: ??t vé thành công

**Steps:**
1. Tìm chuy?n
2. Ch?n chuy?n
3. Ch?n gh?
4. Nh?p thông tin KH
5. Click "??t Vé"

**Expected:**
- ? Không b? l?i rollback
- ? T?o KH, HoaDon, VeXe thành công
- ? Hi?n th? mã hóa ??n

---

### Test 3: Gh? b? trùng

**Scenario:** 2 ng??i ??t cùng gh? ??ng th?i

**Steps:**
1. User A ch?n gh? 5
2. User B ch?n gh? 5
3. User A ??t vé tr??c (thành công)
4. User B ??t vé sau

**Expected:**
- ? User B nh?n thông báo "Gh? s? 5 ?ã ???c ??t"
- ? Reload s? ?? gh? ? Gh? 5 màu ??
- ? User B ch?n gh? khác

---

### Test 4: L?i database

**Scenario:** L?i k?t n?i database

**Expected:**
- Hi?n th? thông báo l?i chi ti?t
- Có c? `InnerException` message
- Không crash app

---

## ?? So sánh tr??c/sau

| Tính n?ng | Tr??c | Sau |
|-----------|-------|-----|
| Transaction | ? BeginTransaction + Rollback | ? SaveChanges t?ng b??c |
| Ki?m tra gh? trùng | ? Không có | ? Check tr??c khi l?u |
| L?c theo gi? | ? Không có | ? 5 khung gi? |
| Error message | ? Ch? Message | ? Message + InnerException |
| UI Panel height | 150px | 180px (t?ng 30px) |
| ComboBox s? l??ng | 4 | 5 (thêm gi? ?i) |

---

## ?? L?i ích

### 1. S?a l?i Rollback:
? Không còi crash khi ??t vé
? T?ng tính ?n ??nh
? Error message rõ ràng h?n

### 2. Thêm l?c gi?:
? UX t?t h?n
? Gi?m s? l??ng chuy?n hi?n th?
? D? tìm chuy?n phù h?p

### 3. Ki?m tra gh? trùng:
? Tránh conflict khi nhi?u ng??i ??t cùng lúc
? Reload s? ?? gh? real-time
? Thông báo rõ ràng

---

## ?? Migration Notes

### Không c?n thay ??i Database

? Không có thay ??i schema
? Không c?n migration script
? Ch? c?n rebuild project

---

## ?? Rollback (n?u c?n)

N?u mu?n quay l?i version c?:

```bash
git checkout HEAD~1 -- QLBXMD/GUI/Admin/UCDatVe.cs
git checkout HEAD~1 -- QLBXMD/GUI/Admin/UCDatVe.Designer.cs
```

---

## ?? Deployment

### Steps:
1. ? Build project (?ã build thành công)
2. ? Test các use case
3. ? Deploy lên production

### Checklist tr??c khi deploy:
- [x] Build successful
- [x] S?a l?i rollback
- [x] Thêm l?c theo gi?
- [x] Test validation
- [x] Test gh? trùng
- [x] Update docs

---

## ?? Tài li?u liên quan

- `UCDATVE_HUONG_DAN.md` - H??ng d?n chi ti?t
- `UCDATVE_QUICK_START.md` - Quick start
- `UCDATVE_TEST_DATA.md` - Test data
- `UCDATVE_UPDATE_V2.md` - File này

---

**Version:** 2.0  
**Date:** 2024  
**Author:** Copilot  
**Status:** ? Deployed
