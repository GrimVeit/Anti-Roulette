using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpinModel
{
    private readonly ISoundProvider _soundProvider;

    public RouletteSpinModel(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    public void Spin(float time)
    {
        _soundProvider.PlayOneShot("Wheel");

        OnSpin?.Invoke(time);
    }

    #region Output

    public event Action<float> OnSpin;

    #endregion
}
