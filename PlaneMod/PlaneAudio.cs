using SonsSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedLoader;
using RedLoader.Utils;
using Channel = FMODCustom.Channel;
using UnityEngine;
using System.Diagnostics;

namespace PlaneMod;

[RegisterTypeInIl2Cpp]
public class PlaneAudio : MonoBehaviour
{
    public static Channel IdleCh;
    public static Channel EngineCh;

    public static void Init()
    {
        SoundTools.RegisterSound("idle", Path.Combine(LoaderEnvironment.ModsDirectory, @"PlaneMod\Audio\idle.mp3"));
        SoundTools.RegisterSound("engine", Path.Combine(LoaderEnvironment.ModsDirectory, @"PlaneMod\Audio\engine.mp3"));
    }

    public static void PlayEngine()
    {
        EngineCh = AudioController.PlaySound("engine", AudioController.SoundType.Sfx, true);
    }

    public static void StopEngine() 
    {
        AudioController.StopSound("engine");
    }

    public void Update()
    {
        if (!PlaneAction.IsFlying) return;
        float pitch = Mathf.Clamp(PlaneAction.Throttle, 0, 100) / 100;
        EngineCh.setPitch(Mathf.Clamp(pitch, 0.2f, 1f));
    }
}
