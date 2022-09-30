using NftFaucet.Plugins.NetworkPlugins.Solana.Networks;

namespace NftFaucet.Plugins.NetworkPlugins.Solana;

public class SolanaNetworkPlugin : INetworkPlugin
{
    public IReadOnlyCollection<INetwork> Networks { get; } = new INetwork[]
    {
        new SolanaMainnetNetwork(),
        new SolanaDevnetNetwork(),
        new SolanaTestnetNetwork(),
    };
}