# H??ng d?n g?n tài kho?n cho Nhân viên và Khách hàng

## Cách 1: Thêm ComboBox tài kho?n tr?c ti?p vào Dialog (Khuyên dùng)

### B??c 1: C?p nh?t NhanVienDialog.Designer.cs (ho?c KhachHangDialog.Designer.cs)

Thêm controls sau vào Dialog:

```csharp
private System.Windows.Forms.Label lblTaiKhoan;
private System.Windows.Forms.ComboBox cbbTaiKhoan;
private System.Windows.Forms.Button btnGanTaiKhoan;

// Trong InitializeComponent(), thêm:
// 
// lblTaiKhoan
// 
this.lblTaiKhoan.AutoSize = true;
this.lblTaiKhoan.Location = new System.Drawing.Point(30, 270);
this.lblTaiKhoan.Name = "lblTaiKhoan";
this.lblTaiKhoan.Size = new System.Drawing.Size(75, 20);
this.lblTaiKhoan.TabIndex = 16;
this.lblTaiKhoan.Text = "Tài kho?n";
// 
// cbbTaiKhoan
// 
this.cbbTaiKhoan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
this.cbbTaiKhoan.FormattingEnabled = true;
this.cbbTaiKhoan.Location = new System.Drawing.Point(200, 267);
this.cbbTaiKhoan.Name = "cbbTaiKhoan";
this.cbbTaiKhoan.Size = new System.Drawing.Size(300, 28);
this.cbbTaiKhoan.TabIndex = 17;
// 
// btnGanTaiKhoan
// 
this.btnGanTaiKhoan.Location = new System.Drawing.Point(510, 267);
this.btnGanTaiKhoan.Name = "btnGanTaiKhoan";
this.btnGanTaiKhoan.Size = new System.Drawing.Size(30, 28);
this.btnGanTaiKhoan.TabIndex = 18;
this.btnGanTaiKhoan.Text = "...";
this.btnGanTaiKhoan.UseVisualStyleBackColor = true;
this.btnGanTaiKhoan.Click += new System.EventHandler(this.btnGanTaiKhoan_Click);

// Thêm vào Controls
this.Controls.Add(this.lblTaiKhoan);
this.Controls.Add(this.cbbTaiKhoan);
this.Controls.Add(this.btnGanTaiKhoan);
```

### B??c 2: C?p nh?t NhanVienDialog.cs (ho?c KhachHangDialog.cs)

```csharp
using QLBXMD.BUS;

public partial class NhanVienDialog : Form
{
    private TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();
    
    public int? SelectedMaTK
    {
        get 
        { 
            if (cbbTaiKhoan.SelectedValue == null || (int)cbbTaiKhoan.SelectedValue == 0)
                return null;
            return cbbTaiKhoan.SelectedValue is int v ? (int?)v : null;
        }
        set 
        { 
            if (value.HasValue && value.Value > 0)
                cbbTaiKhoan.SelectedValue = value.Value;
            else
                cbbTaiKhoan.SelectedIndex = 0;
        }
    }

    public void BindTaiKhoan(IEnumerable<KeyValuePair<int, string>> items, int? currentMaTK = null)
    {
        var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
        list.Insert(0, new KeyValuePair<int, string>(0, "-- Không ch?n --"));
        cbbTaiKhoan.DataSource = list;
        cbbTaiKhoan.DisplayMember = "Value";
        cbbTaiKhoan.ValueMember = "Key";
        
        if (currentMaTK.HasValue && currentMaTK.Value > 0)
            cbbTaiKhoan.SelectedValue = currentMaTK.Value;
        else
            cbbTaiKhoan.SelectedIndex = 0;
    }

    private void btnGanTaiKhoan_Click(object sender, EventArgs e)
    {
        using (var dialog = new GanTaiKhoanDialog())
        {
            // L?y danh sách tài kho?n ch?a g?n
            var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(SelectedMaTK)
                .Select(tk => new KeyValuePair<int, string>(
                    tk.MaTK, 
                    $"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
                ))
                .ToList();
            
            dialog.BindTaiKhoan(availableTK, SelectedMaTK);
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedMaTK = dialog.SelectedMaTK;
                // Refresh ComboBox
                BindTaiKhoan(availableTK, SelectedMaTK);
            }
        }
    }

    private void SetReadOnly(bool ro)
    {
        // ...existing code...
        cbbTaiKhoan.Enabled = !ro;
        btnGanTaiKhoan.Enabled = !ro;
        cbbTaiKhoan.TabStop = !ro;
    }
}
```

### B??c 3: S? d?ng trong UCNhanVien.cs

