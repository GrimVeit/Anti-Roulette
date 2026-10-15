using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorTriggerView : View
{
    [SerializeField] private List<RouletteSector> rouletteSectors = new();

    public void Initialize()
    {
        for (int i = 0; i < rouletteSectors.Count; i++)
        {
            rouletteSectors[i].OnSetSector += SetSector;
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < rouletteSectors.Count; i++)
        {
            rouletteSectors[i].OnSetSector -= SetSector;
        }
    }

    #region Output

    public event Action<int> OnSetSector;

    private void SetSector(int number)
    {
        OnSetSector?.Invoke(number);
    }

    #endregion
}
