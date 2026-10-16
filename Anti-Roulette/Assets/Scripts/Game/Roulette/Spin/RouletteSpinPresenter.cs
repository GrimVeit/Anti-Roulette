using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpinPresenter : IRouletteSpinProvider
{
    private readonly RouletteSpinModel _model;
    private readonly RouletteSpinView _view;

    public RouletteSpinPresenter(RouletteSpinModel model, RouletteSpinView view)
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
        _model.OnSpin += _view.Spin;
    }

    private void DeactivateEvents()
    {
        _model.OnSpin += _view.Spin;
    }

    #region Output

    public event Action OnStopped
    {
        add => _view.OnSpinStopped += value;
        remove => _view.OnSpinStopped -= value;
    }

    #endregion

    #region Input

    public void Spin(float time) => _model.Spin(time); 

    #endregion
}

public interface IRouletteSpinProvider
{
    public event Action OnStopped;

    public void Spin(float time);
}
