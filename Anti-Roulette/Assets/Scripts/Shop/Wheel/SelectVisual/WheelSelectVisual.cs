using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelSelectVisual : MonoBehaviour
{
    public Wheel Wheel => _wheel;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Sprite spriteZero;
    [SerializeField] private Button buttonVisualBuy;
    [SerializeField] private UIEffect effectSelect;
    [SerializeField] private UIEffect effectDeselect;
    [SerializeField] private GameObject objectClose;

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
    }

    public void Open()
    {
        objectClose.SetActive(false);
        imageSprite.sprite = _wheel.SpriteShop;

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

    private void ChooseWheel()
    {
        if (_wheel == null) return;

        OnSelect?.Invoke(_wheel.Index);
    }

    #endregion
}
