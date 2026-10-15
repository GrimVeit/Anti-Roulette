using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorTriggerModel
{
    private readonly IStoreNumberSectorInfo _storeNumberSectorInfo;

    public NumberSectorTriggerModel(IStoreNumberSectorInfo storeNumberSectorInfo)
    {
        _storeNumberSectorInfo = storeNumberSectorInfo;
    }

    public void SetSector(int number)
    {
        if(_storeNumberSectorInfo.CurrentColorNumber.Number == number)
        {
            OnSuccess?.Invoke();
        }
        else
        {
            OnFailed?.Invoke();
        }
    }

    #region Output

    public event Action OnFailed;
    public event Action OnSuccess;

    #endregion
}
