namespace Briosa.Client;

/// <summary>Configures one Briosa gRPC client.</summary>
public sealed record BriosaClientOptions
{
    /// <summary>Gets the Briosa server address.</summary>
    public required Uri Address { get; init; }

    /// <summary>Gets the default deadline applied to each call.</summary>
    public TimeSpan DefaultTimeout { get; init; } = TimeSpan.FromSeconds(30);

    internal void Validate()
    {
        if (!Address.IsAbsoluteUri ||
            (Address.Scheme != Uri.UriSchemeHttp &&
                Address.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "The Briosa address must be an absolute HTTP or HTTPS URI.",
                nameof(Address));
        }

        if (DefaultTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(DefaultTimeout),
                "The default timeout must be positive.");
        }
    }
}
