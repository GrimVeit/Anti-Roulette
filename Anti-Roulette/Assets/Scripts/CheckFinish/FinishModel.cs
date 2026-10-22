using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishModel
{
    public bool IsWin => isWin;

    private bool isWin = false;

    public void Reset()
    {
        isWin = false;

        OnReset?.Invoke();
    }
    public void AddWin()
    {
        isWin = true;

        OnAddWin?.Invoke();
    }

    #region Output

    public event Action OnReset;
    public event Action OnAddWin;

    #endregion
}
