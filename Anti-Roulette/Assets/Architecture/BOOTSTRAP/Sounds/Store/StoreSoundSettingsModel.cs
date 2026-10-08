using System;
using UnityEngine;

public class StoreSoundSettingsModel
{
    public float SoundVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public bool IsMuted_Sound { get; private set; }
    public bool IsMuted_Music { get; private set; }

    private readonly string soundVolumeKey;
    private readonly string musicVolumeKey;
    private readonly string muteKey_Sound;
    private readonly string muteKey_Music;

    public event Action<float> OnChangeSoundVolume;
    public event Action<float> OnChangeMusicVolume;
    public event Action<bool> OnChangeMute_Sound;
    public event Action<bool> OnChangeMute_Music;

    public StoreSoundSettingsModel(
        string soundVolumeKey,
        string musicVolumeKey,
        string muteKey_Sound,
        string muteKey_Music)
    {
        this.soundVolumeKey = soundVolumeKey;
        this.musicVolumeKey = musicVolumeKey;
        this.muteKey_Sound = muteKey_Sound;
        this.muteKey_Music = muteKey_Music;
    }

    public void Initialize()
    {
        SoundVolume = PlayerPrefs.GetFloat(soundVolumeKey, 0.5f);
        MusicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.7f);
        IsMuted_Sound = PlayerPrefs.GetInt(muteKey_Sound, 0) == 1;
        IsMuted_Music = PlayerPrefs.GetInt(muteKey_Music, 0) == 1;

        OnChangeSoundVolume?.Invoke(SoundVolume);
        OnChangeMusicVolume?.Invoke(MusicVolume);
        OnChangeMute_Sound?.Invoke(IsMuted_Sound);
        OnChangeMute_Music?.Invoke(IsMuted_Music);
    }

    public void Dispose()
    {
        PlayerPrefs.SetFloat(soundVolumeKey, SoundVolume);
        PlayerPrefs.SetFloat(musicVolumeKey, MusicVolume);
        PlayerPrefs.SetInt(muteKey_Sound, IsMuted_Sound ? 1 : 0);
        PlayerPrefs.SetInt(muteKey_Music, IsMuted_Music ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void SetSoundVolume(float value)
    {
        value = Mathf.Clamp01(value);

        if (Mathf.Approximately(SoundVolume, value))
            return;

        SoundVolume = value;

        OnChangeSoundVolume?.Invoke(SoundVolume);
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);

        if (Mathf.Approximately(MusicVolume, value))
            return;

        MusicVolume = value;

        Debug.Log(MusicVolume);

        OnChangeMusicVolume?.Invoke(MusicVolume);
    }

    public void SetMute_Sound(bool value)
    {
        if (IsMuted_Sound == value)
            return;

        IsMuted_Sound = value;

        Debug.Log("SOUND MUTE - " + value);

        OnChangeMute_Sound?.Invoke(IsMuted_Sound);
    }

    public void SetMute_Music(bool value)
    {
        if (IsMuted_Music == value)
            return;

        IsMuted_Music = value;

        Debug.Log("MUSIC MUTE - " + value);

        OnChangeMute_Music?.Invoke(IsMuted_Music);
    }
}
