using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpinModel
{
    public void Spin(float time)
    {
        OnSpin?.Invoke(time);
    }

    #region Output

    public event Action<float> OnSpin;

    #endregion
}
