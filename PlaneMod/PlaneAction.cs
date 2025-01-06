using System;
using System.Collections;
using TheForest.Utils;
using UnityEngine;
using RedLoader;
using Sons.Input;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using SonsSdk;

namespace PlaneMod;

[RegisterTypeInIl2Cpp]
public class PlaneAction : MonoBehaviour
{
    public static GameObject Plane;
    public static Rigidbody PlaneRb;
    public static Transform PlaneRotor;
    private static bool _flying;

    static float _throttleIncrement = 0.1f;
    static float _maxThrust = 200f;
    static float _responsiveness = 3f;
    static float _lift = 135f;

    public static float Throttle;
    static float _roll;
    static float _pitch;
    static float _yaw;

    public static float CurrentSpeed;

    public static void InitNew()
    {
        if (Plane != null) Destroy(Plane);

        Texture2D biplane = new(0, 0);
        if (ResourcesLoader.TryGetEmbeddedResourceBytes("biplane", out var biplaneByte))
        {
            biplane = ResourcesLoader.Texture2dFromBytes(biplaneByte);
        }
        else RLog.Error("Couldn't load biplane.png");

        Transform mainCam = LocalPlayer.MainCamTr;

        if (Physics.Raycast(mainCam.position, mainCam.forward, out RaycastHit hit, Mathf.Infinity))
        {
            Plane = Instantiate(AssetLoader.GameObjects.Find(x => x.name == "plane(Clone)"), 
                hit.point + Vector3.up * 1 + mainCam.forward * 5f, 
                Quaternion.identity);

            PlaneRb = Plane.GetComponent<Rigidbody>();
            PlaneRotor = Plane.transform.Find("Rotor").transform;
            GameObject interactable = Interactable.AddInteractable(Plane, 3f, Interactable.InteractableType.Take, biplane);
            interactable.SetActive(false); interactable.SetActive(true);
            Plane.AddComponent<PlaneAction>();
            Plane.AddComponent<PlaneAudio>();
            SonsTools.ShowMessage("Plane spawned!");
        }
    }

    public static void InitNew(Vector3 position, Quaternion rotation)
    {
        if (Plane != null) Destroy(Plane);

        Texture2D biplane = new(0, 0);
        if (ResourcesLoader.TryGetEmbeddedResourceBytes("biplane", out var biplaneByte))
        {
            biplane = ResourcesLoader.Texture2dFromBytes(biplaneByte);
        }
        else RLog.Error("Couldn't load biplane.png");

        Plane = Instantiate(AssetLoader.GameObjects.Find(x => x.name == "plane(Clone)"),
            position,
            rotation);

        PlaneRb = Plane.GetComponent<Rigidbody>();
        PlaneRotor = Plane.transform.Find("Rotor").transform;
        GameObject interactable = Interactable.AddInteractable(Plane, 3f, Interactable.InteractableType.Take, biplane);
        interactable.SetActive(false); interactable.SetActive(true);
        Plane.AddComponent<PlaneAction>();
        Plane.AddComponent<PlaneAudio>();
    }

    public static bool IsFlying
    {
        get { return _flying; }
        set { _flying = value; }
    }

    public static void EnterPlane()
    {
        LocalPlayer.CamRotator.ForceResetRotation(); // vertical
        LocalPlayer.FpCharacter.transform.position = PlaneRb.transform.position + Vector3.up * 2f;
        LocalPlayer.FpCharacter.MovementLocked = true;
        LocalPlayer.FpCharacter._rigidbody.isKinematic = true;
        LocalPlayer.FpCharacter._primaryCollider.enabled = false;
        LocalPlayer.FpCharacter._primaryColliderEnabled = false;
        LocalPlayer.ClothingSystem.gameObject.SetActive(false);
        LocalPlayer.RaceSystem.gameObject.SetActive(false);
        LocalPlayer.Transform.Find("PlayerAnimator/ArmourSystem").gameObject.SetActive(false);
        LocalPlayer.FpCharacter.transform.parent = PlaneRb.transform.root;
        LocalPlayer.MainCamTr.localPosition = Vector3.zero + Vector3.down * 2f + Vector3.back * Config.CameraDistance.Value;
        LocalPlayer.CamRotator.lockRotation = true; // vertical
        LocalPlayer.MainRotator.lockRotation = true;
        IsFlying = true;
    }

