using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTypeVisualPresenter
{
    private readonly ShopTypeVisualModel _model;
    private readonly ShopTypeVisualView _view;

    public ShopTypeVisualPresenter(ShopTypeVisualModel model, ShopTypeVisualView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClickType += _model.ClickType;

        _model.OnOpenType += _view.Open;
        _model.OnCloseType += _view.Close;
    }

    private void DeactivateEvents()
    {
        _view.OnClickType -= _model.ClickType;

        _model.OnOpenType -= _view.Open;
        _model.OnCloseType -= _view.Close;
    }
}
