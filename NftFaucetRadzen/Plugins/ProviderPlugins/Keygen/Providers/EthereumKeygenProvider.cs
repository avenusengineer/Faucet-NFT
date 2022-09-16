using NftFaucetRadzen.Components.CardList;
using NftFaucetRadzen.Models;
using NftFaucetRadzen.Plugins.NetworkPlugins;
using NftFaucetRadzen.Utils;

namespace NftFaucetRadzen.Plugins.ProviderPlugins.Keygen.Providers;

public class EthereumKeygenProvider : IProvider
{
    public Guid Id { get; } = Guid.Parse("ded55b2b-8139-4251-a0fc-ba620f9727c9");
    public string Name { get; } = "Ethereum keygen";
    public string ShortName { get; } = "EthKeygen";
    public string ImageName { get; } = "ecdsa.svg";
    public bool IsSupported { get; } = true;
    public bool CanBeConfigured { get; } = true;
    public bool IsConfigured { get; private set; }
    public EthereumKey Key { get; private set; }

    public List<(string Name, string Value)> GetProperties()
        => new List<(string Name, string Value)>
        {
            ("Private key", Key?.PrivateKey ?? "<null>"),
            ("Address", Key?.Address ?? "<null>"),
        };

    public CardListItemConfiguration GetConfiguration()
    {
        var input = new CardListItemConfigurationObject
        {
            Id = Guid.Parse("5f92930d-7a8f-41e6-aa14-5608185e6f4b"),
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Private key",
            Value = Key?.PrivateKey ?? string.Empty,
        };
        var button = new CardListItemConfigurationObject
        {
            Id = Guid.Parse("cba7789e-188e-405b-80c3-b86da1c17850"),
            Type = CardListItemConfigurationObjectType.Button,
            Name = "Generate new keys",
            ClickAction = () => input.Value = EthereumKey.GenerateNew().PrivateKey,
        };
        return new CardListItemConfiguration
        {
            Objects = new[] { input, button },
            ValidationFunc = objects => Task.FromResult(ResultWrapper.Wrap(() => new EthereumKey(input.Value)).IsSuccess),
            ConfigureAction = objects =>
            {
                Key = new EthereumKey(input.Value);
                IsConfigured = true;
                return Task.CompletedTask;
            },
        };
    }

    public bool IsNetworkSupported(INetwork network)
        => network?.Type == NetworkType.Ethereum;
}
