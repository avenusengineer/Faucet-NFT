using System.Numerics;

namespace NftFaucet.Plugins.Models.Abstraction;

public interface IWallet : INamedEntity, IEntityWithOrder, IStateful, IInitializable, IEntityWithProperties, IConfigurable
{
    public bool IsNetworkSupported(INetwork network);
    public Task<string> GetAddress();
    public Task<BigInteger?> GetBalance(INetwork network);
    public Task<INetwork> GetNetwork(IReadOnlyCollection<INetwork> allKnownNetworks, INetwork selectedNetwork);
    public Task<string> Mint(MintRequest mintRequest);
}
