using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreNumberSectorPresenter : IStoreNumberSectorListener, IStoreNumberSectorProvider, IStoreNumberSectorInfo
{
    private readonly StoreNumberSectorModel _model;

    public StoreNumberSectorPresenter(StoreNumberSectorModel model)
    {
        _model = model;
    }

    public NumberColor CurrentColorNumber => _model.CurrentColorNumber;

    public void ChooseRandom() => _model.ChooseRandom();

    #region Output

    public event Action<NumberColor> OnChooseNumberColor
    {
        add => _model.OnChoose += value;
        remove => _model.OnChoose -= value;
    }

    #endregion
}

public interface IStoreNumberSectorInfo
{
    public NumberColor CurrentColorNumber { get; }
}

public interface IStoreNumberSectorListener
{
    public event Action<NumberColor> OnChooseNumberColor;
}

public interface IStoreNumberSectorProvider
{
    public void ChooseRandom();
}
