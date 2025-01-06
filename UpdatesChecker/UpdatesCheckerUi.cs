namespace UpdatesChecker;

using RedLoader;
using SonsSdk;
using SUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SUI.SUI;

public class UpdatesCheckerUi : MonoBehaviour
{
    public const string MOD_UPDATESTEMP_ID = "ModUpdatesPanelTemp";
    public const string MOD_UPDATES_ID = "ModUpdatesPanel";
    private static readonly Color MainBgBlack = new(0, 0, 0, 0.8f);
    public static GraphicRaycaster Raycaster { get; private set; }

    public static void CreateTemp()
    {
        _ = RegisterNewPanel(MOD_UPDATESTEMP_ID)
                .Pivot(0)
                .Anchor(AnchorType.TopLeft)
                .Size(250, 60)
                .Position(20, -150)
                .Background(SpriteBackground400ppu, MainBgBlack, UnityEngine.UI.Image.Type.Sliced)
            - SLabel
                .RichText($"Checking for updates...")
                .FontColor(Color.white.WithAlpha(0.3f)).FontSize(18).Dock(EDockType.Fill).Alignment(TextAlignmentOptions.Center);
    }

    public static void Create(int updatesCount, string content)
    {
        var panel = RegisterNewPanel(MOD_UPDATES_ID)
                .Pivot(0)
                .Anchor(AnchorType.TopLeft)
                .Size(250, 60)
                .Position(20, -150)
                .Background(SpriteBackground400ppu, MainBgBlack, UnityEngine.UI.Image.Type.Sliced)
            - SLabel
                .RichText($"<color=#34bdeb>{updatesCount}</color> Available {"Update".MakePlural(updatesCount)}")
                .FontColor(Color.white.WithAlpha(0.3f)).FontSize(18).Dock(EDockType.Fill).Alignment(TextAlignmentOptions.Center)              
                .Tooltip(content);

        Raycaster = panel.Root.AddComponent<GraphicRaycaster>();
        TooltipProvider.RaycastFor(Raycaster);
    }
}