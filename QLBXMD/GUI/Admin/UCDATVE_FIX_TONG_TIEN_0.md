# ?? S?A L?I: T?ng ti?n 0 VN? khi ??t vé

## ?? V?n ??

Khi click "??t Vé", dialog xác nh?n hi?n th? **"T?ng ti?n: 0 VN?"** m?c dù ?ã ch?n gh?.

### Screenshot l?i:
```
B?n có ch?c ch?n mu?n ??t 1 vé?
T?ng ti?n: 0 VN?  ? L?I
```

---

## ?? Nguyên nhân

Có 3 nguyên nhân có th?:

### 1. ? **Gh? ch?a ???c thêm vào `selectedSeats`**
- Button gh? click nh?ng không add vào list
- Event handler không ???c g?n ?úng

### 2. ? **`selectedGiaVe` = null ho?c 0**
- Không l?y ???c giá vé t? DataGridView
- Cell "GiaVe" b? null

### 3. ? **`UpdateThongTinChuyen()` không ???c g?i**
- Label không update sau khi ch?n gh?

---

## ? Gi?i pháp ?ã áp d?ng

### A. Thêm Debug Log

#### 1. Trong `BtnSeat_Click()`:
```csharp
private void BtnSeat_Click(object sender, EventArgs e)
{
    Button btn = (Button)sender;
    int seatNumber = (int)btn.Tag;

    if (selectedSeats.Contains(seatNumber))
    {
        selectedSeats.Remove(seatNumber);
        btn.BackColor = Color.FromArgb(0, 199, 174);
    }
    else
    {
        selectedSeats.Add(seatNumber);
        btn.BackColor = Color.FromArgb(245, 128, 37);
    }

    // ? THÊM: Debug log
    Console.WriteLine($"Gh? ?ã ch?n: {string.Join(", ", selectedSeats)}");
    Console.WriteLine($"T?ng s? gh?: {selectedSeats.Count}");
    Console.WriteLine($"Giá vé: {selectedGiaVe}");

    UpdateThongTinChuyen();
    
    // ? S?A: ?n panel n?u b? h?t gh?
    if (selectedSeats.Count > 0)
    {
        pnlThongTinKH.Visible = true;
    }
    else
    {
        pnlThongTinKH.Visible = false;
    }
}
```

#### 2. Trong `UpdateThongTinChuyen()`:
```csharp
private void UpdateThongTinChuyen()
{
    if (selectedMaChuyen.HasValue && selectedGiaVe.HasValue)
    {
        string gheDaChon = selectedSeats.Count > 0 
            ? string.Join(", ", selectedSeats.OrderBy(s => s)) 
            : "Ch?a ch?n";

        lblThongTinChuyen.Text = $"Tuy?n: {selectedDiemDi} ? {selectedDiemDen}\n" +
                                 $"Ngày: {selectedNgayDi:dd/MM/yyyy}\n" +
                                 $"Gh? ?ã ch?n: {gheDaChon}";

        decimal tongTien = selectedSeats.Count * selectedGiaVe.Value;
        lblTongTien.Text = $"T?ng: {tongTien:#,##0} VN?";

        // ? THÊM: Debug log
        Console.WriteLine($"UpdateThongTinChuyen - S? gh?: {selectedSeats.Count}, Giá: {selectedGiaVe}, T?ng: {tongTien}");
    }
    else
    {
        // ? THÊM: Debug null values
        Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}, selectedGiaVe: {selectedGiaVe}");
        
        lblThongTinChuyen.Text = "Ch?a ch?n chuy?n";
        lblTongTien.Text = "T?ng: 0 VN?";
    }
}
```

#### 3. Trong `btnDatVe_Click()`:
```csharp
private void btnDatVe_Click(object sender, EventArgs e)
{
    // ? THÊM: Debug log ??u tiên
    Console.WriteLine("=== B?T ??U ??T VÉ ===");
    Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}");
    Console.WriteLine($"selectedGiaVe: {selectedGiaVe}");
    Console.WriteLine($"selectedSeats.Count: {selectedSeats.Count}");
    Console.WriteLine($"Gh? ?ã ch?n: {string.Join(", ", selectedSeats)}");

    // ...existing validation...

    // ? THÊM: Validate giá vé
    if (!selectedGiaVe.HasValue || selectedGiaVe.Value == 0)
    {
        MessageBox.Show("Giá vé không h?p l?. Vui lòng ch?n l?i chuy?n xe.", 
                        "Thông báo", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
        return;
    }

    // ? S?A: Tính t?ng ti?n tr??c ?? tránh duplicate variable
    decimal tienTam = selectedSeats.Count * selectedGiaVe.Value;

    // ? S?A: Hi?n th? danh sách gh? trong confirmation
    var confirm = MessageBox.Show(
        $"B?n có ch?c ch?n mu?n ??t {selectedSeats.Count} vé?\n" +
        $"Gh?: {string.Join(", ", selectedSeats.OrderBy(s => s))}\n" +
        $"T?ng ti?n: {tienTam:#,##0} VN?",
        "Xác nh?n ??t vé",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

    // ...existing code...
}
```

---

## ?? Cách ki?m tra l?i

### B??c 1: M? Output Window
```
View > Output (Ctrl+Alt+O)
Ch?n "Debug" t? dropdown "Show output from:"
```

### B??c 2: Ch?y ?ng d?ng (F5)

### B??c 3: Test ??t vé và xem log

#### Log khi ch?n gh?:
```
Gh? ?ã ch?n: 5
T?ng s? gh?: 1
Giá vé: 20000
UpdateThongTinChuyen - S? gh?: 1, Giá: 20000, T?ng: 20000
```

