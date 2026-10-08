using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopTypeVisualView : View
{
    [SerializeField] private List<ShopTypeVisual> shopTypeVisuals = new();

    [Header("Buttons")]
    [SerializeField] private Color colorOpen;
    [SerializeField] private Color colorClose;
    [SerializeField] private float durationColor;

    private readonly Dictionary<ShopType, ShopTypeVisual> shopVisuals = new();

    public void Initialize()
    {
        for (int i = 0; i < shopTypeVisuals.Count; i++)
        {
            shopVisuals.Add(shopTypeVisuals[i].Type, shopTypeVisuals[i]);
        }

        foreach (var visual in shopVisuals.Values)
        {
            visual.OnClickType += ClickType;
            visual.Initialize();
        }
    }

    public void Dispose()
    {
        foreach (var visual in shopVisuals.Values)
        {
            visual.OnClickType -= ClickType;
            visual.Dispose();
        }

        shopTypeVisuals.Clear();
        shopVisuals.Clear();
    }

    public void Open(ShopType type)
    {
        if(TryGetShopTypeVisual(type, out var visual))
        {
            visual.Open(colorOpen, durationColor);
        }
    }

    public void Close(ShopType type)
    {
        if (TryGetShopTypeVisual(type, out var visual))
        {
            visual.Close(colorClose, durationColor);
        }
    }

    private bool TryGetShopTypeVisual(ShopType type, out ShopTypeVisual visual)
    {
        if (shopVisuals != null && shopVisuals.TryGetValue(type, out visual))
        {
            return true;
        }

        Debug.LogWarning($"Not found BackgroundShopVisual with ShopType - {type}");

        visual = null;
        return false;
    }

    #region Output

    public event Action<ShopType> OnClickType;

    private void ClickType(ShopType type)
    {
        OnClickType?.Invoke(type);
    }

    #endregion

    [Serializable]
    private class ShopTypeVisual
    {
        public ShopType Type => type;

        [SerializeField] private ShopType type;
        [SerializeField] private Button button;
        [SerializeField] private Image imageButton;
        [SerializeField] private Panel panel;

        private Tween tweenColor;

        public void Initialize()
        {
            button.onClick.AddListener(ClickType);

            panel.Initialize();
        }

        public void Dispose()
        {
            button.onClick.RemoveListener(ClickType);

            panel.Dispose();
        }

        public void Open(Color color, float time)
        {
            if(panel.IsOpen) return;

            tweenColor?.Kill();

            panel.Show();
            tweenColor = imageButton.DOColor(color, time);
        }

        public void Close(Color color, float time)
        {
            if (!panel.IsOpen) return;

            tweenColor?.Kill();

            panel.Hide();
            tweenColor = imageButton.DOColor(color, time);
        }

        #region Output

        public event Action<ShopType> OnClickType;

        private void ClickType()
        {
            OnClickType?.Invoke(type);
        }

        #endregion
    }

}

public enum ShopType
{
    Table, Wheel, Chip
}


