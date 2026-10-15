using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorTriggerPresenter : INumberSectorTriggerListener
{
    private readonly NumberSectorTriggerModel _model;
    private readonly NumberSectorTriggerView _view;

    public NumberSectorTriggerPresenter(NumberSectorTriggerModel model, NumberSectorTriggerView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialze()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnSetSector += _model.SetSector;
    }

    private void DeactivateEvents()
    {
        _view.OnSetSector -= _model.SetSector;
    }

    #region Output

    public event Action OnFailed
    {
        add => _model.OnFailed += value;
        remove => _model.OnFailed -= value;
    }

    public event Action OnSuccess
    {
        add => _model.OnSuccess += value;
        remove => _model.OnSuccess -= value;
    }

    #endregion
}

public interface INumberSectorTriggerListener
{
    public event Action OnFailed;
    public event Action OnSuccess;
}
