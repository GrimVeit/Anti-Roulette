public class VolumeSettingsPresenter
{
    private readonly VolumeSettingsModel _model;
    private readonly VolumeSettingsView _view;

    public VolumeSettingsPresenter(VolumeSettingsModel model, VolumeSettingsView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        _view.OnChangeSoundMute += _model.SetSoundVolume;
        _view.OnChangeMusicMute += _model.SetMusicVolume;

        _model.OnSoundMuteChanged += _view.SetSoundVolume;
        _model.OnMusicMuteChanged += _view.SetMusicVolume;

        _view.Initialize();
        _model.Initialize();
    }

    public void Dispose()
    {
        _view.OnChangeSoundMute -= _model.SetSoundVolume;
        _view.OnChangeMusicMute -= _model.SetMusicVolume;

        _model.OnSoundMuteChanged -= _view.SetSoundVolume;
        _model.OnMusicMuteChanged -= _view.SetMusicVolume;

        _view.Dispose();
        _model.Dispose();
    }
}