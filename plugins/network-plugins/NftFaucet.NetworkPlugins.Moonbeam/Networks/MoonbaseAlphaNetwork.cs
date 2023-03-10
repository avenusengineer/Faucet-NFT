using System.Globalization;
using NftFaucet.Domain.Models;
using NftFaucet.Domain.Models.Abstraction;
using NftFaucet.Domain.Models.Enums;
using NftFaucet.Plugins.Models;

namespace NftFaucet.NetworkPlugins.Moonbeam.Networks;

public sealed class MoonbaseAlphaNetwork : Network
{
    public override Guid Id { get; } = Guid.Parse("3232de5b-78bd-4b2f-8048-4aa3e16547bd");
    public override string Name { get; } = "Moonbase Alpha";
    public override string ShortName { get; } = "MoonAlpha";
    public override ulong? ChainId { get; } = 1287;
    public override int? Order { get; } = 3;
    public override string MainCurrency { get; } = "DEV";
    public override string SmallestCurrency { get; } = "wei";
    public override string ImageName { get; } = "moonbeam-black.svg";
    public override NetworkType Type { get; } = NetworkType.Ethereum;
    public override NetworkSubtype SubType { get; } = NetworkSubtype.Moonbase;
    public override Uri PublicRpcUrl { get; } = new Uri("https://moonbase-alpha.public.blastapi.io");
    public override Uri ExplorerUrl { get; } = new Uri("https://moonbase.moonscan.io/"); 

    public override IReadOnlyCollection<IContract> DeployedContracts { get; } = new[]
    {
        new Contract
        {
            Id = Guid.Parse("1d7f52d6-4c96-4b92-af4c-1e6485af9d2f"),
            Name = "ERC-721 Faucet",
            Symbol = "FA721",
            Address = "0x9F64932Be34D5D897C4253D17707b50921f372B6",
            Type = ContractType.Erc721,
            DeploymentTxHash = "0x9bb9cd82a83a708f395cf074ded75264c4fd31f6eeb64729b4fff1eeea2c5c08",
            DeployedAt = DateTime.Parse("Apr-17-2022 02:59:00 PM", CultureInfo.InvariantCulture),
            IsVerified = true,
            MinBalanceRequired = 50000000000000,
        },
        new Contract
        {
            Id = Guid.Parse("9d1b343b-248a-4a22-9543-b114dbb7ae67"),
            Name = "ERC-1155 Faucet",
            Symbol = "FA1155",
            Address = "0xf67C575502fc1cE399a3e1895dDf41847185D7bD",
            Type = ContractType.Erc1155,
            DeploymentTxHash = "0x5f6d1137b59dbb0c00655a2c798b66ef34e42844dd89bbe45eb76b37fa82f718",
            DeployedAt = DateTime.Parse("Apr-17-2022 03:01:36 PM", CultureInfo.InvariantCulture),
            IsVerified = true,
            MinBalanceRequired = 50000000000000,
        },
    };
}
