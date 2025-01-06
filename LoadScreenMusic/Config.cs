using RedLoader;
using Sons.Gameplay;
using SUI;

namespace LoadScreenMusic;

public static class Config
{
    public static ConfigCategory Category { get; private set; }
    public static ConfigEntry<string> MenuFileName { get; private set; }
    public static ConfigEntry<string> LoadingFileName { get; private set; }
    public static ConfigEntry<float> MenuVolume { get; private set; }
    public static ConfigEntry<float> LoadingVolume { get; private set; }
    public static ConfigEntry<bool> StopHeliSound { get; private set; }

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("LoadScreenMusic", "LoadScreenMusic", "LoadScreenMusic.cfg");

        MenuFileName = Category.CreateEntry<string>("Menu file name", "", "Menu audio file name", "Name of audio file");
        MenuFileName.OnValueChanged.Subscribe(SaveConfig);

        LoadingFileName = Category.CreateEntry<string>("Loading file name", "", "Loading audio file name", "Name of audio file");
        LoadingFileName.OnValueChanged.Subscribe(SaveConfig);

        MenuVolume = Category.CreateEntry<float>("Menu volume", 1f, "Menu volume", "Volume adjustment");
        MenuVolume.SetRange(0f, 2f);
        MenuVolume.OnValueChanged.Subscribe(SaveConfig);
        MenuVolume.OnValueChanged.Subscribe(LoadScreenMusic.AdjustMenuVolume);

        LoadingVolume = Category.CreateEntry<float>("Loading volume", 1f, "Loading volume", "Volume adjustment");
        LoadingVolume.SetRange(0f, 2f);
        LoadingVolume.OnValueChanged.Subscribe(SaveConfig);

        StopHeliSound = Category.CreateEntry<bool>("Stop helicopter sound", false, "Stop helicopter sound", "Removes the helicopter sound sfx");
        StopHeliSound.OnValueChanged.Subscribe(SaveConfig);

        Category.SaveToFile(false);
    }

    internal static void SaveConfig<T>(T old, T naw)
    {
        Category.SaveToFile(false);
    }
}