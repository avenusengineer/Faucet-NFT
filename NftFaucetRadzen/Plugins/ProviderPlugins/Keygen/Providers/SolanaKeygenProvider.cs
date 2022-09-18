using CSharpFunctionalExtensions;
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
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Private key",
            Placeholder = "<null>",
            Value = Key?.PrivateKey ?? string.Empty,
            IsDisabled = true,
        };
        var addressInput = new CardListItemConfigurationObject
        {
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Address",
            Placeholder = "<null>",
            Value = Key?.Address ?? string.Empty,
            IsDisabled = true,
        };
        var button = new CardListItemConfigurationObject
        {
            Type = CardListItemConfigurationObjectType.Button,
            Name = "Generate new keys",
            Placeholder = "<null>",
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
            ConfigureAction = objects =>
            {
                var keyResult = ResultWrapper.Wrap(() => new SolanaKey(objects[0].Value));
                if (keyResult.IsFailure)
                    return Task.FromResult(Result.Failure(keyResult.Error));

                Key = keyResult.Value;
                IsConfigured = true;
                return Task.FromResult(Result.Success());
            },
        };
    }

    public bool IsNetworkSupported(INetwork network)
        => network?.Type == NetworkType.Solana;
}
