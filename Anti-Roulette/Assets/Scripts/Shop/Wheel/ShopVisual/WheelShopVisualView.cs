using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelShopVisualView : View
{
    [Header("Reference")]
    [SerializeField] private Transform transformContent;
    [SerializeField] private WheelShopVisual wheelShopVisual_Prefab;

    private Dictionary<int, WheelShopVisual> wheelShopVisuals;

    public void SetWheels(IEnumerable<Wheel> wheels)
    {
        ClearWheels();

        wheelShopVisuals = new Dictionary<int, WheelShopVisual>();

        int index = 0;

        foreach (var wheel in wheels)
        {
            if (wheel.Index == 0) continue;

            WheelShopVisual visual = Instantiate(wheelShopVisual_Prefab, transformContent);

            visual.transform.localPosition = Vector3.zero;
            visual.OnBuyWheel += BuyWheel;
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

            wheelShopVisuals.Add(wheel.Index, visual);

            index++;
        }
    }

    public void OpenWheel(int index)
    {
        if (!TryGetWheelShopVisual(index, out var visual))
            return;

        visual.Open();
    }

    private void ClearWheels()
    {
        if (wheelShopVisuals == null) return;

        foreach (var visual in wheelShopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnBuyWheel -= BuyWheel;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        wheelShopVisuals.Clear();
    }
    private bool TryGetWheelShopVisual(int index, out WheelShopVisual visual)
    {
        if (wheelShopVisuals != null && wheelShopVisuals.TryGetValue(index, out visual))
        {
            return true;
        }

        Debug.LogWarning($"Not found WheelShopVisual with Index - {index}");

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnBuy;

    private void BuyWheel(int index)
    {
        OnBuy?.Invoke(index);
    }

    #endregion
}
