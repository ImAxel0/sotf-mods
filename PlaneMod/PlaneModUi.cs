using UnityEngine;
using SUI;
using static SUI.SUI;
using SonsSdk;

namespace PlaneMod;

public class PlaneModUi
{
    public static Observable<string> ThrottleOb = new("0 <color=red>%</color>");
    public static Observable<string> CurrentSpeedOb = new("0 km/h");

    public static void Create()
    {
        _ = RegisterNewPanel("PlaneUI").Anchor(AnchorType.BottomLeft).Size(280, 100).Position(150, 50).Background(Color.black.WithAlpha(0.7f), EBackground.ShadowPanel).Horizontal().Padding(10)
            - SLabel.Bind(ThrottleOb).FontSize(40)
            - SLabel.Bind(CurrentSpeedOb).FontSize(26);
    }

    public static void UpdateUI()
    {
        TogglePanel("PlaneUI", PlaneAction.IsFlying);
        if (!PlaneAction.IsFlying) return;
        ThrottleOb.Value = PlaneAction.Throttle.ToString("0") + " <color=red>%</color>";

        if (Config.MphMode.Value) CurrentSpeedOb.Value = PlaneAction.CurrentSpeed.ToString("0") + " mph";
        else CurrentSpeedOb.Value = PlaneAction.CurrentSpeed.ToString("0") + " km/h";
    }
}