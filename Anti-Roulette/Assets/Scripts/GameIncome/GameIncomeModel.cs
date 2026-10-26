using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIncomeModel
{
    private readonly IMoneyProvider _moneyProvider;

    private int _currentIncome = 0;

    public GameIncomeModel(IMoneyProvider moneyProvider)
    {
        _moneyProvider = moneyProvider;
    }

    public void Add(int income)
    {
        _currentIncome += income;
        _moneyProvider.ChangeMoney(income);
        OnChangeMoney?.Invoke(_currentIncome);
    }

    public void Reset()
    {
        _currentIncome = 0;
        OnChangeMoney?.Invoke(_currentIncome);
    }

    #region Output

    public event Action<int> OnChangeMoney;

    #endregion
}
