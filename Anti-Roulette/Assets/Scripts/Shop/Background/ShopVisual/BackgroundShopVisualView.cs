using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundShopVisualView : View
{
    [Header("Reference")]
    [SerializeField] private Transform transformContent;
    [SerializeField] private BackgroundShopVisual backgroundShopVisual_Prefab;

    private Dictionary<int, BackgroundShopVisual> backgroundShopVisuals;

    public void SetBackgrounds(IEnumerable<Background> backgrounds)
    {
        ClearBackgrounds();

        backgroundShopVisuals = new Dictionary<int, BackgroundShopVisual>();

        int index = 0;

        foreach (var background in backgrounds)
        {
            if(background.Index == 0) continue;

            BackgroundShopVisual visual = Instantiate(backgroundShopVisual_Prefab, transformContent);

            visual.transform.localPosition = Vector3.zero;
            visual.OnBuyBackground += BuyBackground;
            visual.Initialize();
            visual.SetData(background);

            if (background.IsOpened)
            {
                visual.Open();
            }
            else
            {
                visual.Close();
            }

            backgroundShopVisuals.Add(background.Index, visual);

            index++;
        }
    }

    public void OpenBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.Open();
    }

    private void ClearBackgrounds()
    {
        if (backgroundShopVisuals == null) return;

        foreach (var visual in backgroundShopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnBuyBackground -= BuyBackground;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        backgroundShopVisuals.Clear();
    }
    private bool TryGetBackgroundShopVisual(int index, out BackgroundShopVisual visual)
    {
        if (backgroundShopVisuals != null &&
            backgroundShopVisuals.TryGetValue(index, out visual))
        {
            return true;
        }

        Debug.LogWarning(
            $"Not found BackgroundShopVisual with Index - {index}"
        );

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnBuy;

    private void BuyBackground(int index)
    {
        OnBuy?.Invoke(index);
    }

    #endregion
}