```csharp
using QLBXMD.BUS;

public partial class UCNhanVien : UserControl
{
    private readonly NhanVienBUS _bus = new NhanVienBUS();
    private readonly TaiKhoanBUS _taiKhoanBUS = new TaiKhoanBUS();

    private void btnThem_Click(object sender, EventArgs e)
    {
        using (var dialog = new NhanVienDialog())
        {
            // Bind các combo khác...
            
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
                    MaTK = dialog.SelectedMaTK  // G?n tài kho?n
                };
                
                string result = _bus.Add(nv);
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
                    result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                
                if (result.Contains("thành công"))
                    LoadData();
            }
        }
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (dgv.SelectedRows.Count == 0) return;
        var r = dgv.SelectedRows[0];
        if (r.Cells[0].Value == null) return;
        
        int maNV = int.Parse(r.Cells[0].Value.ToString());
        var existing = _bus.GetById(maNV);
        if (existing == null)
        {
            MessageBox.Show("Không tìm th?y nhân viên.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using (var dialog = new NhanVienDialog())
        {
            // Bind tài kho?n: bao g?m tài kho?n hi?n t?i + tài kho?n ch?a g?n
            var availableTK = _taiKhoanBUS.GetAvailableAccountsForEdit(existing.MaTK)
                .Select(tk => new KeyValuePair<int, string>(
                    tk.MaTK, 
                    $"{tk.TenDangNhap} ({tk.Email}) - {tk.Quyen?.TenQuyen}"
                ))
                .ToList();
            dialog.BindTaiKhoan(availableTK, existing.MaTK);
            
            dialog.SetMode(NhanVienDialogMode.Edit);
            dialog.MaNV = existing.MaNV;
            dialog.TenNV = existing.TenNV;
            dialog.NamSinhValue = existing.NamSinh;
            dialog.DiaChi = existing.DiaChi;
            dialog.SDT = existing.SDT;
            dialog.GioiTinhValue = existing.GioiTinh;
            dialog.TrangThaiValue = existing.TrangThai;
            dialog.SelectedMaTK = existing.MaTK;
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                existing.TenNV = dialog.TenNV;
                existing.NamSinh = dialog.NamSinhValue;
                existing.DiaChi = dialog.DiaChi;
                existing.SDT = dialog.SDT;
                existing.GioiTinh = dialog.GioiTinhValue;
                existing.TrangThai = dialog.TrangThaiValue;
                existing.MaTK = dialog.SelectedMaTK;  // C?p nh?t tài kho?n

                string result = _bus.Update(existing);
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
                    result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                
                if (result.Contains("thành công"))
                    LoadData();
            }
        }
    }
}
```

### B??c 4: Hi?n th? thông tin tài kho?n trong DataGridView

```csharp
private void SetupGridHeaders()
{
    dgv.Columns.Clear();
    dgv.Columns.Add("MaNV", "Mã NV");
    dgv.Columns.Add("TenNV", "Tên nhân viên");
    dgv.Columns.Add("NamSinh", "N?m sinh");
    dgv.Columns.Add("DiaChi", "??a ch?");
    dgv.Columns.Add("SDT", "S? ?i?n tho?i");
    dgv.Columns.Add("GioiTinh", "Gi?i tính");
    dgv.Columns.Add("TrangThai", "Tr?ng thái");
    dgv.Columns.Add("TaiKhoan", "Tài kho?n");  // Thêm c?t này
}

private void LoadData()
{
    try
    {
        var list = _bus.GetAll() ?? new List<NhanVien>();
        
        dgv.Rows.Clear();
        foreach (var nv in list)
        {
            string taiKhoan = "Ch?a g?n";
            if (nv.MaTK.HasValue && nv.TaiKhoan != null)
            {
                taiKhoan = nv.TaiKhoan.TenDangNhap;
            }
            
            dgv.Rows.Add(
                nv.MaNV,
                nv.TenNV,
                nv.NamSinh.ToString("dd/MM/yyyy"),
                nv.DiaChi,
                nv.SDT,
                nv.GioiTinh == true ? "Nam" : "N?",
                nv.TrangThai == true ? "Ho?t ??ng" : "Ngh? vi?c",
                taiKhoan  // Hi?n th? tên ??ng nh?p
            );
        }
        
        dgv.ClearSelection();
    }
    catch (Exception ex)
    {
        MessageBox.Show("L?i khi t?i d? li?u: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

## Cách 2: S? d?ng GanTaiKhoanDialog riêng (??n gi?n h?n)

N?u b?n không mu?n thêm ComboBox vào Dialog chính, có th? thêm m?t button "G?n tài kho?n" trong UCNhanVien:

```csharp
// Trong UCNhanVien.Designer.cs
private System.Windows.Forms.Button btnGanTaiKhoan;

// Trong UCNhanVien.cs
private void btnGanTaiKhoan_Click(object sender, EventArgs e)
{
    if (dgv.SelectedRows.Count == 0)
    {
        MessageBox.Show("Vui lòng ch?n nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    var r = dgv.SelectedRows[0];
    if (r.Cells[0].Value == null) return;
    
    int maNV = int.Parse(r.Cells[0].Value.ToString());
    var nv = _bus.GetById(maNV);
    if (nv == null) return;

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
            nv.MaTK = dialog.SelectedMaTK;
            string result = _bus.Update(nv);
            MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
                result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            
            if (result.Contains("thành công"))
                LoadData();
        }
    }
}
```

## L?u ý quan tr?ng

1. **Tài kho?n ch? g?n ???c 1 l?n**: M?t tài kho?n ch? có th? g?n v?i 1 nhân viên HO?C 1 khách hàng, không ???c g?n c? hai.

2. **GetAvailableAccounts()**: Dùng khi thêm m?i - ch? l?y tài kho?n ch?a g?n.

3. **GetAvailableAccountsForEdit(currentMaTK)**: Dùng khi s?a - l?y tài kho?n ch?a g?n + tài kho?n hi?n t?i c?a entity.

4. **Xóa nhân viên/khách hàng**: Khi xóa, nên set MaTK = null thay vì xóa tài kho?n, ?? tài kho?n có th? g?n cho ng??i khác.

5. **Validation**: Nên ki?m tra quy?n c?a tài kho?n khi g?n:
   - Nhân viên nên g?n v?i tài kho?n có quy?n "Admin" ho?c "Staff"
   - Khách hàng nên g?n v?i tài kho?n có quy?n "User"
