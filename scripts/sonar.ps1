# Run from the repository root: .\scripts\sonar.ps1
# Requires JDK 25 and SonarQube running at HostUrl.
[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()]
    [string]$ProjectKey = 'coachbooking',
    [ValidateNotNullOrEmpty()]
    [string]$ProjectName = 'coachbooking',
    [ValidateNotNullOrEmpty()]
    [string]$HostUrl = 'http://localhost:9000'
)

function Get-SonarTokenFromEnvFile {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }

    $fileToken = $null
    foreach ($line in [System.IO.File]::ReadLines($Path)) {
        $tokenEntry = [regex]::Match($line, '^\s*(?:export\s+)?SONAR_TOKEN\s*=\s*(.*)$')
        if (-not $tokenEntry.Success) { continue }
        $value = $tokenEntry.Groups[1].Value.Trim()
        $quotedValue = [regex]::Match($value, '^(["''])(.*?)\1\s*(?:#.*)?$')
        if ($quotedValue.Success) {
            $fileToken = $quotedValue.Groups[2].Value
        }
        else {
            $fileToken = ($value -replace '\s+#.*$', '').Trim()
        }
    }
    return $fileToken
}

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent
$backendDirectory = Join-Path $repoRoot 'backend'
$wrapper = Join-Path $backendDirectory 'gradlew.bat'
if (-not (Test-Path -LiteralPath $wrapper)) {
    throw "Gradle wrapper not found: $wrapper"
}

$previousToken = $env:SONAR_TOKEN
$scanExitCode = 1
Push-Location -LiteralPath $backendDirectory
try {
    if ([string]::IsNullOrWhiteSpace($env:SONAR_TOKEN)) {
        $env:SONAR_TOKEN = Get-SonarTokenFromEnvFile -Path (Join-Path $repoRoot '.env')
    }
    if ([string]::IsNullOrWhiteSpace($env:SONAR_TOKEN)) {
        $secureToken = Read-Host 'SonarQube token (hidden input)' -AsSecureString
        if ($secureToken.Length -eq 0) { throw 'A SonarQube token is required.' }
        $tokenPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureToken)
        try {
            $env:SONAR_TOKEN = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($tokenPointer)
        }
        finally {
            [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($tokenPointer)
            $secureToken.Dispose()
        }
    }

    # SonarScanner reads SONAR_TOKEN from the process environment.
    # Keep the token out of source files and command-line arguments.
    $gradleArguments = @(
        '--no-daemon',
        'test',
        'jacocoTestReport',
        'sonar',
        "-Dsonar.projectKey=$ProjectKey",
        "-Dsonar.projectName=$ProjectName",
        "-Dsonar.host.url=$HostUrl"
    )
    & $wrapper @gradleArguments
    $scanExitCode = $LASTEXITCODE
}
finally {
    $env:SONAR_TOKEN = $previousToken
    Pop-Location
}

exit $scanExitCode
