using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class NumberPresentationState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ISceneService _sceneService;

    public NumberPresentationState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ISceneService sceneService)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _sceneService = sceneService;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;

        _sceneRoot.ShowNumberPresentationPanel();

        await UniTask.WaitForSeconds(1.5f);

        _sceneRoot.HideRoulettePanel();
        _sceneRoot.ShowMainHeaderPanel();

        await UniTask.WaitForSeconds(2.3f, cancellationToken: token);

        _sceneRoot.HideRoulettePanel();
        _sceneRoot.HideNumberPresentationPanel();

        ChangeStateToMain();

        await UniTask.CompletedTask;
    }

    public override void Exit()
    {
        base.Exit();

        _sceneRoot.OnClickExit_MainHeader -= ChangeSceneToMenu;
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MoveChipToStartState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }
}
