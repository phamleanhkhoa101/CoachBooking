# ?? Quick Start - Testing Guide

## ? Ch?y Tests Nhanh (5 phút)

### Option 1: Visual Studio (??n gi?n nh?t)
1. M? Visual Studio
2. M? menu: **Test > Run All Tests** (ho?c `Ctrl + R, A`)
3. Xem k?t qu? trong **Test Explorer**

### Option 2: PowerShell Script (Nhanh nh?t)
```powershell
# Ch?y trong th? m?c QLBXMD
cd QLBXMD.Tests
.\RunTests.ps1
```

### Option 3: Command Line
```bash
dotnet test QLBXMD.Tests\QLBXMD.Tests.csproj
```

---

## ?? Test Checklist Nhanh

### ? Tr??c Khi Commit
```powershell
# 1. Build
dotnet build

# 2. Run quick tests (30 giây)
.\QLBXMD.Tests\RunTests.ps1 -TestType quick

# 3. N?u pass -> Commit OK!
```

### ? Tr??c Khi Push
```powershell
# Run full test suite (2-3 phút)
.\QLBXMD.Tests\RunTests.ps1
```

### ? Tr??c Khi Release
```markdown
1. [ ] All automated tests pass
2. [ ] Manual test checklist completed
3. [ ] No critical bugs
4. [ ] Performance acceptable
5. [ ] Code reviewed
```

---

## ?? Test Types

| Test Type | Th?i Gian | Khi Nào Ch?y |
|-----------|-----------|--------------|
| Quick | ~30s | Tr??c m?i commit |
| Unit | ~1m | Sau thay ??i UI |
| Integration | ~2m | Sau thay ??i logic |
| All | ~3m | Tr??c push/merge |
| Manual | ~20m | Tr??c release |

---

## ?? Test Coverage

```
UCThongKe.cs
??? Constructor ..................... ? Tested
??? SetupDateTimePickers ........... ? Tested
??? SetupCharts .................... ? Tested
??? btnThongKe_Click ............... ? Tested
??? LoadStatistics ................. ? Tested
??? LoadDoanhThuChart .............. ? Tested
??? LoadTuyenDuongChart ............ ? Tested
```

---

## ?? N?u Test Fail?

### 1. Xem Error Message
```bash
# Chi ti?t error
dotnet test --logger "console;verbosity=detailed"
```

### 2. Debug Test
- Visual Studio: Right-click test > **Debug**
- ??t breakpoint trong test ho?c code chính

### 3. Check Logs
- Test Explorer > Click test > Xem Output
- Console output

---

## ?? Tips

### Ch?y Tests C? Th?
```powershell
# Ch? UI tests
.\RunTests.ps1 -TestType ui

# Ch? Chart tests  
.\RunTests.ps1 -TestType charts

# V?i verbose output
.\RunTests.ps1 -Verbose
```

### Skip Slow Tests
```bash
# Ch? quick tests (b? integration)
dotnet test --filter "TestCategory!=Database&TestCategory!=Statistics"
```

---

## ?? More Info

- ?? Chi ti?t ??y ??: `README.md`
- ?? Test th? công: `MANUAL_TEST_CASES.md`
- ?? T?ng quan: `SUMMARY.md`

---

## ? Help

**Tests không ch?y?**
1. Rebuild solution
2. Restart Visual Studio
3. Check `README.md` Troubleshooting section

**Need more help?**
- Read full `README.md`
- Contact team lead

---

**Version:** 1.0  
**Last Updated:** 2025-01-XX
