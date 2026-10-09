using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelectVisualPresenter
{
    private readonly BackgroundSelectVisualModel _model;
    private readonly BackgroundSelectVisualView _view;

    public BackgroundSelectVisualPresenter(BackgroundSelectVisualModel model, BackgroundSelectVisualView view)
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

        _model.OnSetBackgrounds += _view.SetBackgrounds;
        _model.OnOpenBackground += _view.OpenBackground;
        _model.OnSelectBackground += _view.SelectBackground;
        _model.OnDeselectBackground += _view.DeselectBackground;
    }

    private void DeactivateEvents()
    {
        _view.OnSelect -= _model.Select;

        _model.OnSetBackgrounds -= _view.SetBackgrounds;
        _model.OnOpenBackground -= _view.OpenBackground;
        _model.OnSelectBackground -= _view.SelectBackground;
        _model.OnDeselectBackground -= _view.DeselectBackground;
    }
}