    public static void ExitPlane()
    {
        Throttle = 0f;
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
        LocalPlayer.MainRotator.lockRotation = false;
        IsFlying = false;
    }

    private static float ResponsModifier
    {
        get { return (PlaneRb.mass / 10f) * _responsiveness; }
    }

    public static void GetInput()
    {
        if (InputManager.IsKeyboard())
        {
            HandleKeyboard();
        }
        else HandleGamepad();
    }

    public static void HandleGamepad()
    {
        _pitch = Mathf.Clamp(InputSystem.GetAxis(InputSystem.Actions.Vertical), -1, 1);
        _roll = Mathf.Clamp(InputSystem.GetAxis(InputSystem.Actions.Horizontal), -1, 1);
        _yaw = InputSystem.GetAxis(InputSystem.Actions.Rotate);

        if (!Config.InvertedPitch.Value) _pitch = -_pitch;
        if (Config.InvertedRoll.Value) _roll = -_roll;

        _pitch *= 7f;
        _roll *= 7f;
        _yaw *= 1.5f;

        if (InputSystem.InputMapping.@default.BookFlipPageNext.IsPressed()) Throttle += _throttleIncrement;

        else if (InputSystem.InputMapping.@default.BookFlipPagePrevious.IsPressed()) Throttle -= _throttleIncrement * 2;
     
        LocalPlayer.MainRotator.lockRotation = false;

        LocalPlayer.MainCamTr.localPosition = Vector3.zero + Vector3.down * 2f + Vector3.back * Config.CameraDistance.Value;

        Throttle = Mathf.Clamp(Throttle, 0f, 100f);
    }

    public static void HandleKeyboard()
    {
        _pitch = Input.GetAxis("Mouse Y");
        _roll = Input.GetAxis("Mouse X");
        _yaw = Input.GetAxis("Horizontal");

        if (!Config.InvertedPitch.Value) _pitch = -_pitch;
        if (Config.InvertedRoll.Value) _roll = -_roll;

        if (Input.GetKey(KeyCode.W)) Throttle += _throttleIncrement;

        else if (Input.GetKey(KeyCode.S)) Throttle -= _throttleIncrement * 2;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            LocalPlayer.MainRotator.lockRotation = false;
            _pitch = 0f;
            _roll = 0f;
            _yaw = 0f;
        }
        else LocalPlayer.MainRotator.lockRotation = true;

        LocalPlayer.MainCamTr.localPosition = Vector3.zero + Vector3.down * 2f + Vector3.back * Config.CameraDistance.Value;

        Throttle = Mathf.Clamp(Throttle, 0f, 100f);
    }

    public void Update()
    {
        if (InputSystem.InputMapping.@default.Interact.triggered && Interactable.GetUIElement(Plane).IsActive && !IsFlying)
        {
            EnterPlane();
            PlaneAudio.PlayEngine();
        }

        if ((Input.GetKeyDown(KeyCode.G) || InputSystem.InputMapping.@default.Back.triggered) && IsFlying)
        {
            ExitPlane();
            PlaneAudio.StopEngine();
        }

        if (IsFlying) GetInput();
    }

    public void FixedUpdate()
    {
        if (!IsFlying) return;

        if (Config.MphMode.Value) CurrentSpeed = (PlaneRb.velocity.magnitude * 3.6f) / 1.609f;
        else CurrentSpeed = PlaneRb.velocity.magnitude * 3.6f;

        PlaneRb.AddForce(Plane.transform.TransformVector(Vector3.right) * _maxThrust * Throttle);
        PlaneRb.AddTorque(Plane.transform.up * _yaw * ResponsModifier);
        PlaneRb.AddTorque(-Plane.transform.right * _roll * ResponsModifier);
        PlaneRb.AddTorque(-Plane.transform.forward * _pitch * ResponsModifier);
        PlaneRb.AddForce(Vector3.up * PlaneRb.velocity.magnitude * _lift);
        PlaneRotor.Rotate(Vector3.left * Throttle);
    }
}
