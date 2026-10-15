using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RouletteSector : MonoBehaviour, ISector
{
    [SerializeField] private int number;

    public int Number => number;

    public void SetChip(ChipVisual chip)
    {
        Debug.Log($"Фишка попала в сектор {number}");

        OnSetSector?.Invoke(number);
    }

    #region Output

    public event Action<int> OnSetSector;

    #endregion
}

public interface ISector
{
    void SetChip(ChipVisual chip);
}
