using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSelectVisualPresenter
{
    private readonly WheelSelectVisualModel _model;
    private readonly WheelSelectVisualView _view;

    public WheelSelectVisualPresenter(WheelSelectVisualModel model, WheelSelectVisualView view)
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

        _model.OnSetWheels += _view.SetBackgrounds;
        _model.OnOpen += _view.Open;
        _model.OnSelect += _view.Select;
        _model.OnDeselect += _view.Deselect;
    }

    private void DeactivateEvents()
    {
        _view.OnSelect -= _model.Select;

        _model.OnSetWheels -= _view.SetBackgrounds;
        _model.OnOpen -= _view.Open;
        _model.OnSelect -= _view.Select;
        _model.OnDeselect -= _view.Deselect;
    }
}
