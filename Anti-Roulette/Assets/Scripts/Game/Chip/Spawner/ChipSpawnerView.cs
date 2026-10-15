using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpawnerView : View
{
    [SerializeField] private Transform transformSpawn;
    [SerializeField] private ChipVisual chipPrefab;
    [SerializeField] private LayerMask sectorMask;

    private readonly List<ChipVisual> spawnedChips = new();
    public IReadOnlyList<ChipVisual> SpawnedChips => spawnedChips;

    public void Spawn(Chip chip, Vector2 direction, float force)
    {
        ChipVisual chipVisual = Instantiate(chipPrefab, transformSpawn);
        chipVisual.transform.localPosition = Vector3.zero;

        chipVisual.OnFail += Fail;

        chipVisual.Initialize(chip.Sprite);

        spawnedChips.Add(chipVisual);
        chipVisual.Launch(direction, force);
    }

    public void ClearAll()
    {
        foreach (var chip in spawnedChips)
        {
            chip.OnFail -= Fail;
            if (chip != null) Destroy(chip.gameObject);
        }

        spawnedChips.Clear();
    }

    #region Output

    public event Action OnFailed;

    private void Fail()
    {
        OnFailed?.Invoke();
    }

    #endregion
}
