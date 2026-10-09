using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelShopVisualPresenter
{
    private readonly WheelShopVisualModel _model;
    private readonly WheelShopVisualView _view;

    public WheelShopVisualPresenter(WheelShopVisualModel model, WheelShopVisualView view)
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

        _model.OnSetWheels += _view.SetWheels;
        _model.OnOpenWheel += _view.OpenWheel;
    }

    private void DeactivateEvents()
    {
        _view.OnBuy -= _model.Buy;

        _model.OnSetWheels -= _view.SetWheels;
        _model.OnOpenWheel -= _view.OpenWheel;
    }
}
