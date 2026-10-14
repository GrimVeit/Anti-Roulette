using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class NumberPresentationState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;

    public NumberPresentationState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowNumberPresentationPanel();

        await UniTask.WaitForSeconds(3.8f, cancellationToken: token);

        _sceneRoot.HideRoulettePanel();
        _sceneRoot.HideNumberPresentationPanel();

        ChangeStateToMain();

        await UniTask.CompletedTask;
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MoveChipToStartState_Game>());
    }
}
