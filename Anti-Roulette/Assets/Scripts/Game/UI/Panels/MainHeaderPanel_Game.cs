using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHeaderPanel_Game : MoveFadePanel
{
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonRoulette;

    public override void Initialize()
    {
        base.Initialize();

        buttonExit.onClick.AddListener(ClickExit);
        buttonRoulette.onClick.AddListener(ClickRoulette);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonExit.onClick.RemoveListener(ClickExit);
        buttonRoulette.onClick.RemoveListener(ClickRoulette);
    }

    #region Output

    public event Action OnClickExit;
    public event Action OnClickRoulette;

    private void ClickExit()
    {
        OnClickExit?.Invoke();
    }

    private void ClickRoulette()
    {
        OnClickRoulette?.Invoke();
    }

    #endregion
}
