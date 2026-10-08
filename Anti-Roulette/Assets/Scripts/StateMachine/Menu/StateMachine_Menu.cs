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

        //states[typeof(HoldOnStartState_Menu)] = new HoldOnStartState_Menu(this, sessionInfo, root);
        states[typeof(MainState_Menu)] = new MainState_Menu(this, root, container.Resolve<ISceneService>());
        states[typeof(SettingsState_Menu)] = new SettingsState_Menu(this, root);
        states[typeof(LeaderboardState_Menu)] = new LeaderboardState_Menu(this, root);
        states[typeof(ShopState_Menu)] = new ShopState_Menu(this, root);
    }

    public void Initialize()
    {
        SetState(GetState<MainState_Menu>());
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
