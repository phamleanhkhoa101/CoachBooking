# H??ng D?n Ch?y Tests - QLBXMD

## ?? M?c L?c
1. [Gi?i Thi?u](#gi?i-thi?u)
2. [Cài ??t](#cài-??t)
3. [Ch?y Unit Tests](#ch?y-unit-tests)
4. [Ch?y Integration Tests](#ch?y-integration-tests)
5. [Ch?y Manual Tests](#ch?y-manual-tests)
6. [Xem K?t Qu?](#xem-k?t-qu?)
7. [Troubleshooting](#troubleshooting)

---

## Gi?i Thi?u

Project QLBXMD có 3 lo?i tests:
1. **Unit Tests** - Test các thành ph?n UI riêng l?
2. **Integration Tests** - Test t??ng tác v?i database
3. **Manual Tests** - Test th? công theo checklist

---

## Cài ??t

### Yêu C?u
- Visual Studio 2019/2022
- .NET Framework 4.8
- SQL Server (cho Integration Tests)
- MSTest Framework (?ã ???c cài ??t)

### Cài ??t Test Project

#### B??c 1: Add Test Project vào Solution
```bash
# M? Developer Command Prompt ho?c PowerShell
cd C:\Users\phaml\Desktop\bxmd\QLBXMD\

# T?o test project
dotnet new mstest -n QLBXMD.Tests -o QLBXMD.Tests -f net48
```

#### B??c 2: Add Reference ??n Project Chính
```bash
cd QLBXMD.Tests
dotnet add reference ..\QLBXMD\QLBXMD.csproj
```

#### B??c 3: Install Packages (N?u C?n)
```bash
# EntityFramework (cho integration tests)
dotnet add package EntityFramework -v 6.5.1

# Moq (cho mocking)
dotnet add package Moq -v 4.18.4
```

#### B??c 4: Add vào Solution
```bash
# Quay v? th? m?c solution
cd ..
dotnet sln add QLBXMD.Tests\QLBXMD.Tests.csproj
```

---

## Ch?y Unit Tests

### Ph??ng Pháp 1: S? D?ng Visual Studio

#### Test Explorer
1. M? Visual Studio
2. M? menu: **Test > Test Explorer**
3. Build solution: `Ctrl + Shift + B`
4. Trong Test Explorer, click **Run All Tests**

![Test Explorer](https://i.imgur.com/example.png)

#### Run Specific Test
1. Trong Test Explorer, tìm test mu?n ch?y
2. Right-click > **Run**
3. Ho?c click vào test và nh?n `Ctrl + R, T`

#### Debug Test
1. ??t breakpoint trong test code
2. Right-click test > **Debug**
3. Ho?c `Ctrl + R, Ctrl + T`

---

### Ph??ng Pháp 2: Command Line

#### Ch?y T?t C? Tests
```bash
# M? PowerShell/CMD t?i th? m?c solution
cd C:\Users\phaml\Desktop\bxmd\QLBXMD\

# Ch?y t?t c? tests
dotnet test QLBXMD.Tests\QLBXMD.Tests.csproj
```

#### Ch?y Tests Theo Category
```bash
# Ch? ch?y UI tests
dotnet test --filter "TestCategory=UI"

# Ch? ch?y Function tests
dotnet test --filter "TestCategory=Function"

# Ch? ch?y Chart tests
dotnet test --filter "TestCategory=Charts"
```

#### Ch?y Test C? Th?
```bash
# Ch?y 1 test method
dotnet test --filter "FullyQualifiedName~Constructor_ShouldInitializeSuccessfully"
```

#### Xem Output Chi Ti?t
```bash
# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

---

## Ch?y Integration Tests

### Chu?n B? Database

#### 1. ??m B?o SQL Server ?ang Ch?y
```sql
-- Ki?m tra connection
SELECT @@VERSION;
```

#### 2. Ki?m Tra Connection String
```xml
<!-- File: App.config -->
<connectionStrings>
  <add name="BXMDEntities" 
       connectionString="metadata=...;provider connection string=&quot;data source=YOUR_SERVER;initial catalog=QLBXMD;...&quot;" 
       providerName="System.Data.EntityClient" />
</connectionStrings>
```

#### 3. T?o D? Li?u Test (Tùy Ch?n)
```sql
-- Script trong file: QLBXMD.Tests\TestData.sql
-- Ch?y script này ?? t?o d? li?u test
```

### Ch?y Integration Tests

```bash
# Ch?y t?t c? integration tests
dotnet test --filter "TestCategory=Database|TestCategory=Statistics"

# Ho?c ch? Statistics tests
dotnet test --filter "TestCategory=Statistics"

# Ho?c ch? Database tests
dotnet test --filter "TestCategory=Database"
```

### ?? L?u Ý
- Integration tests s? **READ** d? li?u th?t t? database
- Không th?c hi?n INSERT/UPDATE/DELETE trong production database
- Nên s? d?ng test database riêng

---

## Ch?y Manual Tests

### S? D?ng Checklist

1. M? file: `QLBXMD.Tests\MANUAL_TEST_CASES.md`
2. In ra ho?c m? trên màn hình th? 2
3. Th?c hi?n t?ng test case theo th? t?
4. ?ánh d?u Pass/Fail cho m?i case

### Test Scenarios

#### Scenario 1: Test Load D? Li?u Ban ??u
```
1. M? ?ng d?ng
2. ??ng nh?p Admin
3. Click "Th?ng Kê"
4. Ki?m tra:
   ? T? ngày = Ngày ??u tháng
   ? ??n ngày = Hôm nay
   ? Hi?n th? d? li?u
   ? Bi?u ?? render
```

#### Scenario 2: Test Filter
```
1. Ch?n T? ngày: 01/01/2024
2. Ch?n ??n ngày: 31/01/2024
3. Click "Th?ng Kê"
4. Ki?m tra d? li?u c?p nh?t
```

#### Scenario 3: Test Validation
```
1. Ch?n T? ngày > ??n ngày
2. Click "Th?ng Kê"
3. Ki?m tra hi?n th? l?i
```

---

## Xem K?t Qu?

### Visual Studio Test Explorer

#### Xem Summary
- **Green checkmark**: Test passed
- **Red X**: Test failed
- **Yellow exclamation**: Test skipped

#### Xem Chi Ti?t
1. Click vào test
2. Xem panel bên d??i
3. ??c error message, stack trace

#### Export Results
1. Trong Test Explorer, click icon **Export**
2. Ch?n format (CSV, XML, TRX)
3. Save file

---

### Command Line Output

```bash
# K?t qu? m?u
Test run for C:\...\QLBXMD.Tests.dll (.NETFramework,Version=v4.8)
Microsoft (R) Test Execution Command Line Tool Version 17.0.0

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    15, Skipped:     0, Total:    15, Duration: 2 s
```

---

### Test Results File (TRX)

```bash
# T?o TRX file
dotnet test --logger "trx;LogFileName=testresults.trx"

# File s? ???c t?o t?i:
# QLBXMD.Tests\TestResults\testresults.trx
```

#### Xem TRX File
1. M? Visual Studio
2. Menu: **Test > Analyze Test Run > Import Test Results**
3. Ch?n file `.trx`
4. View trong Test Explorer

---

## Troubleshooting

### ? L?i: Test Project Không Build

**Tri?u ch?ng:**
```
Error CS0246: The type or namespace name 'UCThongKe' could not be found
```

**Gi?i pháp:**
```bash
# 1. Ki?m tra reference
dotnet list QLBXMD.Tests\QLBXMD.Tests.csproj reference

# 2. Add l?i n?u thi?u
dotnet add QLBXMD.Tests\QLBXMD.Tests.csproj reference QLBXMD\QLBXMD.csproj

# 3. Rebuild
dotnet build
```

---

### ? L?i: Tests Không Hi?n Th? Trong Test Explorer

**Gi?i pháp:**
1. Clean solution: `Build > Clean Solution`
2. Rebuild: `Build > Rebuild Solution`
3. Restart Visual Studio
4. Reopen Test Explorer

---

### ? L?i: Database Connection Failed

**Tri?u ch?ng:**
```
System.Data.SqlClient.SqlException: Cannot open database "QLBXMD"
```

**Gi?i pháp:**
1. Ki?m tra SQL Server ?ang ch?y
2. Ki?m tra connection string trong `App.config`
3. Test connection:
   ```sql
   sqlcmd -S YOUR_SERVER -d QLBXMD -E
   ```

---

### ? L?i: Test B? Timeout

**Tri?u ch?ng:**
```
Test 'LoadStatistics_ShouldCompleteWithinTimeout' exceeded timeout of 5000ms
```

**Gi?i pháp:**
1. T?ng timeout:
   ```csharp
   [Timeout(10000)] // 10 giây
   ```
2. Optimize query trong code
3. Add index vào database

---

### ? L?i: Access Denied khi ch?y test

**Tri?u ch?ng:**
```
System.UnauthorizedAccessException: Access to the path is denied
```

**Gi?i pháp:**
1. Ch?y Visual Studio as Administrator
2. Ki?m tra quy?n file/folder
3. Disable antivirus t?m th?i

---

## Best Practices

### ? DO

1. **Ch?y tests tr??c khi commit**
   ```bash
   git add .
   dotnet test
   git commit -m "Your message"
   ```

2. **Vi?t test cho bug fix**
   - Tìm bug
   - Vi?t test reproduce bug
   - Fix bug
   - Verify test pass

3. **Maintain test code**
   - Refactor tests khi refactor code
   - Update tests khi requirements thay ??i

4. **Use meaningful test names**
   ```csharp
   // Good
   [TestMethod]
   public void GetDoanhThu_WithValidDateRange_ShouldReturnPositiveValue()
   
   // Bad
   [TestMethod]
   public void Test1()
   ```

---

### ? DON'T

1. **Không skip tests**
   ```csharp
   // Bad
   [Ignore("Will fix later")]
   public void SomeTest() { }
   ```

2. **Không hard-code data**
   ```csharp
   // Bad
   Assert.AreEqual(12345, tongDoanhThu);
   
   // Good
   Assert.IsTrue(tongDoanhThu >= 0);
   ```

3. **Không test private methods tr?c ti?p**
   - Test through public interface
   - S? d?ng reflection ch? khi th?c s? c?n

4. **Không có dependencies gi?a tests**
   - M?i test ph?i independent
   - Không d?a vào th? t? ch?y

---

## CI/CD Integration

### Azure DevOps Pipeline

```yaml
# azure-pipelines.yml
trigger:
- master

pool:
  vmImage: 'windows-latest'

steps:
- task: NuGetCommand@2
  inputs:
    restoreSolution: '**/*.sln'

- task: VSBuild@1
  inputs:
    solution: '**/*.sln'
    configuration: 'Release'

- task: VSTest@2
  inputs:
    testSelector: 'testAssemblies'
    testAssemblyVer2: |
      **\*Tests.dll
      !**\*TestAdapter.dll
      !**\obj\**
    searchFolder: '$(System.DefaultWorkingDirectory)'
```

---

### GitHub Actions

```yaml
# .github/workflows/tests.yml
name: Run Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '4.8.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

---

## Resources

### Documentation
- [MSTest Documentation](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [Entity Framework Testing](https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/)
- [Moq Documentation](https://github.com/moq/moq4/wiki/Quickstart)

### Tools
- [Test Explorer](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer)
- [Code Coverage](https://docs.microsoft.com/en-us/visualstudio/test/using-code-coverage-to-determine-how-much-code-is-being-tested)
- [Live Unit Testing](https://docs.microsoft.com/en-us/visualstudio/test/live-unit-testing)

---

## Contact & Support

N?u g?p v?n ??:
1. Xem [Troubleshooting](#troubleshooting)
2. Check GitHub Issues
3. Contact team lead

**Version:** 1.0  
**Last Updated:** 2025-01-XX  
**Author:** QLBXMD Team
