using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSpinPanel_Menu : MovePanel
{
    [SerializeField] private Button buttonSpin;

    public override void Initialize()
    {
        base.Initialize();

        buttonSpin.onClick.AddListener(ClickSpin);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonSpin.onClick.RemoveListener(ClickSpin);
    }

    #region Output

    public event Action OnClickSpin;

    private void ClickSpin()
    {
        OnClickSpin?.Invoke();
    }

    #endregion
}
