using RedLoader;
using SonsSdk;

namespace TeleportMap;

public static class Config
{
    public static ConfigCategory Category { get; private set; }
    public static KeybindConfigEntry MapKey { get; private set; }

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("TeleportMap", "TeleportMap", "TeleportMap.cfg");

        MapKey = Category.CreateKeybindEntry("MapKey", RedLoader.Preferences.EInputKey.m, "Toggle key", "Key to show/hide the interactive map");
        MapKey.Notify(TeleportMapUi.ToggleMap);
    }
}