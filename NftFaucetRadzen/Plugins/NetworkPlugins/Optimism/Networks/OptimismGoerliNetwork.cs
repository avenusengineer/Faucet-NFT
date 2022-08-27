namespace NftFaucetRadzen.Plugins.NetworkPlugins.Optimism.Networks;

public class OptimismGoerliNetwork : INetwork
{
    public Guid Id { get; } = Guid.Parse("fe4f3f37-bec9-4f35-9063-8682160b1f9d");
    public string Name { get; } = "Optimism Goerli";
    public string ShortName { get; } = "OpGoerli";
    public ulong? ChainId { get; } = 420;
    public string Currency { get; } = "ETH";
    public string ImageName { get; } = "optimism-black.svg";
    public bool IsSupported { get; } = false;
    public bool IsTestnet { get; } = true;
    public bool IsDeprecated { get; } = false;
    public NetworkType Type { get; } = NetworkType.Optimism;
    public string Erc721ContractAddress { get; } = null;
    public string Erc1155ContractAddress { get; } = null;
}
