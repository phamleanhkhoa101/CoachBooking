# H??ng D?n S? D?ng Trang Th?ng Kê

## T?ng Quan

Trang **Th?ng Kê** (UCThongKe) cung c?p cái nhìn t?ng quan v? ho?t ??ng kinh doanh c?a h? th?ng qu?n lý bán vé xe, bao g?m:
- Th?ng kê t?ng quan (Doanh thu, S? chuy?n xe, S? khách hàng, S? vé)
- Bi?u ?? doanh thu theo ngày
- Bi?u ?? top 5 tuy?n ???ng ph? bi?n

---

## Các Tính N?ng

### 1. B? L?c Th?i Gian

![Filter Section](image)

**Các tr??ng:**
- **T? ngày**: Ch?n ngày b?t ??u th?ng kê
- **??n ngày**: Ch?n ngày k?t thúc th?ng kê
- **Nút Th?ng Kê**: Áp d?ng b? l?c và c?p nh?t d? li?u

**Giá tr? m?c ??nh:**
- T? ngày: Ngày ??u tiên c?a tháng hi?n t?i
- ??n ngày: Ngày hi?n t?i

**L?u ý:**
- Ngày b?t ??u không ???c l?n h?n ngày k?t thúc
- H? th?ng s? t? ??ng thêm 23:59:59 vào ngày k?t thúc ?? bao g?m toàn b? ngày

---

### 2. Th? Th?ng Kê T?ng Quan

