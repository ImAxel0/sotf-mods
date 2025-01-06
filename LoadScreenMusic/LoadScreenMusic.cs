using SonsSdk;
using SUI;
using RedLoader;
using RedLoader.Utils;
using Sons.FMOD;
using Il2CppSystem.Runtime.Remoting.Messaging;
using Sons.Music;
using System.Collections;
using UnityEngine;
using FMODCustom;
using System.Diagnostics;

namespace LoadScreenMusic;

public class LoadScreenMusic : SonsMod
{
    static Channel _MenuCh;
    static Channel _LoadingCh;
    static bool _isValidMenuAudio;
    static bool _isValidLoadingAudio;

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    internal void StopHeliSound()
    {
        if (Config.StopHeliSound.Value) GameObject.Find("SonsTitleHelicopterController/introCrash_ANIM_helicopters/masterRotate/radiusOffset/Main Camera/HelicopterAudioEmitter")?.SetActive(false);
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config), true);

        if (Config.StopHeliSound.Value) StopHeliSound();

        string menuPath = Path.Combine(LoaderEnvironment.ModsDirectory, @"LoadScreenMusic\Audios", @Config.MenuFileName.Value);
        if (!string.IsNullOrEmpty(Config.MenuFileName.Value) && File.Exists(menuPath))
        {
            SoundTools.RegisterSound("MenuMusic", menuPath);
            _isValidMenuAudio = true;

            MusicManager.Stop();
            _MenuCh = AudioController.PlaySound("MenuMusic", AudioController.SoundType.Music, true, Config.MenuVolume.Value);
        }
        else RLog.Warning("Menu audio filename is invalid or file doesn't exist");

        string loadingPath = Path.Combine(LoaderEnvironment.ModsDirectory, @"LoadScreenMusic\Audios", @Config.LoadingFileName.Value);
        if (!string.IsNullOrEmpty(Config.LoadingFileName.Value) && File.Exists(loadingPath))
        {
            SoundTools.RegisterSound("LoadingMusic", loadingPath);
            _isValidLoadingAudio = true;
        }
        else RLog.Warning("Loading screen audio filename is invalid or file doesn't exist");
    }

    protected override void OnSonsSceneInitialized(ESonsScene sonsScene)
    {
        if (sonsScene == ESonsScene.Title && _isValidMenuAudio)
        {
            MusicManager.Stop();
            if (Config.StopHeliSound.Value) StopHeliSound();
            if (!AudioController.IsPlaying("MenuMusic"))
            {
                _MenuCh = AudioController.PlaySound("MenuMusic", AudioController.SoundType.Music, true, Config.MenuVolume.Value);
            }
        }

        if (sonsScene == ESonsScene.Loading)
        {
            if (_isValidMenuAudio) AudioController.StopSound("MenuMusic");

            if (_isValidLoadingAudio) _LoadingCh = AudioController.PlaySound("LoadingMusic", AudioController.SoundType.Music, true, Config.LoadingVolume.Value);
        }
    }

    protected override void OnGameStart()
    {
        if (_isValidLoadingAudio) AudioController.StopSound("LoadingMusic");
    }

    public static void AdjustMenuVolume(float a, float b)
    {
        _MenuCh?.setVolume(b / 2);
    }
}