using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel_Menu : MoveFadePanel
{
    [SerializeField] private Button buttonExit;

    public override void Initialize()
    {
        base.Initialize();

        buttonExit.onClick.AddListener(ClickSettings);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonExit.onClick.RemoveListener(ClickSettings);
    }

    #region Output

    public event Action OnClickExit_Settings;

    private void ClickSettings()
    {
        OnClickExit_Settings?.Invoke();
    }

    #endregion
}
