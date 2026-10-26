using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpinState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly IRouletteSpinProvider _rouletteSpinProvider;

    public RouletteSpinState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IRouletteSpinProvider rouletteSpinProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _rouletteSpinProvider = rouletteSpinProvider;
    }

    public void Enter()
    {
        _rouletteSpinProvider.OnStopped += ChangeStateToRoulette;

        _sceneRoot.ShowRoulettePanel();
        _rouletteSpinProvider.Spin(1.7f);
    }

    public void Exit()
    {
        _rouletteSpinProvider.OnStopped -= ChangeStateToRoulette;
    }

    private void ChangeStateToRoulette()
    {
        _stateProvider.SetState(_stateProvider.GetState<RouletteState_Game>());
    }
}
