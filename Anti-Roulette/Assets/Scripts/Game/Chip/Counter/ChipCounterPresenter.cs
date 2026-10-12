using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCounterPresenter
{
    private readonly ChipCounterModel _model;
    private readonly ChipCounterView _view;

    public ChipCounterPresenter(ChipCounterModel model, ChipCounterView view)
    {
        _model = model; _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnChangeCount += _view.SetCount;
    }

    private void DeactivateEvents()
    {
        _model.OnChangeCount -= _view.SetCount;
    }
}
