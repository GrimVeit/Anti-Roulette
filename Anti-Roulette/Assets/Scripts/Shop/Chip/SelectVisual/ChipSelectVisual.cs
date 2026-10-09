using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipSelectVisual : MonoBehaviour
{
    public Chip Chip => _chip;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Sprite spriteZero;
    [SerializeField] private Button buttonVisualBuy;
    [SerializeField] private UIEffect effectSelect;
    [SerializeField] private UIEffect effectDeselect;
    [SerializeField] private GameObject objectClose;

    private Chip _chip;

    public void Initialize()
    {
        buttonVisualBuy.onClick.AddListener(Choose);
    }

    public void Dispose()
    {
        buttonVisualBuy.onClick.RemoveListener(Choose);
    }

    public void SetData(Chip chip)
    {
        _chip = chip;
    }

    public void Open()
    {
        objectClose.SetActive(false);
        imageSprite.sprite = _chip.SpriteShop;

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

    public event Action<int> OnSelect;

    private void Choose()
    {
        if (_chip == null) return;

        OnSelect?.Invoke(_chip.Index);
    }

    #endregion
}