#### ?? T?ng Doanh Thu
- **Màu n?n**: Xanh d??ng (#2196F3)
- **Hi?n th?**: T?ng doanh thu t? các hóa ??n ?ã thanh toán trong kho?ng th?i gian ?ã ch?n
- **??nh d?ng**: #,##0 VN?
- **Ngu?n d? li?u**: `HoaDons.Where(TrangThai == true).Sum(TongTien)`

#### ?? S? Chuy?n Xe
- **Màu n?n**: Xanh lá (#4CAF50)
- **Hi?n th?**: S? l??ng chuy?n xe kh?i hành trong kho?ng th?i gian
- **Ngu?n d? li?u**: `ChuyenXes.Where(NgayGioKhoiHanh trong kho?ng).Count()`

#### ?? S? Khách Hàng
- **Màu n?n**: Tím (#9C27B0)
- **Hi?n th?**: S? khách hàng duy nh?t ?ã mua vé trong kho?ng th?i gian
- **Ngu?n d? li?u**: `HoaDons.Where(TrangThai == true).Select(MaKH).Distinct().Count()`

#### ?? S? Vé ?ã Bán
- **Màu n?n**: Cam (#FF9800)
- **Hi?n th?**: T?ng s? vé ???c bán trong kho?ng th?i gian
- **Ngu?n d? li?u**: `HoaDons.Where(TrangThai == true).SelectMany(VeXes).Count()`

---

### 3. Bi?u ?? Doanh Thu Theo Ngày

**Lo?i bi?u ??**: C?t (Column Chart)

**??c ?i?m:**
- Hi?n th? doanh thu theo t?ng ngày trong kho?ng th?i gian ?ã ch?n
- Tr?c X: Ngày (??nh d?ng dd/MM)
- Tr?c Y: Doanh thu (VN?)
- Màu c?t: Xanh d??ng (#2196F3)
- Hi?n th? giá tr? trên m?i c?t

**D? li?u:**
```csharp
HoaDons
  .Where(h => h.TrangThai == true && h.NgayLap >= tuNgay && h.NgayLap <= denNgay)
  .GroupBy(h => DbFunctions.TruncateTime(h.NgayLap))
  .Select(g => new { Ngay = g.Key, TongTien = g.Sum(h => h.TongTien) })
```

**Tr??ng h?p ??c bi?t:**
- N?u không có d? li?u: Hi?n th? "Không có d? li?u" v?i giá tr? 0

---

### 4. Bi?u ?? Top 5 Tuy?n ???ng Ph? Bi?n

**Lo?i bi?u ??**: Tròn (Pie Chart)

**??c ?i?m:**
- Hi?n th? 5 tuy?n ???ng có nhi?u vé bán nh?t
- M?i ph?n hi?n th?:
  - Tên tuy?n: "?i?m ?i - ?i?m ??n"
  - S? vé bán ???c
- Màu s?c:
  - Xanh d??ng (#2196F3)
  - Xanh lá (#4CAF50)
  - Cam (#FF9800)
  - Tím (#9C27B0)
  - ?? (#F44336)

**D? li?u:**
```csharp
VeXes
  .Where(v => v.HoaDon.TrangThai == true && 
             v.HoaDon.NgayLap >= tuNgay && 
             v.HoaDon.NgayLap <= denNgay)
  .GroupBy(v => v.ChuyenXe.TuyenDuong.MaTuyen)
  .Select(g => new {
    DiemDi = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDi,
    DiemDen = g.FirstOrDefault().ChuyenXe.TuyenDuong.DiemDen,
    SoVe = g.Count()
  })
  .OrderByDescending(x => x.SoVe)
  .Take(5)
```

**Tr??ng h?p ??c bi?t:**
- N?u không có d? li?u: Hi?n th? ph?n màu xám v?i label "Không có d? li?u"

---

## C?u Trúc Code

### File: `UCThongKe.cs`

```csharp
// Ph??ng th?c chính
- UCThongKe(): Constructor, kh?i t?o DateTimePicker và Chart
- SetupDateTimePickers(): Thi?t l?p giá tr? m?c ??nh cho b? l?c ngày
- SetupCharts(): C?u hình giao di?n và thu?c tính c?a các bi?u ??
- UCThongKe_Load(): T?i d? li?u khi UserControl ???c load
- btnThongKe_Click(): X? lý s? ki?n khi nh?n nút Th?ng Kê
- LoadStatistics(): T?i t?t c? d? li?u th?ng kê
- LoadDoanhThuChart(): T?i d? li?u cho bi?u ?? doanh thu
- LoadTuyenDuongChart(): T?i d? li?u cho bi?u ?? tuy?n ???ng
```

### File: `UCThongKe.Designer.cs`

```csharp
// Các controls
- pnlTop: Panel tiêu ??
- pnlFilter: Panel b? l?c
- pnlStats: Panel ch?a 4 th? th?ng kê
- pnlCharts: Panel ch?a 2 bi?u ??
- chartDoanhThu: Bi?u ?? doanh thu
- chartTuyenDuong: Bi?u ?? tuy?n ???ng
```

---

## Th? Vi?n S? D?ng

### System.Windows.Forms.DataVisualization.Charting

**Yêu c?u:**
- .NET Framework 4.0 tr? lên
- Reference: `System.Windows.Forms.DataVisualization`

**Thêm vào project:**
```xml
<Reference Include="System.Windows.Forms.DataVisualization" />
```

**Using statements:**
```csharp
using System.Windows.Forms.DataVisualization.Charting;
```

---

## H??ng D?n S? D?ng

### Xem Th?ng Kê M?c ??nh

1. M? ?ng d?ng v?i quy?n Admin
2. Click vào menu "Th?ng Kê"
3. H? th?ng t? ??ng hi?n th? th?ng kê t? ??u tháng ??n hi?n t?i

### L?c Theo Kho?ng Th?i Gian

1. Ch?n **T? ngày** trong DateTimePicker
2. Ch?n **??n ngày** trong DateTimePicker
3. Click nút **Th?ng Kê**
4. H? th?ng c?p nh?t t?t c? th?ng kê và bi?u ??

### Phân Tích D? Li?u

**Xem doanh thu theo ngày:**
- Quan sát bi?u ?? c?t bên trái
- Các ngày có c?t cao = doanh thu cao
- Di chu?t vào c?t ?? xem giá tr? chính xác

**Xem tuy?n ???ng ph? bi?n:**
- Quan sát bi?u ?? tròn bên ph?i
- Ph?n l?n = tuy?n ???ng có nhi?u vé bán
- Di chu?t vào ph?n ?? xem thông tin chi ti?t

**Theo dõi xu h??ng:**
- So sánh doanh thu theo tu?n/tháng
- Xác ??nh ngày có doanh thu cao nh?t
- Tìm tuy?n ???ng c?n t?ng c??ng

---

## Troubleshooting

### L?i "The type or namespace name 'DataVisualization' does not exist"

**Nguyên nhân:** Ch?a thêm reference System.Windows.Forms.DataVisualization

**Gi?i pháp:**
1. M? file `.csproj`
2. Thêm dòng sau vào ph?n `<ItemGroup>` ch?a các Reference:
```xml
<Reference Include="System.Windows.Forms.DataVisualization" />
```
3. Rebuild solution

### Bi?u ?? Hi?n th? "Không có d? li?u"

**Nguyên nhân:**
- Không có hóa ??n nào trong kho?ng th?i gian
- T?t c? hóa ??n ch?a thanh toán (TrangThai = false)

**Gi?i pháp:**
- Ki?m tra l?i kho?ng th?i gian
- Ki?m tra d? li?u trong database
- ??m b?o có hóa ??n ?ã thanh toán

### Bi?u ?? Không C?p Nh?t

**Nguyên nhân:** L?i k?t n?i database ho?c exception

**Gi?i pháp:**
1. Ki?m tra connection string trong App.config
2. Xem Output window ?? ??c error message
3. Debug t?ng ph??ng th?c Load ?? tìm l?i

---

## M? R?ng Tính N?ng

### Thêm Bi?u ?? M?i

```csharp
// 1. Thêm Chart control trong Designer
private System.Windows.Forms.DataVisualization.Charting.Chart chartMoi;

// 2. Kh?i t?o trong SetupCharts()
private void SetupCharts()
{
    // ...existing code...
    
    chartMoi.Titles.Add("Tiêu ?? Bi?u ??");
    chartMoi.Titles[0].Font = new Font("Segoe UI", 14F, FontStyle.Bold);
}

// 3. Load d? li?u
private void LoadBieuDoMoi(BXMDEntities db, DateTime tuNgay, DateTime denNgay)
{
    // Query d? li?u
    var data = db.YourTable
        .Where(x => x.NgayTao >= tuNgay && x.NgayTao <= denNgay)
        .GroupBy(x => x.Category)
        .Select(g => new { Category = g.Key, Count = g.Count() })
        .ToList();
    
    // T?o series
    var series = new Series
    {
        Name = "Series Name",
        ChartType = SeriesChartType.Column
    };
    
    // Thêm data points
    foreach (var item in data)
    {
        series.Points.AddXY(item.Category, item.Count);
    }
    
    chartMoi.Series.Add(series);
}
```

### Export Bi?u ?? Ra File

```csharp
// Export to Image
private void ExportChart(Chart chart, string filePath)
{
    chart.SaveImage(filePath, ChartImageFormat.Png);
}

// Usage
ExportChart(chartDoanhThu, "DoanhThu.png");
```

### In Báo Cáo

```csharp
// Print Chart
private void PrintChart(Chart chart)
{
    chart.Printing.Print(true);
}
```

---

## Best Practices

1. **Performance:**
   - Ch? query d? li?u c?n thi?t
   - S? d?ng `DbFunctions.TruncateTime()` thay vì `.Date` trong LINQ to Entities
   - Gi?i h?n s? l??ng records (ví d?: Top 5)

2. **User Experience:**
   - Hi?n th? loading indicator khi query d? li?u l?n
   - Cung c?p thông báo l?i rõ ràng
   - Cho phép user ch?n kho?ng th?i gian linh ho?t

3. **Code Quality:**
   - Tách riêng logic query và UI update
   - S? d?ng try-catch ?? x? lý exception
   - Comment code ph?c t?p

4. **Maintainability:**
   - Tách các ph??ng th?c load chart ra riêng
   - S? d?ng constants cho màu s?c và font
   - Tuân theo naming convention

---

## Tài Li?u Tham Kh?o

- [Microsoft Chart Controls Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/chart-control-overview-windows-forms)
- [Chart Types in .NET](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datavisualization.charting.seriescharttype)
- [Entity Framework LINQ Queries](https://docs.microsoft.com/en-us/ef/ef6/querying/)

---

## Changelog

### Version 1.0 (2025-01-XX)
- ? Th?ng kê t?ng quan (4 th?)
- ? Bi?u ?? doanh thu theo ngày
- ? Bi?u ?? top 5 tuy?n ???ng
- ? B? l?c theo kho?ng th?i gian
- ? Responsive design

### Future Updates
- ?? Export báo cáo Excel
- ?? In báo cáo PDF
- ?? L?c theo nhà xe
- ?? So sánh doanh thu theo tháng/n?m
- ?? Th?ng kê nhân viên bán vé nhi?u nh?t
