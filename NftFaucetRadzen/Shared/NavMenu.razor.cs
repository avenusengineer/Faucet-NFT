using Microsoft.AspNetCore.Components;
using NftFaucetRadzen.Components;
using NftFaucetRadzen.Models;

namespace NftFaucetRadzen.Shared;

public partial class NavMenu : BasicComponent
{
    protected string SelectedNetworkName => AppState?.SelectedNetwork?.ShortName;
    protected string SelectedProviderName => AppState?.SelectedProvider?.ShortName;
    protected string SelectedContractName => AppState?.SelectedContract?.Symbol;
    protected string SelectedTokenName => AppState?.SelectedToken?.Name;
    protected string SelectedUploadName => AppState?.SelectedUploadLocation?.Name;

    private bool CollapseNavMenu { get; set; } = true;

    private string NavMenuCssClass => CollapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        CollapseNavMenu = !CollapseNavMenu;
    }
}
