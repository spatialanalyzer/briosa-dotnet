[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$ClientPackagePath,
    [Parameter(Mandatory)][ValidatePattern('^[a-f0-9]{64}$')][string]$ClientPackageSha256,
    [Parameter(Mandatory)][ValidatePattern('^[0-9]+\.[0-9]+\.[0-9]+(?:-[0-9A-Za-z.-]+)?$')][string]$ClientVersion,
    [Parameter(Mandatory)][string]$ArtifactPath,
    [Parameter(Mandatory)][string]$LockPath,
    [Parameter(Mandatory)][string]$EvidencePath,
    [string]$PublishedPackageUrl,
    [string]$FixtureExecutable = 'dotnet'
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$targetRoot = Split-Path -Parent $PSScriptRoot
$package = [IO.Path]::GetFullPath($ClientPackagePath, $targetRoot)
if ((Get-FileHash -LiteralPath $package -Algorithm SHA256).Hash.ToLowerInvariant() -cne $ClientPackageSha256) {
    throw 'Client package digest differs from the retained release record.'
}
$temporaryBase = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$consumer = Join-Path $temporaryBase "briosa-published-consumer-$([Guid]::NewGuid().ToString('N'))"
[IO.Directory]::CreateDirectory($consumer) | Out-Null
$previousPythonPath = [Environment]::GetEnvironmentVariable('PYTHONPATH')
try {
    $expectedName = 'Briosa.2024.1.0508.5'
    $archive = [IO.Compression.ZipFile]::OpenRead($package)
    try {
        $entry = @($archive.Entries | Where-Object { $_.FullName -like '*.nuspec' })
        if ($entry.Count -ne 1) { throw 'Expected exactly one NuGet package identity.' }
        $reader = [IO.StreamReader]::new($entry[0].Open())
        try { $metadata = ([xml]$reader.ReadToEnd()).package.metadata } finally { $reader.Dispose() }
        if ($metadata.id -cne $expectedName -or $metadata.version -cne $ClientVersion) {
            throw 'NuGet package identity differs from the retained release record.'
        }
    }
    finally { $archive.Dispose() }
    $feed = Join-Path $consumer 'feed'
    [IO.Directory]::CreateDirectory($feed) | Out-Null
    Copy-Item -LiteralPath $package -Destination (Join-Path $feed "$expectedName.$ClientVersion.nupkg")
    Copy-Item -LiteralPath (Join-Path $targetRoot 'tools/Briosa.Client.Conformance/Program.cs') -Destination $consumer
    $project = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings><Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup><PackageReference Include="$expectedName" Version="[$ClientVersion]" /></ItemGroup>
</Project>
"@
    $project | Set-Content (Join-Path $consumer 'Consumer.csproj') -Encoding utf8
    # An isolated cache ensures the verified package, not a previous local build, is loaded.
    $config = @'
<configuration>
  <packageSources><clear/><add key="retained" value="feed"/><add key="nuget" value="https://api.nuget.org/v3/index.json"/></packageSources>
  <packageSourceMapping><packageSource key="retained"><package pattern="Briosa.*"/></packageSource><packageSource key="nuget"><package pattern="*"/></packageSource></packageSourceMapping>
</configuration>
'@
    $config | Set-Content (Join-Path $consumer 'NuGet.Config') -Encoding utf8
    & $FixtureExecutable restore (Join-Path $consumer 'Consumer.csproj') --configfile (Join-Path $consumer 'NuGet.Config') --packages (Join-Path $consumer 'packages')
    if ($LASTEXITCODE -ne 0) { throw 'Published .NET consumer restore failed.' }
    & $FixtureExecutable build (Join-Path $consumer 'Consumer.csproj') -c Release --no-restore
    if ($LASTEXITCODE -ne 0) { throw 'Published .NET consumer build failed.' }
    $fixture = Join-Path $consumer 'bin/Release/net10.0/Consumer.dll'
    $arguments = @{
        ArtifactPath = $ArtifactPath; LockPath = $LockPath; EvidencePath = $EvidencePath
        FixturePath = $fixture
        ClientPackage = @{
            name = $expectedName; version = $ClientVersion; sha256 = $ClientPackageSha256
            publishedUrl = $PublishedPackageUrl
        }
    }

    & (Join-Path $PSScriptRoot 'Test-Conformance.ps1') @arguments
}
finally {
    [Environment]::SetEnvironmentVariable('PYTHONPATH', $previousPythonPath)
    $resolved = [IO.Path]::GetFullPath($consumer)
    if (-not $resolved.StartsWith($temporaryBase, [StringComparison]::OrdinalIgnoreCase) -or $resolved -eq $temporaryBase) {
        throw 'Refusing cleanup outside the temporary consumer directory.'
    }
    if (Test-Path -LiteralPath $resolved) { Remove-Item -LiteralPath $resolved -Recurse -Force }
}

