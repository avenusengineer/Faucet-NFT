using NftFaucetRadzen.Plugins.NetworkPlugins;

namespace NftFaucetRadzen.Plugins.ProviderPlugins.Phantom.Providers;

public class PhantomProvider : IProvider
{
    public Guid Id { get; } = Guid.Parse("ae860901-5441-463c-a16e-4786f041500d");
    public string Name { get; } = "Phantom";
    public string ShortName { get; } = "Phantom";
    public string ImageName { get; } = "phantom.svg";
    public bool IsSupported { get; } = true;
    public bool CanBeConfigured { get; } = true;
    public bool IsConfigured { get; private set; }

    public void Configure()
    {
        IsConfigured = true;
    }

    public List<(string Name, string Value)> GetProperties()
        => new List<(string Name, string Value)>
        {
            ("Installed", "YES"),
            ("Connected", IsConfigured ? "YES" : "NO"),
        };

    public bool IsNetworkSupported(INetwork network)
        => network?.Type == NetworkType.Solana;
}
