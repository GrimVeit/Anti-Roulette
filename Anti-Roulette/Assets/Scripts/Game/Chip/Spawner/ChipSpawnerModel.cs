using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpawnerModel
{
    private readonly IChipInfoProvider _chipInfoProvider;

    public ChipSpawnerModel(IChipInfoProvider chipInfoProvider)
    {
        _chipInfoProvider = chipInfoProvider;
    }

    public void Spawn(Vector2 direction, float power)
    {
        OnSpawn?.Invoke(_chipInfoProvider.GetCurrentChip(), direction, power);
    }

    #region Output

    public event Action<Chip, Vector2, float> OnSpawn;

    #endregion
}
