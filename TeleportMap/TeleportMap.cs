using SonsSdk;
using SUI;
using TheForest.Utils;
using UnityEngine;

namespace TeleportMap;

public class TeleportMap : SonsMod
{
    public TeleportMap()
    {
    }

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        SdkEvents.OnInWorldUpdate.Subscribe(OnWorldUpdate);
        SdkEvents.OnInWorldUpdate.Subscribe(UpdateDotPosition);
        TeleportMapUi.Create();
        SettingsRegistry.CreateSettings(this, null, typeof(Config), false, OnSettingsClosed);
    }

    static void OnSettingsClosed()
    {
        Config.Category.SaveToFile();
    }

    public static void TryTeleportPlayer()
    {
        Vector3 mousePos = Input.mousePosition;

        float x = (mousePos.x - 360) * 5.5f;
        float z = (mousePos.y - 360) * 5.5f;

        if (Physics.Raycast(new Vector3(x, 1000, z), Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore))
        {
            LocalPlayer.Transform.position = hit.point + Vector3.up * 2f;
        }
    }

    public static void UpdateDotPosition()
    {
        Vector2 playerPos = new((LocalPlayer.Transform.position.x / 5.5f) + 360, (LocalPlayer.Transform.position.z / 5.5f) + 360);
        TeleportMapUi.MapDot.ImageObject.transform.position = new Vector3(playerPos.x, playerPos.y);
    }

    private void OnWorldUpdate()
    {
        TeleportMapUi.PlayerPos.X.Value = LocalPlayer.Transform.position.x.ToString("0");
        TeleportMapUi.PlayerPos.Y.Value = LocalPlayer.Transform.position.y.ToString("0");
        TeleportMapUi.PlayerPos.Z.Value = LocalPlayer.Transform.position.z.ToString("0");
    }
}