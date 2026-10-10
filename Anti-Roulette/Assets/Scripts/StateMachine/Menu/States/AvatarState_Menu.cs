using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public AvatarState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_Avatar += ChangeStateToMain;

        _sceneRoot.ShowAvatarPanel();
        _sceneRoot.ShowBackgroundFadePanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_Avatar -= ChangeStateToMain;

        _sceneRoot.HideAvatarPanel();
        _sceneRoot.HideBackgroundFadePanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
