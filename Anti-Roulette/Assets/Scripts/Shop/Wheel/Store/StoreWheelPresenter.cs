using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreWheelPresenter : IWheelInfoProvider, IWheelListener, IWheelProvider
{
    private readonly StoreWheelModel _model;

    public StoreWheelPresenter(StoreWheelModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Provider

    public void OpenWheel(int index)
    {
        _model.OpenWheel(index);
    }

    public void SelectWheel(int index)
    {
        _model.SelectWheel(index);
    }

    #endregion

    #region Info

    public Wheel GetWheel(int index)
    {
        return _model.GetWheel(index);
    }

    public IReadOnlyList<Wheel> GetWheels()
    {
        return _model.GetWheels();
    }

    public Wheel GetCurrentWheel()
    {
        return _model.GetCurrentWheel();
    }

    public int CurrentWheelIndex => _model.GetCurrentWheelIndex();

    public bool IsWheelOpened(int index)
    {
        return _model.IsWheelOpened(index);
    }

    public bool IsWheelSelected(int index)
    {
        return _model.IsWheelSelected(index);
    }

    #endregion

    #region Listener

    public event Action<Wheel> OnOpenWheel
    {
        add => _model.OnOpenWheel += value;
        remove => _model.OnOpenWheel -= value;
    }

    public event Action<Wheel> OnSelectWheel
    {
        add => _model.OnSelectWheel += value;
        remove => _model.OnSelectWheel -= value;
    }

    #endregion
}

public interface IWheelProvider
{
    void OpenWheel(int index);
    void SelectWheel(int index);
}

public interface IWheelInfoProvider
{
    Wheel GetWheel(int index);

    IReadOnlyList<Wheel> GetWheels();

    Wheel GetCurrentWheel();

    int CurrentWheelIndex { get; }

    bool IsWheelOpened(int index);

    bool IsWheelSelected(int index);
}

public interface IWheelListener
{
    event Action<Wheel> OnOpenWheel;
    event Action<Wheel> OnSelectWheel;
}
