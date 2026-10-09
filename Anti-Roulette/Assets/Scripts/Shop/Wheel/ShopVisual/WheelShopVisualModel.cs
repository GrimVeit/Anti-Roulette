using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelShopVisualModel
{
    private readonly IWheelInfoProvider _wheelInfoProvider;
    private readonly IWheelListener _wheelListener;
    private readonly IWheelProvider _wheelProvider;
    private readonly IMoneyProvider _moneyProvider;

    public WheelShopVisualModel(IWheelInfoProvider wheelInfoProvider, IWheelListener wheelListener, IWheelProvider wheelProvider, IMoneyProvider moneyProvider)
    {
        _wheelInfoProvider = wheelInfoProvider;
        _wheelListener = wheelListener;
        _wheelProvider = wheelProvider;
        _moneyProvider = moneyProvider;
    }

    public void Initialize()
    {
        _wheelListener.OnOpenWheel += OpenWheel;

        OnSetWheels?.Invoke(_wheelInfoProvider.GetWheels());
    }

    public void Dispose()
    {
        _wheelListener.OnOpenWheel -= OpenWheel;
    }

    private void OpenWheel(Wheel wheel)
    {
        OnOpenWheel?.Invoke(wheel.Index);
    }

    public void Buy(int index)
    {
        if (_wheelInfoProvider.IsWheelOpened(index)) return;

        var wheel = _wheelInfoProvider.GetWheel(index);

        if (!_moneyProvider.CanAfford(wheel.Price))
        {
            //NO MONEY
            return;
        }

        _moneyProvider.ChangeMoney(-wheel.Price);

        _wheelProvider.OpenWheel(wheel.Index);
    }

    #region Output

    public event Action<IReadOnlyList<Wheel>> OnSetWheels;
    public event Action<int> OnOpenWheel;

    #endregion
}
