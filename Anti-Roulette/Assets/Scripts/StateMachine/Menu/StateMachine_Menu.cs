using System;
using System.Collections;
using System.Collections.Generic;
using BaCon;
using UnityEngine;

public class StateMachine_Menu : IStateProvider
{
    private readonly Dictionary<Type, IState> states = new();

    private IState _currentState;

    public StateMachine_Menu(DIContainer container)
    {
        var root = container.Resolve<UIRoot_Menu>();

        states[typeof(HoldOnStartState_Menu)] = new HoldOnStartState_Menu(this);

        states[typeof(Registration_CheckAuthorizationState_Menu)] = new Registration_CheckAuthorizationState_Menu(this, container.Resolve<IAuthenticationInfoProvider>(), container.Resolve<IAuthenticationProvider>());
        states[typeof(Registration_NicknameProfileInputState_Menu)] = new Registration_NicknameProfileInputState_Menu(this, container.Resolve<IPlayerProfileEventsProvider>(), root);
        states[typeof(Registration_AvatarState_Menu)] = new Registration_AvatarState_Menu(this, root);
        states[typeof(RegistrationState_Game)] = new RegistrationState_Game(this, container.Resolve<IPlayerProfileInfoProvider>(), container.Resolve<IAuthenticationProvider>(), container.Resolve<IPlayerDatabaseProvider>(), root, container.Resolve<IMoneyProvider>(), container.Resolve<IAuthLoadStatusProvider>());
        states[typeof(Registration_StartMainState_Menu)] = new Registration_StartMainState_Menu(this, container.Resolve<IPlayerDatabaseProvider>(), container.Resolve<IAuthenticationInfoProvider>(), container.Resolve<IPlayerProfileInfoProvider>(), container.Resolve<IDatabaseProvider>(), container.Resolve<IMoneyInfoProvider>());

        states[typeof(MainState_Menu)] = new MainState_Menu(this, root, container.Resolve<ISceneService>());
        states[typeof(SettingsState_Menu)] = new SettingsState_Menu(this, root);
        states[typeof(LeaderboardState_Menu)] = new LeaderboardState_Menu(this, root);
        states[typeof(ShopState_Menu)] = new ShopState_Menu(this, root);
        states[typeof(AvatarState_Menu)] = new AvatarState_Menu(this, root);
    }

    public void Initialize()
    {
        SetState(GetState<HoldOnStartState_Menu>());
    }

    public void Dispose()
    {
        _currentState?.Exit();
        _currentState = null;
    }

    public IState GetState<T>() where T : IState
    {
        return states[typeof(T)];
    }

    public void SetState(IState state)
    {
        _currentState?.Exit();

        _currentState = state;
        _currentState.Enter();
    }
}
