using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChipShopVisual : MonoBehaviour
{
    public Chip Chip => _chip;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Button buttonVisualBuy;
    [SerializeField] private TextMeshProUGUI textPrice;
    [SerializeField] private UIEffect effectOpen;
    [SerializeField] private UIEffect effectClose;

    private Chip _chip;

    public void Initialize()
    {
        buttonVisualBuy.onClick.AddListener(ChooseChip);
    }

    public void Dispose()
    {
        buttonVisualBuy.onClick.RemoveListener(ChooseChip);
    }

    public void SetData(Chip chip)
    {
        _chip = chip;

        imageSprite.sprite = _chip.SpriteShop;
        textPrice.text = _chip.Price.ToString();
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

    public event Action<int> OnBuyChip;

    private void ChooseChip()
    {
        if (_chip == null) return;

        OnBuyChip?.Invoke(_chip.Index);
    }

    #endregion
}
