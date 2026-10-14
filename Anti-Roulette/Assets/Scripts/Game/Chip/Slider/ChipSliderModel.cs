using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSliderModel
{
    public void Activate()
    {
        OnActivate?.Invoke();
    }

    public void Deactivate()
    {
        OnDeactivate?.Invoke();
    }

    #region Output

    public event Action OnActivate;
    public event Action OnDeactivate;

    #endregion
}
