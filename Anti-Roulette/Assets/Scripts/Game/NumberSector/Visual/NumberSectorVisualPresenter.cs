using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorVisualPresenter
{
    private readonly NumberSectorVisualModel _model;
    private readonly NumberSectorVisualView _view;

    public NumberSectorVisualPresenter(NumberSectorVisualModel model, NumberSectorVisualView view)
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
        _model.OnNumberColorChanged += _view.SetNumberColor;
    }

    private void DeactivateEvents()
    {
        _model.OnNumberColorChanged -= _view.SetNumberColor;
    }
}
