using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShopVisualPresenter
{
    private readonly BackgroundShopVisualModel _model;
    private readonly BackgroundShopVisualView _view;

    public BackgroundShopVisualPresenter(BackgroundShopVisualModel model, BackgroundShopVisualView view)
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
        _view.OnBuy += _model.Buy;

        _model.OnSetBackgrounds += _view.SetBackgrounds;
        _model.OnOpenBackground += _view.OpenBackground;
    }

    private void DeactivateEvents()
    {
        _view.OnBuy -= _model.Buy;

        _model.OnSetBackgrounds -= _view.SetBackgrounds;
        _model.OnOpenBackground -= _view.OpenBackground;
    }
}
