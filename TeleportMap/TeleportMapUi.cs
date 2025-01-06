namespace TeleportMap;

using SUI;
using System.Reflection;
using UnityEngine;
using static SUI.SUI;
using static SonsAxLib.AXSUI;
using SonsSdk;
using TheForest.UI.Multiplayer;
using TheForest;

public class TeleportMapUi
{
    public static Observable<bool> ShowMap = new(false);
    public static SImageOptions MapDot;

    public struct PlayerPos
    {
        public static Observable<string> X = new("");
        public static Observable<string> Y = new("");
        public static Observable<string> Z = new("");
    }

    public static void Create()
    {
        if (TryGetEmbeddedResourceBytes("Cartography", out var mapBytes) && TryGetEmbeddedResourceBytes("playerdot", out var playerdotBytes))
        {
            Texture mapTex = ByteToTex(mapBytes);
            Texture playerdotTex = ByteToTex(playerdotBytes);

            var mapPanel = AxCreatePanel("InteractiveMapStandalone", false, new Vector2(720, 720), AnchorType.BottomLeft, Color.black, EBackground.None, true).BindVisibility(ShowMap)
                - SImage.Dock(EDockType.Fill).AspectRatio(UnityEngine.UI.AspectRatioFitter.AspectMode.HeightControlsWidth).Texture(mapTex).OnClick(TeleportMap.TryTeleportPlayer);

            var coordsPanel = AxCreatePanel("CoordsPanelStandalone", false, new Vector2(45, 65), AnchorType.TopLeft, Color.black.WithAlpha(0), EBackground.None).Vertical(2, "EC").Padding(2)
                - AxTextDynamic(PlayerPos.X, 16, TMPro.TextAlignmentOptions.Left).FontColor(new Color32(211, 31, 52, 255))
                - AxTextDynamic(PlayerPos.Y, 16, TMPro.TextAlignmentOptions.Left).FontColor(new Color32(211, 31, 52, 255))
                - AxTextDynamic(PlayerPos.Z, 16, TMPro.TextAlignmentOptions.Left).FontColor(new Color32(211, 31, 52, 255));
            mapPanel.Add(coordsPanel);

            MapDot = SImage.Texture(playerdotTex);
            MapDot.Pivot(0.5f, 0.5f);
            MapDot.Size(16, 16);
            mapPanel.Add(MapDot);
        }
    }

    public static void ToggleMap()
    {
        if (DebugConsole.Instance._showConsole)
            return;

        if (BoltNetwork.isRunning)
        {
            if (ChatBox.IsChatOpen)
            {
                ShowMap.Value = false;
                HudGui.Instance.gameObject.SetActive(true);
                return;
            }
        }
        ShowMap.Value = !ShowMap.Value;
        HudGui.Instance.gameObject.SetActive(!ShowMap.Value);
    }

    private static bool TryGetEmbeddedResourceBytes(string name, out byte[] bytes)
    {
        bytes = null;

        var executingAssembly = Assembly.GetExecutingAssembly();

        var desiredManifestResources = executingAssembly.GetManifestResourceNames().FirstOrDefault(resourceName => {
            var assemblyName = executingAssembly.GetName().Name;
            return !string.IsNullOrEmpty(assemblyName) && resourceName.StartsWith(assemblyName) && resourceName.Contains(name);
        });

        if (string.IsNullOrEmpty(desiredManifestResources))
            return false;

        using (var ms = new MemoryStream())
        {
            executingAssembly.GetManifestResourceStream(desiredManifestResources).CopyTo(ms);
            bytes =  ms.ToArray();
            return true;
        }
    }

    private static Texture2D ByteToTex(byte[] imgBytes)
    {
        Texture2D tex = new(2, 2);
        tex.LoadImage(imgBytes);
        return tex;
    }
}