#### Log khi click "??t Vé":
```
=== B?T ??U ??T VÉ ===
selectedMaChuyen: 2
selectedGiaVe: 20000
selectedSeats.Count: 1
Gh? ?ã ch?n: 5
```

---

## ?? Các case l?i và fix

### Case 1: `selectedSeats.Count = 0`
**Tri?u ch?ng:**
```
Gh? ?ã ch?n: 
T?ng s? gh?: 0
```

**Nguyên nhân:** Event handler không ???c g?n

**Fix:**
```csharp
// Trong CreateSeatButtons()
btnSeat.Click += BtnSeat_Click; // ? ??m b?o có dòng này
```

---

### Case 2: `selectedGiaVe = null`
**Tri?u ch?ng:**
```
selectedGiaVe: 
UpdateThongTinChuyen - Giá: , T?ng: 0
```

**Nguyên nhân:** Không l?y ???c giá t? DataGridView

**Fix:**
```csharp
// Trong dgvChuyenXe_CellContentClick()
selectedGiaVe = Convert.ToDecimal(row.Cells["GiaVe"].Value);

// ? THÊM: Debug log
Console.WriteLine($"Giá vé v?a ch?n: {selectedGiaVe}");
```

---

### Case 3: `selectedGiaVe = 0`
**Tri?u ch?ng:**
```
selectedGiaVe: 0
T?ng: 0
```

**Nguyên nhân:** Database có giá vé = 0

**Fix:** Check database:
```sql
SELECT * FROM ChuyenXe WHERE MaChuyen = 2
-- GiaVeThucTe ph?i > 0
```

---

### Case 4: Button gh? không ??i màu
**Tri?u ch?ng:** Click gh? nh?ng không ??i sang màu cam

**Nguyên nhân:** 
- Button b? disable
- Event không fire

**Fix:**
```csharp
// Check trong CreateSeatButtons()
if (bookedSeats.Contains(i))
{
    btnSeat.BackColor = Color.FromArgb(217, 83, 79);
    btnSeat.Enabled = false; // ? Ch? disable gh? ?ã ??t
}
else
{
    btnSeat.BackColor = Color.FromArgb(0, 199, 174);
    btnSeat.Click += BtnSeat_Click; // ? G?n event
}
```

---

## ?? Checklist debug

Khi g?p l?i "T?ng ti?n: 0 VN?", check theo th? t?:

- [ ] **1. Output Window có log không?**
  - N?u không ? Application không ch?y mode Debug
  - Fix: Run b?ng F5 (không ph?i Ctrl+F5)

- [ ] **2. Log "Gh? ?ã ch?n" có hi?n không?**
  - N?u không ? Event `BtnSeat_Click` không fire
  - Fix: Check line `btnSeat.Click += BtnSeat_Click;`

- [ ] **3. `selectedSeats.Count` có > 0 không?**
  - N?u không ? Button gh? không add vào list
  - Fix: Check logic trong `BtnSeat_Click()`

- [ ] **4. `selectedGiaVe` có giá tr? không?**
  - N?u null ? Không l?y ???c giá t? DataGridView
  - Fix: Check cell "GiaVe" trong `dgvChuyenXe_CellContentClick()`

- [ ] **5. `selectedGiaVe > 0` không?**
  - N?u = 0 ? Database có v?n ??
  - Fix: Check `ChuyenXe.GiaVeThucTe` trong database

- [ ] **6. `UpdateThongTinChuyen()` có ???c g?i không?**
  - Check log "UpdateThongTinChuyen - ..."
  - N?u không ? Add call sau khi ch?n gh?

---

## ? K?t qu? sau khi fix

### Before:
```
B?n có ch?c ch?n mu?n ??t 1 vé?
T?ng ti?n: 0 VN?  ?
```

### After:
```
B?n có ch?c ch?n mu?n ??t 1 vé?
Gh?: 5
T?ng ti?n: 20,000 VN?  ?
```

---

## ?? Testing Steps

### Test Case 1: Ch?n 1 gh?
1. Ch?n chuy?n xe (Giá 20,000 VN?)
2. Click gh? s? 5
3. Ki?m tra label: **"T?ng: 20,000 VN?"** ?
4. Click "??t Vé"
5. Dialog hi?n: **"T?ng ti?n: 20,000 VN?"** ?

### Test Case 2: Ch?n nhi?u gh?
1. Ch?n chuy?n xe (Giá 20,000 VN?)
2. Click gh? 5, 6, 7
3. Ki?m tra label: **"T?ng: 60,000 VN?"** ?
4. Click "??t Vé"
5. Dialog hi?n: **"Gh?: 5, 6, 7\nT?ng ti?n: 60,000 VN?"** ?

### Test Case 3: B? ch?n gh?
1. Ch?n gh? 5 ? T?ng: 20,000 VN?
2. Click l?i gh? 5 (b? ch?n)
3. Ki?m tra label: **"T?ng: 0 VN?"** ?
4. Panel KH ph?i ?n ?

---

## ?? Notes

### Debug trong Production
N?u mu?n t?t debug log trong production:

```csharp
#if DEBUG
    Console.WriteLine($"Debug: {message}");
#endif
```

### Log vào file
N?u c?n log vào file:

```csharp
using System.IO;

File.AppendAllText("debug_log.txt", 
    $"[{DateTime.Now}] Gh?: {string.Join(", ", selectedSeats)}\n");
```

---

**Version:** 2.1  
**Date:** 2024  
**Status:** ? Fixed
