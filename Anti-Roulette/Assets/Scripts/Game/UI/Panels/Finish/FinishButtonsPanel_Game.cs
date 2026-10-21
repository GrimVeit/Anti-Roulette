using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishButtonsPanel_Game : MoveFadePanel
{
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonRestart;

    public override void Initialize()
    {
        base.Initialize();

        buttonExit.onClick.AddListener(ClickExit);
        buttonRestart.onClick.AddListener(ClickRestart);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonExit.onClick.RemoveListener(ClickExit);
        buttonRestart.onClick.RemoveListener(ClickRestart);
    }

    #region Output

    public event Action OnClickExit;
    public event Action OnClickRestart;

    private void ClickExit()
    {
        OnClickExit?.Invoke();
    }

    private void ClickRestart()
    {
        OnClickRestart?.Invoke();
    }

    #endregion
}
