using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoseState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;

    public LoseState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowMissPanel();
        _sceneRoot.HideMainPanel();

        await UniTask.WaitForSeconds(2.4f);

        _sceneRoot.HideMissPanel();
        _sceneRoot.ShowMainPanel();

        ChangeStateToRouletteClick();

        await UniTask.CompletedTask;
    }

    private void ChangeStateToRouletteClick()
    {
        _stateProvider.SetState(_stateProvider.GetState<HoldClickRouletteState_Game>());
    }
}
