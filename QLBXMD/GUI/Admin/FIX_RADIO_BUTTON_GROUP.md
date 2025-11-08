# ? Fix: L?i Ch?n C? 2 Ph??ng Th?c Thanh Toán

## ?? V?n ??

Trong dialog xác nh?n ??t vé, c? hai radio button "Ti?n m?t" và "MoMo" ??u ???c ch?n cùng lúc, vi ph?m quy t?c c?a radio button (ch? ch?n 1 trong nhi?u).

## ?? Nguyên Nhân

Radio buttons ch? t? ??ng group v?i nhau khi chúng có **cùng parent container**. Trong thi?t k? ban ??u:

```
gbThanhToan (GroupBox)
??? pnlTienMat (Panel)
?   ??? rdoTienMat (RadioButton) ? Parent = pnlTienMat
??? pnlMoMo (Panel)
    ??? rdoMoMo (RadioButton) ? Parent = pnlMoMo
```

Vì `rdoTienMat` có parent là `pnlTienMat` và `rdoMoMo` có parent là `pnlMoMo`, chúng không nh?n ra nhau nh? m?t nhóm.

## ? Gi?i Pháp

Di chuy?n c? hai radio button ra cùng parent `gbThanhToan`:

```
gbThanhToan (GroupBox)
??? rdoTienMat (RadioButton) ? Parent = gbThanhToan
??? rdoMoMo (RadioButton) ? Parent = gbThanhToan
??? pnlTienMat (Panel) - Ch? ?? hi?n th? info
??? pnlMoMo (Panel) - Ch? ?? hi?n th? info
```

## ?? Thay ??i Code

### 1. XacNhanDatVeDialog.Designer.cs

**Thay ??i control hierarchy:**

```csharp
// ? TR??C (SAI)
this.pnlTienMat.Controls.Add(this.rdoTienMat);
this.pnlMoMo.Controls.Add(this.rdoMoMo);

// ? SAU (?ÚNG)
this.gbThanhToan.Controls.Add(this.rdoMoMo);
this.gbThanhToan.Controls.Add(this.rdoTienMat);
this.gbThanhToan.Controls.Add(this.pnlMoMo);
this.gbThanhToan.Controls.Add(this.pnlTienMat);
```

**C?p nh?t v? trí radio buttons:**

```csharp
// rdoTienMat - ??t overlay trên pnlTienMat
this.rdoTienMat.Location = new System.Drawing.Point(35, 78);
this.rdoTienMat.Checked = true; // M?c ??nh ch?n
this.rdoTienMat.TabStop = true;

// rdoMoMo - ??t overlay trên pnlMoMo
this.rdoMoMo.Location = new System.Drawing.Point(35, 266);
```

**Lo?i b? radio button kh?i panel controls:**

```csharp
// pnlTienMat - CH? ch?a label info
this.pnlTienMat.Controls.Add(this.lblTienMatInfo);

// pnlMoMo - CH? ch?a label info
this.pnlMoMo.Controls.Add(this.lblMoMoInfo);
```

### 2. XacNhanDatVeDialog.cs

**Thêm event handlers cho panel click:**

```csharp
private void pnlTienMat_Click(object sender, EventArgs e)
{
    rdoTienMat.Checked = true;
}

private void pnlMoMo_Click(object sender, EventArgs e)
{
    rdoMoMo.Checked = true;
}
```

**Thêm visual feedback:**

```csharp
private void UpdatePanelStyles()
{
    // C?p nh?t màu border cho panel ???c ch?n
    if (rdoTienMat.Checked)
    {
        pnlTienMat.BackColor = Color.FromArgb(230, 244, 255); // Light blue
        pnlMoMo.BackColor = Color.White;
    }
    else
    {
        pnlTienMat.BackColor = Color.White;
        pnlMoMo.BackColor = Color.FromArgb(255, 240, 250); // Light pink
    }
}
```

**G?i UpdatePanelStyles() khi load và khi thay ??i:**

```csharp
private void XacNhanDatVeDialog_Load(object sender, EventArgs e)
{
    // ...existing code...
    rdoTienMat.Checked = true;
    UpdatePanelStyles(); // ? Thêm dòng này
}

private void rdoTienMat_CheckedChanged(object sender, EventArgs e)
{
    if (rdoTienMat.Checked)
    {
        PhuongThucThanhToan = "Ti?n m?t";
        UpdatePanelStyles(); // ? Thêm dòng này
    }
}

private void rdoMoMo_CheckedChanged(object sender, EventArgs e)
{
    if (rdoMoMo.Checked)
    {
        PhuongThucThanhToan = "MoMo";
        UpdatePanelStyles(); // ? Thêm dòng này
    }
}
```

