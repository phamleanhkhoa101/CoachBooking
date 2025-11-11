# ============================================
# QLBXMD Test Runner Script
# ============================================
# M?c ?ích: Script PowerShell ?? ch?y tests nhanh
# S? d?ng: .\RunTests.ps1 [option]
# ============================================

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("all", "unit", "integration", "ui", "charts", "stats", "quick")]
    [string]$TestType = "all",
    
    [Parameter(Mandatory=$false)]
    [switch]$Verbose,
    
    [Parameter(Mandatory=$false)]
    [switch]$Coverage
)

# Colors
$Green = "Green"
$Red = "Red"
$Yellow = "Yellow"
$Cyan = "Cyan"

# Header
Write-Host "`n============================================" -ForegroundColor $Cyan
Write-Host "   QLBXMD Test Runner" -ForegroundColor $Cyan
Write-Host "============================================`n" -ForegroundColor $Cyan

# Check if test project exists
$testProject = "QLBXMD.Tests\QLBXMD.Tests.csproj"
if (-not (Test-Path $testProject)) {
    Write-Host "? Test project not found: $testProject" -ForegroundColor $Red
    Write-Host "Please run setup first." -ForegroundColor $Yellow
    exit 1
}

# Build first
Write-Host "?? Building solution..." -ForegroundColor $Cyan
$buildResult = dotnet build --configuration Debug 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed!" -ForegroundColor $Red
    Write-Host $buildResult
    exit 1
}
Write-Host "? Build successful`n" -ForegroundColor $Green

# Prepare test command
$testCommand = "dotnet test $testProject"

# Add verbosity if requested
if ($Verbose) {
    $testCommand += " --logger `"console;verbosity=detailed`""
}

# Add filter based on test type
switch ($TestType) {
    "unit" {
        Write-Host "?? Running Unit Tests..." -ForegroundColor $Cyan
        $testCommand += " --filter `"TestCategory=Constructor|TestCategory=Validation|TestCategory=UI`""
    }
    "integration" {
        Write-Host "?? Running Integration Tests..." -ForegroundColor $Cyan
        $testCommand += " --filter `"TestCategory=Database|TestCategory=Statistics`""
    }
    "ui" {
        Write-Host "?? Running UI Tests..." -ForegroundColor $Cyan
        $testCommand += " --filter `"TestCategory=UI`""
    }
    "charts" {
        Write-Host "?? Running Chart Tests..." -ForegroundColor $Cyan
        $testCommand += " --filter `"TestCategory=Charts|TestCategory=ChartData`""
    }
    "stats" {
        Write-Host "?? Running Statistics Tests..." -ForegroundColor $Cyan
        $testCommand += " --filter `"TestCategory=Statistics`""
    }
    "quick" {
        Write-Host "? Running Quick Tests (Unit only)..." -ForegroundColor $Cyan
        $testCommand += " --filter `"TestCategory=Constructor|TestCategory=Validation`""
    }
    default {
        Write-Host "?? Running All Tests..." -ForegroundColor $Cyan
    }
}

# Add code coverage if requested
if ($Coverage) {
    Write-Host "?? Code coverage enabled" -ForegroundColor $Yellow
    $testCommand += " --collect:`"Code Coverage`""
}

Write-Host "`nExecuting: $testCommand`n" -ForegroundColor $Yellow

# Run tests
$startTime = Get-Date
Invoke-Expression $testCommand
$exitCode = $LASTEXITCODE
$endTime = Get-Date
$duration = ($endTime - $startTime).TotalSeconds

# Summary
Write-Host "`n============================================" -ForegroundColor $Cyan
Write-Host "   Test Summary" -ForegroundColor $Cyan
Write-Host "============================================" -ForegroundColor $Cyan

if ($exitCode -eq 0) {
    Write-Host "? All tests PASSED!" -ForegroundColor $Green
} else {
    Write-Host "? Some tests FAILED!" -ForegroundColor $Red
}

Write-Host "??  Duration: $([math]::Round($duration, 2)) seconds" -ForegroundColor $Yellow

if ($Coverage) {
    Write-Host "`n?? Code coverage report generated in TestResults folder" -ForegroundColor $Cyan
}

Write-Host "`n============================================`n" -ForegroundColor $Cyan

# Exit with same code as test command
exit $exitCode
