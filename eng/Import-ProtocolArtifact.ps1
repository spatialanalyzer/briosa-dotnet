[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$ArtifactPath,

    [switch]$Update,

    [ValidateSet("github_release", "github_actions_artifact", "source_commit_bootstrap")]
    [string]$SourceChannel = "github_release"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$resolvedArtifact = [IO.Path]::GetFullPath($ArtifactPath, $repositoryRoot)
$lockPath = Join-Path $repositoryRoot "protocol.lock.json"
$generatedRoot = Join-Path $repositoryRoot "src\Briosa.Client\Generated"
$temporaryBase = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$temporaryRoot = Join-Path $temporaryBase "briosa-dotnet-protocol-$([Guid]::NewGuid().ToString('N'))"

function Write-Utf8File {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)][string]$Content
    )

    $normalized = [regex]::Replace($Content, "\r\n?", [string][char]10)
    [IO.Directory]::CreateDirectory((Split-Path -Parent $Path)) | Out-Null
    [IO.File]::WriteAllText(
        $Path,
        $normalized,
        [Text.UTF8Encoding]::new($false))
}

function Assert-Equal {
    param(
        [AllowNull()][object]$Actual,
        [AllowNull()][object]$Expected,
        [Parameter(Mandatory)][string]$Message
    )

    if ($Actual -ne $Expected) {
        throw $Message
    }
}

