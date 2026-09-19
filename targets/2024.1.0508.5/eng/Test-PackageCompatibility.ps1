[CmdletBinding()]
param(
    [string]$PackageDirectory = 'artifacts/package',
    [string]$LegacyArtifact,
    [string]$CurrentArtifact,
    [string]$CurrentLock = 'artifacts/current-conformance/conformance.lock.json',
    [string]$FixtureExecutable = 'dotnet'
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$version = ([xml](Get-Content (Join-Path $root 'src/Briosa.Client/Briosa.Client.csproj') -Raw)).Project.PropertyGroup.VersionPrefix
$packages = @(Get-ChildItem -LiteralPath ([IO.Path]::GetFullPath($PackageDirectory, $root)) -Filter '*.nupkg' -File)
if ($packages.Count -ne 1) { throw 'Expected exactly one target client package.' }
$legacy = Get-Content (Join-Path $root 'conformance.lock.json') -Raw | ConvertFrom-Json
$current = Get-Content ([IO.Path]::GetFullPath($CurrentLock, $root)) -Raw | ConvertFrom-Json
if (-not $LegacyArtifact) { $LegacyArtifact = Join-Path $root "artifacts/conformance/$($legacy.artifact.file_name)" }
if (-not $CurrentArtifact) { $CurrentArtifact = Join-Path $root "artifacts/current-conformance/$($current.artifact.file_name)" }
foreach ($pair in @(
    @{ name = 'legacy'; artifact = $LegacyArtifact; lock = 'conformance.lock.json' },
    @{ name = 'current'; artifact = $CurrentArtifact; lock = $CurrentLock }
)) {
    $arguments = @{
        ClientPackagePath = $packages[0].FullName
        ClientPackageSha256 = (Get-FileHash $packages[0].FullName -Algorithm SHA256).Hash.ToLowerInvariant()
        ClientVersion = $version; ArtifactPath = $pair.artifact; LockPath = $pair.lock
        EvidencePath = Join-Path $root "artifacts/$($pair.name)-package-evidence.json"
        FixtureExecutable = $FixtureExecutable
    }
    & (Join-Path $PSScriptRoot 'Test-PublishedClientConformance.ps1') @arguments
}

