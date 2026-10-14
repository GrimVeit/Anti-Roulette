using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorVisualModel
{
    private readonly IStoreNumberSectorListener _numberSectorListener;

    public NumberSectorVisualModel(IStoreNumberSectorListener numberSectorListener)
    {
        _numberSectorListener = numberSectorListener;
    }

    public void Initialize()
    {
        _numberSectorListener.OnChooseNumberColor += ChooseNumberColor;
    }

    public void Dispose()
    {
        _numberSectorListener.OnChooseNumberColor -= ChooseNumberColor;
    }

    private void ChooseNumberColor(NumberColor numberColor)
    {
        OnNumberColorChanged?.Invoke(numberColor);
    }

    #region Output

    public event Action<NumberColor> OnNumberColorChanged;

    #endregion
}
