using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoveChipToStartState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly IChipPseudoProvider _pseudoProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly INumberSectorHighlightProvider _numberSectorHighlightProvider;

    public MoveChipToStartState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IChipPseudoProvider chipPseudoProvider, INumberSectorHighlightProvider numberSectorHighlightProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _pseudoProvider = chipPseudoProvider;
        _numberSectorHighlightProvider = numberSectorHighlightProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowMainPanel();

        await UniTask.WaitForSeconds(0.5f);

        _numberSectorHighlightProvider.Show();

        await _pseudoProvider.MoveToStartAsync(token);

        ChangeStateToMain();

        await UniTask.CompletedTask;
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Game>());
    }
}
