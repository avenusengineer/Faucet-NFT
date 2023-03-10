using System.Globalization;
using NftFaucet.Domain.Models;
using NftFaucet.Domain.Models.Abstraction;
using NftFaucet.Domain.Models.Enums;
using NftFaucet.Plugins.Models;

namespace NftFaucet.NetworkPlugins.BinanceSmartChain.Networks;

public sealed class BscTestnetNetwork : Network
{
    public override Guid Id { get; } = Guid.Parse("b8d4aa13-acc1-47ee-9e8c-c00f0b67772c");
    public override string Name { get; } = "Binance Smart Chain Testnet";
    public override string ShortName { get; } = "BSC test";
    public override ulong? ChainId { get; } = 97;
    public override int? Order { get; } = 2;
    public override string MainCurrency { get; } = "tBNB";
    public override string SmallestCurrency { get; } = "wei";
    public override string ImageName { get; } = "bnb-black.svg";
    public override NetworkType Type { get; } = NetworkType.Ethereum;
    public override NetworkSubtype SubType { get; } = NetworkSubtype.Bsc;
    public override Uri PublicRpcUrl { get; } = new Uri("https://data-seed-prebsc-1-s1.binance.org:8545/");
    public override Uri ExplorerUrl { get; } = new Uri("https://testnet.bscscan.com/");

    public override IReadOnlyCollection<IContract> DeployedContracts { get; } = new[]
    {
        new Contract
        {
            Id = Guid.Parse("ba5366b7-1140-441d-b9d6-c0cc8c670ee0"),
            Name = "ERC-721 Faucet",
            Symbol = "FA721",
            Address = "0xe6Ee919A81dA4Dad1E632614BA4FDb8d748eB278",
            Type = ContractType.Erc721,
            DeploymentTxHash = "0x9c5bc988b48cac17c0215914ffd9e6c703b45b76073d495b9fb5dca7923566b4",
            DeployedAt = DateTime.Parse("Jul-21-2022 11:08:17 AM", CultureInfo.InvariantCulture),
            IsVerified = true,
            MinBalanceRequired = 50000000000000,
        },
        new Contract
        {
            Id = Guid.Parse("acfacbcf-8d9d-4b56-98aa-963b209416d9"),
            Name = "ERC-1155 Faucet",
            Symbol = "FA1155",
            Address = "0xa6D787D1EC987a96bA2A8bF4DaE79234e4a2125a",
            Type = ContractType.Erc1155,
            DeploymentTxHash = "0x1517f670d35159800c28c802ef62cb4d099e6e321737f130907fffc32f9c14cc",
            DeployedAt = DateTime.Parse("Jul-21-2022 11:25:14 AM", CultureInfo.InvariantCulture),
            IsVerified = true,
            MinBalanceRequired = 50000000000000,
        },
    };
}
