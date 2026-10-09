using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSelectVisualModel
{
    private readonly IWheelInfoProvider _wheelInfoProvider;
    private readonly IWheelListener _wheelListener;
    private readonly IWheelProvider _wheelProvider;

    public WheelSelectVisualModel(IWheelInfoProvider wheelInfoProvider, IWheelListener wheelListener, IWheelProvider wheelProvider)
    {
        _wheelInfoProvider = wheelInfoProvider;
        _wheelListener = wheelListener;
        _wheelProvider = wheelProvider;
    }

    public void Initialize()
    {
        _wheelListener.OnOpenWheel += Open;
        _wheelListener.OnSelectWheel += Select;

        OnSetWheels?.Invoke(_wheelInfoProvider.GetWheels());
        OnSelect?.Invoke(_wheelInfoProvider.CurrentWheelIndex);
    }

    public void Dispose()
    {
        _wheelListener.OnOpenWheel -= Open;
        _wheelListener.OnSelectWheel -= Select;
    }

    public void Select(int index)
    {
        if (_wheelInfoProvider.CurrentWheelIndex == index) return;

        if (!_wheelInfoProvider.GetWheel(index).IsOpened) return;

        OnDeselect?.Invoke(_wheelInfoProvider.CurrentWheelIndex);

        _wheelProvider.SelectWheel(index);
    }

    private void Open(Wheel wheel)
    {
        OnOpen?.Invoke(wheel.Index);
    }

    private void Select(Wheel wheel)
    {
        OnSelect?.Invoke(wheel.Index);
    }

    #region Output

    public event Action<IReadOnlyList<Wheel>> OnSetWheels;
    public event Action<int> OnOpen;
    public event Action<int> OnSelect;
    public event Action<int> OnDeselect;

    #endregion
}
