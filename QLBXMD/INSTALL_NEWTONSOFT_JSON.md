# ?? Cài ??t Newtonsoft.Json Package

## V?n ??

Project s? d?ng `packages.config` format (legacy), không h? tr? `dotnet add package` command.

## ? Gi?i Pháp: Cài ??t Th? Công

### B??c 1: Restore Packages trong Visual Studio

1. M? Solution trong Visual Studio
2. Right-click Solution ? **Restore NuGet Packages**
3. Ho?c: Tools ? NuGet Package Manager ? **Package Manager Console**
4. Ch?y l?nh:
   ```powershell
   Install-Package Newtonsoft.Json -Version 13.0.3
   ```

### B??c 2: (Alternative) Download Manual

N?u không dùng Visual Studio:

1. Download Newtonsoft.Json 13.0.3 t?:
   https://www.nuget.org/packages/Newtonsoft.Json/13.0.3

2. Extract file `.nupkg` (??i extension thành `.zip`)

3. Copy file `Newtonsoft.Json.dll` t? `lib/net45` vào folder `packages/Newtonsoft.Json.13.0.3/lib/net45/`

4. Thêm reference trong `.csproj`:
   ```xml
   <Reference Include="Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL">
     <HintPath>..\packages\Newtonsoft.Json.13.0.3\lib\net45\Newtonsoft.Json.dll</HintPath>
   </Reference>
   ```

### B??c 3: Verify Installation

Build project và check:
- Không có error v? missing Newtonsoft.Json
- Services\MoMoPaymentService.cs compile thành công

## ?? File ?ã C?p Nh?t

? `packages.config` - ?ã thêm Newtonsoft.Json entry

## ?? Sau Khi Cài ??t

Các class s? ho?t ??ng:
- `Services\MoMoPaymentService` - Service thanh toán MoMo
- `MoMoPaymentResponse` - Response model
- `MoMoCallbackRequest` - Callback model

## ?? L?u Ý

N?u Visual Studio báo l?i v? namespace, th?:
1. Close và reopen Visual Studio
2. Clean Solution
3. Rebuild Solution
