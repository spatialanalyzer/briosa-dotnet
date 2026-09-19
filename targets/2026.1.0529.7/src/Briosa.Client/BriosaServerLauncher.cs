using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Briosa;

internal interface IBriosaServerLauncher
{
    Task<IOwnedBriosaServer> LaunchAsync(BriosaLoggingOptions? logging, CancellationToken cancellationToken);
    Task<IOwnedBriosaServer> LaunchAsync(BriosaStartOptions options, CancellationToken cancellationToken) =>
        LaunchAsync(options.Logging, cancellationToken);
}

internal interface IOwnedBriosaServer : IAsyncDisposable
{
    Uri Address { get; }
    bool HasExited { get; }
    BriosaInstallation? Installation => null;
}

internal sealed class BriosaServerLauncher : IBriosaServerLauncher
{
    internal const string ServerPathEnvironmentVariable = "BRIOSA_SERVER_PATH";

    public Task<IOwnedBriosaServer> LaunchAsync(BriosaLoggingOptions? logging, CancellationToken cancellationToken)
        => LaunchAsync(new BriosaStartOptions { Logging = logging }, cancellationToken);

    public Task<IOwnedBriosaServer> LaunchAsync(BriosaStartOptions options, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var installation = BriosaInstallations.Resolve(options.ServerSelection);
        var verified = InstalledServerDiscovery.ReadExecutable(installation.ExecutablePath, installation.Scope);
        if (verified != installation) throw new BriosaStartupException("server-installation-changed");
        var executablePath = installation.ExecutablePath;
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
        if (options.ServerSelection.SpatialAnalyzerExecutablePath is { } saPath)
            startInfo.ArgumentList.Add("--Briosa:SpatialAnalyzer:ExecutablePath=" + saPath);
        foreach (var argument in options.Logging?.ToArguments() ?? [])
            startInfo.ArgumentList.Add(argument);

        try
        {
            var process = Process.Start(startInfo) ??
                throw new BriosaStartupException("server-process-not-created");
            return Task.FromResult<IOwnedBriosaServer>(new OwnedBriosaServer(
                process,
                new Uri($"http://127.0.0.1:{port}", UriKind.Absolute), installation));
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

internal sealed class OwnedBriosaServer(Process process, Uri address, BriosaInstallation? installation = null)
    : IOwnedBriosaServer
{
    private readonly Process _process = process;

    public Uri Address { get; } = address;
    public BriosaInstallation? Installation { get; } = installation;

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
