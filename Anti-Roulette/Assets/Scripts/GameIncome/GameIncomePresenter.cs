using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIncomePresenter : IGameIncomeProvider
{
    private readonly GameIncomeModel _model;
    private readonly GameIncomeView _view;

    public GameIncomePresenter(GameIncomeModel model, GameIncomeView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnChangeMoney += _view.SetMoney;
    }

    private void DeactivateEvents()
    {
        _model.OnChangeMoney -= _view.SetMoney;
    }

    #region Input

    public void Add(int income) => _model.Add(income);
    public void Reset() => _model.Reset();

    #endregion
}

public interface IGameIncomeProvider
{
    public void Add(int income);
    public void Reset();
}
