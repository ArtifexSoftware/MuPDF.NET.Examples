#Requires -Version 5.1
<#
.SYNOPSIS
  Run all MuPDF.NET.Examples projects and report PASS/FAIL/SKIP vs Expected/ baselines.

.PARAMETER UpdateExpected
  Refresh Expected/ baselines from the current NuGet packages.
#>
param(
    [switch] $UpdateExpected,
    [string] $Configuration = 'Release'
)

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

# Do not use $IsWindows — it is a read-only automatic variable in PowerShell 6+.
$useCmdHost = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::Windows)

dotnet build -c $Configuration
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$projects = @(Get-ChildItem -Recurse -Filter *.csproj |
    Where-Object { $_.Directory.Name -match '^\d{2}-' } |
    Sort-Object FullName)

$fail = 0
$skip = 0
$pass = 0

foreach ($p in $projects) {
    $name = $p.Directory.Name

    $argList = @(
        'run', '--project', $p.FullName,
        '-c', $Configuration, '--no-build'
    )
    if ($UpdateExpected) {
        $argList += '--'
        $argList += '--update-expected'
    }

    $tmpName = "mupdf-examples-" + [guid]::NewGuid().ToString('N') + ".log"
    $tmp = if ($env:TEMP) { Join-Path $env:TEMP $tmpName } else { Join-Path ([IO.Path]::GetTempPath()) $tmpName }

    if ($useCmdHost) {
        # cmd so native AVs / stderr do not abort the PowerShell driver on Windows.
        $joined = ($argList | ForEach-Object {
            if ($_ -match '\s') { '"' + $_ + '"' } else { $_ }
        }) -join ' '
        cmd /c "dotnet $joined > `"$tmp`" 2>&1"
    }
    else {
        & dotnet @argList *> $tmp
    }

    $out = Get-Content -Raw -ErrorAction SilentlyContinue $tmp
    Remove-Item $tmp -Force -ErrorAction SilentlyContinue

    if ($out -match 'SKIP —') {
        Write-Host "SKIP  $name"
        $skip++
        continue
    }

    $ok = if ($UpdateExpected) {
        $out -match 'Baselines updated'
    } else {
        $out -match 'PASS —'
    }

    if ($ok) {
        Write-Host "PASS  $name"
        $pass++
    }
    else {
        Write-Host "FAIL  $name"
        if ($out) { Write-Host $out }
        $fail++
    }
}

Write-Host ""
Write-Host "PASS: $pass  SKIP: $skip  FAIL: $fail  /  $($projects.Count) projects"
exit $(if ($fail -eq 0) { 0 } else { 1 })
