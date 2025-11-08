# ?? H??ng d?n s?a l?i encoding ti?ng Vi?t

## ?? V?n ??
File `UCDatVe.Designer.cs` b? l?i encoding, các ký t? ti?ng Vi?t hi?n th? sai:
- `Ð?t Vé` ? `??t Vé`
- `Ði?m di` ? `?i?m ?i`
- `T?ng` ? `T?ng`
- `N?` ? `N?`

## ? Gi?i pháp

### Option 1: S?a trong Visual Studio (Khuy?n ngh?)
1. **M? file trong Visual Studio**
2. **File** > **Advanced Save Options**
3. Ch?n:
   - **Encoding**: `Unicode (UTF-8 with signature) - Codepage 65001`
   - **Line endings**: `Windows (CR LF)`
4. Click **OK**
5. Save file (Ctrl+S)

### Option 2: Rebuild Form
1. M? `UCDatVe.cs` trong Designer View (double-click)
2. Ch?nh s?a b?t k? label nào (thêm/xóa space)
3. Save ? Designer s? t? ??ng rebuild v?i encoding ?úng

### Option 3: Find & Replace (Nhanh nh?t)
1. M? `UCDatVe.Designer.cs`
2. **Edit** > **Find and Replace** > **Replace in Files**
3. Thay th? t?ng c?m:

| Tìm | Thay b?ng |
|-----|----------|
| `Ð?t Vé` | `??t Vé` |
| `Ði?m di` | `?i?m ?i` |
| `Ði?m d?n` | `?i?m ??n` |
| `Ngày di` | `Ngày ?i` |
| `Gi? di` | `Gi? ?i` |
| `Tìm chuy?n` | `Tìm chuy?n` |
| `T?t c?` | `T?t c?` |
| `Danh sách chuy?n xe` | `Danh sách chuy?n xe` |
| `Ch?n gh? ng?i` | `Ch?n gh? ng?i` |
| `T?ng trên` | `T?ng trên` |
| `T?ng du?i` | `T?ng d??i` |
| `Tr?ng` | `Tr?ng` |
| `Ðang ch?n` | `?ang ch?n` |
| `Ðã d?t` | `?ã ??t` |
| `Thông tin khách hàng` | `Thông tin khách hàng` |
| `H? tên:` | `H? tên:` |
| `S? di?n tho?i:` | `S? ?i?n tho?i:` |
| `Nam sinh:` | `N?m sinh:` |
| `Gi?i tính:` | `Gi?i tính:` |
| `N?` | `N?` |
| `Chua ch?n chuy?n` | `Ch?a ch?n chuy?n` |
| `T?ng: 0 VNÐ` | `T?ng: 0 VN?` |
| `Ð?T VÉ XE` | `??T VÉ XE` |

4. Save file

---

## ?? Ki?m tra sau khi s?a

### 1. Build project
```
Build > Build Solution (Ctrl+Shift+B)
```

Ph?i không có l?i!

### 2. Ch?y ?ng d?ng
```
Debug > Start Debugging (F5)
```

### 3. Ki?m tra UI
Vào **??t vé** và xem các label:
- ? "??T VÉ XE" (title)
- ? "?i?m ?i" / "?i?m ??n" / "Ngày ?i"
- ? "Gi? ?i"  
- ? "Tìm chuy?n" (button)
- ? "T?t c?" trong combobox Gi? ?i
- ? "Danh sách chuy?n xe"
- ? "Ch?n gh? ng?i"
- ? "T?ng trên" / "T?ng d??i"
- ? "Tr?ng" / "?ang ch?n" / "?ã ??t"
- ? "Thông tin khách hàng"
- ? "H? tên" / "S? ?i?n tho?i" / "N?m sinh" / "Gi?i tính"
- ? "Nam" / "N?" trong combobox
- ? "Ch?a ch?n chuy?n"
- ? "T?ng: 0 VN?"
- ? "??t Vé" (button)

---

## ?? Nguyên nhân

### 1. File ???c l?u v?i encoding sai
- **Encoding hi?n t?i**: ANSI/Windows-1252
- **Encoding ?úng**: UTF-8 with BOM

### 2. Copy/paste t? source sai encoding
- Text ???c copy t? file có encoding khác

### 3. Git không preserve encoding
- `.gitattributes` không ???c c?u hình ?úng

---

## ??? Phòng ng?a l?i encoding trong t??ng lai

### 1. C?u hình Visual Studio
**Tools** > **Options** > **Environment** > **Documents**
- ? Enable: "Save documents as Unicode when data cannot be saved in codepage"

### 2. T?o file `.editorconfig`
```ini
# .editorconfig
root = true

[*]
charset = utf-8
end_of_line = crlf
insert_final_newline = true

[*.cs]
charset = utf-8-bom
```

### 3. C?u hình `.gitattributes`
```
*.cs text eol=crlf encoding=utf-8
*.Designer.cs text eol=crlf encoding=utf-8
```

### 4. Luôn check tr??c khi commit
```bash
# Ki?m tra encoding
file -i UCDatVe.Designer.cs

# Expected output:
# UCDatVe.Designer.cs: text/x-c; charset=utf-8
```

---

## ?? Troubleshooting

### ? V?n ?? 1: Sau khi s?a v?n l?i
**Nguyên nhân:** Cache c?a Visual Studio

**Gi?i pháp:**
```
1. Close Visual Studio
2. Delete folder: bin/, obj/
3. Reopen Visual Studio
4. Rebuild project
```

### ? V?n ?? 2: File Designer.cs t? ??ng b? ghi ?è
**Nguyên nhân:** M? Form Designer mode

**Gi?i pháp:**
1. Không m? `UCDatVe.cs` trong Designer mode sau khi s?a encoding
2. Ho?c s?a xong ? commit ngay

### ? V?n ?? 3: M?t s? label v?n b? l?i
**Nguyên nhân:** Ch?a thay th? h?t

**Gi?i pháp:**
```
1. View > Output (Ctrl+W, O)
2. Ch?y app
3. Xem UI, note các label còn l?i
4. Quay l?i Designer.cs, tìm và s?a
```

---

## ? Checklist

- [ ] File > Advanced Save Options ? UTF-8 with signature
- [ ] Find & Replace t?t c? text ti?ng Vi?t
- [ ] Build successful (no errors)
- [ ] Ch?y app, check UI
- [ ] T?t c? label hi?n th? ?úng
- [ ] Commit v?i message: "Fix Vietnamese encoding in UCDatVe.Designer.cs"
- [ ] C?u hình `.editorconfig` và `.gitattributes`

---

**L?u ý:** Sau khi s?a xong, **KHÔNG** m? file `UCDatVe.cs` trong Designer mode n?u không c?n thi?t, vì Visual Studio có th? t? ??ng rebuild Designer.cs v?i encoding sai.

N?u c?n ch?nh s?a UI:
1. Backup file `UCDatVe.Designer.cs` tr??c
2. Ch?nh s?a trong Designer
3. So sánh v?i backup, fix l?i encoding n?u b? l?i
4. Commit
