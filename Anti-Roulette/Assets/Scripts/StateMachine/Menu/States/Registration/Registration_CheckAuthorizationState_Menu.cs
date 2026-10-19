using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public sealed class Registration_CheckAuthorizationState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IAuthenticationInfoProvider _authenticationInfo;

    public Registration_CheckAuthorizationState_Menu(
        IStateProvider stateProvider,
        IAuthenticationInfoProvider authenticationInfo)
    {
        _stateProvider = stateProvider;
        _authenticationInfo = authenticationInfo;
    }

    public void Enter()
    {
        if (_authenticationInfo.IsAuthorized)
            _stateProvider.SetState(_stateProvider.GetState<Registration_StartMainState_Menu>());
        else
            _stateProvider.SetState(_stateProvider.GetState<Registration_NicknameProfileInputState_Menu>());
    }

    public void Exit()
    {

    }
}
