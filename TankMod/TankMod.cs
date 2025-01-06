using SonsSdk;
using SonsSdk.Attributes;
using UnityEngine;
using TheForest.Utils;
using RedLoader.Utils;
using FMODCustom;
using System.Reflection;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Sons.Input;
using SUI;

namespace TankMod;

public class TankMod : SonsMod
{
    public static GameObject SpawnedTank;
    public static Channel EngineCh;
    public static bool IsInTank;

    public TankMod()
    {
        OnUpdateCallback = OnUpdate;
    }

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    [DebugCommand("spawntank")]
    private void SpawnTank()
    {
        if (SpawnedTank)
        {
            UnityEngine.Object.Destroy(SpawnedTank);
        }

        var tank = UnityEngine.Object.Instantiate(Tank.TankPrefab, SonsTools.GetPositionInFrontOfPlayer(8, 2), Quaternion.Euler(Vector3.zero));
        SpawnedTank = tank;

        tank.GetChildren().ForEach(child => {

            var meshRend = child.GetComponent<MeshRenderer>();
            if (meshRend)
            {
                meshRend.sharedMaterial.shader = Shader.Find("Sons/HDRPLit");
            }
        });

        tank.transform.Find("turret/muzzle").GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("Sons/HDRPLit");

        TryGetEmbeddedResourceBytes("tank", out var tankIconBytes);       

        GameObject interactable = Interactable.AddInteractable(tank, 4f, Interactable.InteractableType.Take, Texture2dFromBytes(tankIconBytes));
        interactable.SetActive(false); interactable.SetActive(true);

        tank.AddComponent<TankController>();

        int idx = 0;
        foreach (var wheelCollider in tank.transform.Find("WheelColliders").GetChildren())
        {
            var wheelController = wheelCollider.gameObject.AddComponent<WheelController>();
            wheelController.wheelModel = tank.transform.GetChild(idx);
            wheelController.motorized = true;

            if (idx == 0 || idx == 1 || idx == 2 || idx == 3)
            {
                wheelController.steerable = true;
            }

            idx++;
        }
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config), false);
        SoundTools.RegisterSound("tankshoot", Path.Combine(LoaderEnvironment.ModsDirectory, "TankMod\\tankshoot.wav"));
        SoundTools.RegisterSound("tankengine", Path.Combine(LoaderEnvironment.ModsDirectory, "TankMod\\tankengine.wav"));
    }

    void OnUpdate()
    {
        if (!SpawnedTank)
            return;

        if (InputSystem.InputMapping.@default.Interact.triggered && Interactable.GetUIElement(SpawnedTank).IsActive && !IsInTank)
        {
            EnterTank();
        }

        if ((Input.GetKeyDown(KeyCode.G) || InputSystem.InputMapping.@default.Back.triggered) && IsInTank)
        {
            ExitTank();
        }
    }

    public static void EnterTank()
    {
        LocalPlayer.CamRotator.ForceResetRotation(); // vertical
        LocalPlayer.FpCharacter.transform.position = SpawnedTank.transform.position + Vector3.up * 2f;
        LocalPlayer.FpCharacter.MovementLocked = true;
        LocalPlayer.FpCharacter._rigidbody.isKinematic = true;
        LocalPlayer.FpCharacter._primaryCollider.enabled = false;
        LocalPlayer.FpCharacter._primaryColliderEnabled = false;
        LocalPlayer.ClothingSystem.gameObject.SetActive(false);
        LocalPlayer.RaceSystem.gameObject.SetActive(false);
        LocalPlayer.Transform.Find("PlayerAnimator/ArmourSystem").gameObject.SetActive(false);
        LocalPlayer.FpCharacter.transform.parent = SpawnedTank.transform;
        LocalPlayer.MainCamTr.localPosition = Vector3.zero + Vector3.down * Config.CameraHeight.Value + Vector3.back * Config.CameraDistance.Value;
        LocalPlayer.CamRotator.lockRotation = true; // vertical
        EngineCh = AudioController.PlaySound("tankengine", AudioController.SoundType.Sfx, true);
        IsInTank = true;
    }

    public static void ExitTank()
    {
        LocalPlayer.FpCharacter.MovementLocked = false;
        LocalPlayer.FpCharacter._rigidbody.isKinematic = false;
        LocalPlayer.FpCharacter._primaryCollider.enabled = true;
        LocalPlayer.FpCharacter._primaryColliderEnabled = true;
        LocalPlayer.ClothingSystem.gameObject.SetActive(true);
        LocalPlayer.RaceSystem.gameObject.SetActive(true);
        LocalPlayer.Transform.Find("PlayerAnimator/ArmourSystem").gameObject.SetActive(true);
        LocalPlayer.FpCharacter.transform.parent = null;
        LocalPlayer.MainCamTr.localPosition = Vector3.zero;
        LocalPlayer.CamRotator.lockRotation = false;
        AudioController.StopSound("tankengine");
        IsInTank = false;
    }

    public static bool TryGetEmbeddedResourceBytes(string name, out byte[] bytes)
    {
        bytes = null;

        var executingAssembly = Assembly.GetExecutingAssembly();

        var desiredManifestResources = executingAssembly.GetManifestResourceNames().FirstOrDefault(resourceName => {
            var assemblyName = executingAssembly.GetName().Name;
            return !string.IsNullOrEmpty(assemblyName) && resourceName.StartsWith(assemblyName) && resourceName.Contains(name);
        });

        if (string.IsNullOrEmpty(desiredManifestResources))
            return false;

        using (var ms = new MemoryStream())
        {
            executingAssembly.GetManifestResourceStream(desiredManifestResources).CopyTo(ms);
            bytes =  ms.ToArray();
            return true;
        }
    }

    public static Texture2D Texture2dFromBytes(byte[] imgBytes)
    {
        Texture2D tex = new(2, 2);
        tex.LoadImage(imgBytes);
        return tex;
    }
}