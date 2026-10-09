using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundShopVisual : MonoBehaviour
{
    public Background Background => _background;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Button buttonVisualBuy;
    [SerializeField] private TextMeshProUGUI textPrice;
    [SerializeField] private UIEffect effectOpen;
    [SerializeField] private UIEffect effectClose;

    private Background _background;

    public void Initialize()
    {
        buttonVisualBuy.onClick.AddListener(ChooseBackground);
    }

    public void Dispose()
    {
        buttonVisualBuy.onClick.RemoveListener(ChooseBackground);
    }

    public void SetData(Background background)
    {
        _background = background;

        imageSprite.sprite = _background.SpriteShop;
        textPrice.text = _background.Price.ToString();
    }

    public void Open()
    {
        if(effectClose.IsActive)
            effectClose.PlayHide();

        effectOpen.PlayShow();
    }

    public void Close()
    {
        if(effectOpen.IsActive)
            effectOpen.PlayHide();

        effectClose.PlayShow();
    }

    #region Output

    public event Action<int> OnBuyBackground;

    private void ChooseBackground()
    {
        if(_background == null) return;

        OnBuyBackground?.Invoke(_background.Index);
    }

    #endregion
}
