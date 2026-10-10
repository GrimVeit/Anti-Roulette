using System;
using UnityEngine;

public class UIRoot_Game : UIRoot
{
    [Header("Main")]
    [SerializeField] private MainHeaderPanel_Game mainHeaderPanel;

    public override void Initialize()
    {
        ActivateEvents();

        mainHeaderPanel.Initialize();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        mainHeaderPanel.Dispose();
    }

    private void ActivateEvents()
    {
        mainHeaderPanel.OnClickExit += ClickExit_MainHeader;
    }

    private void DeactivateEvents()
    {
        mainHeaderPanel.OnClickExit -= ClickExit_MainHeader;
    }

    #region Input

    public void ShowMainHeaderPanel()
    {
        ShowPanel(mainHeaderPanel);
    }

    public void HideMainHeaderPanel()
    {
        HidePanel(mainHeaderPanel);
    }

    #endregion

    #region Output

    public event Action OnClickExit_MainHeader;

    private void ClickExit_MainHeader()
    {
        OnClickExit_MainHeader?.Invoke();
    }

    #endregion
}