**Wire up click events cho label trong panel:**

```csharp
// Trong Designer.cs
this.lblTienMatInfo.Click += new System.EventHandler(this.pnlTienMat_Click);
this.lblMoMoInfo.Click += new System.EventHandler(this.pnlMoMo_Click);
```

## ?? C?i Thi?n UX

### 1. Visual Feedback
- Panel ???c ch?n có màu n?n khác (light blue cho ti?n m?t, light pink cho MoMo)
- D? nh?n bi?t option nào ?ang ???c ch?n

### 2. Clickable Area
- Click vào b?t k? ?âu trong panel ? Ch?n radio button
- Click vào label info ? Ch?n radio button
- Cursor thay ??i thành `Hand` khi hover panel

### 3. Default Selection
- M?c ??nh ch?n "Ti?n m?t" khi m? dialog
- `rdoTienMat.Checked = true` và `rdoTienMat.TabStop = true`

## ?? Testing

### Test Cases

? **Test 1: Ch? ch?n ???c 1 radio button**
- Click "Ti?n m?t" ? Ch? có rdoTienMat ???c ch?n
- Click "MoMo" ? rdoTienMat b? b? ch?n, ch? có rdoMoMo ???c ch?n

? **Test 2: Click vào panel**
- Click vào pnlTienMat ? rdoTienMat ???c ch?n
- Click vào pnlMoMo ? rdoMoMo ???c ch?n

? **Test 3: Click vào label**
- Click vào lblTienMatInfo ? rdoTienMat ???c ch?n
- Click vào lblMoMoInfo ? rdoMoMo ???c ch?n

? **Test 4: Visual feedback**
- Ch?n "Ti?n m?t" ? pnlTienMat có màu n?n xanh nh?t
- Ch?n "MoMo" ? pnlMoMo có màu n?n h?ng nh?t

? **Test 5: Default state**
- M? dialog ? "Ti?n m?t" ???c ch?n m?c ??nh
- PhuongThucThanhToan = "Ti?n m?t"

## ?? Ki?n Th?c WinForms

### Radio Button Grouping

**Quy t?c:**
1. Radio buttons v?i **cùng parent** t? ??ng group
2. Ch? 1 radio button trong group có th? ???c ch?n
3. Setting `Checked = true` trên 1 button s? t? ??ng uncheck các button khác trong group

**Container levels:**
```
Form
??? GroupBox (LEVEL 1)
    ??? RadioButton A (GROUP 1) ? Cùng parent
    ??? RadioButton B (GROUP 1) ? Cùng parent
    ??? Panel X (LEVEL 2)
    ?   ??? RadioButton C (GROUP 2) ? Parent khác
    ??? Panel Y (LEVEL 2)
        ??? RadioButton D (GROUP 2) ? Parent khác
```

### Best Practices

1. **Grouping:**
   ```csharp
   // ? ?ÚNG - Cùng parent
   groupBox.Controls.Add(radio1);
   groupBox.Controls.Add(radio2);
   
   // ? SAI - Parent khác nhau
   panel1.Controls.Add(radio1);
   panel2.Controls.Add(radio2);
   ```

2. **Default Selection:**
   ```csharp
   radio1.Checked = true;
   radio1.TabStop = true; // Nh?n focus khi Tab
   ```

3. **Event Handling:**
   ```csharp
   // Ch? check khi Checked = true
   private void radio1_CheckedChanged(object sender, EventArgs e)
   {
       if (radio1.Checked) // ? Quan tr?ng!
       {
           // Do something
       }
   }
   ```

## ?? K?t Qu?

? Ch? ch?n ???c 1 ph??ng th?c thanh toán t?i 1 th?i ?i?m
? Click vào panel ho?c label ?? ch?n
? Visual feedback rõ ràng
? M?c ??nh ch?n "Ti?n m?t"
? Code clean và d? maintain

## ?? Before vs After

### Before (L?i)
```
? Ti?n m?t  (Checked)
? MoMo      (Checked) ? C? HAI CÙNG CHECKED
```

### After (?úng)
```
? Ti?n m?t  (Checked)
? MoMo      (Unchecked) ? CH? 1 ???C CH?N
```

---

**Status:** ? Fixed
**Build:** ? Successful
**Testing:** ? Passed

