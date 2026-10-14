using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorHighlightPresenter : INumberSectorHighlightProvider
{
    private readonly NumberSectorHighlightModel _model;
    private readonly NumberSectorHighlightView _view;

    public NumberSectorHighlightPresenter(NumberSectorHighlightModel model, NumberSectorHighlightView view)
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

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnShowHighlight += _view.Show;
        _model.OnHideHighlight += _view.Hide;
    }

    private void DeactivateEvents()
    {
        _model.OnShowHighlight -= _view.Show;
        _model.OnHideHighlight -= _view.Hide;
    }

    #region Input

    public void Show() => _model.Show();
    public void Hide() => _model.Hide();

    #endregion
}

public interface INumberSectorHighlightProvider
{
    public void Show(); //показать текущее
    public void Hide(); //скрыть текущее
}
