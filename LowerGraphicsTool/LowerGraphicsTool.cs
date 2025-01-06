using Sons.Loading;
using SonsSdk;
using SUI;
using TheForest.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LowerGraphicsTool;

public class LowerGraphicsTool : SonsMod
{
    public static GameObject FpsMeter;
    public static bool AlwaysShowFps;

    public LowerGraphicsTool()
    {
    }

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config), false);
    }

    protected override void OnGameStart()
    {
        Config.ApplyConfig();
        FpsMeter = SceneManager.GetSceneByName(SonsSceneManager.SonsMainSceneName).GetRootGameObjects().FirstWithName("SampleFpsTool");
        FpsMeter.GetChildren().ForEach(child => { child.gameObject.SetActive(true); });
        LowerGraphicsToolUi.Create();
    }
}