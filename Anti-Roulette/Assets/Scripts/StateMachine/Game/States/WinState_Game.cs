using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WinState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;

    public WinState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowHitPanel();
        _sceneRoot.HideMainPanel();

        await UniTask.WaitForSeconds(4f);

        _sceneRoot.HideHitPanel();
        _sceneRoot.ShowMainPanel();

        ChangeStateToRouletteClick();

        await UniTask.CompletedTask;
    }

    private void ChangeStateToRouletteClick()
    {
        _stateProvider.SetState(_stateProvider.GetState<HoldClickRouletteState_Game>());
    }
}
