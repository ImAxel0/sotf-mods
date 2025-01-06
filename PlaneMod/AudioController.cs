using SonsSdk;
using FMODCustom;
using Sons.Settings;

namespace PlaneMod;

public class AudioController
{
    static Dictionary<string, Channel> _idChannelPair = new();
    static float _correctionFactor = 0.5f;

    public enum SoundType
    {
        Music = 0,
        Sfx = 1,
    }

    public static Channel PlaySound(string id, SoundType type, bool shouldLoop = false, float volAdjustmant = 1, float? pitch = null)
    {
        float volume = (type == SoundType.Music) ? AudioSettings._musicVolume : AudioSettings._sfxVolume;

        Channel ch = SoundTools.PlaySound(id, volume * AudioSettings._masterVolume * _correctionFactor * volAdjustmant, pitch);

        if (shouldLoop)
        {
            ch.setMode(MODE.LOOP_NORMAL);
            _idChannelPair.Add(id, ch);
        }
        return ch;
    }

    public static bool StopSound(string id)
    {
        if (_idChannelPair.TryGetValue(id, out Channel chMusic))
        {
            chMusic.stop();
            _idChannelPair.Remove(id);
            return true;
        }
        return false;
    }
}
