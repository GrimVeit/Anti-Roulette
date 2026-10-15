using System;
using UnityEngine;

public class ChipSpawnerPresenter : IChipSpawnerProvider, IChipSpawnerListener
{
    private readonly ChipSpawnerModel _model;
    private readonly ChipSpawnerView _view;

    public ChipSpawnerPresenter(ChipSpawnerModel model, ChipSpawnerView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnSpawn += _view.Spawn;
    }

    private void DeactivateEvents()
    {
        _model.OnSpawn -= _view.Spawn;
    }

    #region Output

    public event Action OnFailed
    {
        add => _view.OnFailed += value;
        remove => _view.OnFailed -= value;
    }

    #endregion

    #region Input

    public void Spawn(Vector2 direction, float power) => _model.Spawn(direction, power);

    #endregion
}

public interface IChipSpawnerListener
{
    public event Action OnFailed;
}

public interface IChipSpawnerProvider
{
    public void Spawn(Vector2 direction, float power);
}
