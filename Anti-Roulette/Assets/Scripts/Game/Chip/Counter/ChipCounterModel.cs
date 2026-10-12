using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCounterModel
{
    private readonly IChipPseudoListener _chipPseudoListener;
    private readonly int _allCount;

    public ChipCounterModel(IChipPseudoListener chipPseudoListener, int allCount)
    {
        _chipPseudoListener = chipPseudoListener;
        _allCount = allCount;
    }

    public void Initialize()
    {
        _chipPseudoListener.OnCurrentCount += ChangeCount;

        ChangeCount(_allCount);
    }

    public void Dispose()
    {
        _chipPseudoListener.OnCurrentCount -= ChangeCount;
    }

    private void ChangeCount(int currentCount)
    {
        OnChangeCount?.Invoke(currentCount, _allCount);
    }

    #region Output

    public event Action<int, int> OnChangeCount;

    #endregion
}
