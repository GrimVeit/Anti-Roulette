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

        states[typeof(RouletteSpinState_Game)] = new RouletteSpinState_Game(this, root, container.Resolve<IRouletteSpinProvider>());
        states[typeof(RouletteState_Game)] = new RouletteState_Game(this, container.Resolve<IStoreNumberSectorProvider>());
        states[typeof(NumberPresentationState_Game)] = new NumberPresentationState_Game(this, root, sceneService);
        states[typeof(MoveChipToStartState_Game)] = new MoveChipToStartState_Game(this, root, container.Resolve<IChipPseudoProvider>(), container.Resolve<INumberSectorHighlightProvider>(), sceneService);
        states[typeof(MainState_Game)] = new MainState_Game(this, sceneService, root, container.Resolve<IChipSpawnerProvider>(), container.Resolve<IChipArrowInfo>(), container.Resolve<IChipSliderListen>(), container.Resolve<IChipSliderActivator>(), container.Resolve<IChipArrowActivator>(), container.Resolve<IChipPseudoProvider>());
        states[typeof(ChipMoveState_Game)] = new ChipMoveState_Game(this, container.Resolve<IChipSpawnerListener>(), container.Resolve<INumberSectorTriggerListener>(), root, sceneService);
        states[typeof(LoseChipState_Game)] = new LoseChipState_Game(this, root, sceneService);
        states[typeof(WinChipState_Game)] = new WinChipState_Game(this, root, container.Resolve<IMoneyProvider>(), sceneService, container.Resolve<IFinishProvider>());
        states[typeof(CheckFinishState_Game)] = new CheckFinishState_Game(this, container.Resolve<IChipCounterInfo>(), container.Resolve<IFinishInfo>());
        states[typeof(HoldClickRouletteState_Game)] = new HoldClickRouletteState_Game(this, root, container.Resolve<IRouletteHighlightActivator>(), sceneService);
        states[typeof(LoseState_Game)] = new LoseState_Game(this, root, sceneService, container.Resolve<IFinishProvider>(), container.Resolve<IChipPseudoProvider>());
        states[typeof(WinState_Game)] = new WinState_Game(this, root, sceneService, container.Resolve<IFinishProvider>(), container.Resolve<IChipPseudoProvider>());
    }

    public void Initialize()
    {
        SetState(GetState<HoldOnStartState_Game>());
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
