[CmdletBinding()]
param([string]$PackageDirectory = "artifacts/package")

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$targetRoot = Split-Path -Parent $PSScriptRoot
$packages = [IO.Path]::GetFullPath($PackageDirectory, $targetRoot)
[xml]$project = Get-Content (Join-Path $targetRoot "src/Briosa.Client/Briosa.Client.csproj")
$id = [string]$project.Project.PropertyGroup.PackageId
$version = [string]$project.Project.PropertyGroup.VersionPrefix
$temporaryBase = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$consumer = Join-Path $temporaryBase "briosa-dotnet-consumer-$([Guid]::NewGuid().ToString('N'))"
[IO.Directory]::CreateDirectory($consumer) | Out-Null
try {
    $escapedPackages = [Security.SecurityElement]::Escape($packages)
    $isolatedCache = [Security.SecurityElement]::Escape((Join-Path $consumer "packages"))
    @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework><RestorePackagesPath>$isolatedCache</RestorePackagesPath><RestoreSources>$escapedPackages;https://api.nuget.org/v3/index.json</RestoreSources></PropertyGroup>
  <ItemGroup><PackageReference Include="$id" Version="$version" /></ItemGroup>
</Project>
"@ | Set-Content (Join-Path $consumer "Consumer.csproj")
    @"
using Briosa;
await using var client = new BriosaClient();
if (typeof(BriosaClient).Assembly.GetName().Name != "$id") throw new System.Exception("Wrong target assembly");
System.Console.WriteLine("Verified using Briosa from $id");
"@ | Set-Content (Join-Path $consumer "Program.cs")
    dotnet run --project (Join-Path $consumer "Consumer.csproj") --configuration Release
    if ($LASTEXITCODE -ne 0) { throw "The packed NuGet consumer failed." }
}
finally {
    $resolved = [IO.Path]::GetFullPath($consumer)
    if ($resolved.StartsWith($temporaryBase, [StringComparison]::OrdinalIgnoreCase)) {
        Remove-Item -LiteralPath $resolved -Recurse -Force
    }
}