function Get-RelativeFiles {
    param([Parameter(Mandatory)][string]$Root)

    return @(
        Get-ChildItem -LiteralPath $Root -File -Recurse |
            ForEach-Object {
                [IO.Path]::GetRelativePath($Root, $_.FullName).Replace('\', '/')
            } |
            Sort-Object)
}

function Compare-FileTrees {
    param(
        [Parameter(Mandatory)][string]$Expected,
        [Parameter(Mandatory)][string]$Actual
    )

    $expectedFiles = Get-RelativeFiles -Root $Expected
    $actualFiles = Get-RelativeFiles -Root $Actual
    if (Compare-Object $expectedFiles $actualFiles) {
        throw "Generated protocol file paths have drifted."
    }

    foreach ($relativePath in $expectedFiles) {
        $expectedHash = (Get-FileHash -LiteralPath (Join-Path $Expected $relativePath) -Algorithm SHA256).Hash
        $actualHash = (Get-FileHash -LiteralPath (Join-Path $Actual $relativePath) -Algorithm SHA256).Hash
        if ($expectedHash -ne $actualHash) {
            throw "Generated protocol file '$relativePath' has drifted."
        }
    }
}

if (-not (Test-Path -LiteralPath $resolvedArtifact -PathType Leaf)) {
    throw "The protocol artifact does not exist."
}
if (-not $Update -and -not (Test-Path -LiteralPath $lockPath -PathType Leaf)) {
    throw "protocol.lock.json does not exist. Use -Update for an intentional import."
}

$artifactHash = (Get-FileHash -LiteralPath $resolvedArtifact -Algorithm SHA256).Hash.ToLowerInvariant()
$externalChecksumPath = "$resolvedArtifact.sha256"
if (-not (Test-Path -LiteralPath $externalChecksumPath -PathType Leaf)) {
    throw "The adjacent protocol ZIP checksum does not exist."
}
$externalChecksum = (Get-Content -LiteralPath $externalChecksumPath -Raw).Trim()
$expectedChecksum = "$artifactHash  $([IO.Path]::GetFileName($resolvedArtifact))"
Assert-Equal $externalChecksum $expectedChecksum "The external protocol ZIP checksum does not match."

[IO.Directory]::CreateDirectory($temporaryRoot) | Out-Null
try {
    $extractRoot = Join-Path $temporaryRoot "artifact"
    Expand-Archive -LiteralPath $resolvedArtifact -DestinationPath $extractRoot
    $bundleDirectories = @(Get-ChildItem -LiteralPath $extractRoot -Directory)
    if ($bundleDirectories.Count -ne 1) {
        throw "The protocol artifact must contain exactly one top-level directory."
    }

    $bundleRoot = $bundleDirectories[0].FullName
    $manifestPath = Join-Path $bundleRoot "manifest.json"
    $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
    Assert-Equal $manifest.schema_version 2 "Unsupported protocol manifest schema."
    Assert-Equal $manifest.artifact_kind "briosa_protocol" "Unexpected artifact kind."
    Assert-Equal $manifest.client_generation_contract "standard-protobuf-grpc" "Unsupported client generation contract."
    Assert-Equal ([IO.Path]::GetFileNameWithoutExtension($resolvedArtifact)) $manifest.artifact_name "The protocol artifact file name does not match its manifest."

    $checksumEntries = @{}
    foreach ($line in Get-Content -LiteralPath (Join-Path $bundleRoot "files.sha256")) {
        $match = [regex]::Match($line, '^([0-9a-f]{64})  (.+)$')
        if (-not $match.Success) {
            throw "Malformed files.sha256 entry."
        }

        $checksumEntries[$match.Groups[2].Value] = $match.Groups[1].Value
    }
    foreach ($entry in $checksumEntries.GetEnumerator()) {
        $contentPath = Join-Path $bundleRoot $entry.Key
        if (-not (Test-Path -LiteralPath $contentPath -PathType Leaf)) {
            throw "Protocol content '$($entry.Key)' is missing."
        }

        $actualHash = (Get-FileHash -LiteralPath $contentPath -Algorithm SHA256).Hash.ToLowerInvariant()
        Assert-Equal $actualHash $entry.Value "Protocol content checksum mismatch."
    }
    $checksummedPaths = @($checksumEntries.Keys | Sort-Object)
    $actualChecksummedPaths = @(
        Get-RelativeFiles -Root $bundleRoot |
            Where-Object { $_ -ne "files.sha256" } |
            Sort-Object)
    if (Compare-Object $checksummedPaths $actualChecksummedPaths) {
        throw "The protocol artifact contains missing or unchecked files."
    }

    $manifestFiles = @($manifest.files | Sort-Object path)
    $actualManifestPaths = @(
        $actualChecksummedPaths |
            Where-Object { $_ -ne "manifest.json" } |
            Sort-Object)
    if (Compare-Object @($manifestFiles.path) $actualManifestPaths) {
        throw "The protocol manifest file list is incomplete."
    }
    foreach ($manifestFile in $manifestFiles) {
        Assert-Equal $manifestFile.sha256 $checksumEntries[$manifestFile.path] "Protocol manifest checksum drift."
    }

    if (-not $Update) {
        $lock = Get-Content -LiteralPath $lockPath -Raw | ConvertFrom-Json
        Assert-Equal $lock.schema_version 2 "Unsupported protocol lock schema."
        Assert-Equal $artifactHash $lock.artifact.sha256 "Protocol ZIP checksum drift."
        Assert-Equal $manifest.artifact_name $lock.artifact.name "Artifact name drift."
        Assert-Equal $manifest.briosa_version $lock.artifact.briosa_version "Briosa version drift."
        Assert-Equal $manifest.source_revision $lock.artifact.source_revision "Source revision drift."
        Assert-Equal $manifest.protocol_schema_sha256 $lock.protocol.schema_sha256 "Protocol schema drift."
        Assert-Equal $manifest.descriptor_set_sha256 $lock.protocol.descriptor_sha256 "Descriptor drift."
        Assert-Equal $manifest.protocol_package $lock.protocol.package "Protocol package drift."
        Assert-Equal $manifest.client_generation_contract $lock.protocol.generation_contract "Client generation contract drift."
        Assert-Equal $manifest.spatial_analyzer_target $lock.target.spatial_analyzer "SA target drift."
    }

    $packagesLine = & dotnet nuget locals global-packages --list
    if ($LASTEXITCODE -ne 0) {
        throw "Could not locate the NuGet global-packages directory."
    }
    $packagesRoot = ($packagesLine -replace '^global-packages:\s*', '').Trim()
    [xml]$packageVersions = Get-Content -LiteralPath (Join-Path $repositoryRoot "Directory.Packages.props")
    $grpcToolsVersion = [string](
        $packageVersions.Project.ItemGroup.PackageVersion |
            Where-Object Include -EQ "Grpc.Tools").Version
    $grpcToolsRoot = Join-Path $packagesRoot "grpc.tools\$grpcToolsVersion\tools\windows_x64"
    $protoc = Join-Path $grpcToolsRoot "protoc.exe"
    $plugin = Join-Path $grpcToolsRoot "grpc_csharp_plugin.exe"
    if (-not (Test-Path -LiteralPath $protoc -PathType Leaf) -or
        -not (Test-Path -LiteralPath $plugin -PathType Leaf)) {
        throw "Restore Grpc.Tools $grpcToolsVersion before importing the protocol artifact."
    }

    $protoRoot = Join-Path $bundleRoot "proto"
    $generatedProtocolRoot = Join-Path $temporaryRoot "generated\Protocol"
    [IO.Directory]::CreateDirectory($generatedProtocolRoot) | Out-Null
    $protoFiles = @(
        Get-ChildItem -LiteralPath $protoRoot -Filter "*.proto" -File -Recurse |
            ForEach-Object {
                [IO.Path]::GetRelativePath($protoRoot, $_.FullName).Replace('\', '/')
            } |
            Sort-Object)
    $protocArguments = @(
        "--proto_path=$protoRoot",
        "--csharp_out=$generatedProtocolRoot",
        "--grpc_out=$generatedProtocolRoot",
        "--grpc_opt=no_server",
        "--plugin=protoc-gen-grpc=$plugin") + $protoFiles
    Push-Location $protoRoot
    try {
        & $protoc @protocArguments
        if ($LASTEXITCODE -ne 0) {
            throw "C# protocol generation failed."
        }
    }
    finally {
        Pop-Location
    }

    foreach ($generatedSource in Get-ChildItem -LiteralPath $generatedProtocolRoot -Filter "*.cs" -File) {
        $content = [IO.File]::ReadAllText($generatedSource.FullName)
        $content = $content.Replace(
            "namespace Briosa {",
            "namespace Briosa.Client.Transport {")
        $content = $content.Replace(
            "global::Briosa.",
            "global::Briosa.Client.Transport.")
        Write-Utf8File -Path $generatedSource.FullName -Content $content
    }

    $generatedIdentityRoot = Join-Path $temporaryRoot "generated"
    $identity = @"
// <auto-generated />
namespace Briosa.Client.Transport;

/// <summary>Exact protocol artifact identity used to generate this client package.</summary>
internal static class BriosaProtocolIdentity
{
    /// <summary>Gets the protocol artifact's stable name.</summary>
    public const string ArtifactName = "$($manifest.artifact_name)";
    /// <summary>Gets the SHA-256 of the complete protocol ZIP.</summary>
    public const string ArtifactSha256 = "$artifactHash";
    /// <summary>Gets the Briosa version coordinate used to build the artifact.</summary>
    public const string BriosaVersion = "$($manifest.briosa_version)";
    /// <summary>Gets the immutable Briosa source revision used to build the artifact.</summary>
    public const string SourceRevision = "$($manifest.source_revision)";
    /// <summary>Gets the aggregate canonical protobuf-source fingerprint.</summary>
    public const string ProtocolSchemaSha256 = "$($manifest.protocol_schema_sha256)";
    /// <summary>Gets the pure protobuf descriptor-set fingerprint.</summary>
    public const string DescriptorSetSha256 = "$($manifest.descriptor_set_sha256)";
    /// <summary>Gets the stable public protobuf package.</summary>
    public const string ProtocolPackage = "$($manifest.protocol_package)";
    /// <summary>Gets the standard client-generation contract.</summary>
    public const string ClientGenerationContract = "$($manifest.client_generation_contract)";
    /// <summary>Gets the exact SpatialAnalyzer release target.</summary>
    public const string SpatialAnalyzerTarget = "$($manifest.spatial_analyzer_target)";
}
"@
    Write-Utf8File -Path (Join-Path $generatedIdentityRoot "BriosaProtocolIdentity.g.cs") -Content $identity

    if ($Update) {
        $lock = [ordered]@{
            schema_version = 2
            artifact = [ordered]@{
                name = [string]$manifest.artifact_name
                file_name = [IO.Path]::GetFileName($resolvedArtifact)
                sha256 = $artifactHash
                briosa_version = [string]$manifest.briosa_version
                source_revision = [string]$manifest.source_revision
                source_repository = "https://github.com/spatialanalyzer/briosa"
                source_channel = $SourceChannel
            }
            protocol = [ordered]@{
                generation_contract = [string]$manifest.client_generation_contract
                schema_sha256 = [string]$manifest.protocol_schema_sha256
                descriptor_sha256 = [string]$manifest.descriptor_set_sha256
                package = [string]$manifest.protocol_package
            }
            target = [ordered]@{
                spatial_analyzer = [string]$manifest.spatial_analyzer_target
            }
        }

        $resolvedGeneratedRoot = [IO.Path]::GetFullPath($generatedRoot)
        $resolvedRepositoryRoot = [IO.Path]::GetFullPath($repositoryRoot)
        if (-not $resolvedGeneratedRoot.StartsWith(
                $resolvedRepositoryRoot,
                [StringComparison]::OrdinalIgnoreCase)) {
            throw "Generated protocol path escaped the repository."
        }
        if (Test-Path -LiteralPath $resolvedGeneratedRoot) {
            Remove-Item -LiteralPath $resolvedGeneratedRoot -Recurse -Force
        }
        Copy-Item -LiteralPath $generatedIdentityRoot -Destination $resolvedGeneratedRoot -Recurse
        Write-Utf8File -Path $lockPath -Content (($lock | ConvertTo-Json -Depth 10) + [char]10)
        Write-Host "Updated generated protocol code and protocol.lock.json."
    }
    else {
        Compare-FileTrees -Expected $generatedIdentityRoot -Actual $generatedRoot
        Write-Host "Verified protocol artifact identity and generated-code drift."
    }
}
finally {
    $resolvedTemporaryRoot = [IO.Path]::GetFullPath($temporaryRoot)
    if ($resolvedTemporaryRoot.StartsWith(
            $temporaryBase,
            [StringComparison]::OrdinalIgnoreCase) -and
        (Test-Path -LiteralPath $resolvedTemporaryRoot)) {
        Remove-Item -LiteralPath $resolvedTemporaryRoot -Recurse -Force
    }
}
