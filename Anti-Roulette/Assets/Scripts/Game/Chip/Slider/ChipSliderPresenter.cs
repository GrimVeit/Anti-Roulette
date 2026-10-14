using System;

public class ChipSliderPresenter : IChipSliderListen, IChipSliderActivator
{
    private readonly ChipSliderModel _model;
    private readonly ChipSliderView _view;

    public ChipSliderPresenter(ChipSliderModel model, ChipSliderView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnActivate += _view.Activate;
        _model.OnDeactivate += _view.Deactivate;
    }

    private void DeactivateEvents()
    {
        _model.OnActivate -= _view.Activate;
        _model.OnDeactivate -= _view.Deactivate;
    }

    #region Input

    public void Activate() => _model.Activate();
    public void Deactivate() => _model.Deactivate();

    #endregion

    #region Output

    public event Action<float> OnShotPower
    {
        add => _view.OnShotPower += value;
        remove => _view.OnShotPower -= value;
    }

    #endregion
}

public interface IChipSliderActivator
{
    public void Activate();
    public void Deactivate();
}

public interface IChipSliderListen
{
    public event Action<float> OnShotPower;
}
