using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoseChipState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ISceneService _sceneService;

    public LoseChipState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ISceneService sceneService)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _sceneService = sceneService;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;

        _sceneRoot.ShowMissPanel();
        _sceneRoot.HideMainPanel();

        await UniTask.WaitForSeconds(2.4f);

        _sceneRoot.HideMissPanel();
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
