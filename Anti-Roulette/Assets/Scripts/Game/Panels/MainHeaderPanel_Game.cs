using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHeaderPanel_Game : MoveFadePanel
{
    [SerializeField] private Button buttonExit;

    public override void Initialize()
    {
        base.Initialize();

        buttonExit.onClick.AddListener(ClickExit);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonExit.onClick.RemoveListener(ClickExit);
    }

    #region Output

    public event Action OnClickExit;

    private void ClickExit()
    {
        OnClickExit?.Invoke();
    }

    #endregion
}
