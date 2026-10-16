using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WinState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly IMoneyProvider _moneyProvider;

    public WinState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IMoneyProvider moneyProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _moneyProvider = moneyProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _moneyProvider.ChangeMoney(10);

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
