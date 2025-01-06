using AdvancedTerrainGrass;
using TheForest.Utils;
using UnityEngine;

namespace LowerGraphicsTool;

public class Settings
{
    static GameObject _billboards;
    static GameObject _poolBushes;
    static GameObject _poolSmallBush;
    static GameObject _poolPlant;
    static GameObject _poolMoss;
    static GameObject _postProcessing;
    static GameObject _fogZones;
    static GameObject _outsideReflections;
    static GameObject _Lakes;
    static GameObject _Streams;
    static GameObject _Waterfalls;
    static GameObject _Ocean;

    public static void AlwaysShowFps(bool onoff)
    {
        Config.AlwaysShowFps.Value = LowerGraphicsTool.AlwaysShowFps = onoff;
    }

    public static void SetBillboards(bool onoff)
    {
        _billboards ??= GameObject.Find("_Billboards_");
        Config.Billboards.Value = onoff;
        _billboards.SetActive(onoff);
    }

    public static void CameraLOD(float value)
    {
        Config.CamFarClipPlane.Value = LocalPlayer.MainCam.farClipPlane = value;
    }

    public static void SetGrass(bool onoff)
    {
        Config.Grass.Value = GrassManager._instance.DoRenderGrass = onoff;
    }

    public static void SetPoolBushes(bool onoff)
    {
        _poolBushes ??= GameObject.Find("GameManagers/Pooling/Pool_Bushes");
        Config.PoolBushes.Value = onoff;
        _poolBushes.SetActive(onoff);
    }

    public static void SetPoolSmallBush(bool onoff)
    {
        _poolSmallBush ??= GameObject.Find("GameManagers/Pooling/Pool_SmallBush");
        Config.PoolSmallBush.Value = onoff;
        _poolSmallBush.SetActive(onoff);
    }

    public static void SetPoolPlant(bool onoff)
    {
        _poolPlant ??= GameObject.Find("GameManagers/Pooling/Pool_Plant");
        Config.PoolPlant.Value = onoff;
        _poolPlant.SetActive(onoff);
    }

    public static void SetPoolMoss(bool onoff)
    {
        _poolMoss ??= GameObject.Find("GameManagers/Pooling/Pool_Moss");
        Config.PoolMoss.Value = onoff;
        _poolMoss.SetActive(onoff);
    }

    public static void SetTerraingHeightMap(bool onoff)
    {
        int value = onoff ? 1 : 0;
        Config.TerrainHeightmapMaximumLOD.Value = onoff;
        GameObject.Find("TerrainAndDetailLocators/Site02Terrain Tess").GetComponent<Terrain>().heightmapMaximumLOD = value;
    }

    public static void SunMoonShadows(bool onoff)
    {
        int value = onoff ? -1 : 2;
        Config.SunMoonCullingMask.Value = onoff;
        GameObject.Find("SunLight").GetComponent<Light>().cullingMask = value;
        GameObject.Find("MoonLight").GetComponent<Light>().cullingMask = value;
    }

    public static void SetPostProcessingEffects(bool onoff)
    {
        _postProcessing ??= GameObject.Find("Atmosphere/PostProcessingEffects");
        Config.PostProcessingEffects.Value = onoff;
        _postProcessing.SetActive(onoff);
    }

    public static void SetFogZones(bool onoff)
    {
        _fogZones ??= GameObject.Find("FogZones");
        Config.FogZones.Value = onoff;
        _fogZones.SetActive(onoff);
    }

    public static void SetOutsideReflections(bool onoff)
    {
        _outsideReflections ??= GameObject.Find("OutsideReflections");
        Config.OutsideReflections.Value = onoff;
        _outsideReflections.SetActive(onoff);
    }

    public static void SetLakes(bool onoff)
    {
        _Lakes ??= GameObject.Find("Lakes");
        Config.Lakes.Value = onoff;
        _Lakes.SetActive(onoff);
    }

    public static void SetStreams(bool onoff)
    {
        _Streams ??= GameObject.Find("Streams");
        Config.Streams.Value = onoff;
        _Streams.SetActive(onoff);
    }

    public static void SetWaterfalls(bool onoff)
    {
        _Waterfalls ??= GameObject.Find("Waterfalls");
        Config.Waterfalls.Value = onoff;
        _Waterfalls.SetActive(onoff);
    }

    public static void SetOcean(bool onoff)
    {
        _Ocean ??= GameObject.Find("Atmosphere/CrestOcean/Ocean");
        Config.Ocean.Value = onoff;
        _Ocean.SetActive(onoff);
    }
}
