using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipArrowPresenter
{
    private readonly ChipArrowModel _model;
    private readonly ChipArrowView _view;

    public ChipArrowPresenter(ChipArrowModel model, ChipArrowView view)
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
}

public interface IChipArrowProvider
{

}
