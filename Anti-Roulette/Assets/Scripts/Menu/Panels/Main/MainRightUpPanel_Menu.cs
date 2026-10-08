using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainRightUpPanel_Menu : MoveFadePanel
{
    [SerializeField] private Button buttonLeaders;
    [SerializeField] private Button buttonShop;

    public override void Initialize()
    {
        base.Initialize();

        buttonLeaders.onClick.AddListener(ClickLeaders);
        buttonShop.onClick.AddListener(ClickShop);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonLeaders.onClick.RemoveListener(ClickLeaders);
        buttonShop.onClick.RemoveListener(ClickShop);
    }

    #region Output

    public event Action OnClickLeaders;
    public event Action OnClickShop;

    private void ClickLeaders()
    {
        OnClickLeaders?.Invoke();
    }

    private void ClickShop()
    {
        OnClickShop?.Invoke();
    }

    #endregion
}
