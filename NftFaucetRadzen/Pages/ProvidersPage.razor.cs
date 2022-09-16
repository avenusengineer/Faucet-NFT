using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NftFaucetRadzen.Components;
using NftFaucetRadzen.Components.CardList;
using NftFaucetRadzen.Plugins.ProviderPlugins;
using Radzen;

namespace NftFaucetRadzen.Pages;

public partial class ProvidersPage : BasicComponent
{
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    protected override void OnInitialized()
    {
        Providers = AppState.Storage.Providers.Where(x => AppState.SelectedNetwork != null && x.IsNetworkSupported(AppState.SelectedNetwork)).ToArray();
        RefreshCards();
    }

    private IProvider[] Providers { get; set; }
    private CardListItem[] ProviderCards { get; set; }

    private void RefreshCards()
    {
        ProviderCards = Providers.Select(MapCardListItem).ToArray();
    }

    private CardListItem MapCardListItem(IProvider provider)
    {
        var configuration = provider.CanBeConfigured ? provider.GetConfiguration() : null;
        return new CardListItem
        {
            Id = provider.Id,
            ImageLocation = provider.ImageName != null ? "./images/" + provider.ImageName : null,
            Header = provider.Name,
            IsDisabled = !provider.IsSupported,
            Properties = provider.GetProperties().ToArray(),
            Badges = new[]
            {
                (Settings?.RecommendedProviders?.Contains(provider.Id) ?? false)
                    ? new CardListItemBadge {Style = BadgeStyle.Success, Text = "Recommended"}
                    : null,
                !provider.IsSupported
                    ? new CardListItemBadge {Style = BadgeStyle.Light, Text = "Not Supported"}
                    : null,
            }.Where(x => x != null).ToArray(),
            Configuration = configuration == null ? null : new CardListItemConfiguration
            {
                Objects = configuration.Objects,
                ValidationFunc = configuration.ValidationFunc,
                ConfigureAction = x =>
                {
                    var result = configuration.ConfigureAction(x);
                    RefreshCards();
                    return result;
                },
            },
        };
    }
}
