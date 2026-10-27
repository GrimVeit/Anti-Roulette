using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTypeVisualModel
{
    private ShopType _currentShopType = ShopType.Chip;

    private readonly ISoundProvider _soundProvider;

    public ShopTypeVisualModel(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        OnOpenType?.Invoke(_currentShopType);
    }

    public void ClickType(ShopType shopType)
    {
        if(_currentShopType == shopType) return;

        _soundProvider.PlayOneShot("Click_Popup");

        OnCloseType?.Invoke(_currentShopType);

        _currentShopType = shopType;
        OnOpenType?.Invoke(_currentShopType);
    }

    #region Output

    public event Action<ShopType> OnOpenType;
    public event Action<ShopType> OnCloseType;

    #endregion
}
