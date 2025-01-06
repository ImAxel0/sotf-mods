using HarmonyLib;
using Sons.Weapon;
using SonsSdk;
using SUI;
using TheForest.Utils;

namespace AmmoUi;

public class AmmoUi : SonsMod
{
    public static int RemainingAmmo;
    public static int TotalAmmo;

    public AmmoUi()
    {

    }

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config), false);
        
        AmmoUiUi.Create();
    }

    protected override void OnSonsSceneInitialized(ESonsScene sonsScene)
    {
        if (sonsScene == ESonsScene.Title)
        {
            HarmonyInstance?.UnpatchSelf();
            AmmoUiUi.ShowPanel.Set(false);
        }
    }

    protected override void OnGameStart()
    {
        HarmonyInstance.Patch(AccessTools.Method(typeof(RangedWeapon), nameof(RangedWeapon.LateUpdate)),
            new HarmonyMethod(typeof(AmmoUi), nameof(UpdateAmmoCounter)));

        LocalPlayer._instance.gameObject.AddComponent<AxelAmmoUI>();
    }

    public static void UpdateAmmoCounter(ref RangedWeapon __instance)
    {
        RemainingAmmo = __instance.GetAmmo().GetRemainingAmmo();
        TotalAmmo = LocalPlayer.Inventory.AmountOf(__instance.GetAmmo()._type);
    }
}