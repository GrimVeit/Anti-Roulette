using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registration_NicknameProfileInputState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IPlayerProfileEventsProvider _profileEvents;

    private readonly UIRoot_Menu _sceneRoot;

    public Registration_NicknameProfileInputState_Menu(
        IStateProvider stateProvider,
        IPlayerProfileEventsProvider profileEvents,
        UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _profileEvents = profileEvents;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickRegistrate_Registration += ChangeStateToRegistration;
        _sceneRoot.OnClickSelectAvatar_Registration += ChangeStateToSelectAvatar;

        _sceneRoot.ShowRegistrationPanel();
        _sceneRoot.ShowRegistrationGirlPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickRegistrate_Registration -= ChangeStateToRegistration;
        _sceneRoot.OnClickSelectAvatar_Registration -= ChangeStateToSelectAvatar;
    }

    private void ChangeStateToRegistration()
    {
        _stateProvider.SetState(_stateProvider.GetState<RegistrationState_Game>());
    }

    private void ChangeStateToSelectAvatar()
    {
        _sceneRoot.HideRegistrationGirlPanel();
        _sceneRoot.HideRegistrationPanel();

        _stateProvider.SetState(_stateProvider.GetState<Registration_AvatarState_Menu>());
    }
}
