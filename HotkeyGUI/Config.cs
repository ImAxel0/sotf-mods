using RedLoader;
using SonsSdk;
using SonsSdk.Attributes;

namespace HotkeyGUI;

[SettingsUiMode(SettingsUiMode.ESettingsUiMode.OptIn)]
public static class Config
{
    public static ConfigCategory Category { get; private set; }

    [SettingsUiInclude]
    public static KeybindConfigEntry OpenKey { get; private set; }
    [SettingsUiInclude]
    public static ConfigEntry<bool> BackgroundBlur { get; private set; }

    public static ConfigEntry<string> Item1 { get; private set; }
    public static ConfigEntry<string> Item2 { get; private set; }
    public static ConfigEntry<string> Item3 { get; private set; }
    public static ConfigEntry<string> Item4 { get; private set; }
    public static ConfigEntry<string> Item5 { get; private set; }
    public static ConfigEntry<string> Item6 { get; private set; }
    public static ConfigEntry<string> Item7 { get; private set; }
    public static ConfigEntry<string> Item8 { get; private set; }
    public static ConfigEntry<string> Item9 { get; private set; }
    public static ConfigEntry<string> Item10 { get; private set; }
    public static ConfigEntry<string> Item11 { get; private set; }
    public static ConfigEntry<string> Item12 { get; private set; }
    public static ConfigEntry<string> Item13 { get; private set; }
    public static ConfigEntry<string> Item14 { get; private set; }
    public static ConfigEntry<string> Item15 { get; private set; }
    public static ConfigEntry<string> Item16 { get; private set; }
    public static ConfigEntry<string> Item17 { get; private set; }
    public static ConfigEntry<string> Item18 { get; private set; }
    public static ConfigEntry<string> Item19 { get; private set; }
    public static ConfigEntry<string> Item20 { get; private set; }
    public static ConfigEntry<string> Item21 { get; private set; }
    public static ConfigEntry<string> Item22 { get; private set; }
    public static ConfigEntry<string> Item23 { get; private set; }
    public static ConfigEntry<string> Item24 { get; private set; }
    public static ConfigEntry<string> Item25 { get; private set; }
    public static ConfigEntry<string> Item26 { get; private set; }
    public static ConfigEntry<string> Item27 { get; private set; }
    public static ConfigEntry<string> Item28 { get; private set; }
    public static ConfigEntry<string> Item29 { get; private set; }
    public static ConfigEntry<string> Item30 { get; private set; }
    public static ConfigEntry<string> Item31 { get; private set; }
    public static ConfigEntry<string> Item32 { get; private set; }
    public static ConfigEntry<string> Item33 { get; private set; }
    public static ConfigEntry<string> Item34 { get; private set; }
    public static ConfigEntry<string> Item35 { get; private set; }
    public static ConfigEntry<string> Item36 { get; private set; }

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("HotkeyGUI", "HotkeyGUI", "HotkeyGUI.cfg");
        OpenKey = Category.CreateKeybindEntry("OpenKey", RedLoader.Preferences.EInputKey.leftAlt, "Open key");
        OpenKey.Notify(HotkeyGUI.TogglePanel);

        BackgroundBlur = Category.CreateEntry("BackgroundBlur", false, "Blurred background");
        BackgroundBlur.OnValueChanged.Subscribe(HotkeyGUIUi.SetBackgroundBlur);
        BackgroundBlur.OnValueChanged.Subscribe(SaveConfig);

        Item1 = Category.CreateEntry("Item 1", "379");
        Item2 = Category.CreateEntry("Item 2", "379");
        Item3 = Category.CreateEntry("Item 3", "379");
        Item4 = Category.CreateEntry("Item 4", "379");
        Item5 = Category.CreateEntry("Item 5", "379");
        Item6 = Category.CreateEntry("Item 6", "379");
        Item7 = Category.CreateEntry("Item 7", "379");
        Item8 = Category.CreateEntry("Item 8", "379");
        Item9 = Category.CreateEntry("Item 9", "379");
        Item10 = Category.CreateEntry("Item 10", "379");
        Item11 = Category.CreateEntry("Item 11", "379");
        Item12 = Category.CreateEntry("Item 12", "379");
        Item13 = Category.CreateEntry("Item 13", "379");
        Item14 = Category.CreateEntry("Item 14", "379");
        Item15 = Category.CreateEntry("Item 15", "379");
        Item16 = Category.CreateEntry("Item 16", "379");
        Item17 = Category.CreateEntry("Item 17", "379");
        Item18 = Category.CreateEntry("Item 18", "379");
        Item19 = Category.CreateEntry("Item 19", "379");
        Item20 = Category.CreateEntry("Item 20", "379");
        Item21 = Category.CreateEntry("Item 21", "379");
        Item22 = Category.CreateEntry("Item 22", "379");
        Item23 = Category.CreateEntry("Item 23", "379");
        Item24 = Category.CreateEntry("Item 24", "379");
        Item25 = Category.CreateEntry("Item 25", "379");
        Item26 = Category.CreateEntry("Item 26", "379");
        Item27 = Category.CreateEntry("Item 27", "379");
        Item28 = Category.CreateEntry("Item 28", "379");
        Item29 = Category.CreateEntry("Item 29", "379");
        Item30 = Category.CreateEntry("Item 30", "379");
        Item31 = Category.CreateEntry("Item 31", "379");
        Item32 = Category.CreateEntry("Item 32", "379");
        Item33 = Category.CreateEntry("Item 33", "379");
        Item34 = Category.CreateEntry("Item 34", "379");
        Item35 = Category.CreateEntry("Item 35", "379");
        Item36 = Category.CreateEntry("Item 36", "379");
    }

    private static void SaveConfig(bool a, bool b)
    {
        Category.SaveToFile(false);
    }
}