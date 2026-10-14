using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpawnerPresenter : IChipSpawnerProvider
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

    #region Input

    public void Spawn(Vector2 direction, float power) => _model.Spawn(direction, power);

    #endregion
}

public interface IChipSpawnerProvider
{
    public void Spawn(Vector2 direction, float power);
}
