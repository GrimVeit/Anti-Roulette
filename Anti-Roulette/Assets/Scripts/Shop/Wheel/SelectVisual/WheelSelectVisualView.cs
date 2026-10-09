using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSelectVisualView : View
{
    [Header("Reference")]
    [SerializeField] private Transform transformContent;
    [SerializeField] private WheelSelectVisual shopVisual_Prefab;

    private Dictionary<int, WheelSelectVisual> shopVisuals;

    public void SetBackgrounds(IEnumerable<Wheel> wheels)
    {
        ClearBackgrounds();

        shopVisuals = new Dictionary<int, WheelSelectVisual>();

        int index = 0;

        foreach (var wheel in wheels)
        {
            WheelSelectVisual visual = Instantiate(shopVisual_Prefab, transformContent);

            visual.transform.localPosition = Vector3.zero;
            visual.OnSelect += SelectVis;
            visual.Initialize();
            visual.SetData(wheel);

            if (wheel.IsOpened)
            {
                visual.Open();
            }
            else
            {
                visual.Close();
            }

            shopVisuals.Add(wheel.Index, visual);

            index++;
        }
    }

    public void Open(int index)
    {
        if (!TryGetShopVisual(index, out var visual))
            return;

        visual.Open();
    }

    public void Select(int index)
    {
        if (!TryGetShopVisual(index, out var visual))
            return;

        visual.Select();
    }

    public void Deselect(int index)
    {
        if (!TryGetShopVisual(index, out var visual))
            return;

        visual.Deselect();
    }

    private void ClearBackgrounds()
    {
        if (shopVisuals == null) return;

        foreach (var visual in shopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnSelect -= SelectVis;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        shopVisuals.Clear();
    }
    private bool TryGetShopVisual(int index, out WheelSelectVisual visual)
    {
        if (shopVisuals != null && shopVisuals.TryGetValue(index, out visual))
        {
            return true;
        }

        Debug.LogWarning( $"Not found WheelShopVisual with Index - {index}");

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnSelect;

    private void SelectVis(int index)
    {
        OnSelect?.Invoke(index);
    }

    #endregion
}
