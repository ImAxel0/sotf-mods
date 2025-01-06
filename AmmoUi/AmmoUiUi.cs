using SUI;
using UnityEngine;
using SonsSdk;

using static SUI.SUI;
using static AmmoUi.AXSUI;

namespace AmmoUi;

public class AmmoUiUi
{
    public static SContainerOptions AmmoPanel;
    public static Observable<string> AmmoInfo = new("");
    public static Observable<bool> ShowPanel = new(false);
    public static Observable<Texture> AmmoIcon = new(new Texture());

    public static SImageOptions Icon;
    public static SLabelOptions Text;

    public static void Create()
    {
        AmmoPanel = AxCreatePanel("BottomRight", false, new Vector2(500, 150), AnchorType.BottomLeft, Color.black.WithAlpha(0))
            .Horizontal(0, "EE")
            .BindVisibility(ShowPanel);

        Icon = SImage.Bind(AmmoIcon).Dock(EDockType.Fill).AspectRatio(UnityEngine.UI.AspectRatioFitter.AspectMode.HeightControlsWidth);
        Text = AxTextDynamic(AmmoInfo, 18, TMPro.TextAlignmentOptions.Left).FontColor(Color.white);

        AmmoPanel.Add(Icon);
        AmmoPanel.Add(Text);

        OnOpacityChange(0, Config.Opacity.Value);
        OnSizeChange(0, Config.Size.Value);
    }

    public static void OnOpacityChange(float before, float after)
    {
        Icon.ImageObject.color = Color.white.WithAlpha(after);
        Text.FontColor(Color.white.WithAlpha(after));
        Config.Category.SaveToFile(false);
    }

    public static void OnSizeChange(float before, float after)
    {
        AmmoPanel.RectTransform.localScale = new Vector2(after, after);
        Config.Category.SaveToFile(false);
    }
}