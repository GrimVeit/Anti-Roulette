using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSelectVisualPresenter
{
    private readonly ChipSelectVisualModel _model;
    private readonly ChipSelectVisualView _view;

    public ChipSelectVisualPresenter(ChipSelectVisualModel model, ChipSelectVisualView view)
    {
        _model = model;
        _view = view;
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
        _view.OnSelect += _model.Select;

        _model.OnSetChips += _view.SetBackgrounds;
        _model.OnOpen += _view.Open;
        _model.OnSelect += _view.Select;
        _model.OnDeselect += _view.Deselect;
    }

    private void DeactivateEvents()
    {
        _view.OnSelect -= _model.Select;

        _model.OnSetChips -= _view.SetBackgrounds;
        _model.OnOpen -= _view.Open;
        _model.OnSelect -= _view.Select;
        _model.OnDeselect -= _view.Deselect;
    }
}
