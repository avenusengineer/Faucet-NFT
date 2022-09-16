using NftFaucetRadzen.Components.CardList;
using NftFaucetRadzen.Models;
using NftFaucetRadzen.Plugins.NetworkPlugins;
using NftFaucetRadzen.Utils;

namespace NftFaucetRadzen.Plugins.ProviderPlugins.Keygen.Providers;

public class SolanaKeygenProvider : IProvider
{
    public Guid Id { get; } = Guid.Parse("4c1a8ac5-60ca-4024-aae6-3c9852a6535c");
    public string Name { get; } = "Solana keygen";
    public string ShortName { get; } = "SolKeygen";
    public string ImageName { get; } = "ecdsa.svg";
    public bool IsSupported { get; } = true;
    public bool IsConfigured { get; private set; }
    public SolanaKey Key { get; private set; }

    public Task Configure(CardListItemConfigurationObject[] items)
    {
        Key = SolanaKey.GenerateNew();
        IsConfigured = true;
        return Task.CompletedTask;
    }

    public CardListItemProperty[] GetProperties()
        => new CardListItemProperty[]
        {
            new CardListItemProperty{ Name = "Private key", Value = Key?.PrivateKey ?? "<null>" },
            new CardListItemProperty{ Name = "Address", Value = Key?.Address ?? "<null>" },
        };

    public CardListItemConfiguration GetConfiguration()
    {
        var privateKeyInput = new CardListItemConfigurationObject
        {
            Id = Guid.Parse("17c198eb-4635-4229-a86f-051dcd7ca440"),
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Private key",
            Value = Key?.PrivateKey ?? string.Empty,
            IsDisabled = true,
        };
        var addressInput = new CardListItemConfigurationObject
        {
            Id = Guid.Parse("e02b71f7-538f-4527-abd4-011c43cbdb79"),
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Address",
            Value = Key?.Address ?? string.Empty,
            IsDisabled = true,
        };
        var button = new CardListItemConfigurationObject
        {
            Id = Guid.Parse("6eeb1400-aae0-46c1-ab94-ae80029ce5cb"),
            Type = CardListItemConfigurationObjectType.Button,
            Name = "Generate new keys",
            ClickAction = () =>
            {
                var generatedKey = SolanaKey.GenerateNew();
                privateKeyInput.Value = generatedKey.PrivateKey;
                addressInput.Value = generatedKey.Address;
            },
        };
        return new CardListItemConfiguration
        {
            Objects = new[] { privateKeyInput, addressInput, button },
            ValidationFunc = objects => Task.FromResult(ResultWrapper.Wrap(() => new SolanaKey(privateKeyInput.Value)).IsSuccess),
            ConfigureAction = objects =>
            {
                Key = new SolanaKey(privateKeyInput.Value);
                IsConfigured = true;
                return Task.CompletedTask;
            },
        };
    }

    public bool IsNetworkSupported(INetwork network)
        => network?.Type == NetworkType.Solana;
}
