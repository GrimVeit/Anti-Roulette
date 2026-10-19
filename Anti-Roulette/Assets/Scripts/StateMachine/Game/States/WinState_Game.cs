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
    private readonly ISceneService _sceneService;

    public WinState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IMoneyProvider moneyProvider, ISceneService sceneService)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _moneyProvider = moneyProvider;
        _sceneService = sceneService;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;

        _moneyProvider.ChangeMoney(10);

        _sceneRoot.ShowHitPanel();
        _sceneRoot.HideMainPanel();

        await UniTask.WaitForSeconds(4f);

        _sceneRoot.HideHitPanel();
        _sceneRoot.ShowMainPanel();

        ChangeStateToRouletteClick();

        await UniTask.CompletedTask;
    }

    public override void Exit()
    {
        base.Exit();

        _sceneRoot.OnClickExit_MainHeader -= ChangeSceneToMenu;
    }

    private void ChangeStateToRouletteClick()
    {
        _stateProvider.SetState(_stateProvider.GetState<HoldClickRouletteState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }
}
