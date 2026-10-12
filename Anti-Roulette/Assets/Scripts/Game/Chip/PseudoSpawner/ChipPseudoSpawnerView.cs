using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipPseudoSpawnerView : View
{
    [SerializeField] private Transform transformSpawn;
    [SerializeField] private ChipPseudo chipPseudo_Prefab;
    [SerializeField] private Transform transformPointSpawn;
    [SerializeField] private Transform transformPointStart;

    private readonly List<ChipPseudo> _chips = new();

    public void SpawnChip(Chip chip)
    {
        var chipPseudo = Instantiate(chipPseudo_Prefab, transformSpawn);
        chipPseudo.SetData(chip.Sprite);
        chipPseudo.transform.localPosition = transformPointSpawn.localPosition;
        _chips.Add(chipPseudo);
    }

    public void MoveToStart()
    {
        if (_chips.Count == 0) return;

        OnStartMove?.Invoke();

        var topChip = _chips[^1];
        topChip.MoveTo(transformPointStart, 0.3f, () => OnEndMove?.Invoke());
    }

    public void Delete()
    {
        if (_chips.Count == 0) return;

        var topChip = _chips[^1];
        _chips.RemoveAt(_chips.Count - 1);

        if (topChip != null)
        {
            topChip.KillTween();
            Destroy(topChip.gameObject);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _chips.Count; i++)
        {
            var chip = _chips[i];
            if (chip == null) continue;

            chip.KillTween();
            Destroy(chip.gameObject);
        }

        _chips.Clear();
    }

    #region Output

    public event Action OnStartMove;
    public event Action OnEndMove;

    #endregion
}