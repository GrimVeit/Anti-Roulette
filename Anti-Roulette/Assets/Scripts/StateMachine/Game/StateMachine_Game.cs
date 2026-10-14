using System;
using System.Collections.Generic;
using BaCon;
using UnityEngine;

public class StateMachine_Game : IStateProvider
{
    private readonly Dictionary<Type, IState> states = new();

    private IState _currentState;

    public StateMachine_Game(DIContainer container)
    {
        var root = container.Resolve<UIRoot_Game>();
        var sceneService = container.Resolve<ISceneService>();
        var soundProvider = container.Resolve<ISoundProvider>();

        states[typeof(HoldOnStartState_Game)] = new HoldOnStartState_Game(this, root);

        states[typeof(RouletteState_Game)] = new RouletteState_Game(this, root, container.Resolve<IStoreNumberSectorProvider>());
        states[typeof(NumberPresentationState_Game)] = new NumberPresentationState_Game(this, root);
        states[typeof(MoveChipToStartState_Game)] = new MoveChipToStartState_Game(this, root, container.Resolve<IChipPseudoProvider>(), container.Resolve<INumberSectorHighlightProvider>());
        states[typeof(MainState_Game)] = new MainState_Game(this, sceneService, root, container.Resolve<IChipSpawnerProvider>(), container.Resolve<IChipArrowInfo>(), container.Resolve<IChipSliderListen>(), container.Resolve<IChipSliderActivator>(), container.Resolve<IChipArrowActivator>(), container.Resolve<IChipPseudoProvider>());
    }

    public void Initialize()
    {
        SetState(GetState<RouletteState_Game>());
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
