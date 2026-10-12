using System;
using UnityEngine;

public class UIRoot_Game : UIRoot
{
    [Header("Main")]
    [SerializeField] private MainHeaderPanel_Game mainHeaderPanel;
    [SerializeField] private RoulettePanel_Game roulettePanel;
    [SerializeField] private NumberPresentationPanel_Game numberPresentationPanel;
    [SerializeField] private HitPanel_Game hitPanel;
    [SerializeField] private MissPanel_Game missPanel;

    public override void Initialize()
    {
        ActivateEvents();

        mainHeaderPanel.Initialize();
        roulettePanel.Initialize();
        numberPresentationPanel.Initialize();
        hitPanel.Initialize();
        missPanel.Initialize();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        mainHeaderPanel.Dispose();
        roulettePanel.Dispose();
        numberPresentationPanel.Dispose();
        hitPanel.Dispose();
        missPanel.Dispose();
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



    public void ShowRoulettePanel()
    {
        ShowPanel(roulettePanel);
    }

    public void HideRoulettePanel()
    {
        HidePanel(roulettePanel);
    }



    public void ShowNumberPresentationPanel()
    {
        ShowPanel(numberPresentationPanel);
    }

    public void HideNumberPresentationPanel()
    {
        HidePanel(numberPresentationPanel);
    }



    public void ShowHitPanel()
    {
        ShowPanel(hitPanel);
    }

    public void HideHitPanel()
    {
        HidePanel(hitPanel);
    }



    public void ShowMissPanel()
    {
        ShowPanel(missPanel);
    }

    public void HideMissPanel()
    {
        HidePanel(missPanel);
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
