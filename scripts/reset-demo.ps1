param([switch]$Apply)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent

function Invoke-Docker {
    param([string[]]$DockerArguments)
    & docker @DockerArguments
    if ($LASTEXITCODE -ne 0) {
        throw "Docker command failed (exit $LASTEXITCODE). Data reset was not completed."
    }
}

$dbRunning = Invoke-Docker -DockerArguments @('inspect', '--format', '{{.State.Running}}', 'coachbooking-db')
if ($dbRunning -ne 'true') { throw 'coachbooking-db must be running.' }
$labels = Invoke-Docker -DockerArguments @('inspect', '--format', '{{json .Config.Labels}}', 'coachbooking-db') | ConvertFrom-Json
if ($labels.'com.docker.compose.project' -ne 'coachbooking') { throw 'Target container must belong to the coachbooking Compose project.' }
$dbName = Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'printenv', 'POSTGRES_DB')
$dbUser = Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'printenv', 'POSTGRES_USER')
if ($dbName -ne 'coachbooking' -or $dbUser -ne 'coachbooking') {
    throw 'This opt-in script targets only the documented local coachbooking database/user.'
}

Write-Host 'Preserved: accounts, roles, customer/employee profiles, authentication data and Flyway history.'
Write-Host 'Replaced: transport catalogs, trips/schedules, seat holds, tickets, invoices and payments.'
Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'psql', '-U', $dbUser, '-d', $dbName, '-X',
    '-v', 'ON_ERROR_STOP=1', '-c',
    "SELECT 'accounts' resource,count(*) FROM accounts UNION ALL SELECT 'trips',count(*) FROM trips UNION ALL SELECT 'tickets',count(*) FROM tickets UNION ALL SELECT 'invoices',count(*) FROM invoices;")
if (-not $Apply) {
    Write-Host 'Preview only. To back up and apply the authorized local demo reset: .\scripts\reset-demo.ps1 -Apply'
    return
}

$schemaReady = Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'psql', '-U', $dbUser, '-d', $dbName,
    '-X', '-tAc', "SELECT count(*) FROM flyway_schema_history WHERE version='9' AND success;")
if ($schemaReady.Trim() -ne '1') { throw 'Apply Flyway through V9 before using this script.' }

$wasRunning = Invoke-Docker -DockerArguments @('inspect', '--format', '{{.State.Running}}', 'coachbooking-backend')
$backupDirectory = Join-Path $repoRoot 'backend/build/demo-backups'
New-Item -ItemType Directory -Force -Path $backupDirectory | Out-Null
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$backupFile = Join-Path $backupDirectory "coachbooking-before-reset-$stamp.dump"
$containerBackup = "/tmp/coachbooking-before-reset-$stamp.dump"
try {
    if ($wasRunning -eq 'true') {
        Invoke-Docker -DockerArguments @('stop', 'coachbooking-backend') | Out-Null
    }
    Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'pg_dump', '-U', $dbUser, '-d', $dbName,
        '-Fc', '-f', $containerBackup)
    Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'pg_restore', '--list', $containerBackup) | Out-Null
    Invoke-Docker -DockerArguments @('cp', "coachbooking-db:$containerBackup", $backupFile)
    if ((Get-Item -LiteralPath $backupFile).Length -eq 0) { throw 'Backup is empty; aborting reset.' }
    Write-Host "Backup saved outside Git tracking: $backupFile"
    Invoke-Docker -DockerArguments @('cp', (Join-Path $PSScriptRoot 'reset-demo.sql'),
        'coachbooking-db:/tmp/coachbooking-reset-demo.sql')
    Invoke-Docker -DockerArguments @('exec', 'coachbooking-db', 'psql', '-U', $dbUser, '-d', $dbName,
        '-X', '-v', 'ON_ERROR_STOP=1', '-v', 'confirm_demo_reset=RESET_COACHBOOKING_DEMO',
        '-f', '/tmp/coachbooking-reset-demo.sql')
    Write-Host 'Demo reset committed. Existing user accounts and profiles were preserved.'
}
finally {
    if ($wasRunning -eq 'true') {
        Invoke-Docker -DockerArguments @('start', 'coachbooking-backend') | Out-Null
    }
}
