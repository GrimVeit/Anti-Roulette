using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipShopVisualPresenter
{
    private readonly ChipShopVisualModel _model;
    private readonly ChipShopVisualView _view;

    public ChipShopVisualPresenter(ChipShopVisualModel model, ChipShopVisualView view)
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

        _model.OnSetChips += _view.SetChips;
        _model.OnOpenChip += _view.OpenChip;
    }

    private void DeactivateEvents()
    {
        _view.OnBuy -= _model.Buy;

        _model.OnSetChips -= _view.SetChips;
        _model.OnOpenChip -= _view.OpenChip;
    }
}
