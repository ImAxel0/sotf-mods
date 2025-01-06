using RedLoader;
using RedLoader.Utils;
using SonsSdk;
using SonsSdk.Attributes;
using SUI;
using UnityEngine;

namespace PlaneMod;

public class PlaneMod : SonsMod
{
    internal PlaneMod() 
    {
        OnGUICallback = PlaneModUi.UpdateUI;
    }

    [DebugCommand("spawnplane")]
    private void SpawnPlane()
    {
        if (PlaneAction.IsFlying)
        {
            SonsTools.ShowMessage("Can't spawn a new plane right now");
            return;
        }
        PlaneAction.InitNew();
    }

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config), false);
        AssetLoader.LoadAllBundles();
        PlaneAudio.Init();
        PlaneModUi.Create();
        SdkEvents.AfterSaveLoading.Subscribe(Config.SavePlane);
    }

    protected override void OnSonsSceneInitialized(ESonsScene sonsScene)
    {
        if (sonsScene == ESonsScene.Title) AssetLoader.GameObjects.Clear();
    }

    protected override void OnGameStart()
    {
        AssetLoader.LoadAllAssets();
        if (Config.SavedPosition.Value != Vector3.zero) PlaneAction.InitNew(Config.SavedPosition.Value, Config.SavedRotation.Value);
    }
}