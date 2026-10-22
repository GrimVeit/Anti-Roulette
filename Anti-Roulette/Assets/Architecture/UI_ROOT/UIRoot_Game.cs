using System;
using UnityEngine;

public class UIRoot_Game : UIRoot
{
    [Header("Main")]
    [SerializeField] private MainHeaderPanel_Game mainHeaderPanel;
    [SerializeField] private MainPanel_Game mainPanel;
    [SerializeField] private RoulettePanel_Game roulettePanel;
    [SerializeField] private NumberPresentationPanel_Game numberPresentationPanel;
    [SerializeField] private HitPanel_Game hitPanel;
    [SerializeField] private MissPanel_Game missPanel;

    [Header("Finish")]
    [SerializeField] private FadePanel finishBackgroundPanel;
    [SerializeField] private FinishButtonsPanel_Game finishButtonsPanel;
    [SerializeField] private FinishGirlPanel_Game finishGirlPanel;
    [SerializeField] private FinishLinePanel_Game finishLinePanel;

    public override void Initialize()
    {
        ActivateEvents();

        mainHeaderPanel.Initialize();
        mainPanel.Initialize();
        roulettePanel.Initialize();
        numberPresentationPanel.Initialize();
        hitPanel.Initialize();
        missPanel.Initialize();

        finishBackgroundPanel.Initialize();
        finishButtonsPanel.Initialize();
        finishGirlPanel.Initialize();
        finishLinePanel.Initialize();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        HideHitPanel();
        HideMainHeaderPanel();
        HideMainPanel();
        HideMissPanel();
        HideNumberPresentationPanel();
        HideRoulettePanel();
        HideFinishBackgroundPanel();
        HideFinishButtonsPanel();
        HideFinishGirlPanel();
        HideFinishLinePanel();

        mainHeaderPanel.Dispose();
        mainPanel.Dispose();
        roulettePanel.Dispose();
        numberPresentationPanel.Dispose();
        hitPanel.Dispose();
        missPanel.Dispose();

        finishBackgroundPanel.Dispose();
        finishButtonsPanel.Dispose();
        finishGirlPanel.Dispose();
        finishLinePanel.Dispose();
    }

    private void ActivateEvents()
    {
        mainHeaderPanel.OnClickExit += ClickExit_MainHeader;
        mainHeaderPanel.OnClickRoulette += ClickRoulette_MainHeader;

        finishButtonsPanel.OnClickExit += ClickExit_FinishButtons;
        finishButtonsPanel.OnClickRestart += ClickRestart_FinishButtons;
    }

    private void DeactivateEvents()
    {
        mainHeaderPanel.OnClickExit -= ClickExit_MainHeader;
        mainHeaderPanel.OnClickRoulette -= ClickRoulette_MainHeader;

        finishButtonsPanel.OnClickExit -= ClickExit_FinishButtons;
        finishButtonsPanel.OnClickRestart -= ClickRestart_FinishButtons;
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




    public void ShowMainPanel()
    {
        ShowPanel(mainPanel);
    }

    public void HideMainPanel()
    {
        HidePanel(mainPanel);
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





    public void ShowFinishBackgroundPanel()
    {
        ShowPanel(finishBackgroundPanel);
    }

    public void HideFinishBackgroundPanel()
    {
        HidePanel(finishBackgroundPanel);
    }


    public void ShowFinishButtonsPanel()
    {
        ShowPanel(finishButtonsPanel);
    }

    public void HideFinishButtonsPanel()
    {
        HidePanel(finishButtonsPanel);
    }


    public void ShowFinishGirlPanel()
    {
        ShowPanel(finishGirlPanel);
    }

    public void HideFinishGirlPanel()
    {
        HidePanel(finishGirlPanel);
    }


    public void ShowFinishLinePanel()
    {
        ShowPanel(finishLinePanel);
    }

    public void HideFinishLinePanel()
    {
        HidePanel(finishLinePanel);
    }

    #endregion



    #region Output

    public event Action OnClickExit_MainHeader;
    public event Action OnClickRoulette_MainHeader;

    private void ClickExit_MainHeader()
    {
        OnClickExit_MainHeader?.Invoke();
    }

    private void ClickRoulette_MainHeader()
    {
        OnClickRoulette_MainHeader?.Invoke();
    }



    public event Action OnClickExit_FinishButtons;
    public event Action OnClickRestart_FinishButtons;

    private void ClickExit_FinishButtons()
    {
        OnClickExit_FinishButtons?.Invoke();
    }

    private void ClickRestart_FinishButtons()
    {
        OnClickRestart_FinishButtons?.Invoke();
    }

    #endregion
}
