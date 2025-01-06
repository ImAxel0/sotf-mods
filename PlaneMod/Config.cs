using Il2CppInterop.Generator.MetadataAccess;
using RedLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlaneMod;

public class Config
{
    public static ConfigCategory PlaneCategory { get; private set; }
    public static ConfigEntry<int> CameraDistance { get; private set; }
    public static ConfigEntry<bool> InvertedPitch { get; private set; }
    public static ConfigEntry<bool> InvertedRoll { get; private set; }
    public static ConfigEntry<bool> MphMode { get; private set; }
    public static ConfigEntry<Vector3> SavedPosition { get; private set; }
    public static ConfigEntry<Quaternion> SavedRotation { get; private set; }

    public static void Init()
    {
        PlaneCategory = ConfigSystem.CreateFileCategory("Plane settings", "Plane settings", "PlaneMod.cfg");
        CameraDistance = PlaneCategory.CreateEntry<int>("Camera distance", 15, "Camera distance", "Back distance of the main camera from the plane");
        CameraDistance.SetRange(10, 20);
        CameraDistance.OnValueChanged.Subscribe(SaveSettings);
        InvertedPitch = PlaneCategory.CreateEntry<bool>("Invert pitch", true, "Invert pitch", "Invert the pitch control");
        InvertedPitch.OnValueChanged.Subscribe(SaveSettings);
        InvertedRoll = PlaneCategory.CreateEntry<bool>("Invert roll", false, "Invert roll", "Invert the roll control");
        InvertedRoll.OnValueChanged.Subscribe(SaveSettings);
        MphMode = PlaneCategory.CreateEntry<bool>("Mph display mode", false, "Mph display mode", "Display mph instead of km/h");
        MphMode.OnValueChanged.Subscribe(SaveSettings);
        SavedPosition = PlaneCategory.CreateEntry<Vector3>("Saved position", Vector3.zero, "Saved position", "Plane last saved position", true, true);
        SavedRotation = PlaneCategory.CreateEntry<Quaternion>("Saved rotation", Quaternion.identity, "Saved rotation", "Plane last saved rotation", true, true);
        PlaneCategory.SaveToFile(false);
    }

    public static void SavePlane(bool bo)
    {
        if (!PlaneAction.Plane) return;
        PlaneCategory.GetEntry<Vector3>("Saved position").Value = PlaneAction.Plane.transform.position;
        PlaneCategory.GetEntry<Quaternion>("Saved rotation").Value = PlaneAction.Plane.transform.localRotation;
        PlaneCategory.SaveToFile(false);
    }

    internal static void SaveSettings<T>(T a, T b)
    {
        PlaneCategory.SaveToFile(false);
    }
}
