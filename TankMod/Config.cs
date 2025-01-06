using RedLoader;

namespace TankMod;

public class Config
{
    public static ConfigCategory TankCategory { get; private set; }
    public static ConfigEntry<int> CameraDistance { get; private set; }
    public static ConfigEntry<int> CameraHeight { get; private set; }

    public static void Init()
    {
        TankCategory = ConfigSystem.CreateFileCategory("Tank Settings", "Tank Settings", "TankMod.cfg");
        CameraDistance = TankCategory.CreateEntry<int>("Camera distance", 30, "Camera distance", "Distance of the main camera from the tank");
        CameraDistance.SetRange(15, 40);
        CameraDistance.OnValueChanged.Subscribe((int x, int y) => TankCategory.SaveToFile(false));

        CameraHeight = TankCategory.CreateEntry<int>("Camera height", -3, "Camera height (inverted)", "Height of the camera (inverted)");
        CameraHeight.SetRange(-5, 5);
        CameraHeight.OnValueChanged.Subscribe((int x, int y) => TankCategory.SaveToFile(false));
    }
}
