using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelShopVisual : MonoBehaviour
{
    public Wheel Wheel => _wheel;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Button buttonVisualBuy;
    [SerializeField] private TextMeshProUGUI textPrice;
    [SerializeField] private UIEffect effectOpen;
    [SerializeField] private UIEffect effectClose;

    private Wheel _wheel;

    public void Initialize()
    {
        buttonVisualBuy.onClick.AddListener(ChooseWheel);
    }

    public void Dispose()
    {
        buttonVisualBuy.onClick.RemoveListener(ChooseWheel);
    }

    public void SetData(Wheel wheel)
    {
        _wheel = wheel;

        imageSprite.sprite = _wheel.SpriteShop;
        textPrice.text = _wheel.Price.ToString();
    }

    public void Open()
    {
        if (effectClose.IsActive)
            effectClose.PlayHide();

        effectOpen.PlayShow();
    }

    public void Close()
    {
        if (effectOpen.IsActive)
            effectOpen.PlayHide();

        effectClose.PlayShow();
    }

    #region Output

    public event Action<int> OnBuyWheel;

    private void ChooseWheel()
    {
        if (_wheel == null) return;

        OnBuyWheel?.Invoke(_wheel.Index);
    }

    #endregion
}
