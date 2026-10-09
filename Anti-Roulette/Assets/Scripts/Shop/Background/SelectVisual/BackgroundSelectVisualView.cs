using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelectVisualView : View
{
    [Header("Reference")]
    [SerializeField] private Transform transformContent;
    [SerializeField] private BackgroundSelectVisual backgroundShopVisual_Prefab;

    private Dictionary<int, BackgroundSelectVisual> backgroundShopVisuals;

    public void SetBackgrounds(IEnumerable<Background> backgrounds)
    {
        ClearBackgrounds();

        backgroundShopVisuals = new Dictionary<int, BackgroundSelectVisual>();

        int index = 0;

        foreach (var background in backgrounds)
        {
            BackgroundSelectVisual visual = Instantiate(backgroundShopVisual_Prefab, transformContent);

            visual.transform.localPosition = Vector3.zero;
            visual.OnBuyBackground += Select;
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

    public void SelectBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.Select();
    }

    public void DeselectBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.Deselect();
    }

    private void ClearBackgrounds()
    {
        if (backgroundShopVisuals == null) return;

        foreach (var visual in backgroundShopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnBuyBackground -= Select;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        backgroundShopVisuals.Clear();
    }
    private bool TryGetBackgroundShopVisual(int index, out BackgroundSelectVisual visual)
    {
        if (backgroundShopVisuals != null && backgroundShopVisuals.TryGetValue(index, out visual))
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

    public event Action<int> OnSelect;

    private void Select(int index)
    {
        OnSelect?.Invoke(index);
    }

    #endregion
}
