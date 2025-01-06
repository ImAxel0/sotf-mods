using SUI;
using UnityEngine;
using SonsSdk;
using TheForest.Utils;

namespace LowerGraphicsTool;

using static SonsAxLib.AXSUI;
using static SUI.SUI;

public class LowerGraphicsToolUi
{
    public static SContainerOptions MainPanel { get; set; }
    public static readonly string MainPanelId = "MainPanel";
    public static bool ShowPanel;

    public static void Create()
    {
        MainPanel = AxCreateSidePanel(MainPanelId, true, "Lower graphics tool", Side.Left, 700, null, EBackground.None, true).Material(PanelBlur.GetForShade(0.6f)).OverrideSorting(100).Active(false);

        var mainContainer = AxGetMainContainer((SPanelOptions)MainPanel);

        mainContainer.Add(AxMenuCheckBox("<color=blue>Always show FPS</color>", Settings.AlwaysShowFps, new Observable<bool>(LowerGraphicsTool.AlwaysShowFps)));

        mainContainer.Add(AxDivider("<color=yellow>LOD</color>"));
        mainContainer.Add(AxMenuCheckBox("Far trees/bushes", Settings.SetBillboards, Config.Billboards.Value));
        mainContainer.Add(AxMenuSliderFloat("Camera view distance", LabelPosition.Top, 10, 24000, Config.CamFarClipPlane.Value, 1, Settings.CameraLOD));

        mainContainer.Add(AxDivider("<color=yellow>Vegetation</color>"));
        mainContainer.Add(AxMenuCheckBox("Grass", Settings.SetGrass, Config.Grass.Value));
        mainContainer.Add(AxMenuCheckBox("Bushes", Settings.SetPoolBushes, Config.PoolBushes.Value));
        mainContainer.Add(AxMenuCheckBox("Small bushes", Settings.SetPoolSmallBush, Config.PoolSmallBush.Value));
        mainContainer.Add(AxMenuCheckBox("Small plants", Settings.SetPoolPlant, Config.PoolPlant.Value));
        mainContainer.Add(AxMenuCheckBox("Moss", Settings.SetPoolMoss, Config.PoolMoss.Value));

        mainContainer.Add(AxDivider("<color=yellow>Terrain</color>"));
        mainContainer.Add(AxMenuCheckBox("Lower terrain detail", Settings.SetTerraingHeightMap, Config.TerrainHeightmapMaximumLOD.Value));

        mainContainer.Add(AxDivider("<color=yellow>Post processing/Shadows</color>"));
        mainContainer.Add(AxMenuCheckBox("Sun/moon shadows", Settings.SunMoonShadows, Config.SunMoonCullingMask.Value));
        mainContainer.Add(AxMenuCheckBox("Post processing effects (leave enabled if ↑ is on)", Settings.SetPostProcessingEffects, Config.PostProcessingEffects.Value));
        mainContainer.Add(AxMenuCheckBox("Fog zones", Settings.SetFogZones, Config.FogZones.Value));
        mainContainer.Add(AxMenuCheckBox("Outside reflections", Settings.SetOutsideReflections, Config.OutsideReflections.Value));

        mainContainer.Add(AxDivider("<color=yellow>Water</color>"));
        mainContainer.Add(AxMenuCheckBox("Lakes", Settings.SetLakes, Config.Lakes.Value));
        mainContainer.Add(AxMenuCheckBox("Streams", Settings.SetStreams, Config.Streams.Value));
        mainContainer.Add(AxMenuCheckBox("Waterfalls", Settings.SetWaterfalls, Config.Waterfalls.Value));
        mainContainer.Add(AxMenuCheckBox("Ocean", Settings.SetOcean, Config.Ocean.Value));

        //mainContainer.Add(AxMenuButton("Run benchmark (1m:15s [Can't stop])", Benchmark.RunBenchmark, null, EBackground.RoundedStandard));
        //mainContainer.Add(AxMenuButton("Reset to default", Config.ResetToDefault, null, EBackground.RoundedStandard));

        CreateBenchmarkUi();
    }

    public static SContainerOptions BenchmarkResultsPanel { get; set; }
    public static SContainerOptions BlackScreenPanel { get; set; }
    public static Observable<string> MaxFps = new("");
    public static Observable<string> AverageFps = new("");
    public static Observable<string> MinFps = new("");

    private static void CreateBenchmarkUi()
    {
        BenchmarkResultsPanel = RegisterNewPanel("BenchmarkResults", true).Background(Color.black.WithAlpha(0.95f), EBackground.ShadowPanel).Anchor(AnchorType.MiddleCenter).Size(1280, 300).Vertical(2, "EC").Padding(25).OverrideSorting(999).Active(false)
            - SLabelDivider.Text("Results").FontColor(new Color32(17, 122, 67, 255)).FontSize(50)
            - SLabel.Bind(MaxFps).Alignment(TMPro.TextAlignmentOptions.Left).FontSize(40)
            - SLabel.Bind(AverageFps).Alignment(TMPro.TextAlignmentOptions.Left).FontSize(40)
            - SLabel.Bind(MinFps).Alignment(TMPro.TextAlignmentOptions.Left).FontSize(40);
        var bottomPanel = SContainer.Horizontal(0, "EE").Background(Color.black.WithAlpha(0), EBackground.RoundedStandard)
            - SBgButton.Text("Save results").Color(new Color32(100, 195, 85, 255)).Background(EBackground.RoundedStandard).Notify(Benchmark.SaveBenchmark)
            - SBgButton.Text("Exit").Color(new Color32(211, 31, 52, 255)).Background(EBackground.RoundedStandard).Notify(Benchmark.ExitBenchmark);

        BenchmarkResultsPanel.Add(bottomPanel);

        BlackScreenPanel = RegisterNewPanel("BlackScreenPanel").Dock(EDockType.Fill).Background(Color.black, EBackground.None).OverrideSorting(999).Active(false);
    }

    public static void ToggleMenu()
    {
        if (!LocalPlayer._instance) return;

        ShowPanel = !ShowPanel;
        bool showFps = LowerGraphicsTool.AlwaysShowFps ? true : ShowPanel;
        LowerGraphicsTool.FpsMeter.SetActive(showFps);
        MainPanel.Active(ShowPanel);
    }
}