using RedLoader;

namespace AmmoUi;

public static class Config
{
    public static ConfigCategory Category { get; private set; }

    public static ConfigEntry<float> Opacity { get; private set; }
    public static ConfigEntry<float> Size { get; private set; }

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("AmmoUi", "AmmoUi", "AmmoUi.cfg");

        Opacity = Category.CreateEntry<float>("opacity", 1, "Opacity");
        Opacity.SetRange(0, 1);
        Opacity.OnValueChanged.Subscribe(AmmoUiUi.OnOpacityChange);

        Size = Category.CreateEntry<float>("size", 1, "Size");
        Size.SetRange(0.1f, 2);
        Size.OnValueChanged.Subscribe(AmmoUiUi.OnSizeChange);
    }
}