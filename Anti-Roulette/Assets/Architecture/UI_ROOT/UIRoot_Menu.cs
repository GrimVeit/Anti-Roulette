using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot_Menu : UIRoot
{
    [SerializeField] private BackgroundFadePanel_Menu backgroundFadePanel;

    [Header("Main")]
    [SerializeField] private MainLeftUpPanel_Menu mainLeftUpPanel;
    [SerializeField] private MainRightUpPanel_Menu mainRightUpPanel;
    [SerializeField] private MainSpinPanel_Menu mainSpinPanel;

    [Header("Leaderbaord")]
    [SerializeField] private LeaderboardHeaderPanel_Menu leaderboardHeaderPanel;
    [SerializeField] private LeaderboardPanel leaderboardPanel;

    [Header("Shop")]
    [SerializeField] private ShopHeaderPanel_Menu shopHeaderPanel;
    [SerializeField] private ShopPanel_Menu shopPanel;

    [Header("Settings")]
    [SerializeField] private SettingsPanel_Menu settingsPanel;

    public override void Initialize()
    {
        base.Initialize();

        ActivateEvents();

        backgroundFadePanel.Initialize();

        mainLeftUpPanel.Initialize();
        mainRightUpPanel.Initialize();
        mainSpinPanel.Initialize();

        leaderboardHeaderPanel.Initialize();
        leaderboardPanel.Initialize();

        shopHeaderPanel.Initialize();
        shopPanel.Initialize();

        settingsPanel.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        DeactivateEvents();

        backgroundFadePanel.Dispose();

        mainLeftUpPanel.Dispose();
        mainRightUpPanel.Dispose();
        mainSpinPanel.Dispose();

        leaderboardHeaderPanel.Dispose();
        leaderboardPanel.Dispose();

        shopHeaderPanel.Dispose();
        shopPanel.Dispose();

        settingsPanel.Dispose();
    }

    private void ActivateEvents()
    {
        mainLeftUpPanel.OnClickAvatars += ClickAvatars_MainLeftUp;
        mainLeftUpPanel.OnClickSettings += ClickSettings_MainLeftUp;

        mainRightUpPanel.OnClickLeaders += ClickLeaders_MainRightUp;
        mainRightUpPanel.OnClickShop += ClickShop_MainRightUp;

        mainSpinPanel.OnClickSpin += ClickPlay_MainSpin;


        leaderboardHeaderPanel.OnClickExit += ClickExit_LeaderbaordHeader;


        shopHeaderPanel.OnClickExit += ClickExit_ShopHeader;


        settingsPanel.OnClickExit_Settings += ClickExit_Settings;
    }

    private void DeactivateEvents()
    {
        mainLeftUpPanel.OnClickAvatars -= ClickAvatars_MainLeftUp;
        mainLeftUpPanel.OnClickSettings -= ClickSettings_MainLeftUp;

        mainRightUpPanel.OnClickLeaders -= ClickLeaders_MainRightUp;
        mainRightUpPanel.OnClickShop -= ClickShop_MainRightUp;

        mainSpinPanel.OnClickSpin -= ClickPlay_MainSpin;


        leaderboardHeaderPanel.OnClickExit -= ClickExit_LeaderbaordHeader;


        shopHeaderPanel.OnClickExit -= ClickExit_ShopHeader;


        settingsPanel.OnClickExit_Settings -= ClickExit_Settings;
    }

    #region Input

    public void ShowBackgroundFadePanel()
    {
        ShowPanel(backgroundFadePanel);
    }

    public void HideBackgroundFadePanel()
    {
        HidePanel(backgroundFadePanel);
    }




    public void ShowMainLeftUpPanel()
    {
        ShowPanel(mainLeftUpPanel);
    }

    public void HideMainLeftUpPanel()
    {
        HidePanel(mainLeftUpPanel);
    }



    public void ShowMainRightUpPanel()
    {
        ShowPanel(mainRightUpPanel);
    }

    public void HideMainRightUpPanel()
    {
        HidePanel(mainRightUpPanel);
    }



    public void ShowMainSpinPanel()
    {
        ShowPanel(mainSpinPanel);
    }

    public void HideMainSpinPanel()
    {
        HidePanel(mainSpinPanel);
    }









    public void ShowLeaderboardHeaderPanel()
    {
        ShowPanel(leaderboardHeaderPanel);
    }

    public void HideLeaderboardHeaderPanel()
    {
        HidePanel(leaderboardHeaderPanel);
    }


    public void ShowLeaderboardPanel()
    {
        ShowPanel(leaderboardPanel);
    }

    public void HideLeaderbaordPanel()
    {
        HidePanel(leaderboardPanel);
    }








    public void ShowShopHeaderPanel()
    {
        ShowPanel(shopHeaderPanel);
    }

    public void HideShopHeaderPanel()
    {
        HidePanel(shopHeaderPanel);
    }

    public void ShowShopPanel()
    {
        ShowPanel(shopPanel);
    }

    public void HideShopPanel()
    {
        HidePanel(shopPanel);
    }





    public void ShowSettingsPanel()
    {
        ShowPanel(settingsPanel);
    }

    public void HideSettingsPanel()
    {
        HidePanel(settingsPanel);
    }

    #endregion

    #region Output

    #region MAIN

    public event Action OnClickSettings_MainLeftUp;
    public event Action OnClickAvatars_MainLeftUp;

    private void ClickSettings_MainLeftUp()
    {
        OnClickSettings_MainLeftUp?.Invoke();
    }

    private void ClickAvatars_MainLeftUp()
    {
        OnClickAvatars_MainLeftUp?.Invoke();
    }

    //-----------------------------------------------------//

    public event Action OnClickLeaders_MainRightUp;
    public event Action OnClickShop_MainRightUp;

    private void ClickLeaders_MainRightUp()
    {
        OnClickLeaders_MainRightUp?.Invoke();
    }

    private void ClickShop_MainRightUp()
    {
        OnClickShop_MainRightUp?.Invoke();
    }

    //-----------------------------------------------------//

    public event Action OnClickPlay_MainSpin;

    private void ClickPlay_MainSpin()
    {
        OnClickPlay_MainSpin?.Invoke();
    }

    #endregion





    #region LEADERS

    public event Action OnClickExit_LeaderboardHeader;

    private void ClickExit_LeaderbaordHeader()
    {
        OnClickExit_LeaderboardHeader?.Invoke();
    }

    #endregion



    #region SHOP

    public event Action OnClickExit_ShopHeader;

    private void ClickExit_ShopHeader()
    {
        OnClickExit_ShopHeader?.Invoke();
    }

    #endregion



    #region SHOP

    public event Action OnClickExit_Settings;

    private void ClickExit_Settings()
    {
        OnClickExit_Settings?.Invoke();
    }

    #endregion

    #endregion
}
