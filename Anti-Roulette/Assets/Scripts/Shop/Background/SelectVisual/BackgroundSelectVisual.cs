using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSelectVisual : MonoBehaviour
{
    public Background Background => _background;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Sprite spriteZero;
    [SerializeField] private Button buttonVisualBuy;
    [SerializeField] private UIEffect effectSelect;
    [SerializeField] private UIEffect effectDeselect;
    [SerializeField] private GameObject objectClose;

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
    }

    public void Open()
    {
        objectClose.SetActive(false);
        imageSprite.sprite = _background.SpriteShop;

        Deselect();
    }

    public void Close()
    {
        objectClose.SetActive(true);
        imageSprite.sprite = spriteZero;

        effectDeselect.PlayHide();
        effectSelect.PlayHide();
    }

    public void Deselect()
    {
        if (effectSelect.IsActive)
            effectSelect.PlayHide();

        effectDeselect.PlayShow();
    }

    public void Select()
    {
        if (effectDeselect.IsActive)
            effectDeselect.PlayHide();

        effectSelect.PlayShow();
    }

    #region Output

    public event Action<int> OnBuyBackground;

    private void ChooseBackground()
    {
        if (_background == null) return;

        OnBuyBackground?.Invoke(_background.Index);
    }

    #endregion
}
