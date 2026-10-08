using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public ShopState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_ShopHeader += ChangeStateToMain;

        _sceneRoot.ShowShopHeaderPanel();
        _sceneRoot.ShowShopPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_ShopHeader -= ChangeStateToMain;

        _sceneRoot.HideShopHeaderPanel();
        _sceneRoot.HideShopPanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
