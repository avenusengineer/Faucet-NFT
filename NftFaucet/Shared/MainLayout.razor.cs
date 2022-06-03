using AntDesign;
using NftFaucet.Components;
using NftFaucet.Models.Enums;

namespace NftFaucet.Shared;

public class MainLayoutComponent : LayoutBasicComponent
{
    private const int StepsCount = 5;

    protected bool IsFirstStep => AppState.Navigation.CurrentStep == 1;
    protected bool IsLastStep => AppState.Navigation.CurrentStep == StepsCount;
    protected string BackButtonStyle => $"visibility: {(IsFirstStep || IsLastStep ? "hidden" : "visible")}";
    protected string ForwardButtonStyle => $"visibility: {(IsLastStep ? "hidden" : "visible")}";

    protected string ForwardButtonText => AppState.Navigation.CurrentStep switch
    {
        1 => "Confirm network selection",
        2 => "Review NFT",
        3 => "Review mint",
        4 => "Send me this NFT!",
        _ => "Next"
    };

    protected string ForwardButtonIcon => AppState.Navigation.CurrentStep switch
    {
        4 => "send",
        _ => "arrow-right",
    };

    protected PresetColor ChainColor => AppState?.Metamask?.Network switch
    {
        EthereumNetwork.EthereumMainnet => PresetColor.Cyan,
        EthereumNetwork.Ropsten => PresetColor.Volcano,
        EthereumNetwork.Rinkeby => PresetColor.Gold,
        EthereumNetwork.Goerli => PresetColor.GeekBlue,
        EthereumNetwork.Kovan => PresetColor.Purple,
        EthereumNetwork.OptimismMainnet => PresetColor.Cyan,
        EthereumNetwork.OptimismKovan => PresetColor.Purple,
        EthereumNetwork.PolygonMainnet => PresetColor.Cyan,
        EthereumNetwork.PolygonMumbai => PresetColor.Pink,
        EthereumNetwork.MoonbeamMainnet => PresetColor.Cyan,
        EthereumNetwork.MoonbaseAlpha => PresetColor.Pink,
        EthereumNetwork.ArbitrumMainnetBeta => PresetColor.Cyan,
        EthereumNetwork.ArbitrumRinkeby => PresetColor.Gold,
        EthereumNetwork.ArbitrumGoerli => PresetColor.GeekBlue,
        EthereumNetwork.AvalancheMainnet => PresetColor.Cyan,
        EthereumNetwork.AvalancheFuji => PresetColor.Pink,
        _ => PresetColor.Yellow,
    };

}
