using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSoundSettingsPresenter : ISoundSettingsInfoProvider, ISoundSettingsEventsProvider, ISoundSettingsProvider
{
    private readonly StoreSoundSettingsModel _model;

    public StoreSoundSettingsPresenter(StoreSoundSettingsModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Info

    public float SoundVolume => _model.SoundVolume;
    public float MusicVolume => _model.MusicVolume;
    public bool IsMuted_Sound => _model.IsMuted_Sound;
    public bool IsMuted_Music => _model.IsMuted_Music;

    #endregion

    #region Events

    public event Action<float> OnChangeSoundVolume
    {
        add => _model.OnChangeSoundVolume += value;
        remove => _model.OnChangeSoundVolume -= value;
    }

    public event Action<float> OnChangeMusicVolume
    {
        add => _model.OnChangeMusicVolume += value;
        remove => _model.OnChangeMusicVolume -= value;
    }

    public event Action<bool> OnChangeMute_Sound
    {
        add => _model.OnChangeMute_Sound += value;
        remove => _model.OnChangeMute_Sound -= value;
    }

    public event Action<bool> OnChangeMute_Music
    {
        add => _model.OnChangeMute_Music += value;
        remove => _model.OnChangeMute_Music -= value;
    }

    #endregion

    #region Input

    public void SetSoundVolume(float value)
    {
        _model.SetSoundVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        _model.SetMusicVolume(value);
    }

    public void SetMute_Sound(bool value)
    {
        _model.SetMute_Sound(value);
    }

    public void SetMute_Music(bool value)
    {
        _model.SetMute_Music(value);
    }

    #endregion
}

public interface ISoundSettingsInfoProvider
{
    float SoundVolume { get; }
    float MusicVolume { get; }
    bool IsMuted_Sound { get; }
    bool IsMuted_Music { get; }
}

public interface ISoundSettingsEventsProvider
{
    event Action<float> OnChangeSoundVolume;
    event Action<float> OnChangeMusicVolume;
    event Action<bool> OnChangeMute_Sound;
    event Action<bool> OnChangeMute_Music;
}

public interface ISoundSettingsProvider
{
    void SetSoundVolume(float value);
    void SetMusicVolume(float value);

    void SetMute_Sound(bool value);
    void SetMute_Music(bool value);
}
