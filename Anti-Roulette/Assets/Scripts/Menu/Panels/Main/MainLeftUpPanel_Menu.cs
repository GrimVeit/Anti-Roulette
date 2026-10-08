using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLeftUpPanel_Menu : MoveFadePanel
{
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonAvatars;

    public override void Initialize()
    {
        base.Initialize();

        buttonSettings.onClick.AddListener(ClickSettings);
        buttonAvatars.onClick.AddListener(ClickAvatars);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonSettings.onClick.RemoveListener(ClickSettings);
        buttonAvatars.onClick.RemoveListener(ClickAvatars);
    }

    #region Output

    public event Action OnClickSettings;
    public event Action OnClickAvatars;

    private void ClickSettings()
    {
        OnClickSettings?.Invoke();
    }

    private void ClickAvatars()
    {
        OnClickAvatars?.Invoke();
    }

    #endregion
}
