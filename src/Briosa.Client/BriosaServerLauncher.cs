using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Transport = Briosa.Client.Transport;

namespace Briosa;

internal interface IBriosaServerLauncher
{
    Task<IOwnedBriosaServer> LaunchAsync(CancellationToken cancellationToken);
}

internal interface IOwnedBriosaServer : IAsyncDisposable
{
    Uri Address { get; }
    bool HasExited { get; }
}

internal sealed class BriosaServerLauncher : IBriosaServerLauncher
{
    internal const string ServerPathEnvironmentVariable = "BRIOSA_SERVER_PATH";

    public Task<IOwnedBriosaServer> LaunchAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var executablePath = ResolveExecutablePath();
        var port = ReserveLoopbackPort();
        var startInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            WorkingDirectory = Path.GetDirectoryName(executablePath)!,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };
        startInfo.ArgumentList.Add($"--Briosa:Endpoint:Port={port}");

        try
        {
            var process = Process.Start(startInfo) ??
                throw new BriosaStartupException("server-process-not-created");
            return Task.FromResult<IOwnedBriosaServer>(new OwnedBriosaServer(
                process,
                new Uri($"http://127.0.0.1:{port}", UriKind.Absolute)));
        }
        catch (BriosaException)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is InvalidOperationException or
                System.ComponentModel.Win32Exception)
        {
            throw new BriosaStartupException("server-process-start-failed", exception);
        }
    }

    private static string ResolveExecutablePath()
    {
        var configured = Environment.GetEnvironmentVariable(
            ServerPathEnvironmentVariable);
        var candidates = new[]
        {
            configured,
            Path.Combine(AppContext.BaseDirectory, "briosa-server", "Briosa.Server.exe"),
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Briosa",
                "servers",
                Transport.BriosaProtocolIdentity.BriosaVersion,
                $"sa-{Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget}",
                "Briosa.Server.exe"),
        };

        foreach (var candidate in candidates)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                continue;
            }

            var fullPath = Path.GetFullPath(candidate);
            if (string.Equals(
                    Path.GetFileName(fullPath),
                    "Briosa.Server.exe",
                    StringComparison.OrdinalIgnoreCase) &&
                File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        throw new BriosaStartupException("server-distribution-not-found");
    }

    private static int ReserveLoopbackPort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        try
        {
            listener.Start();
            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }
}

internal sealed class OwnedBriosaServer(Process process, Uri address)
    : IOwnedBriosaServer
{
    private readonly Process _process = process;

    public Uri Address { get; } = address;

    public bool HasExited => _process.HasExited;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                await _process.WaitForExitAsync().ConfigureAwait(false);
            }
        }
        catch (InvalidOperationException)
        {
        }
        finally
        {
            _process.Dispose();
        }
    }
}
