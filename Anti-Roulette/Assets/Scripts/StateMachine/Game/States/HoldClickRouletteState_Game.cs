using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldClickRouletteState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly IRouletteHighlightActivator _highlightActivator;

    public HoldClickRouletteState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IRouletteHighlightActivator highlightActivator)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _highlightActivator = highlightActivator;
    }

    public void Enter()
    {
        _sceneRoot.OnClickRoulette_MainHeader += ChangeStateToRoulette;

        _highlightActivator.Activate();
    }

    public void Exit()
    {
        _sceneRoot.OnClickRoulette_MainHeader -= ChangeStateToRoulette;

        _highlightActivator.Deactivate();

        _sceneRoot.HideMainPanel();
        _sceneRoot.HideMainHeaderPanel();
    }

    private void ChangeStateToRoulette()
    {
        _stateProvider.SetState(_stateProvider.GetState<RouletteSpinState_Game>());
    }
}
