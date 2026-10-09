using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipShopVisualView : View
{
    [Header("Reference")]
    [SerializeField] private Transform transformContent;
    [SerializeField] private ChipShopVisual chipShopVisual_Prefab;

    private Dictionary<int, ChipShopVisual> chipShopVisuals;

    public void SetChips(IEnumerable<Chip> chips)
    {
        ClearChips();

        chipShopVisuals = new Dictionary<int, ChipShopVisual>();

        int index = 0;

        foreach (var chip in chips)
        {
            if (chip.Index == 0) continue;

            ChipShopVisual visual = Instantiate(chipShopVisual_Prefab, transformContent);

            visual.transform.localPosition = Vector3.zero;
            visual.OnBuyChip += BuyChip;
            visual.Initialize();
            visual.SetData(chip);

            if (chip.IsOpened)
            {
                visual.Open();
            }
            else
            {
                visual.Close();
            }

            chipShopVisuals.Add(chip.Index, visual);

            index++;
        }
    }

    public void OpenChip(int index)
    {
        if (!TryGetChipShopVisual(index, out var visual))
            return;

        visual.Open();
    }

    private void ClearChips()
    {
        if (chipShopVisuals == null) return;

        foreach (var visual in chipShopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnBuyChip -= BuyChip;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        chipShopVisuals.Clear();
    }
    private bool TryGetChipShopVisual(int index, out ChipShopVisual visual)
    {
        if (chipShopVisuals != null && chipShopVisuals.TryGetValue(index, out visual))
        {
            return true;
        }

        Debug.LogWarning($"Not found ChipShopVisual with Index - {index}");

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnBuy;

    private void BuyChip(int index)
    {
        OnBuy?.Invoke(index);
    }

    #endregion
}
