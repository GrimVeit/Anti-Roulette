using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPresenter : IFinishInfo, IFinishProvider
{
    private readonly FinishModel _model;
    private readonly FinishView _view;

    public FinishPresenter(FinishModel model, FinishView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Lose();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnAddWin += _view.Win;
        _model.OnReset += _view.Lose;
    }

    private void DeactivateEvents()
    {
        _model.OnAddWin -= _view.Win;
        _model.OnReset -= _view.Lose;
    }

    public void Reset() => _model.Reset();
    public void AddWin() => _model.AddWin();

    public bool IsWin => _model.IsWin;
}

public interface IFinishInfo
{
    public bool IsWin { get; }
}

public interface IFinishProvider
{
    public void Reset();
    public void AddWin();
}
