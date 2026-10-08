using System;
using UnityEngine;

public class VolumeSettingsModel
{
    private readonly ISoundSettingsInfoProvider infoProvider;
    private readonly ISoundSettingsProvider settingsProvider;
    private readonly ISoundSettingsEventsProvider eventsProvider;

    public VolumeSettingsModel(ISoundSettingsInfoProvider infoProvider, ISoundSettingsProvider settingsProvider, ISoundSettingsEventsProvider eventsProvider)
    {
        this.infoProvider = infoProvider;
        this.settingsProvider = settingsProvider;
        this.eventsProvider = eventsProvider;
    }

    public void Initialize()
    {
        eventsProvider.OnChangeMute_Sound += HandleSoundVolumeChanged;
        eventsProvider.OnChangeMute_Music += HandleMusicVolumeChanged;

        OnSoundMuteChanged?.Invoke(infoProvider.IsMuted_Sound);
        OnMusicMuteChanged?.Invoke(infoProvider.IsMuted_Music);
    }

    public void Dispose()
    {
        eventsProvider.OnChangeMute_Sound -= HandleSoundVolumeChanged;
        eventsProvider.OnChangeMute_Music -= HandleMusicVolumeChanged;
    }

    public void SetSoundVolume(bool isMute)
    {
        settingsProvider.SetMute_Sound(isMute);
    }

    public void SetMusicVolume(bool isMute)
    {
        settingsProvider.SetMute_Music(isMute);
    }

    private void HandleSoundVolumeChanged(bool value)
    {
        OnSoundMuteChanged?.Invoke(value);
    }

    private void HandleMusicVolumeChanged(bool value)
    {
        OnMusicMuteChanged?.Invoke(value);
    }

    #region Output

    public event Action<bool> OnSoundMuteChanged;
    public event Action<bool> OnMusicMuteChanged;

    #endregion
}
