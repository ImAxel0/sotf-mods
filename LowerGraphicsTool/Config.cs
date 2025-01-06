using RedLoader;
using Sons.Environment;
using SonsSdk;
using SonsSdk.Attributes;

namespace LowerGraphicsTool;

[SettingsUiMode(SettingsUiMode.ESettingsUiMode.OptIn)]
public static class Config
{
    public static ConfigCategory Category { get; private set; }

    [SettingsUiInclude]
    public static KeybindConfigEntry OpenKey { get; private set; }

    public static ConfigEntry<bool> AlwaysShowFps { get; private set; }
    public static ConfigEntry<bool> Billboards { get; private set; }
    public static ConfigEntry<float> CamFarClipPlane { get; private set; }
    public static ConfigEntry<bool> Grass { get; private set; }
    public static ConfigEntry<bool> PoolBushes { get; private set; }
    public static ConfigEntry<bool> PoolSmallBush { get; private set; }
    public static ConfigEntry<bool> PoolPlant { get; private set; }
    public static ConfigEntry<bool> PoolMoss { get; private set; }
    public static ConfigEntry<bool> TerrainHeightmapMaximumLOD { get; private set; }
    public static ConfigEntry<bool> SunMoonCullingMask { get; private set; }
    public static ConfigEntry<bool> PostProcessingEffects { get; private set; }
    public static ConfigEntry<bool> FogZones { get; private set; }
    public static ConfigEntry<bool> OutsideReflections { get; private set; }
    public static ConfigEntry<bool> Lakes { get; private set; }
    public static ConfigEntry<bool> Streams { get; private set; }
    public static ConfigEntry<bool> Waterfalls { get; private set; }
    public static ConfigEntry<bool> Ocean { get; private set; }

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("LowerGraphicsTool", "LowerGraphicsTool", "LowerGraphicsTool.cfg");

        OpenKey = Category.CreateKeybindEntry("OpenKey", "F5", "Open Key");
        OpenKey.OnValueChanged.Subscribe(SaveConfig);
        OpenKey.Notify(LowerGraphicsToolUi.ToggleMenu);

        AlwaysShowFps = Category.CreateEntry("AlwaysShowFps", true, "", "", true);
        AlwaysShowFps.OnValueChanged.Subscribe(SaveConfig);

        Billboards = Category.CreateEntry("Billboards", true, "", "", true); // removes some distant trees/bushes
        Billboards.OnValueChanged.Subscribe(SaveConfig);
        CamFarClipPlane = Category.CreateEntry("CamFarClipPlane", 24000f, "", "", true); // a sort of global LOD
        CamFarClipPlane.OnValueChanged.Subscribe(SaveConfig);
        Grass = Category.CreateEntry("Grass", true, "", "", true);
        Grass.OnValueChanged.Subscribe(SaveConfig);
        PoolBushes = Category.CreateEntry("PoolBushes", true, "", "", true);
        PoolBushes.OnValueChanged.Subscribe(SaveConfig);
        PoolSmallBush = Category.CreateEntry("PoolSmallBush", true, "", "", true);
        PoolSmallBush.OnValueChanged.Subscribe(SaveConfig);
        PoolPlant = Category.CreateEntry("PoolPlant", true, "", "", true);
        PoolPlant.OnValueChanged.Subscribe(SaveConfig);
        PoolMoss = Category.CreateEntry("PoolMoss", true, "", "", true);
        PoolMoss.OnValueChanged.Subscribe(SaveConfig);
        TerrainHeightmapMaximumLOD = Category.CreateEntry("TerrainHeightmapMaximumLOD", false, "", "", true); // 1 for perf
        TerrainHeightmapMaximumLOD.OnValueChanged.Subscribe(SaveConfig);
        SunMoonCullingMask = Category.CreateEntry("SunCullingMask", true, "", "", true); // 2 for perf
        SunMoonCullingMask.OnValueChanged.Subscribe(SaveConfig);
        PostProcessingEffects = Category.CreateEntry("PostProcessingEffects", true, "", "", true);
        PostProcessingEffects.OnValueChanged.Subscribe(SaveConfig);
        FogZones = Category.CreateEntry("FogZones", true, "", "", true);
        FogZones.OnValueChanged.Subscribe(SaveConfig);
        OutsideReflections = Category.CreateEntry("OutsideReflections", true, "", "", true);
        OutsideReflections.OnValueChanged.Subscribe(SaveConfig);
        Lakes = Category.CreateEntry("Lakes", true, "", "", true);
        Lakes.OnValueChanged.Subscribe(SaveConfig);
        Streams = Category.CreateEntry("Streams", true, "", "", true);
        Streams.OnValueChanged.Subscribe(SaveConfig);
        Waterfalls = Category.CreateEntry("Waterfalls", true, "", "", true);
        Waterfalls.OnValueChanged.Subscribe(SaveConfig);
        Ocean = Category.CreateEntry("Ocean", true, "", "", true);
        Ocean.OnValueChanged.Subscribe(SaveConfig);
    }

    public static void ApplyConfig()
    {
        Settings.SetBillboards(Billboards.Value);
        Settings.CameraLOD(CamFarClipPlane.Value);
        Settings.SetGrass(Grass.Value);
        Settings.SetPoolBushes(PoolBushes.Value);
        Settings.SetPoolSmallBush(PoolSmallBush.Value);
        Settings.SetPoolPlant(PoolPlant.Value);
        Settings.SetPoolMoss(PoolMoss.Value);
        Settings.SetTerraingHeightMap(TerrainHeightmapMaximumLOD.Value);
        Settings.SunMoonShadows(SunMoonCullingMask.Value);
        Settings.SetPostProcessingEffects(PostProcessingEffects.Value);
        Settings.SetFogZones(FogZones.Value);
        Settings.SetOutsideReflections(OutsideReflections.Value);
        Settings.SetLakes(Lakes.Value);
        Settings.SetStreams(Streams.Value);
        Settings.SetWaterfalls(Waterfalls.Value);
        Settings.SetOcean(Ocean.Value);
    }

    public static void ResetToDefault()
    {
        Settings.SetBillboards(Billboards.DefaultValue);
        Settings.CameraLOD(CamFarClipPlane.DefaultValue);
        Settings.SetGrass(Grass.DefaultValue);
        Settings.SetPoolBushes(PoolBushes.DefaultValue);
        Settings.SetPoolSmallBush(PoolSmallBush.DefaultValue);
        Settings.SetPoolPlant(PoolPlant.DefaultValue);
        Settings.SetPoolMoss(PoolMoss.DefaultValue);
        Settings.SetTerraingHeightMap(TerrainHeightmapMaximumLOD.DefaultValue);
        Settings.SunMoonShadows(SunMoonCullingMask.DefaultValue);
        Settings.SetPostProcessingEffects(PostProcessingEffects.DefaultValue);
        Settings.SetFogZones(FogZones.DefaultValue);
        Settings.SetOutsideReflections(OutsideReflections.DefaultValue);
        Settings.SetLakes(Lakes.DefaultValue);
        Settings.SetStreams(Streams.DefaultValue);
        Settings.SetWaterfalls(Waterfalls.DefaultValue);
        Settings.SetOcean(Ocean.DefaultValue);
    }

    public static void SaveConfig<T>(T one, T two)
    {
        Category.SaveToFile(false);
    }
}