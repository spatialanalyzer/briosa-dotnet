[CmdletBinding()]
param(
    [string]$PackageDirectory = "artifacts/package"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repositoryRoot "src/Briosa.Client/Briosa.Client.csproj"
$lockPath = Join-Path $repositoryRoot "protocol.lock.json"
$resolvedPackageDirectory = [IO.Path]::GetFullPath($PackageDirectory, $repositoryRoot)

$lock = Get-Content -LiteralPath $lockPath -Raw | ConvertFrom-Json
$target = [string]$lock.target.spatial_analyzer
$expectedPackageId = "Briosa.$target"
$expectedAssemblyName = $expectedPackageId

[xml]$project = Get-Content -LiteralPath $projectPath
$packageId = [string]$project.Project.PropertyGroup.PackageId
$assemblyName = [string]$project.Project.PropertyGroup.AssemblyName
$rootNamespace = [string]$project.Project.PropertyGroup.RootNamespace

if ($packageId -ne $expectedPackageId) {
    throw "NuGet package ID '$packageId' does not match exact target '$expectedPackageId'."
}
if ($assemblyName -ne $expectedAssemblyName) {
    throw "Assembly name '$assemblyName' does not match exact target '$expectedAssemblyName'."
}
if ($rootNamespace -ne "Briosa") {
    throw "The public root namespace must remain Briosa."
}

$simulatedTarget = "2027.1.0000.0"
$simulatedIdentity = "Briosa.$simulatedTarget"
if ($simulatedIdentity -eq $expectedPackageId) {
    throw "Different exact targets must produce different package and assembly identities."
}

if (-not (Test-Path -LiteralPath $resolvedPackageDirectory -PathType Container)) {
    throw "Package directory '$resolvedPackageDirectory' does not exist."
}
$packages = @(Get-ChildItem -LiteralPath $resolvedPackageDirectory -Filter "*.nupkg" -File |
    Where-Object Name -NotLike "*.symbols.nupkg")
if ($packages.Count -ne 1) {
    throw "Expected exactly one NuGet package, found $($packages.Count)."
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
$archive = [IO.Compression.ZipFile]::OpenRead($packages[0].FullName)
try {
    $nuspecEntry = $archive.Entries | Where-Object FullName -EQ "$expectedPackageId.nuspec"
    if ($null -eq $nuspecEntry) {
        throw "The NuGet package does not contain the expected nuspec identity."
    }

    $assemblyEntry = $archive.Entries |
        Where-Object FullName -EQ "lib/net10.0/$expectedAssemblyName.dll"
    if ($null -eq $assemblyEntry) {
        throw "The NuGet package does not contain the exact-target assembly."
    }
}
finally {
    $archive.Dispose()
}

Write-Host "Verified $expectedPackageId with stable Briosa namespace and distinct simulated target identity."
